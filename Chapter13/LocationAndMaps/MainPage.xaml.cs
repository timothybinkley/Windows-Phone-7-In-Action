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
using System.Device.Location;
using System.Text;
using Microsoft.Phone.Controls.Maps;
using LocationAndMaps.GeocodeService;
using System.Collections.ObjectModel;

namespace LocationAndMaps
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher service;
        GeoCoordinate previous = new GeoCoordinate();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            routeLine.Locations = new LocationCollection();
        }

        private void startDefault_Click(object sender, EventArgs e)
        {
            StartService(GeoPositionAccuracy.Default);
        }

        private void startHigh_Click(object sender, EventArgs e)
        {
            StartService(GeoPositionAccuracy.High);
        }

        private void stop_Click(object sender, EventArgs e)
        {
            StopService();
        }

        private void StartService(GeoPositionAccuracy accuracy)
        {
            if (service != null)
                StopService();

            service = new GeoCoordinateWatcher(accuracy);
            service.MovementThreshold = 1.0;
            service.PositionChanged += service_PositionChanged;
            service.StatusChanged += service_StatusChanged;

            status.Text = string.Format("Permission: {0}\n", service.Permission);
            position.Text = string.Empty;

            if (service.Permission == GeoPositionPermission.Granted)
                service.Start();
        }

        void service_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            status.Text = string.Format("Status: {0}        Desired accuracy: {1}", e.Status, service.DesiredAccuracy);
        }

        void service_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (service.Status == GeoPositionStatus.Ready)
            {
                GeoCoordinate location = e.Position.Location;
                UpdatePositionText(location);
                UpdateMap(location);
                previous = location;
            }
        }

        private void UpdateMap(GeoCoordinate location)
        {
            if (previous.IsUnknown)
            {
                Pushpin startPin = CreatePushpin(location, Colors.Green);
                routeLayer.AddChild(startPin, startPin.Location);
            }

            pinLayer.Children.Clear();
            Pushpin pin = CreatePushpin(location, Colors.Blue, "You");
            pinLayer.AddChild(pin, pin.Location);

            mapControl.Center = location;
            mapControl.SetView(LocationRect.CreateLocationRect(routeLine.Locations));
            routeLine.Locations.Add(location);
        }

        private void UpdatePositionText(GeoCoordinate loc)
        {
            StringBuilder b = new StringBuilder();
            b.AppendFormat("Latitude:     {0} ± {1:F0} meters\n", FormatCoordinate(loc.Latitude, 'N', 'S'), loc.HorizontalAccuracy);
            b.AppendFormat("Longitude:  {0} ± {1:F0} meters\n", FormatCoordinate(loc.Longitude, 'E', 'W'), loc.HorizontalAccuracy);
            b.AppendFormat("Altitude:     {0:F0} ± {1:F0} meters\n", loc.Altitude, loc.VerticalAccuracy);
            b.AppendFormat("Heading:     {0:F0} degrees from true north\n", loc.Course);
            b.AppendFormat("Speed:        {0:F0} meters/second\n", loc.Speed);

            double distance = Double.NaN;
            if (!loc.IsUnknown && !previous.IsUnknown)
                distance = loc.GetDistanceTo(previous);

            b.AppendFormat("Distance:   {0:F0} meters from previous reading\n\n", distance);
            position.Text = b.ToString();
        }

        private string FormatCoordinate(double coordinate, char positive, char negative)
        {
            char direction = coordinate >= 0 ? positive : negative;
            coordinate = Math.Abs(coordinate);
            double degrees = Math.Floor(coordinate);
            double minutes = Math.Floor((coordinate - degrees) * 60.0D);
            double seconds = (((coordinate - degrees) * 60.0D) - minutes) * 60.0D;
            string result = string.Format("{0}{1:F0}° {2:F0}' {3:F1}\"", direction, degrees, minutes, seconds);
            return result;
        }

        private void StopService()
        {
            if (service != null)
            {
                service.Stop();
                service.PositionChanged -= service_PositionChanged;
                service.StatusChanged -= service_StatusChanged;
                service.Dispose();
                service = null;
                status.Text = "The Location Service has been stopped.";
            }
        }

        private Pushpin CreatePushpin(GeoCoordinate location, Color color, string label = null)
        {
            StackPanel content = new StackPanel { Height = 30, Orientation = System.Windows.Controls.Orientation.Horizontal };
            content.Children.Add(new Ellipse { Height = 20, Width = 20, Fill = new SolidColorBrush(color) });
            content.Children.Add(new TextBlock { Text = label });
            return new Pushpin { Location = location, Content = content };
        }

        private void geocode_Click(object sender, EventArgs e)
        {
            GeocodeServiceClient geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            geocodeService.ReverseGeocodeCompleted += geocodeService_ReverseGeocodeCompleted;

            ReverseGeocodeRequest geocodeRequest = new ReverseGeocodeRequest()
            {
                Credentials = new Credentials
                {
                    ApplicationId = "enter your developer key here"
                },
                Location = new GeocodeLocation
                {
                    Latitude = previous.Latitude,
                    Longitude = previous.Longitude,
                }
            };
            geocodeService.ReverseGeocodeAsync(geocodeRequest);
        }

        void geocodeService_ReverseGeocodeCompleted(object sender, ReverseGeocodeCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null ||
               e.Result.ResponseSummary.StatusCode != ResponseStatusCode.Success)
            {
                MessageBox.Show("Unable to complete the ReverseGeocode request");
                return;
            }
            GeocodeResponse response = e.Result;
            if (response.Results.Count > 0)
            {
                GeocodeResult address = response.Results[0];
                MessageBox.Show(address.DisplayName);
            }
        }
    }
}