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
using Microsoft.Phone.Net.NetworkInformation;

namespace NetInfoSample1 {
    public partial class MainPage : PhoneApplicationPage {
        // Constructor
        public MainPage() {
            InitializeComponent();

            UsingDeviceInformationAPI();

            //NetworkInterface.GetIsNetworkAvailable();
        }

        void UsingDeviceInformationAPI() {
            CellularMobileOperatorTextBlock.Text = string.Format("CellularMobileOperator : {0}", DeviceNetworkInformation.CellularMobileOperator);
            IsCellularDataEnabledTextBlock.Text = string.Format("IsCellularDataEnabled : {0}", DeviceNetworkInformation.IsCellularDataEnabled);
            IsCellularDataRoamingEnabledTextBlock.Text = string.Format("IsCellularDataRoamingEnabled : {0}", DeviceNetworkInformation.IsCellularDataRoamingEnabled);
            IsNetworkAvailableTextBlock.Text = string.Format("IsNetworkAvailable : {0}", DeviceNetworkInformation.IsNetworkAvailable);
            IsWiFiEnabledTextBlock.Text = string.Format("IsWiFiEnabled : {0}", DeviceNetworkInformation.IsWiFiEnabled);

            //DeviceNetworkInformation.ResolveHostNameAsync(new DnsEndPoint("http://google.com", 0), (b) => {
            //    Console.Write(b.HostName);
            //}, null);

            DeviceNetworkInformation.NetworkAvailabilityChanged += new EventHandler<NetworkNotificationEventArgs>(DeviceNetworkInformation_NetworkAvailabilityChanged);
        }
        void DeviceNetworkInformation_NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e) {
            string change = string.Empty;
            switch (e.NotificationType) {
                case NetworkNotificationType.InterfaceConnected:
                    change = "Connected to ";
                    break;
                case NetworkNotificationType.InterfaceDisconnected:
                    change = "Disconnected from ";
                    break;
                case NetworkNotificationType.CharacteristicUpdate:
                    change = "Characteristics changed for ";
                    break;
                default:
                    change = "Unknown change with ";
                    break;
            }

            string changeInformation = String.Format(" {0} {1} {2} ({3})",
                  DateTime.Now.ToString(), change, e.NetworkInterface.InterfaceName,
                  e.NetworkInterface.InterfaceType.ToString());
            
            Dispatcher.BeginInvoke(() => {
                NetworkAvailabilityChangedTextBlock.Text += changeInformation;
            });  
        }

        
        //private void ShowNetwork() {
        //    try {
        //        var netInterfaceType = Phone.NetworkInterface.NetworkInterfaceType;

        //        networkTypeTextblock.Text = string.Format("Network Status : {0}", netInterfaceType);

        //        if (netInterfaceType == Phone.NetworkInterfaceType.None) {
        //            netIndicatorEllipse.Fill = new SolidColorBrush(Colors.Red);                   
        //        }

        //        networkStatusTextblock.Text = string.Format("Network Status : {0}", Phone.NetworkInterface.GetIsNetworkAvailable() ? "Online" : "Off");
        //    }
        //    catch (Exception ex) {
        //        networkStatusTextblock.Text = string.Format("{0} using NetworkInterface:\r\n{1}", ex.GetType().Name, ex.Message);
        //    }
        //}
    }
}