using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NotificationSimulator.Services;

namespace NotificationSimulator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        private void SendToast_Click(object sender, RoutedEventArgs e) {
            var client = new NotificationServiceClient();
            client.SendToast(new Toast() { Text1 = ToastText1.Text, Text2 = ToastText2.Text, BackingInterval = ToastBackingInterval.ImmediateDelivery });
        }

        private void SendTile_Click(object sender, RoutedEventArgs e) {
            var client = new NotificationServiceClient();
            client.SendTitle(new Title() {
                Title1 = new TitleItem() { Title = TileTitle1.Text, Count = TileCount1.Text, BackgroundImagePath = TileImage1.Text },
                Title2 = new TitleItem() { Title = TileTitle2.Text, Count = TileCount2.Text, BackgroundImagePath = TileImage2.Text },
                BackingInterval = TitleBackingInterval.ImmediateDelivery
            });
        }

        private void SendRaw_Click(object sender, RoutedEventArgs e) {
            var client = new NotificationServiceClient();
            client.SendRaw(new Raw() { Text1 = Rawtext1.Text, Text2 = Rawtext2.Text, BackingInterval = RawBackingInterval.ImmediateDelivery });
        }
    }
}
