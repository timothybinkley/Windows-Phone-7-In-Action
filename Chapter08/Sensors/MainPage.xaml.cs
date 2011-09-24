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
using System.Windows.Threading;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace Sensors
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer timer;
        Accelerometer accelSensor;
        Compass compassSensor;
        Gyroscope gyroSensor;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(66);


            if (Accelerometer.IsSupported)
            {
                accelSensor = new Accelerometer();
                accelSensor.TimeBetweenUpdates = TimeSpan.FromMilliseconds(66);
            }
                        
            if (Compass.IsSupported)
            {
                compassSensor = new Compass();
                compassSensor.TimeBetweenUpdates = TimeSpan.FromMilliseconds(66);
                compassSensor.Calibrate += compassSensor_Calibrate;
            }

            if (Gyroscope.IsSupported)
            {
                gyroSensor = new Gyroscope();
                gyroSensor.TimeBetweenUpdates = TimeSpan.FromMilliseconds(66);
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (!timer.IsEnabled)
            {
                String runningMessage = "Reading: ";
                if (Accelerometer.IsSupported)
                {
                    accelSensor.Start();
                    runningMessage += "Accelerometer ";
                }

                if (Compass.IsSupported)
                {
                    compassSensor.Start();
                    runningMessage += "Compass ";
                }

                if (Gyroscope.IsSupported)
                {
                    gyroSensor.Start();
                    runningMessage += "Gyroscope ";
                }

                timer.Start();
                message.Text = runningMessage;
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            accelSensor.Stop();
            compassSensor.Stop();
            gyroSensor.Stop();
            message.Text = "Press start";
        }

        void timer_Tick(object sender, EventArgs e)
        {
            ReadAccelerometerData();
            ReadCompassData();
            ReadGyroscopeData();
        }

        private void ReadAccelerometerData()
        {
            if (Accelerometer.IsSupported)
            {
                Vector3 reading = accelSensor.CurrentValue.Acceleration;
                // height of control = 400; height of postive bar = 200; max value = 2;  
                // set scale at 200/2 = 100  
                accelX.SetValue(reading.X, 100.0f);
                accelY.SetValue(reading.Y, 100.0f);
                accelZ.SetValue(reading.Z, 100.0f);
            }
        }

        void ReadCompassData()
        {
            if (Compass.IsSupported)
            {
                CompassReading currentValue = compassSensor.CurrentValue;
                Vector3 reading = currentValue.MagnetometerReading;
                // height of control = 400; height of postive bar = 200; approximate max value = 50;
                // set scale at 200/50 = 4
                compassX.SetValue(reading.X, 4.0f);
                compassY.SetValue(reading.Y, 4.0f);
                compassZ.SetValue(reading.Z, 4.0f);

                heading.Text = String.Format("Compass (µT)        Heading {0} +/- {1} degrees", currentValue.TrueHeading, currentValue.HeadingAccuracy);
            }
        }

        void ReadGyroscopeData()
        {
            if (Gyroscope.IsSupported)
            {
                Vector3 reading = gyroSensor.CurrentValue.RotationRate;
                // height of control = 400; height of postive bar = 200; reasonable max value = 2pi = 6.25 (1.5 rotation per second)
                // set scale at 200/6.25 = 32
                gyroX.SetValue(reading.X, 32.0f);
                gyroY.SetValue(reading.Y, 32.0f);
                gyroZ.SetValue(reading.Z, 32.0f);
            }
        }

        void compassSensor_Calibrate(object sender, CalibrationEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show("The compass sensor needs to be calibrated. Wave the phone around in the air until the heading accuracy value is less than 20 degrees")
            );
        }
    }
}