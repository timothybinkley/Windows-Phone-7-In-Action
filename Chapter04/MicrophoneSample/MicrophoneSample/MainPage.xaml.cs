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

namespace MicrophoneSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Microphone microphone = Microphone.Default;
        private MemoryStream stream;
        private const string FILE_NAME = "recording";
        private const string EXTENSION = "ps";
        private int index;
        byte[] buffer;

        public MainPage()
        {
            InitializeComponent();

            stream = new MemoryStream();

            microphone.BufferDuration = TimeSpan.FromSeconds(1);
            buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];

            microphone.BufferReady += microphone_BufferReady;
        }

        void microphone_BufferReady(object sender, EventArgs e)
        {
            microphone.GetData(buffer);
            stream.Write(buffer, 0, buffer.Length);
        }

        private void recordButton_Click(object sender, RoutedEventArgs e)
        {
            if (microphone.State == MicrophoneState.Stopped)
            {
                microphone.Start();
                recordButton.Content = "Stop";
            }
            else
            {
                microphone.Stop();
                WriteFile(stream);
                recordButton.Content = "Start";
            }
        }

        private void WriteFile(MemoryStream s)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fullFileName = FILE_NAME + index.ToString() + "." + EXTENSION;
                if (storage.FileExists(fullFileName))
                {
                    storage.DeleteFile(fullFileName);
                }

                using (var file = storage.OpenFile(fullFileName, FileMode.CreateNew))
                {
                    s.WriteTo(file);
                }
                playList.Items.Add(fullFileName);
                index++;
            }

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            var fileName = (((sender as Button).Parent as StackPanel).Children[1] as TextBlock).Text;

            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(fileName))
                {
                    var file = storage.OpenFile(fileName, FileMode.Open, FileAccess.Read);
                    var bytes = new byte[file.Length];
                    file.Read(bytes, 0, bytes.Length);
                    PlayFile(bytes);
                }
            }
        }

        private void PlayFile(byte[] file)
        {
            if (file == null || file.Length == 0) return;

            var se = new SoundEffect(file, microphone.SampleRate, AudioChannels.Mono);
            SoundEffect.MasterVolume = 0.7f;
            se.Play();
        }
    }
}