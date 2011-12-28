using System;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Shell;

namespace NotificationClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string CHANNEL_NAME = "PushNotificationChannel";
        HttpNotificationChannel channel;

        public MainPage()
        {
            InitializeComponent();
            SetupChannel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            notificationMessage.Text = string.Format("Launched with Uri:\n{0}", e.Uri);
        }

        private void copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(channelUri.Text);
        }

        void SetupChannel()
        {
            bool newChannel = false;
            channel = HttpNotificationChannel.Find(CHANNEL_NAME);
            if (channel == null)
            {
                channel = new HttpNotificationChannel(CHANNEL_NAME);
                newChannel = true;
            }

            channel.ConnectionStatusChanged += channel_ConnectionStatusChanged;
            channel.ChannelUriUpdated += channel_ChannelUriUpdated;
            channel.ErrorOccurred += channel_ErrorOccurred;
            channel.ShellToastNotificationReceived += channel_ShellToastNotificationReceived;

            if (newChannel)
            {
                channel.Open();
                channel.BindToShellTile();
                channel.BindToShellToast();
            }

            channelStatus.Text = channel.ConnectionStatus.ToString();
            
            if(channel.ChannelUri != null)
                channelUri.Text = channel.ChannelUri.ToString();
        }

        void channel_ConnectionStatusChanged(object sender, NotificationChannelConnectionEventArgs e)
        {
            Dispatcher.BeginInvoke(() => channelStatus.Text = e.ConnectionStatus.ToString());
        }

        void channel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Dispatcher.BeginInvoke(() => channelUri.Text = e.ChannelUri.ToString());
        }

        void channel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.GetType().Name + ": " + e.Message);
            Dispatcher.BeginInvoke(() => MessageBox.Show(e.Message, e.GetType().Name, MessageBoxButton.OK));
        }

        void channel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            string title, content, parameter;

            e.Collection.TryGetValue("wp:Text1", out title);
            e.Collection.TryGetValue("wp:Text2", out content);
            e.Collection.TryGetValue("wp:Param", out parameter);

            string message = string.Format("Toast notification received.\nTitle: {0}\nContent: {1}\nParameter: {2}\n\n{3}",
                title, content, parameter, DateTime.Now);

            Dispatcher.BeginInvoke(() => notificationMessage.Text = message);
        }

        private void updateTile_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = tileDefaultImage.IsChecked.Value ? "Background.png" : tileBlueImage.IsChecked.Value ? "Blue.jpg" : "Green.jpg";
            string backImagePath = tileBackNoImage.IsChecked.Value ? "" : tileBackBlueImage.IsChecked.Value ? "Blue.jpg" : "Green.jpg";
            int badgeCount;
            Int32.TryParse(tileBadgeCount.Text, out badgeCount);

            Uri tileUri = new Uri("/", UriKind.Relative);
            foreach (ShellTile tile in ShellTile.ActiveTiles)
            {
                if (tile.NavigationUri == tileUri)
                {
                    // if the tile was found, update it
                    StandardTileData tileData = new StandardTileData
                    {
                        BackgroundImage = new Uri(imagePath, UriKind.Relative),
                        Count = badgeCount,
                        Title = tileTitle.Text,
                    
                        BackBackgroundImage = new Uri(backImagePath, UriKind.Relative),
                        BackTitle = tileBackTitle.Text,
                        BackContent = tileBackContent.Text,
                    };
                    tile.Update(tileData);
                    break;
                }
            }
        }
    }
}