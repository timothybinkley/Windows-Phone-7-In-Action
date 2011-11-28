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
using Wp7OData.ViewModels;
using System.Windows.Threading;
using System.IO;
using System.Globalization;


namespace Wp7OData {
    public class MainViewModel : INotifyPropertyChanged {
        public MainViewModel() {
            this.Items = new ObservableCollection<CategoryModel>();
        }

        public bool IsDataLoaded {
            get;
            private set;
        }

        private ObservableCollection<CategoryModel> items;

        public ObservableCollection<CategoryModel> Items {
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

        internal void LoadData() {
            var client = new ODataClient();
            client.LoadCategories((result) => Items = new ObservableCollection<CategoryModel>(result));
        }
    }
}