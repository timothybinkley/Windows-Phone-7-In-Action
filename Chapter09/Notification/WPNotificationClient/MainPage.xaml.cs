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
using System.IO.IsolatedStorage;
using Microsoft.Phone.Notification;
using WPNotificationClient.Services;
using System.Collections.ObjectModel;

namespace WPNotificationClient {
    public partial class MainPage : PhoneApplicationPage {
        // Constructor
        Guid _appId;
        HttpNotificationChannel _channel;
        private const string CHANNEL_NAME = "PushNotificationChannel";

        public MainPage() {
            InitializeComponent();

            if (IsolatedStorageSettings.ApplicationSettings.Contains("AppID")) {
                _appId = (Guid)IsolatedStorageSettings.ApplicationSettings["AppID"];
            }
            else {
                _appId = Guid.NewGuid();
                IsolatedStorageSettings.ApplicationSettings["AppID"] = _appId;
            }
        }

        void SetUpChannel() {
            _channel = HttpNotificationChannel.Find(CHANNEL_NAME);

            if (_channel == null) {                
                _channel = new HttpNotificationChannel(CHANNEL_NAME);
                _channel.ChannelUriUpdated += OnChannelUriUpdated;
                _channel.ErrorOccurred += OnErrorOccurred;
                _channel.Open();
            }
            else {                
                RegisterForNotifications();
            }
        }

        void RegisterForNotifications() {
            RegisterWithSubscriptionService();
            _channel.ShellToastNotificationReceived += OnShellToastNotificationReceived;
            _channel.HttpNotificationReceived += OnHttpNotificationReceived;
            _channel.ErrorOccurred += OnErrorOccurred;
        }

        void RegisterWithSubscriptionService() {
            var svc = new NotificationServiceClient();
            svc.SubscribeCompleted += (s, e) => {
                if (e.Error != null) {
                    
                }
            };
            svc.SubscribeAsync(_appId, _channel.ChannelUri.ToString());
        }
        
        void UnRegisterForNotifications() {
            _channel.HttpNotificationReceived -= OnHttpNotificationReceived;
            _channel.ShellToastNotificationReceived -= OnShellToastNotificationReceived;
            _channel.ErrorOccurred -= OnErrorOccurred;
        }

        void OnChannelUriUpdated(object sender, NotificationChannelUriEventArgs e) {
            _channel.ErrorOccurred -= OnErrorOccurred;
            _channel.ChannelUriUpdated -= OnChannelUriUpdated;

            _channel = HttpNotificationChannel.Find(CHANNEL_NAME);

            if (!_channel.IsShellTileBound) {
                Collection<Uri> uris = new Collection<Uri>();
                uris.Add(new Uri("http://chris.59north.com/"));
                _channel.BindToShellTile(uris);
            }

            if (!_channel.IsShellToastBound)
                _channel.BindToShellToast();

            RegisterForNotifications();

            
        }

        void OnErrorOccurred(object sender, NotificationChannelErrorEventArgs e) {
            
            //OnErrorOccurred(e.Message);
        }
        void OnHttpNotificationReceived(object sender, HttpNotificationEventArgs e) {
            //StreamReader sr = new StreamReader(e.Notification.Body);
            //OnRawMessageReceived(sr.ReadToEnd());
            //sr.Close();
        }
        void OnShellToastNotificationReceived(object sender, NotificationEventArgs e) {
            //ToastData td = new ToastData(e.Collection["wp:Text1"], e.Collection["wp:Text2"]);
            //OnToastReceived(td);
        }
    }
}