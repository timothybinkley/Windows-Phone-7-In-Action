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
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Wp7OData.Services;


namespace Wp7OData {
    public partial class DetailsPage : PhoneApplicationPage {
        // Constructor
        public DetailsPage() {
            InitializeComponent();
        }

        int index;
        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e) {            
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex)) {
                index = int.Parse(selectedIndex);
                if (!App.ViewModel.IsDataLoaded) {
                    App.ViewModel.LoadDataCompleted += (__, _e) => {
                        DataContext = App.ViewModel.Items[index];
                    };
                    App.ViewModel.LoadData();                    
                }
                
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e) {

            var shellTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("selectedItem=" + index.ToString()));

            if (shellTile != null) {
                return;
            }

            var category = DataContext as Category;

            // Create the tile data with some proeprties
            var tileData = new StandardTileData {
                Title = category.CategoryName,
                Count = 12,
                BackTitle = category.CategoryName,
                BackContent = category.Description,                
            };

            // create the tile at the start screen
            ShellTile.Create(new Uri("/DetailsPage.xaml?selectedItem=" + index.ToString(), UriKind.Relative), tileData);
        }
    }
}