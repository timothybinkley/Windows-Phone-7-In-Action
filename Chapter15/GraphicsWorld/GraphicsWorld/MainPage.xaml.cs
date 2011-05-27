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
using Microsoft.Devices.Sensors;

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
            NavigationService.Navigate(new Uri("/GamePage.xaml?Input=" + input, UriKind.Relative));
        }

        private void demoGame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml?Input=0", UriKind.Relative));
        }
    }
}