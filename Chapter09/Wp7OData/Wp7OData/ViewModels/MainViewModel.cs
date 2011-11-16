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


namespace Wp7OData {
    public class MainViewModel : INotifyPropertyChanged {
        
        Dispatcher dispatcher;

        public MainViewModel(Dispatcher dispatcher) {
            this.dispatcher = dispatcher;
            this.Items = new ObservableCollection<CategoryModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<CategoryModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty {
            get {
                return _sampleProperty;
            }
            set {
                if (value != _sampleProperty) {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData() {
            HttpWebRequest request =
             (HttpWebRequest)HttpWebRequest.Create(@"http://localhost:32026/Northwind/Northwind.svc/Categories/");
            request.Method = "GET";            
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
            this.IsDataLoaded = true;
        }

        private void ReadCallback(IAsyncResult asynchronousResult) {
            try {

                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = request.EndGetResponse(asynchronousResult) as HttpWebResponse;

                //Namesapces
                //xml:base="http://localhost:32026/Northwind/Northwind.svc/" 
                XNamespace nsBase = "http://localhost:32026/Northwind/Northwind.svc/";

                //xmlns:d="http://schemas.microsoft.com/ado/2007/08/dataservices" 
                XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";

                //xmlns:m="http://schemas.microsoft.com/ado/2007/08/dataservices/metadata" 
                XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

                //xmlns="http://www.w3.org/2005/Atom
                XNamespace atom = "http://www.w3.org/2005/Atom";

                if (response.StatusCode == HttpStatusCode.OK) {
                    var xdoc = XDocument.Load(response.GetResponseStream());
                    foreach (var entity in xdoc.Descendants(atom + "entry")) {
                        var properties = entity.Element(atom + "content").Element(m + "properties");
                        var category = new CategoryModel() {
                            Id = Convert.ToInt32(properties.Element(d + "CategoryID").Value),
                            Name = properties.Element(d + "CategoryName").Value,
                            Description = properties.Element(d + "Description").Value,
                        };                        
                        Items.Add(category);
                    }
                }
                else {

                }
                                
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}