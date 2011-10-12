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
using Microsoft.Xna.Framework;

namespace MotionSensor
{
    public partial class MainPage : PhoneApplicationPage
    {
        Motion sensor;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            if (Motion.IsSupported)
            {
                sensor = new Motion();
                sensor.TimeBetweenUpdates = TimeSpan.FromMilliseconds(66);
                sensor.CurrentValueChanged += sensor_CurrentValueChanged;
                sensor.Calibrate += sensor_Calibrate;
            }
        }

        void sensor_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            MotionReading reading = e.SensorReading;
            Dispatcher.BeginInvoke(() =>
            {
                Vector3 acceleration = reading.DeviceAcceleration;
                // height of control = 400; height of postive bar = 200; max value = 1;  
                // set scale at 200/1 = 200  
                accelX.SetValue(acceleration.X, 200.0f);
                accelY.SetValue(acceleration.Y, 200.0f);
                accelZ.SetValue(acceleration.Z, 200.0f);

                Vector3 gravity = reading.Gravity;
                // height of control = 400; height of postive bar = 200; max value = 2;  
                // set scale at 200/2 = 100  
                gravityX.SetValue(gravity.X, 100.0f);
                gravityY.SetValue(gravity.Y, 100.0f);
                gravityZ.SetValue(gravity.Z, 100.0f);

                Vector3 rotation = reading.DeviceRotationRate;
                // height of control = 400; height of postive bar = 200; reasonable max value = 2pi = 6.25 (1.5 rotation per second)
                // set scale at 200/6.25 = 32
                gyroX.SetValue(rotation.X, 32.0f);
                gyroY.SetValue(rotation.Y, 32.0f);
                gyroZ.SetValue(rotation.Z, 32.0f);

                AttitudeReading attitude = reading.Attitude;
                attitudeX.SetValue(attitude.Pitch, 64.0f); // 0->pi 200/3.14 = 64
                attitudeY.SetValue(attitude.Roll, 128.0f);  // 0->pi/2 200/1.57 = 128
                attitudeZ.SetValue(attitude.Yaw, 32.0f);   // 0->2pi 200/6.28 = 32

                Vector3 worldSpacePoint = new Vector3(0.0f, 10.0f, 0.0f);
                Vector3 bodySpacePoint = Vector3.Transform(worldSpacePoint, attitude.RotationMatrix);
                point.Text = string.Format("Attitude - (0.0, 10.0, 0.0) = ({0:F1}, {1:F1}, {2:F1})",
                    bodySpacePoint.X, bodySpacePoint.Y, bodySpacePoint.Z);

            });
        }

        void sensor_Calibrate(object sender, CalibrationEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show("The compass sensor needs to be calibrated. Wave the phone around in the air until the heading accuracy value is less than 20 degrees")
            );
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (Motion.IsSupported)
                sensor.Start();
        }

        private void stop_Click(object sender, EventArgs e)
        {
            if (Motion.IsSupported)
                sensor.Stop();
        }
    }
}