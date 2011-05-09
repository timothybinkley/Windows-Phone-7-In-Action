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
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace PhotoChooserTaskSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        MediaElement mediaElement = new MediaElement();
        private void OpenPhoto_Click(object sender, RoutedEventArgs e)
        {
            var phoneChooserTask = new PhotoChooserTask();
            phoneChooserTask.ShowCamera = true;
            phoneChooserTask.Completed += new EventHandler<PhotoResult>(phoneChooserTask_Completed);
            phoneChooserTask.Show();
        }


        void phoneChooserTask_Completed(object sender, PhotoResult e)
        {

            if (e.TaskResult == TaskResult.OK)
            {
                var image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
                imageControl.Source = image;
            }
        }
    }
}