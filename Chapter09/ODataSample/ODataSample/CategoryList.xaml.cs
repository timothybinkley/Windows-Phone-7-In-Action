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
using System.Xml.Linq;

namespace ODataSample {
    public partial class CategoryList : UserControl {
        public CategoryList() {
            InitializeComponent();
            LoadData();
        }
        private void LoadData() {
            CategoryListbox.Items.Clear();

            HttpWebRequest request =
               (HttpWebRequest)HttpWebRequest.Create(@"http://localhost:32026/Northwind/Northwind.svc/Categories/");
            request.Method = "GET";            
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
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

                        Dispatcher.BeginInvoke(() => {
                            CategoryListbox.Items.Add(category);
                        }
                        );
                    }
                }
                else {

                }

            }
            catch (Exception ex) {
                ShowMessageBox(ex.Message);
            }

        }

        private  void ShowMessageBox(string message) {
            Dispatcher.BeginInvoke(() => {
                MessageBox.Show(message);
            });
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e) {
            var category = ((Button)sender).Tag as CategoryModel;
            string url = string.Format(@"http://localhost:32026/Northwind/Northwind.svc/Categories({0})", category.Id );
            HttpWebRequest request =
               (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "DELETE";
            request.BeginGetResponse(new AsyncCallback(DeleteCallback), request);
        }

        private void DeleteCallback(IAsyncResult asynchronousResult) {
            try {

                //config.SetEntitySetAccessRule("*", EntitySetRights.All);
                //Change to 0 <add key="CacheTimeOut" value="0"/>
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = request.EndGetResponse(asynchronousResult) as HttpWebResponse;


                if (response.StatusCode == HttpStatusCode.NoContent) {
                    Dispatcher.BeginInvoke(() => {
                    LoadData();
                    });
                }
                else {

                }

            }
            catch (Exception ex) {
                ShowMessageBox(ex.Message);
            }

        }
    }
}
