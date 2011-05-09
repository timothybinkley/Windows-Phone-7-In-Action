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
using Microsoft.Devices;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Media;

namespace MusicVideoHubSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void NowPlaying_Click(object sender, RoutedEventArgs e)
        {
            var mediaHistoryItem = new MediaHistoryItem();            
            StreamResourceInfo streamRexInfo = Application.GetResourceStream(new Uri("album.JPG", UriKind.Relative));
            mediaHistoryItem.ImageStream = streamRexInfo.Stream;
            mediaHistoryItem.Source = "";
            mediaHistoryItem.Title = "NowPlaying";
            mediaHistoryItem.PlayerContext.Add("key", "value");
            MediaHistory.Instance.NowPlaying = mediaHistoryItem;
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            var mediaHistoryItem = new MediaHistoryItem();
            StreamResourceInfo streamRexInfo = Application.GetResourceStream(new Uri("album.JPG", UriKind.Relative));
            mediaHistoryItem.ImageStream = streamRexInfo.Stream;
            mediaHistoryItem.Source = "";
            mediaHistoryItem.Title = "RecentPlay";
            mediaHistoryItem.PlayerContext.Add("key", "value");
            MediaHistory.Instance.WriteRecentPlay(mediaHistoryItem);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            var mediaHistoryItem = new MediaHistoryItem();
            StreamResourceInfo streamRexInfo = Application.GetResourceStream(new Uri("album.JPG", UriKind.Relative));
            mediaHistoryItem.ImageStream = streamRexInfo.Stream;
            mediaHistoryItem.Source = ""; 
            mediaHistoryItem.Title = "MediaHistoryNew";
            MediaHistory.Instance.WriteAcquiredItem(mediaHistoryItem);
        }

        bool _historyItemLaunch = false;            // Indicates if the app was lauched from a MediaHistoryItem.
        const String _playSongKey = "playSong";     // Key for MediaHistoryItem key-value pair.
        Song _playingSong = null;

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            MediaLibrary library = new MediaLibrary();
            
            if (NavigationContext.QueryString.ContainsKey(_playSongKey))
            {
                // We were launched from a history item.
                // Change _playingSong even if something was already playing 
                // because the user directly chose a song history item.

                // Use the navigation context to find the song by name.
                String songToPlay = NavigationContext.QueryString[_playSongKey];

                foreach (Song song in library.Songs)
                {
                    if (0 == String.Compare(songToPlay, song.Name))
                    {
                        _playingSong = song;
                        break;
                    }
                }

                // Set a flag to indicate that we were started from a 
                // history item and that we should immediately start 
                // playing the song once the UI has finished loading.
                _historyItemLaunch = true;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_historyItemLaunch)
            {
                // We were launched from a history item, start playing the song.
                if (_playingSong != null)
                {
                    MediaPlayer.Play(_playingSong);
                }
            }
        }
    }
}