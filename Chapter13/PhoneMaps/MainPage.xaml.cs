using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows;

namespace PhoneMaps
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void mapTask_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(departureTerm.Text))
            {
                MessageBox.Show("Please enter a start location.");
                return;
            }

            var task = new BingMapsTask
            {
                // Center = ...
                SearchTerm = departureTerm.Text,
                ZoomLevel = 15,
            };
            task.Show();
        }

        private void directionTask_Click(object sender, EventArgs e)
        {
            LabeledMapLocation start = null;
            LabeledMapLocation end = null;

            if (!string.IsNullOrEmpty(departureTerm.Text))
                start = new LabeledMapLocation { Label = departureTerm.Text };

            if (!string.IsNullOrEmpty(destinationTerm.Text))
                end = new LabeledMapLocation { Label = destinationTerm.Text };

            if (start == null && end == null)
            {
                MessageBox.Show("Please enter start and/or end locations.");
                return;
            }

            var task = new BingMapsDirectionsTask { Start = start, End = end };
            task.Show();
        }

        private void findMe_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapAndDirections.xaml", UriKind.Relative));
        }

        private void directions_Click(object sender, EventArgs e)
        {
            string uri = string.Format("/MapAndDirections.xaml?departure={0}&destination={1}", departureTerm.Text, destinationTerm.Text);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}