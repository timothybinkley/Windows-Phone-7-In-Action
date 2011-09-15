using System;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace VideoCapture
{
    public partial class MainPage : PhoneApplicationPage
    {
        CaptureSource videoRecorder;
        FileSink fileWriter;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var videoDevices = CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();
            if (videoDevices.Count > 1)
            {
                var recordFrontButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
                recordFrontButton.IsEnabled = true;
            }

            // this is not really part of the application, but a demonstration of the available devices
            var devices = CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();
            foreach (var device in devices)
            {
                var desired = device.DesiredFormat;
                System.Diagnostics.Debug.WriteLine("device: {0} {1} {2}", device.FriendlyName, device.IsDefaultDevice, device.DesiredFormat);
                foreach (var format in device.SupportedFormats)
                {
                    System.Diagnostics.Debug.WriteLine("     format: {0}x{1} {2}, {3}, {4}", format.PixelWidth, format.PixelHeight,
                        format.FramesPerSecond, format.FramesPerSecond, format.Stride);
                }
            }

            var audioDevices = CaptureDeviceConfiguration.GetAvailableAudioCaptureDevices();
            foreach (var device in audioDevices)
            {
                System.Diagnostics.Debug.WriteLine("device: {0} {1} {2}", device.FriendlyName, device.IsDefaultDevice, device.DesiredFormat, device.AudioFrameSize);
                foreach (var format in device.SupportedFormats)
                {
                    System.Diagnostics.Debug.WriteLine("     format: {0}, {1}, {2}, {3}", format.WaveFormat, format.Channels,
                        format.SamplesPerSecond, format.BitsPerSample);
                }
            }
           
        }
        
        private void recordPrimary_Click(object sender, EventArgs e)
        {
            // this method assume the primary or back camera is the default video capture device.
            var videoDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
            StartCapture(videoDevice);
        }

        private void recordFront_Click(object sender, EventArgs e)
        {
            // this method uses the front facing camera is the second device listed in GetAvailableVideoCaptureDevices
            var devices = CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();
            var videoDevice = devices[1];
            StartCapture(videoDevice);
        }

        private void StartCapture(VideoCaptureDevice videoDevice)
        {
            if (videoRecorder == null || videoRecorder.State != CaptureState.Started)
            {
                videoRecorder = new CaptureSource
                {
                    VideoCaptureDevice = videoDevice,
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
                Location = MediaLocationType.Data,
                Media = new Uri("video-recording.mp4", UriKind.Relative),
            };
            task.Show();
        }
    }
}