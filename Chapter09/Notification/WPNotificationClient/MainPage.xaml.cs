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
using System.IO;
using System.Xml.Linq;
using System.Text;

namespace WPNotificationClient {
    public partial class MainPage : PhoneApplicationPage {

        Guid _appId;
        HttpNotificationChannel _channel;
        private const string CHANNEL_NAME = "PushNotificationChannel";

        public MainPage() {
            InitializeComponent();

            //StreamReader sr = new StreamReader("<?xml version=\"1.0\" encoding=\"utf-8\"?><root><Value1>g<Value1><Value2>g<Value2></root>");
            //var doc = XDocument.Load(stream);

            if (IsolatedStorageSettings.ApplicationSettings.Contains("AppID")) {
                _appId = (Guid)IsolatedStorageSettings
                            .ApplicationSettings["AppID"];
            }
            else {
                _appId = Guid.NewGuid();
                IsolatedStorageSettings
                    .ApplicationSettings["AppID"] = _appId;
            }
            SetUpChannel();
        }

        void SetUpChannel() {
            _channel = HttpNotificationChannel.Find(CHANNEL_NAME);

            if (_channel == null) {
                _channel = new HttpNotificationChannel(CHANNEL_NAME);
                _channel.ChannelUriUpdated += OnChannelUriUpdated;
                _channel.Open();
            }
            else {
                _channel.ChannelUriUpdated += OnChannelUriUpdated;
                Subscribe();
            }
        }
        void Subscribe() {
            SubscribeEvents();
            var svc = new NotificationServiceClient();
            svc.SubscribeCompleted += (s, e) => {
                if (e.Error != null) {
                    MessageBox.Show(e.Error.Message);
                }
            };
            svc.SubscribeAsync(_appId, _channel.ChannelUri.ToString());
        }

        private void SubscribeEvents() {
            _channel.ErrorOccurred += OnErrorOccurred;
            _channel.ShellToastNotificationReceived += OnShellToastNotificationReceived;
            _channel.HttpNotificationReceived += OnHttpNotificationReceived;

            if (!_channel.IsShellTileBound) {
                _channel.BindToShellTile();
            }

            if (!_channel.IsShellToastBound) {
                _channel.BindToShellToast();
            }
        }




        void OnChannelUriUpdated(object sender, NotificationChannelUriEventArgs e) {
            SetUpChannel();
        }

        void OnErrorOccurred(object sender, NotificationChannelErrorEventArgs e) {
            MessageBox.Show(e.Message);
        }
        void OnHttpNotificationReceived(object sender, HttpNotificationEventArgs e) {
            StreamReader sr = new StreamReader(e.Notification.Body);
            var doc = XDocument.Load(sr);
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                RawText1Textblock.Text = doc.Descendants("Value1").First().Value;
                RawText1Textblock.Text = doc.Descendants("Value2").First().Value;
            });
            sr.Close();
        }
        void OnShellToastNotificationReceived(object sender, NotificationEventArgs e) {
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                ToastText1Textblock.Text = e.Collection["wp:Text1"];
                ToastText2Textblock.Text = e.Collection["wp:Text2"];
            });
        }
    }
}