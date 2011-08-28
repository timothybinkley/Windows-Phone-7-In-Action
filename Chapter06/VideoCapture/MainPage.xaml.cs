using System;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace VideoCapture
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // this is not really part of the application, but a demonstration of the available devices

            var devices = CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();
            bool request = CaptureDeviceConfiguration.RequestDeviceAccess();
            foreach (var device in devices)
            {
                System.Diagnostics.Debug.WriteLine("device: {0} ", device.FriendlyName);
            }

            var audioDevices = CaptureDeviceConfiguration.GetAvailableAudioCaptureDevices();
            request = CaptureDeviceConfiguration.RequestDeviceAccess();
            foreach (var device in audioDevices)
            {
                System.Diagnostics.Debug.WriteLine("device: {0} ", device.FriendlyName);
            }
           
        }

        CaptureSource videoRecorder;
        FileSink fileWriter;
        
        private void record_Click(object sender, EventArgs e)
        {
            if (videoRecorder == null || videoRecorder.State != CaptureState.Started)
            {
                videoRecorder = new CaptureSource
                {
                    VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice(),
                    AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice()
                };

                var brush = new VideoBrush();
                brush.SetSource(videoRecorder);
                videoContainer.Fill = brush;

                fileWriter = new FileSink
                {
                    CaptureSource = videoRecorder,
                    IsolatedStorageFileName = "video-recording.mp4",
                };

                videoRecorder.Start();
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            if (videoRecorder != null && videoRecorder.State == CaptureState.Started)
            {
                videoRecorder.Stop();
                videoRecorder = null;
                fileWriter = null;
                videoContainer.Fill = new SolidColorBrush(Colors.Gray);
            }
        }

        private void play_Click(object sender, EventArgs e)
        {
            var task = new MediaPlayerLauncher
            {
                //Controls = MediaPlaybackControls.Stop,
                Location = MediaLocationType.Install,
                Media = new Uri("mytone.wma", UriKind.Relative),
                //Orientation = MediaPlayerOrientation.Portrait,
            };
            task.Show();
        }
    }
}