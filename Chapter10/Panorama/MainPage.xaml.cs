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
using System.IO.IsolatedStorage;

namespace Panorama
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            int selectedIndex;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("selection", out selectedIndex))
            {
                panorama.DefaultItem = panorama.Items[selectedIndex];
            }
        }

        private void panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["selection"] = panorama.SelectedIndex;
        }

    }
}