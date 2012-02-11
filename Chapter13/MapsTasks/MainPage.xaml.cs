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

namespace MapsTasks
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

    }
}