using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Controls;

namespace UserInterfaceControls.Toolkit
{
    public partial class ContextMenuSample : PhoneApplicationPage
    {
        public ContextMenuSample()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            MessageBox.Show(menuItem.Name, "Menu Item Clicked", MessageBoxButton.OK);
        }

        private void menuItem2Enabled_Clicked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            menuItem2.IsEnabled = checkBox.IsChecked.Value;
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("menu opened");
        }

    }
}