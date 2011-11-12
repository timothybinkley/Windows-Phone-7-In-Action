using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

using Microsoft.Phone.Net.NetworkInformation;
using SystemNet = System.Net.NetworkInformation;


namespace NetworkInfo {
    public class MainViewModel : INotifyPropertyChanged {
        public MainViewModel() {            
            this.SystemNetInfo = new ObservableCollection<ItemViewModel>();
            this.PhoneNetInfo = new ObservableCollection<ItemViewModel>();
            this.DeviceInfo = new ObservableCollection<ItemViewModel>();
        }        

        public ObservableCollection<ItemViewModel> SystemNetInfo { get; private set; }
        public ObservableCollection<ItemViewModel> PhoneNetInfo { get; private set; }
        public ObservableCollection<ItemViewModel> DeviceInfo { get; private set; }

        public bool IsDataLoaded {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData() {

            LoadSystemNetworkInformation();

            LoadPhoneNetworkInformation();

            LoadDeviceInformation();

            this.IsDataLoaded = true;
        }
        
        private void LoadPhoneNetworkInformation() {
            var netInterfaceType = NetworkInterface.NetworkInterfaceType;
            this.PhoneNetInfo.Add(new ItemViewModel() { LineOne = "network type", LineTwo = netInterfaceType.ToString().ToLower() });
            this.PhoneNetInfo.Add(new ItemViewModel() { LineOne = "network status", LineTwo = NetworkInterface.GetIsNetworkAvailable() ? "On" : "Off" });
        }

        private void LoadSystemNetworkInformation() {
            this.SystemNetInfo.Add(new ItemViewModel() { LineOne = "network status", LineTwo = SystemNet.NetworkInterface.GetIsNetworkAvailable() ? "On" : "Off" });
            SystemNet.NetworkChange.NetworkAddressChanged += (__, e) => {
                this.SystemNetInfo.Clear();
                this.SystemNetInfo.Add(new ItemViewModel() { LineOne = "network status", LineTwo = SystemNet.NetworkInterface.GetIsNetworkAvailable() ? "On" : "Off" });
            };
        }


        private void LoadDeviceInformation() {
            this.DeviceInfo.Add(new ItemViewModel() { LineOne = "cellular operator", LineTwo = DeviceNetworkInformation.CellularMobileOperator.ToLower() });
            this.DeviceInfo.Add(new ItemViewModel() { LineOne = "data enabled", LineTwo = DeviceNetworkInformation.IsCellularDataEnabled.ToString().ToLower() });
            this.DeviceInfo.Add(new ItemViewModel() { LineOne = "roaming enabled", LineTwo = DeviceNetworkInformation.IsCellularDataRoamingEnabled.ToString().ToLower() });
            this.DeviceInfo.Add(new ItemViewModel() { LineOne = "network status", LineTwo = DeviceNetworkInformation.IsNetworkAvailable.ToString().ToLower() });
            this.DeviceInfo.Add(new ItemViewModel() { LineOne = "wifi enabled", LineTwo = DeviceNetworkInformation.IsWiFiEnabled.ToString().ToLower() });

            //DeviceNetworkInformation.NetworkAvailabilityChanged += new EventHandler<NetworkNotificationEventArgs>(DeviceNetworkInformation_NetworkAvailabilityChanged);
        }

        //void DeviceNetworkInformation_NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e) {
        //    string change = string.Empty;
        //    switch (e.NotificationType) {
        //        case NetworkNotificationType.InterfaceConnected:
        //            change = "Connected to ";
        //            break;
        //        case NetworkNotificationType.InterfaceDisconnected:
        //            change = "Disconnected from ";
        //            break;
        //        case NetworkNotificationType.CharacteristicUpdate:
        //            change = "Characteristics changed for ";
        //            break;
        //        default:
        //            change = "Unknown change with ";
        //            break;
        //    }

        //    string changeInformation = String.Format(" {0} {1} {2} ({3})",
        //          DateTime.Now.ToString(), change, e.NetworkInterface.InterfaceName,
        //          e.NetworkInterface.InterfaceType.ToString());

        //    //Dispatcher.BeginInvoke(() => {
        //    //    NetworkAvailabilityChangedTextBlock.Text += changeInformation;
        //    //});
        //}  

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}