using System;
using System.Windows;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;

namespace GraphicsWorld
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            motionSensor.IsEnabled = Motion.IsSupported;
        }

        private void playGame(object sender, RoutedEventArgs e)
        {
            int input = 0;
            if (thumbstick.IsChecked.HasValue && thumbstick.IsChecked.Value)
                input = 1;
            else if (buttonPad.IsChecked.HasValue && buttonPad.IsChecked.Value)
                input = 2;
            else if (gestures.IsChecked.HasValue && gestures.IsChecked.Value)
                input = 3;
            else
                input = 4;
            NavigationService.Navigate(GamePage.BuildNavigationUri(input));
        }

        private void demoGame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(GamePage.BuildNavigationUri(0));
        }
    }
}