using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Net;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Threading;
using System.IO;
using System.Globalization;
using Wp7OData.Services;
using Microsoft.Phone.Shell;


namespace Wp7OData {
    public class MainViewModel : INotifyPropertyChanged {
        public MainViewModel() {
            this.Items = new ObservableCollection<Category>();
        }

        public bool IsDataLoaded {
            get;
            private set;
        }

        private ObservableCollection<Category> items;

        public ObservableCollection<Category> Items {
            get { return items; }
            set { 
                items = value;
                NotifyPropertyChanged("Items");
            }
        }
       

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public EventHandler LoadDataCompleted;

        public void LoadData() {
            var client = new Services.CategoryServiceClient();
            client.GetCategoriesCompleted += new EventHandler<Services.GetCategoriesCompletedEventArgs>(client_GetCategoriesCompleted);
            client.GetCategoriesAsync();
        }

        void client_GetCategoriesCompleted(object sender, Services.GetCategoriesCompletedEventArgs e) {
            if (e.Error == null) {
                Items = e.Result;
                if (LoadDataCompleted != null) {
                    LoadDataCompleted(this, null);
                }
                if (items != null && items.Count > 0) {
                    var category = items.First();
                    UpdateTitle(category);
                }                
            }
        }

        private static void UpdateTitle(Category category) {
            var tileToFind = ShellTile.ActiveTiles.FirstOrDefault();
            if (tileToFind != null) {
                var newTileData = new StandardTileData {
                    Title = category.CategoryName,
                    BackContent = category.Description
                };

                tileToFind.Update(newTileData);
            }
        }
    }
}