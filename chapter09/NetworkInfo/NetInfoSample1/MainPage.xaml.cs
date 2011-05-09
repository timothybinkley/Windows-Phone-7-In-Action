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
using System.Net.NetworkInformation;
using System.ComponentModel;

using Phone = Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO;
using System.Xml;
using System.Windows.Media.Imaging;

namespace NetInfoSample1 {
    public partial class MainPage : PhoneApplicationPage {
        NetworkInterfaceType netInterfaceType;
        bool isNetworkAvailable;

        public MainPage() {
            InitializeComponent();
            DetectNetwork();
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(NetworkChange_NetworkAddressChanged);
        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e) {
            DetectNetwork();
        }

        private void DetectNetwork() {
            var worker = new BackgroundWorker();
            worker.DoWork += (_, __) => { GetNetworkInfo(); };
            worker.RunWorkerCompleted += (_, __) => {
                Deployment.Current.Dispatcher.BeginInvoke(() => ShowNetwork());
            };
            worker.RunWorkerAsync();
        }

        private void GetNetworkInfo() {
            netInterfaceType = Phone.NetworkInterface.NetworkInterfaceType;
            isNetworkAvailable = Phone.NetworkInterface.GetIsNetworkAvailable();
        }

        private void ShowNetwork() {
            try {
                networkTypeTextblock.Text = string.Format("Network Type : {0}", netInterfaceType);

                var isNetWorkTypeNone = (netInterfaceType == Phone.NetworkInterfaceType.None);
                netIndicatorEllipse.Fill = new SolidColorBrush(isNetWorkTypeNone ? Colors.Red : Color.FromArgb(0xFF, 56, 0xEF, 12));

                networkStatusTextblock.Text = string.Format("Network Status : {0}", isNetworkAvailable ? "Online" : "Off");
            }
            catch (Exception ex) {
                networkStatusTextblock.Text = string.Format("{0} using NetworkInterface:\r\n{1}", ex.GetType().Name, ex.Message);
            }
        }

        private void testConnectionButton_Click(object sender, RoutedEventArgs e) {
            var client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(new Uri("http://manning.com/perga/perga_cover150.jpg"));

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri("http://manning.com"));

            if (!client.IsBusy) {
                client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
                client.OpenReadAsync(new Uri("http://manning.com/skeet2/skeet2_cover150.jpg"));
            }


            //HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(new Uri("http://manning.com/perga/perga_cover150.jpg"));
            //webrequest.Method = "GET";
            //webrequest.BeginGetResponse(new AsyncCallback(ReadCallback), webrequest);


        }

        //private void ReadCallback(IAsyncResult asynchronousResult) {
        //    try {
        //        HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
        //        HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

        //        Stream stream = response.GetResponseStream();
        //        Dispatcher.BeginInvoke(() => {
        //            var bitmap = new BitmapImage();
        //            bitmap.SetSource(stream);
        //            imageControl.Source = bitmap;
        //        });
        //    }
        //    catch (Exception ex) {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
            if (e.Error == null) {
                MessageBox.Show("Yah!! Got it!");
            }
            else {
                MessageBox.Show(e.Error.Message);
            }
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            if (e.Error == null) {

            }
            else {
                MessageBox.Show(e.Error.Message);
            }
        }


    }
}

