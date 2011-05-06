using System;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using System.Windows.Shapes;
using System.Windows;

namespace Sensors
{
    public partial class MainPage : PhoneApplicationPage
    {
        Accelerometer accelerometer;
        /*
        Compass compass;
        Gyroscope gyro;
        MotionSensor motion;
        */

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void UpdateReading( double max, double x, double y, double z)
        {
            double top = x <= 0.0 ? 175.0 : 175.0 - x * 175.0 / max;
            double bottom = x >= 0.0 ? 175.0 : 175.0 + x * 175.0 / 2;
            bar1.Margin = new Thickness(30.0, top, 30.0, bottom);

            top = y <= 0.0 ? 175.0 : 175.0 - y * 175.0 / 2;
            bottom = y >= 0.0 ? 175.0 : 175.0 + y * 175.0 / 2;
            bar2.Margin = new Thickness(30.0, top, 30.0, bottom);

            top = z <= 0.0 ? 175.0 : 175.0 - z * 175.0 / 2;
            bottom = z >= 0.0 ? 175.0 : 175.0 + z * 175.0 / 2;
            bar3.Margin = new Thickness(30.0, top, 30.0, bottom);
        }

        private void StopActiveSensor()
        {
            if (accelerometer != null)
            {
                accelerometer.ReadingChanged -= accelerometer_CurrentValueChanged;
                accelerometer = null;
            }
            //if (compass != null)
            //{
            //    compass.CurrentValueChanged -= compass_CurrentValueChangedChanged;
            //    compass = null;    
            //}
            //if (gyro != null)
            //{
            //    gyro.CurrentValueChanged -= compass_CurrentValueChangedChanged;
            //    gyro = null;    
            //}
            //if (motion != null)
            //{
            //    motion.CurrentValueChanged -= compass_CurrentValueChangedChanged;
            //    motion = null;    
            //}

            header1.Text = "";
            header2.Text = "";
            header3.Text = "";
            message.Text = "Select a sensor from the application bar.";

            UpdateReading(1.0, 0.0, 0.0, 0.0);
        }

        private void accelerometer_Click(object sender, EventArgs e)
        {
            if (accelerometer == null)
            {
                StopActiveSensor();

                header1.Text = "X";
                header2.Text = "Y";
                header3.Text = "Z";
                message.Text = "Reading acceleration data from the accelerometer.";

                accelerometer = new Accelerometer();
                // accelerometer.TimeBetweenUpdates = Timespan.FromMilliseconds(33);
                accelerometer.ReadingChanged += accelerometer_CurrentValueChanged;
                accelerometer.Start();
            }
        }

        void accelerometer_CurrentValueChanged(object sender, AccelerometerReadingEventArgs e) // SensorReadingEventArgs<???> e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                UpdateReading(2.0, e.X, e.Y, e.Z);
                //var reading = e.SensorReading.Acceleration;
                //UpdateReading(2.0, reading.X, reading.Y, reading.Z);
            });
        }

        private void compass_Click(object sender, EventArgs e)
        {
            //if (compass == null)
            {
                StopActiveSensor();

                header1.Text = "X";
                header2.Text = "Y";
                header3.Text = "Z";
                message.Text = "Reading magnetometer data from the compass.";

                //compass = new Compass();
                //compass.TimeBetweenUpdates = Timespan.FromMilliseconds(33);
                //compass.CurrentValueChanged += compass_CurrentValueChangedChanged;
                //compass.start();
            }
        }

        void compass_CurrentValueChanged(object sender) // SensorReadingEventArgs<???> e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                //var reading = e.SensorReading.MagnetometerReading;
                //UpdateReading(360.0, reading.X, reading.Y, reading.Z);
            });
        }

        private void gyro_Click(object sender, EventArgs e)
        {
            //if (gyro == null)
            {
                StopActiveSensor();

                header1.Text = "X";
                header2.Text = "Y";
                header3.Text = "Z";
                message.Text = "Reading rotation rates from the gyroscope.";

                //gyro = new Gyroscope();
                //gyro.TimeBetweenUpdates = Timespan.FromMilliseconds(33);
                //gyro.CurrentValueChanged += gyro_CurrentValueChangedChanged;
                //gyro.start();
            }
        }

        void gyro_CurrentValueChanged(object sender) // SensorReadingEventArgs<???> e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                //var reading = e.SensorReading.RotationRate;
                //UpdateReading(360.0, reading.X, reading.Y, reading.Z);
            });
        }

        private void motion_Click(object sender, EventArgs e)
        {
            //if (motion == null)
            {
                StopActiveSensor();

                header1.Text = "Yaw";
                header2.Text = "Pitch";
                header3.Text = "Roll";
                message.Text = "Reading attitude from from the motion sensor";

                //motion = new MotionSensor();
                //motion.TimeBetweenUpdates = Timespan.FromMilliseconds(33);
                //motion.CurrentValueChanged += motion_CurrentValueChangedChanged;
                //motion.start();
            }
        }

        void motion_CurrentValueChanged(object sender) // SensorReadingEventArgs<???> e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                //var reading = e.SensorReading.Attitude;
                //UpdateReading(360.0, reading.Yaw, reading.Pitch, reading.Roll);
            });
        }
    }
}