using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Client.ViewModel.Properties;
using log4net;
using Shared;
using Utility;

namespace Client.ViewModel
{
    /// <summary>
    /// Defines a View Model for a View.
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The logging mechanism for View Models.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof (ViewModel));

        /// <summary>
        /// The client's <see cref="IServiceRegistry" />.
        /// </summary>
        protected readonly IServiceRegistry ServiceRegistry;

        private bool? isInDesignMode;

        /// <summary>
        /// Creates a new View Model.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        protected ViewModel(IServiceRegistry serviceRegistry)
        {
            if (!IsInDesignMode)
            {
                ServiceRegistry = serviceRegistry;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend or Visual Studio).
        /// </summary>
        protected bool IsInDesignMode
        {
            get
            {
                if (!isInDesignMode.HasValue)
                {
#if DEBUG
#if SILVERLIGHT
isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    DependencyProperty prop = DesignerProperties.IsInDesignModeProperty;
                    isInDesignMode =
                        (bool) DependencyPropertyDescriptor.FromProperty(prop, typeof (FrameworkElement)).Metadata.DefaultValue;

                    if (!isInDesignMode.Value)
                    {
                        if (Process.GetCurrentProcess().ProcessName.StartsWith(@"devenv"))
                        {
                            isInDesignMode = true;
                        }
                    }
#endif

#else
isInDesignMode = false;
#endif
                }
                return isInDesignMode.Value;
            }
        }

        /// <summary>
        /// Raised when a View Model property has been changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a View Model property has been changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that has been changed.</param>
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            EventUtility.SafeFireEvent(PropertyChanged, this, propertyName);
        }
    }
}