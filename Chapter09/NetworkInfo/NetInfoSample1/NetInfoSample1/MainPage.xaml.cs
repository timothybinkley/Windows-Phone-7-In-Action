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
using Phone = Microsoft.Phone.Net.NetworkInformation;

namespace NetInfoSample1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ShowNetwork();

            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(NetworkChange_NetworkAddressChanged);
        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            ShowNetwork();
        }

        private void ShowNetwork()
        {
            try
            {
                var netInterfaceType = Phone.NetworkInterface.NetworkInterfaceType;

                networkTypeTextblock.Text = string.Format("Network Status : {0}", netInterfaceType);

                if (netInterfaceType == Phone.NetworkInterfaceType.None)
                {
                    netIndicatorEllipse.Fill = new SolidColorBrush(Colors.Red);
                    testConnectionTextblock.IsEnabled = false;
                }

                networkStatusTextblock.Text = string.Format("Network Status : {0}", Phone.NetworkInterface.GetIsNetworkAvailable() ? "Online" : "Off");
            }
            catch (Exception ex)
            {
                networkStatusTextblock.Text = string.Format("{0} using NetworkInterface:\r\n{1}", ex.GetType().Name, ex.Message);
            }
        }



        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("Yah!! Got it!");
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        private void testConnectionTextblock_Click(object sender, RoutedEventArgs e)
        {
            var client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri("http://microsoft.com"));
        }
    }
}