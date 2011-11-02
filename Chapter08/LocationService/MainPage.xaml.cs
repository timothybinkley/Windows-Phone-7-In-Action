using System;
using System.Device.Location;
using System.Text;
using Microsoft.Phone.Controls;

namespace LocationService
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher service;
        GeoCoordinate previous = new GeoCoordinate();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
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
            status.Text = string.Format("Status: {0}\n", e.Status);
        }

        void service_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (service.Status == GeoPositionStatus.Ready)
            {
                GeoCoordinate location = e.Position.Location;

                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("Latitude:      {0}\n", FormatCoordinate(location.Latitude, 'N', 'S'));
                builder.AppendFormat("Longitude:   {0}\n", FormatCoordinate(location.Longitude, 'E', 'W'));
                builder.AppendFormat("                     ± {0:F0} meters\n\n", location.HorizontalAccuracy);

                builder.AppendFormat("Altitude:   {0:F0} ± {1:F0} meters\n\n", location.Altitude, location.VerticalAccuracy);

                builder.AppendFormat("Heading:   {0:F0} degrees from true north\n", location.Course);
                builder.AppendFormat("Speed:      {0:F0} meters/second\n", location.Speed);

                double distance = Double.NaN;
                if (!location.IsUnknown && !previous.IsUnknown)
                    distance = location.GetDistanceTo(previous);

                builder.AppendFormat("Distance:   {0:F0} meters from previous reading\n\n", distance);

                previous = location;

                builder.AppendFormat("Reading taken at {0:hh:mm:ss.fff tt}\n", e.Position.Timestamp);
                builder.AppendFormat("Desired accuracy: {0}", service.DesiredAccuracy);

                position.Text = builder.ToString();
            }
        }

        private string FormatCoordinate(double coordinate, char positive, char negative)
        {
            char direction = coordinate >= 0 ? positive : negative;
            coordinate = Math.Abs(coordinate);
            double degrees = Math.Floor(coordinate);
            double minutes = Math.Floor((coordinate - degrees) * 60.0D);
            double seconds = (((coordinate - degrees) * 60.0D)- minutes) * 60.0D;
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
                position.Text += "\nThe Location Service has been stopped.";
            }
        }

    }
}