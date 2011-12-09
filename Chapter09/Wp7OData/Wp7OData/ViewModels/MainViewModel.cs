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

        public void LoadData() {
            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(@"http://localhost:32026/Northwind/Northwind.svc/Categories/");
            request.Method = "GET";
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);

        }

        private void ReadCallback(IAsyncResult result) {

            Deployment.Current.Dispatcher.BeginInvoke(() => {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;

                XNamespace nsBase = "http://localhost:32026/Northwind/Northwind.svc/";

                XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
                XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
                XNamespace atom = "http://www.w3.org/2005/Atom";

                if (response.StatusCode == HttpStatusCode.OK) {
                    var xdoc = XDocument.Load(response.GetResponseStream());
                    foreach (var entity in xdoc.Descendants(atom + "entry")) {
                        var properties = entity.Element(atom + "content")
                                               .Element(m + "properties");

                        var category = new CategoryModel() {
                            Id = Convert.ToInt32(properties.Element(d + "CategoryID").Value),
                            CategoryName = properties.Element(d + "CategoryName").Value,
                            Description = properties.Element(d + "Description").Value
                        };
                        Items.Add(category);
                    }
                }
                else {
                    MessageBox.Show("Exception");
                }
            });

        }

    }
}