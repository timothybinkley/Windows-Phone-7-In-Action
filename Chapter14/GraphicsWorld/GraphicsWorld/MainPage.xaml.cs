using System.Windows;
using Microsoft.Phone.Controls;

namespace GraphicsWorld
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void playGame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(GamePage.BuildNavigationUri());
        }
    }
}