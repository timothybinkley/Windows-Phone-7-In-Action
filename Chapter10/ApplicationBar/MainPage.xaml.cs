using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ApplicationBar
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            InitializeApplicationBar();
        }

        private void InitializeApplicationBar()
        {
            button1 = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            menuItem1 = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
        }

        private void item_Clicked(object sender, EventArgs e)
        {
            var button = sender as ApplicationBarIconButton;
            if (button != null)
            {
                MessageBox.Show(button.Text, "Button Clicked", MessageBoxButton.OK);
            }
            else
            {
                var menuItem = sender as ApplicationBarMenuItem;
                MessageBox.Show(menuItem.Text, "Menu Item Clicked", MessageBoxButton.OK);
            }
        }

        private void appBarVisible_Clicked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            ApplicationBar.IsVisible = checkBox.IsChecked.Value;
        }

        private void appBarEnabled_Clicked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            ApplicationBar.IsMenuEnabled = checkBox.IsChecked.Value;
        }

        private void button1Enabled_Clicked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            button1.IsEnabled = checkBox.IsChecked.Value;
        }

        private void menuItem1Enabled_Clicked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            menuItem1.IsEnabled = checkBox.IsChecked.Value;
        }

        private void button1Show_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            if (checkBox.IsChecked.Value)
                ApplicationBar.Buttons.Insert(0, button1);
            else
                ApplicationBar.Buttons.Remove(button1);
        }

        private void menuItem1Show_Clicked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            if (checkBox.IsChecked.Value)
                ApplicationBar.MenuItems.Insert(0, menuItem1);
            else
                ApplicationBar.MenuItems.Remove(menuItem1);
        }

        private void button1_Clicked(object sender, EventArgs e)
        {
            if (button1.Text == "alpha")
            {
                button1.Text = "omega";
                button1.IconUri = new Uri("/Images/omega.png", UriKind.Relative);
            }
            else
            {
                button1.Text = "alpha";
                button1.IconUri = new Uri("/Images/alpha.png", UriKind.Relative);
            }
        }

        private void ApplicationBar_StateChanged(object sender, ApplicationBarStateChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("IsMenuVisible: " + e.IsMenuVisible);
        }
    }
}