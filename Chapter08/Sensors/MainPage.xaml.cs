﻿using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
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
                string runningMessage = "Reading: ";
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
                messageBlock.Text = runningMessage;
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            if(Accelerometer.IsSupported)
                accelSensor.Stop();
            if(Compass.IsSupported)
                compassSensor.Stop();
            if(Gyroscope.IsSupported)
                gyroSensor.Stop();
            messageBlock.Text = "Press start";
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
                AccelerometerReading reading = accelSensor.CurrentValue;
                Vector3 acceleration = reading.Acceleration;
                // height of control = 400; height of postive bar = 200; max value = 2;  
                // set scale at 200/2 = 100  
                accelX.Value = acceleration.X;
                accelY.Value = acceleration.Y;
                accelZ.Value = acceleration.Z;
            }
        }

        void ReadCompassData()
        {
            if (Compass.IsSupported && compassSensor.IsDataValid)
            {
                CompassReading reading = compassSensor.CurrentValue;
                Vector3 magnetic = reading.MagnetometerReading;
                // height of control = 400; height of postive bar = 200; approximate max value = 50;
                // set scale at 200/50 = 4
                compassX.Value = magnetic.X;
                compassY.Value = magnetic.Y;
                compassZ.Value = magnetic.Z;

                heading.Text = string.Format("Compass (µT)        Heading {0} +/- {1} degrees", reading.TrueHeading, reading.HeadingAccuracy);
            }
        }

        void ReadGyroscopeData()
        {
            if (Gyroscope.IsSupported)
            {
                GyroscopeReading reading = gyroSensor.CurrentValue;
                Vector3 rotation = reading.RotationRate;
                // height of control = 400; height of postive bar = 200; reasonable max value = 2pi = 6.25 (1.5 rotation per second)
                // set scale at 200/6.25 = 32
                gyroX.Value = rotation.X;
                gyroY.Value = rotation.Y;
                gyroZ.Value = rotation.Z;
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