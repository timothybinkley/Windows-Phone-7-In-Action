using System;
using System.Device.Location;
using System.Text;
using Microsoft.Phone.Controls;

namespace LocationSensor
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher sensor;
        GeoCoordinate previous = new GeoCoordinate();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void startDefault_Click(object sender, EventArgs e)
        {
            StartSensor(GeoPositionAccuracy.Default);
        }

        private void startHigh_Click(object sender, EventArgs e)
        {
            StartSensor(GeoPositionAccuracy.High);
        }

        private void stop_Click(object sender, EventArgs e)
        {
            StopSensor();
        }
                
        private void StartSensor(GeoPositionAccuracy accuracy)
        {
            if (sensor != null)
                StopSensor();

            sensor = new GeoCoordinateWatcher(accuracy);
            sensor.MovementThreshold = 1.0;
            sensor.PositionChanged += sensor_PositionChanged;
            sensor.StatusChanged += sensor_StatusChanged;

            status.Text = string.Format("Permission: {0}\n", sensor.Permission);
            position.Text = string.Empty;

            if (sensor.Permission == GeoPositionPermission.Granted)
                sensor.Start();
        }

        void sensor_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            status.Text = string.Format("Status: {0}\n", e.Status);
        }

        void sensor_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (sensor.Status == GeoPositionStatus.Ready)
            {
                var location = e.Position.Location;

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
                builder.AppendFormat("Desired accuracy: {0}", sensor.DesiredAccuracy);

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

        private void StopSensor()
        {
            if (sensor != null)
            {
                sensor.Stop();
                sensor.PositionChanged -= sensor_PositionChanged;
                sensor.StatusChanged -= sensor_StatusChanged;
                sensor.Dispose();
                sensor = null;
                position.Text += "\nSensor has been stopped.";
            }
        }

    }
}