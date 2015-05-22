using System.ComponentModel;
using System.Runtime.CompilerServices;
using Client.Model.Properties;

namespace Client.Model
{
    /// <summary>
    /// A model which properties can notify changes.
    /// </summary>
    public abstract class NotifiableModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Fires when a property in the model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Call when a property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property changed.</param>
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}