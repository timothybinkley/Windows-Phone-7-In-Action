using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using PhoneMaps.GeocodeService;
using PhoneMaps.RouteService;

namespace PhoneMaps
{
    public partial class MapAndDirections : PhoneApplicationPage
    {
        GeoCoordinateWatcher locationService;
        GeocodeResult departureGeocode;
        GeocodeResult destinationGeocode;

        public MapAndDirections()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (locationService != null)
            {
                locationService.Stop();
                locationService.PositionChanged -= locationService_PositionChanged;
                locationService.Dispose();
                locationService = null;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            locationService = new GeoCoordinateWatcher();
            locationService.PositionChanged += locationService_PositionChanged;
            locationService.Start();

            if (NavigationContext.QueryString.ContainsKey("departure") &&
                NavigationContext.QueryString.ContainsKey("destination"))
            {
                GeocodeRequest geocodeRequest = new GeocodeRequest()
                {
                    Credentials = new Credentials
                    {
                        ApplicationId = "your developer key here"
                    }
                };

                GeocodeServiceClient geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                geocodeService.GeocodeCompleted += geocodeService_GeocodeCompleted;

                geocodeRequest.Query = NavigationContext.QueryString["departure"];
                geocodeService.GeocodeAsync(geocodeRequest, 0);

                geocodeRequest.Query = NavigationContext.QueryString["destination"];
                geocodeService.GeocodeAsync(geocodeRequest, 1);
            }
        }

        private void locationService_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (NavigationContext.QueryString.Count == 0)
                mapControl.Center = e.Position.Location;

            mapLayer.Children.Clear();

            Pushpin pushPin = CreatePushpin(e.Position.Location, Colors.Blue, "You");

            mapLayer.AddChild(pushPin, pushPin.Location);
        }

        private Pushpin CreatePushpin(GeoCoordinate location, Color color, string label = null)
        {
            StackPanel content = new StackPanel { Height = 30, Orientation = System.Windows.Controls.Orientation.Horizontal };

            content.Children.Add(new Ellipse { Height = 20, Width = 20, Fill = new SolidColorBrush(color) });

            content.Children.Add(new TextBlock { Text = label });

            return new Pushpin { Location = location, Content = content };
        }

        private void geocodeService_GeocodeCompleted(object sender, GeocodeCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null ||
                e.Result.ResponseSummary.StatusCode != GeocodeService.ResponseStatusCode.Success)
            {
                MessageBox.Show("Unable to complete the Geocode request");
                return;
            }

            GeocodeResponse response = e.Result;

            if ((int)e.UserState == 0)
            {
                departureGeocode = response.Results[0];
            }
            else
            {
                destinationGeocode = response.Results[0];
            }

            if (departureGeocode != null && destinationGeocode != null)
                CalculateRoute();
        }

        private void CalculateRoute()
        {
            RouteRequest routeRequest = new RouteRequest
            {
                Credentials = new Credentials
                {
                    ApplicationId = "your developer key here"
                },
                Options = new RouteOptions
                {
                    RoutePathType = RoutePathType.Points
                },
                Waypoints = new ObservableCollection<Waypoint>(),
            };
            routeRequest.Waypoints.Add(CreateWaypoint(departureGeocode));
            routeRequest.Waypoints.Add(CreateWaypoint(destinationGeocode));
           
            RouteServiceClient routeService = new RouteServiceClient("BasicHttpBinding_IRouteService");
            routeService.CalculateRouteCompleted += routeService_CalculateRouteCompleted;
            routeService.CalculateRouteAsync(routeRequest);
        }

        private Waypoint CreateWaypoint(GeocodeResult geocode)
        {
            return new Waypoint
            {
                Description = geocode.DisplayName,
                Location = new Location
                {
                    Longitude = geocode.Locations[0].Longitude,
                    Latitude = geocode.Locations[0].Latitude
                }
            };
        }

        private void routeService_CalculateRouteCompleted(object sender, CalculateRouteCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null ||
                e.Result.ResponseSummary.StatusCode != RouteService.ResponseStatusCode.Success)
            {
                MessageBox.Show("Unable to complete the CalculateRoute request");
                return;
            }

            RouteResult result = e.Result.Result;
            ObservableCollection<Location> points = result.RoutePath.Points;

            var startPin = CreatePushpin((GeoCoordinate)points.First(), Colors.Green);
            routeLayer.AddChild(startPin, startPin.Location);

            var endPin = CreatePushpin((GeoCoordinate)points.Last(), Colors.Red);
            routeLayer.AddChild(endPin, endPin.Location);

            routeLine.Locations = new LocationCollection();
            foreach (Location point in points)
            {
                routeLine.Locations.Add((GeoCoordinate)point);
            }

            mapControl.SetView(LocationRect.CreateLocationRect(routeLine.Locations));

            DisplayItinerary(result.Legs[0].Itinerary);
        }

        private void DisplayItinerary(ObservableCollection<ItineraryItem> itinerary)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in itinerary)
            {
                string cleanedText = Regex.Replace(item.Text, "<.*?>", string.Empty);
                builder.AppendLine(cleanedText);
                if (item.ManeuverType != ManeuverType.ArriveFinish)
                {
                    builder.AppendFormat("Distance: {0}\nTime: {1}\n\n",
                        item.Summary.Distance, item.Summary.TimeInSeconds);
                }
            }
            directionsBlock.Text = builder.ToString();
        }
    }
}