using Microsoft.Phone.Controls;
using System.Device.Location;
using System.Windows;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Navigator
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher coordinateWatcher;
        private MapLayer mapLayer;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            coordinateWatcher = new GeoCoordinateWatcher();
            coordinateWatcher.StatusChanged += coordinateWatcher_StatusChanged;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (coordinateWatcher.Status == GeoPositionStatus.Disabled)
            {
                MessageBox.Show("GPS is disabled: please leave the application and enable it before to launch it again.");
            }
            else
            {
                mapControl.Center.Latitude = -43.531637; // Longitude="" coordinateWatcher.Position.Location.Latitude;
                mapControl.Center.Longitude = 172.636645; // coordinateWatcher.Position.Location.Longitude;
            }
        }

        void coordinateWatcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Status);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var accentBrush = (Brush)Application.Current.Resources["PhoneAccentBrush"];

            var pin = new Pushpin
            {
                Location = new GeoCoordinate
                {
                    Latitude = -43.531637,
                    Longitude = 172.636645
                },
                Background = accentBrush,
                Content = "cool",
            };

            pinLayer.AddChild(pin, pin.Location);

        }
    }
}