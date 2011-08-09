using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Resources;
using Microsoft.Devices;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Navigation;

namespace VoiceRecorder
{
    public partial class MainPage : PhoneApplicationPage
    {
        //private Microphone microphone = Microphone.Default;
      
        private MemoryStream audioStream=null;
        byte[] audioBuffer=null;

        public MainPage()
        {
            InitializeComponent();
                        
            Microphone.Default.BufferDuration = TimeSpan.FromSeconds(1);
            Microphone.Default.BufferReady += microphone_BufferReady;

            DisplayRecordingNames();
        }

        void microphone_BufferReady(object sender, EventArgs e)
        {
            int count = Microphone.Default.GetData(audioBuffer);
            audioStream.Write(audioBuffer, 0, count);
            System.Diagnostics.Debug.WriteLine("Recording {0} bytes", count);
        }

        private void DisplayRecordingNames()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filenames = storage.GetFileNames("*.wav");
                foreach (var filename in filenames)
                {
                    recordingList.Items.Add(filename);
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IDictionary<string, string> queryStrings = NavigationContext.QueryString;
            if (queryStrings.ContainsKey("vrec-filename"))
            {
                PlayFileInBackground(queryStrings["vrec-filename"]);
            }
        }
        
        private void play_Click(object sender, RoutedEventArgs e)
        {
            var playControl = (FrameworkElement)sender;
            var fileName = (string)playControl.Tag;
            PlayFileInBackground(fileName);
        }

        private void record_Click(object sender, EventArgs e)
        {
            if (Microphone.Default.State == MicrophoneState.Stopped)
            {
                recordingList.IsEnabled = false;
                recordingMessage.Visibility = Visibility.Visible;

                audioStream = new MemoryStream();
                audioBuffer = new byte[Microphone.Default.GetSampleSizeInBytes(TimeSpan.FromSeconds(1))];

                Microphone.Default.Start();
            }
        }

        private void stopRecord_Click(object sender, EventArgs e)
        {
            if (Microphone.Default.State == MicrophoneState.Started)
            {
                System.Diagnostics.Debug.WriteLine("..");
                Microphone.Default.Stop();

                microphone_BufferReady(this, EventArgs.Empty);

                audioBuffer = null;

                WriteFile();
                audioStream = null;

                recordingMessage.Visibility = Visibility.Collapsed;
                recordingList.IsEnabled = true;
            }
        }
        
        private void WriteFile()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                int index = 1;
                var fullFileName = string.Format("voice-recording-{0}.wav", index);
                while (storage.FileExists(fullFileName))
                {
                    index++;
                    fullFileName = string.Format("voice-recording-{0}.wav", index);
                }

                using (var file = storage.OpenFile(fullFileName, FileMode.CreateNew))
                using (var writer = new BinaryWriter(file))
                {
                    System.Diagnostics.Debug.WriteLine("Saving {0} bytes", audioStream.Length);

                    writer.Write(new char[4] { 'R', 'I', 'F', 'F' }); // start of the RIFF header
                    writer.Write((Int32)(36 + audioStream.Length)); // FileSize - 8
                    writer.Write(new char[4] { 'W', 'A', 'V', 'E' }); // the characters WAVE intdicate the format of the data
                    writer.Write(new char[4] { 'f', 'm', 't', ' ' });  // the fmt characters specify that this is the section of the file describing the format
                    writer.Write((Int32)16); // size of the WAVEFORMATEX data to follow
                    // WAVEFORMATEX
                    writer.Write((UInt16)1); // wFormatTag = 1 indicates that the audio data is PCM
                    writer.Write((UInt16)1); // nChannels = 1 for mono
                    writer.Write((UInt32)16000); // nSamplesPerSec, Sample rate of the waveform in samples per second
                    writer.Write((UInt32)32000); // nAvgBytesPerSec, Average bytes per second which can be used to determine the time-wise length of the audio
                    writer.Write((UInt16)2); // nBlockAlign, Specifies how each audio block must be aligned in bytes
                    writer.Write((UInt16)16); // wBitsPerSample, How many bits represent a single sample (typically 8 or 16)
                    writer.Write(new char[4] { 'd', 'a', 't', 'a' }); //, The "data" characters specify that the audio data is next in the file

                    writer.Write((Int32)audioStream.Length); // The length of the data in bytes
                    writer.Write(audioStream.GetBuffer(), 0, (int)audioStream.Length); //Data, The rest of the file is the actual samples
                    writer.Flush();
                }
                recordingList.Items.Add(fullFileName);

                // integrate with music hub
                MediaHistory.Instance.WriteAcquiredItem(createMediaHistoryItem(fullFileName, true));
            }
        }

        private void PlayFile(string filename)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(filename))
                {
                    var fileStream = storage.OpenFile(filename, FileMode.Open, FileAccess.Read);
                    
                    var soundEffect = SoundEffect.FromStream(fileStream);
                    soundEffect.Play();

                    // integrate with music hub
                    MediaHistory.Instance.NowPlaying = createMediaHistoryItem(filename, false);
                    MediaHistory.Instance.WriteRecentPlay(createMediaHistoryItem(filename, true));
                }
            }
        }

        private void PlayFileInBackground(string filename)
        {
            Uri fileUri = new Uri(filename, UriKind.Relative);

            BackgroundAudioPlayer.Instance.Track = new AudioTrack(new Uri(filename, UriKind.Relative),
                filename, null, null, null);

            BackgroundAudioPlayer.Instance.Play();
        }

        private MediaHistoryItem createMediaHistoryItem(string fullFileName, Boolean smallSize)
        {
            string imageName = smallSize ? "artwork173.jpg" : "artwork358.jpg";
            StreamResourceInfo imageInfo = Application.GetResourceStream(new Uri(imageName, UriKind.Relative));

            var mediaHistoryItem = new MediaHistoryItem
            {
                ImageStream = imageInfo.Stream,
                Source = "", // must be an empty string
                Title = fullFileName
            };
            mediaHistoryItem.PlayerContext.Add("vrec-filename", fullFileName);

            return mediaHistoryItem;
        }

    }
}