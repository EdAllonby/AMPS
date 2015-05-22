using System.ComponentModel;

namespace Client.View.UI
{
    /// <summary>
    /// Models a dropdown menu checkbox item.
    /// </summary>
    public class DropdownMenuCheckBoxItem : INotifyPropertyChanged
    {
        private bool isSelected;
        private string title;

        /// <summary>
        /// Create a new dropdown menu checkbox item with a title.
        /// </summary>
        /// <param name="title"></param>
        public DropdownMenuCheckBoxItem(string title)
        {
            Title = title;
        }

        /// <summary>
        /// The name of the checkbox item.
        /// </summary>
        public string Title
        {
            get { return title; }
            private set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }

        /// <summary>
        /// If the checkbox item is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// Raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}