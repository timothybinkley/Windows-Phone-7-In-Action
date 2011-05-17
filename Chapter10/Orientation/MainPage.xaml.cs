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

namespace OrientationSupport
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            PageOrientation orientation = e.Orientation;

            if ((orientation & PageOrientation.Landscape) == PageOrientation.Landscape)
            {
                ApplicationTitle.Visibility = Visibility.Collapsed;
                PageTitle.Visibility = Visibility.Collapsed;
                MyPhoto.Height = 400;			
            }
            else if ((orientation & PageOrientation.Portrait) == PageOrientation.Portrait)
            {
                ApplicationTitle.Visibility = Visibility.Visible;
                PageTitle.Visibility = Visibility.Visible;
                MyPhoto.Height = 150;
            }
        }
    }
}