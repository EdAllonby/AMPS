using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox
    {
        /// <summary>
        /// The Displayed Items Dependency Property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(List<string>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null, OnItemsSourceChanged));

        /// <summary>
        /// The Items selected Dependency Property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(List<string>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

        /// <summary>
        /// The Control's text Dependency Property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// The Control's default text Dependency Property.
        /// </summary>
        public static readonly DependencyProperty DefaultTextProperty = DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        private readonly ObservableCollection<DropdownMenuCheckBoxItem> nodeList;

        /// <summary>
        /// Create a new Multi select dropdown menu with combo boxes.
        /// </summary>
        public MultiSelectComboBox()
        {
            InitializeComponent();
            nodeList = new ObservableCollection<DropdownMenuCheckBoxItem>();
        }

        /// <summary>
        /// The items to display.
        /// </summary>
        public List<string> ItemsSource
        {
            get { return (List<string>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// The items selected.
        /// </summary>
        public List<string> SelectedItems
        {
            get { return (List<string>) GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        /// <summary>
        /// The control's text.
        /// </summary>
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// The control's default text.
        /// </summary>
        public string DefaultText
        {
            get { return (string) GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiSelectComboBox) d;
            control.DisplayInControl();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiSelectComboBox) d;
            control.SelectNodes();
            control.SetText();
        }

        private void CheckBoxItemClicked(object sender, RoutedEventArgs e)
        {
            var clickedBox = (CheckBox) sender;

            if ((string) clickedBox.Content == "All")
            {
                if (clickedBox.IsChecked != null && clickedBox.IsChecked.Value)
                {
                    foreach (DropdownMenuCheckBoxItem node in nodeList)
                    {
                        node.IsSelected = true;
                    }
                }
                else
                {
                    foreach (DropdownMenuCheckBoxItem node in nodeList)
                    {
                        node.IsSelected = false;
                    }
                }
            }
            else
            {
                int selectedCount = nodeList.Count(node => node.IsSelected && node.Title != "All");

                DropdownMenuCheckBoxItem firstItem = nodeList.FirstOrDefault(i => i.Title == "All");

                if (firstItem != null)
                {
                    firstItem.IsSelected = selectedCount == nodeList.Count - 1;
                }
            }

            SetSelectedItems();

            SetText();
        }

        private void SelectNodes()
        {
            foreach (DropdownMenuCheckBoxItem node in SelectedItems.Select(keyValue => nodeList.FirstOrDefault(i => i.Title == keyValue)).Where(node => node != null))
            {
                node.IsSelected = true;
            }
        }

        private void SetSelectedItems()
        {
            if (SelectedItems == null)
            {
                SelectedItems = new List<string>();
            }

            SelectedItems.Clear();

            List<string> newSelectedItems = nodeList.Where(node => node.IsSelected && node.Title != "All").Where(node => ItemsSource.Count > 0).Select(item => item.Title).ToList();

            SelectedItems = newSelectedItems;
        }

        private void DisplayInControl()
        {
            nodeList.Clear();

            if (ItemsSource.Count > 0)
            {
                nodeList.Add(new DropdownMenuCheckBoxItem("All"));
            }

            foreach (DropdownMenuCheckBoxItem node in ItemsSource.Select(keyValue => new DropdownMenuCheckBoxItem(keyValue)))
            {
                nodeList.Add(node);
            }

            MultiSelectCombo.ItemsSource = nodeList;
        }

        private void SetText()
        {
            if (SelectedItems != null)
            {
                var displayText = new StringBuilder();
                foreach (DropdownMenuCheckBoxItem node in nodeList)
                {
                    if (node.IsSelected && node.Title == "All")
                    {
                        displayText = new StringBuilder();
                        displayText.Append("All");
                        break;
                    }

                    if (node.IsSelected && node.Title != "All")
                    {
                        displayText.Append(node.Title);
                        displayText.Append(", ");
                    }
                }

                Text = displayText.ToString().TrimEnd(',', ' ');
            }

            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(Text))
            {
                Text = DefaultText;
            }
        }
    }
}