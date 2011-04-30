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

            if (orientation == PageOrientation.Landscape ||
                orientation == PageOrientation.LandscapeLeft ||
                orientation == PageOrientation.LandscapeRight)
            {
                VisualStateManager.GoToState(this, "Landscape", false);
                //ApplicationTitle.Visibility = Visibility.Collapsed;
                //PageTitle.Visibility = Visibility.Collapsed;

                //ContentPanel.RowDefinitions[2].Height = GridLength.Auto; 
                //ContentPanel.RowDefinitions[3].Height = GridLength.Auto;
                
                //MyPhoto.SetValue(Grid.RowProperty, 0);			
                //MyPhoto.SetValue(Grid.RowSpanProperty, 3);		
                //MyPhoto.SetValue(Grid.ColumnProperty, 0);		
                //MyPhoto.SetValue(Grid.ColumnSpanProperty, 3);	
                
                //MyPhotoLabel.SetValue(Grid.RowProperty, 3);		
                //MyPhotoLabel.SetValue(Grid.ColumnProperty, 1);	

            }
            else if (orientation == PageOrientation.Portrait ||
                orientation == PageOrientation.PortraitDown ||
                orientation == PageOrientation.PortraitUp)
            {
                VisualStateManager.GoToState(this, "Portrait", false);
            //    ApplicationTitle.Visibility = Visibility.Visible;
            //    PageTitle.Visibility = Visibility.Visible;
                
            //    ContentPanel.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);								
            //    ContentPanel.RowDefinitions[2].Height = GridLength.Auto;	
            //    ContentPanel.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);								
                
            //    MyPhoto.SetValue(Grid.RowProperty, 1);			
            //    MyPhoto.SetValue(Grid.RowSpanProperty, 1);		
            //    MyPhotoLabel.SetValue(Grid.ColumnProperty, 1);	
            //    MyPhotoLabel.SetValue(Grid.ColumnSpanProperty, 1);
                
            //    MyPhotoLabel.SetValue(Grid.RowProperty, 3);		
            //    MyPhotoLabel.SetValue(Grid.ColumnProperty, 1);		
            }
        }
    }
}