using System.Windows;
using Microsoft.Phone.Controls;

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

    }
}