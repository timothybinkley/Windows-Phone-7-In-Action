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
using System.IO;
using System.Windows.Media.Imaging;

namespace HttpSample {
    public partial class MainPage : PhoneApplicationPage {
        public MainPage() {
            InitializeComponent();
        }

        //private void getImageButton_Click(object sender, RoutedEventArgs e) {
        //    HttpWebRequest webrequest = (HttpWebRequest)WebRequest
        //        .Create(new Uri("http://manning.com/perga/perga_cover150.jpg"));
        //    webrequest.Method = "GET";
        //    webrequest.BeginGetResponse(
        //        new AsyncCallback(ReadCallback), webrequest);
        //}

        //private void ReadCallback(IAsyncResult result) {
        //    HttpWebRequest request = (HttpWebRequest)result.AsyncState;
        //    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

        //    Stream stream = response.GetResponseStream();
        //    Dispatcher.BeginInvoke(() => {
        //        var bitmap = new BitmapImage();
        //        bitmap.SetSource(stream);
        //        imageControl.Source = bitmap;
        //    });
        //}




        private void getImageButton_Click(object sender, RoutedEventArgs e) {
            var client = new WebClient();
            client.OpenReadCompleted += new
                OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(
                new Uri("http://manning.com/perga/perga_cover150.jpg"));
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            if (e.Error == null) {
                Stream stream = e.Result;
                var bitmap = new BitmapImage();
                bitmap.SetSource(stream);
                imageControl.Source = bitmap;
            }
            else {
                MessageBox.Show(e.Error.Message);
            }
        }
    }
}