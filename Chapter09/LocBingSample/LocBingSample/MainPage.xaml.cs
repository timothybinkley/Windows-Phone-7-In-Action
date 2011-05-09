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
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Core;
using System.Device.Location;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls.Maps.Platform;

namespace LocBingSample {
    public partial class MainPage : PhoneApplicationPage {
        const string IMAGE_FOLDERNAME = "Images\\";

        IEnumerator<Country> countriesEnumerator;
        public MainPage() {
            InitializeComponent();

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
        }

        private IEnumerator<Country> GetCountriesEnumerator() {
            var countries = new List<Country>
            {
                new Country { Name = "Argentina",  Location = new GeoCoordinate(-35, -64) , ImagePath = IMAGE_FOLDERNAME + "Argentina-flag.png"},
                new Country { Name = "Bolivia",  Location = new GeoCoordinate (-17, -65) , ImagePath = IMAGE_FOLDERNAME + "Bolivia-flag.png"},
                new Country { Name = "Brazil",  Location = new GeoCoordinate(-10,-55) , ImagePath = IMAGE_FOLDERNAME + "Brazil-flag.png"},
                new Country { Name = "Chile",  Location = new GeoCoordinate(-30, -71) , ImagePath = IMAGE_FOLDERNAME + "Chile-flag.png"},
                new Country { Name = "Colombia",  Location = new GeoCoordinate(4, -72) , ImagePath = IMAGE_FOLDERNAME + "Colombia-flag.png"},
                new Country { Name = "Falkland Islands",  Location = new GeoCoordinate (-51.75,  -59)  , 
                    ImagePath = IMAGE_FOLDERNAME + "Falkland-Islands-flag.png"},
                new Country { Name = "French Guiana",  Location = new GeoCoordinate(4, -53)  , ImagePath = IMAGE_FOLDERNAME + "French-Guiana-flag.png"},
                new Country { Name = "Guyana",  Location = new GeoCoordinate(5,-59)   , ImagePath = IMAGE_FOLDERNAME + "Guyana-flag.png"},
                new Country { Name = "Paraguay",  Location = new GeoCoordinate(-23,-58)   , ImagePath = IMAGE_FOLDERNAME + "Paraguay-flag.png"},
                new Country { Name = "South Georgia and the South Sandwich Islands",  
                    Location = new GeoCoordinate ( -54.5,-37), 
                    ImagePath = IMAGE_FOLDERNAME + "South-Georgia-and-the-South-Sandwich-Islands-flag.png"},
                new Country { Name = "Peru",  Location = new GeoCoordinate (-10,-76)   , ImagePath = IMAGE_FOLDERNAME + "Peru-flag.png"},
                new Country { Name = "Suriname",  Location = new GeoCoordinate (4,  -56)   , ImagePath = IMAGE_FOLDERNAME + "Suriname-flag.png"}
            };

            return countries.GetEnumerator();
        }

        private void aerialModeButton_Click(object sender, RoutedEventArgs e) {
            var isAerialMode = (aerialModeButton.Content.ToString() == "Aerial");
            aerialModeButton.Content = isAerialMode ? "Normal" : "Aerial";
            mapControl.Mode = (isAerialMode ? (MapMode)new AerialMode() : (MapMode)new RoadMode());


        }

        private void zoomInButton_Click(object sender, RoutedEventArgs e) {
            mapControl.ZoomLevel += 1; //ZoomBarVisibility="Visible"        
        }

        private void zoomOutButton_Click(object sender, RoutedEventArgs e) {
            mapControl.ZoomLevel -= 1;

        }


        private void SouthAmericaButton_Click(object sender, RoutedEventArgs e) {
            mapControl.Center = new GeoCoordinate(-14.6047222222, -57.6561111111);             

            countriesEnumerator = GetCountriesEnumerator();

            countriesEnumerator.MoveNext();

            AddPushpin(countriesEnumerator.Current.Name,
                countriesEnumerator.Current.Location.Latitude,
                countriesEnumerator.Current.Location.Longitude,
                countriesEnumerator.Current.ImagePath);
        }

        private void AddPushpin(string name, double latitude, double longitude, string imagePath) {
            Pushpin pin = new Pushpin() { Name = name, Location = new GeoCoordinate(latitude, longitude) };
            pin.Content = new Image() {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Relative)),
                Height = 54,
                Width = 81
            };
            pin.Opacity = 0;
            mapControl.Children.Add(pin);

            FadeInAnimation.Stop();
            FadeInAnimation.SetValue(Storyboard.TargetNameProperty, pin.Name);

            FadeOutAnimation.Stop();
            FadeOutAnimation.SetValue(Storyboard.TargetNameProperty, pin.Name);

            FadeInAnimation.Begin();
        }

        private void FadeInAnimation_Completed(object sender, EventArgs e) {
            FadeOutAnimation.Begin();
        }

        private void FadeOutAnimation_Completed(object sender, EventArgs e) {
            if (countriesEnumerator.MoveNext()) {
                AddPushpin(countriesEnumerator.Current.Name,
                    countriesEnumerator.Current.Location.Latitude,
                    countriesEnumerator.Current.Location.Longitude,
                    countriesEnumerator.Current.ImagePath);
            }
        }

        GeoCoordinateWatcher watcher;
        Pushpin me = new Pushpin();
        private void startButton_Click(object sender, RoutedEventArgs e) {

            if (startButton.IsChecked.Value) {
                watcher.MovementThreshold = 20;
                watcher.Start();
                startButton.Content = "Stop";
                mapControl.Children.Add(me);
            }
            else {
                startButton.Content = "Start";
                mapControl.Children.Remove(me);
                watcher.Stop();
            }

        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e) {
            Deployment.Current.Dispatcher.BeginInvoke(() => PositionChanged(e));
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e) {
            Deployment.Current.Dispatcher.BeginInvoke(() => StatusChanged(e));
        }

        void PositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e) {
            Location currentLocation = new Location() {
                Latitude = e.Position.Location.Latitude,
                Longitude = e.Position.Location.Longitude
            };

            mapControl.Center = currentLocation;
            mapControl.ZoomLevel = 16.0f;
            me.Location = currentLocation;
        }


        void StatusChanged(GeoPositionStatusChangedEventArgs e) {
            switch (e.Status) {
                case GeoPositionStatus.Disabled:
                    statusTextblock.Text = "The Geo service is disabled.";
                    break;
                case GeoPositionStatus.Initializing:
                    statusTextblock.Text = "The Geo service is initializing..";
                    break;
                case GeoPositionStatus.NoData:
                    statusTextblock.Text = "No Data";
                    break;
                case GeoPositionStatus.Ready:
                    statusTextblock.Text = "Ready";
                    break;
            }
        }

    }

    public class Country {
        public string Name { get; set; }
        public GeoCoordinate Location { get; set; }
        public string ImagePath { get; set; }
    }
}