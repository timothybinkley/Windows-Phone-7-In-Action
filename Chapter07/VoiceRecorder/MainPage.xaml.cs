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
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using Microsoft.Devices;

namespace VoiceRecorder
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string FILE_NAME = "recording";
        private const string EXTENSION = "ps";

        private Microphone microphone = Microphone.Default;

        private MemoryStream stream;
        byte[] buffer;

        public MainPage()
        {
            InitializeComponent();

            stream = new MemoryStream();
            microphone.BufferDuration = TimeSpan.FromSeconds(1);
            buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];
            microphone.BufferReady += microphone_BufferReady;
            SoundEffect.MasterVolume = 0.7f;
            LoadFileNames();
        }

        private void LoadFileNames()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filenames = storage.GetFileNames(string.Format("*.{0}", EXTENSION));
                foreach (var filename in filenames)
                {
                    playList.Items.Add(filename);
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            IDictionary<string, string> queryStrings = NavigationContext.QueryString;
            if (queryStrings.ContainsKey("title"))
            {
                PlayFile(queryStrings["title"]);
            }
        }
        
        private void WriteFile(MemoryStream stream)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                int index = 1;
                var fullFileName = string.Format("{0}-{1}.{2}", FILE_NAME, index, EXTENSION);
                while (storage.FileExists(fullFileName))
                {
                    index++;
                    fullFileName = string.Format("{0}-{1}.{2}", FILE_NAME, index, EXTENSION);
                }

                using (var file = storage.OpenFile(fullFileName, FileMode.CreateNew))
                {
                    stream.WriteTo(file);
                }
                playList.Items.Add(fullFileName);

                StreamResourceInfo streamRexInfo = Application.GetResourceStream(new Uri("artwork173.jpg", UriKind.Relative));
                
                var mediaHistoryItem = new MediaHistoryItem
                {
                    ImageStream = streamRexInfo.Stream,
                    Source = "", // must be an empty string
                    Title = fullFileName,
                };
                mediaHistoryItem.PlayerContext.Add("title", fullFileName);
                MediaHistory.Instance.WriteAcquiredItem(mediaHistoryItem);
            }
        }

        private void PlayFile(string fileName)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(fileName))
                {
                    var stream = storage.OpenFile(fileName, FileMode.Open, FileAccess.Read);

                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    var soundEffect = new SoundEffect(bytes, microphone.SampleRate, AudioChannels.Mono);
                    soundEffect.Play();

                    StreamResourceInfo streamRexInfo = Application.GetResourceStream(new Uri("artwork358.jpg", UriKind.Relative));

                    var mediaHistoryItem = new MediaHistoryItem
                    {
                        ImageStream = streamRexInfo.Stream,
                        Source = "", // must be an empty string
                        Title = fileName,
                    };
                    mediaHistoryItem.PlayerContext.Add("title", fileName);
                    MediaHistory.Instance.NowPlaying = mediaHistoryItem;
                }
            }
        }

        void microphone_BufferReady(object sender, EventArgs e)
        {
            microphone.GetData(buffer);
            stream.Write(buffer, 0, buffer.Length);
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            var playControl = (FrameworkElement)sender;
            var fileName = (string)playControl.Tag;
            PlayFile(fileName);
        }

        private void record_Click(object sender, EventArgs e)
        {
            if (microphone.State == MicrophoneState.Stopped)
            {
                microphone.Start();
                playList.IsEnabled = false;
                recordingMessage.Visibility = Visibility.Visible;
            }
        }

        private void stopRecord_Click(object sender, EventArgs e)
        {
            if (microphone.State == MicrophoneState.Started)
            {
                microphone.Stop();
                WriteFile(stream);
                recordingMessage.Visibility = Visibility.Collapsed;
                playList.IsEnabled = true;
            }
        }

    }
}