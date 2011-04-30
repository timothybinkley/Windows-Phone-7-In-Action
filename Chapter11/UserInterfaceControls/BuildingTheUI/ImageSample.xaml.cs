using System;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace UserInterfaceControls.BuildingTheUI
{
    public partial class ImageSample : PhoneApplicationPage
    {
        public ImageSample()
        {
            InitializeComponent();

            image3.Source = new BitmapImage(
                new Uri("http://www.wp7inaction.com/cover.png",
                UriKind.Absolute));

         
        }

    }
}