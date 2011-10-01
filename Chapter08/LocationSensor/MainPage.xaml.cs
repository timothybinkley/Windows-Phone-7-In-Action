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
using System.IO.IsolatedStorage;
using System.IO;
using System.Text;

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

        private void StopSensor()
        {
            if (sensor != null)
            {
                sensor.Stop();
                sensor.PositionChanged -= sensor_PositionChanged;
                sensor.StatusChanged -= sensor_StatusChanged;
                sensor = null;
                position.Text += "\nSensor has been stopped.";
            }
            
            if (writer != null)
            {
                writer.Close();
                writer = null;
            }
            if (file != null)
            {
                file.Close();
                file = null;
            }
        }

        private void StartSensor(GeoPositionAccuracy accuracy)
        {
            position.Text = string.Empty;

            if (sensor != null)
                StopSensor();

            sensor = new GeoCoordinateWatcher(accuracy);
            sensor.MovementThreshold = 1.0;
            sensor.PositionChanged += sensor_PositionChanged;
            sensor.StatusChanged += sensor_StatusChanged;

            status.Text = string.Format("Permission: {0}\n", sensor.Permission);
            

            if (sensor.Permission == GeoPositionPermission.Granted)
                sensor.Start();
        }

        void sensor_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            status.Text = string.Format("Status: {0}\n", e.Status);
        }

        void sensor_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // temporary code
            if (writer != null)
            {
                var location = e.Position.Location;
                double distance = Double.NaN;
                if (!location.IsUnknown && !previous.IsUnknown)
                    distance = location.GetDistanceTo(previous);

                writer.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                    location.Latitude, location.Longitude, location.Altitude,
                    location.Course, location.Speed,
                    location.HorizontalAccuracy, location.VerticalAccuracy,
                    location.IsUnknown, sensor.Status, e.Position.Timestamp,
                    distance, sensor.DesiredAccuracy));

                writer.Flush();
                file.Flush();
            }

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
            double minutes = (coordinate - degrees) * 60.0D;
            double seconds = (minutes - Math.Floor(minutes)) * 60.0D;
            string result = string.Format("{0}{1:F0}° {2:F0}' {3:F1}\"", direction, degrees, minutes, seconds);
            return result;
        }

 
        private void startDefault_Click(object sender, EventArgs e)
        {
            Openfile(); // temporary code
            StartSensor(GeoPositionAccuracy.Default);

        }

        private void startHigh_Click(object sender, EventArgs e)
        {
            Openfile(); // temporary code
            StartSensor(GeoPositionAccuracy.High);

        }

        private void stop_Click(object sender, EventArgs e)
        {
            StopSensor();
        }

        // temporary code
        IsolatedStorageFileStream file;
        StreamWriter writer;

        private void Openfile()
        {
            if (file == null)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    file = store.OpenFile("locationData.txt", System.IO.FileMode.Append);
                    writer = new StreamWriter(file);
                }
            }
        }
    }
}