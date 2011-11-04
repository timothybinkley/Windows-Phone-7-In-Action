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
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using System.Xml;

namespace HttpSample {
    public partial class MainPage : PhoneApplicationPage {
        // Constructor
        public MainPage() {
            InitializeComponent();
            //http://www.odata.org/developers/odata-sdk#http://services.odata.org/(S(readwrite))/OData/OData.svc/Suppliers
            //http://www.odata.org/developers/protocols/uri-conventions#QueryStringOptions
            //different HTTP verbs (GET for reading, PUT for creating, POST for updating, and DELETE for deleting).

          


            Dispatcher.

            //request.BeginGetResponse(asynchronousResult => {
            //    HttpWebRequest request2 = (HttpWebRequest)asynchronousResult.AsyncState;
            //    HttpWebResponse response = (HttpWebResponse)request2.EndGetResponse(asynchronousResult);
            //    if (((HttpWebResponse)response).StatusCode == HttpStatusCode.Created /* 201 */ ) {
            //        Console.WriteLine(
            //          "Created category @ Location= {0}", response.Headers["Location"]);
            //    }
            //    else {
            //        Console.WriteLine("Category creation failed");
            //    }
            //}, request);

        }
        static void CreateCategory(Action complete) {           
            XNamespace ds = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace dsmd = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            var content =
              new XElement(dsmd + "properties",
                new XElement(ds + "CategoryName", "Dessert Topping"),
                new XElement(ds + "Description", "Toppings for desserts")
              );

            
            XNamespace atom = "http://www.w3.org/2005/Atom";
            var entry =
              new XElement(atom + "entry",
                new XElement(atom + "title", "A new Northwind category"),
                new XElement(atom + "id", string.Format("urn:uuid:{0}", Guid.NewGuid())),
                new XElement(atom + "updated", DateTime.Now),
                new XElement(atom + "author",
                new XElement(atom + "name", "Chris Sells")),
                new XElement(atom + "content",
                  new XAttribute("type", "application/xml"),
                  content)
              );

            // Send the HTTP request
            HttpWebRequest request =
             (HttpWebRequest)HttpWebRequest.Create(@"http://localhost:32026/Northwind/Northwind.svc/Categories/");
            request.Method = "POST";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            request.BeginGetRequestStream(getRequestResult => {
                HttpWebRequest request1 = (HttpWebRequest)getRequestResult.AsyncState;
                var stream = request1.EndGetRequestStream(getRequestResult);
                using (var writer = XmlWriter.Create(stream)) {
                    entry.WriteTo(writer);        
                }
                complete.Invoke();
            }, request);
        }

        private static void ReadCallback(IAsyncResult asynchronousResult) {
            try {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

                XNamespace atom = "http://www.w3.org/2005/Atom";

                var xdoc = XDocument.Load(response.GetResponseStream());
                foreach (var entity in xdoc.Descendants(atom + "entry")) {
                    var id = entity.Elements(atom + "id");
                    var title = entity.Elements(atom + "title");
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }

    }
}