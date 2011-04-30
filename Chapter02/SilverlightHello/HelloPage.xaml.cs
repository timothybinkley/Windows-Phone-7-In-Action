using System;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Marketplace;

namespace SilverlightHello
{
    public partial class HelloPage : PhoneApplicationPage
    {
        // Constructor
        public HelloPage()
        {
            InitializeComponent();
            globeBrush = (SolidColorBrush)ContentPanel.Resources["GlobeBrush"];

            LicenseInformation licenseInfo = new LicenseInformation();
#if TRIAL_LICENSE
            bool isInTrialMode = true;
#else
            bool isInTrialMode = licenseInfo.IsTrial();
#endif
            if (isInTrialMode)
            {
                ApplicationTitle.Text += " (TRIAL)";
            }
        }

        private void navigateForwardButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/GreetingPage.xaml?name="
                + nameInput.Text, UriKind.Relative));
        }

        Color[] colors = new Color[] { Colors.Red, Colors.Orange, Colors.Yellow, Colors.Green, Colors.Blue, Colors.Purple };
        int colorIndex = 0;
        SolidColorBrush globeBrush;

        private void Canvas_Tap(object sender, EventArgs e)
        {
            colorIndex++;
            if (colorIndex >= colors.Length)
                colorIndex = 0;
            globeBrush.Color = colors[colorIndex];
        }

        private void Canvas_DoubleTap(object sender, EventArgs e)
        {
            globeBrush.Color = (Color)App.Current.Resources["PhoneAccentColor"];
        }
    }
}
