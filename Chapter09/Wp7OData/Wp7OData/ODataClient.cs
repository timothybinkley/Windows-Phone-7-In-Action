using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Wp7OData.ViewModels;
using System.Xml.Linq;

namespace Wp7OData {
    

    public class ODataAsyncEventArgs<T> where  T : class {
        public Action<T> SuccessAction { get; set; }
        public Action<Exception> ErrorAction { get; set; }
    }

    public interface IODataClient {
        void LoadCategories(Action<IList<CategoryModel>> complete);
        void UpdateCategories(CategoryModel model, Action successAction, Action errorAction);
        void SaveCategories(CategoryModel model, Action successAction, Action errorAction);
        void DeleteCategories(int categoryId, Action successAction, Action errorAction);

    }

    public class ODataClient : IODataClient {

        XNamespace nsBase = "http://localhost:32026/Northwind/Northwind.svc/";
        XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        XNamespace atom = "http://www.w3.org/2005/Atom";

        

        public void LoadCategories(Action<IList<CategoryModel>> complete) {

            LoadCompletedAction = complete;

            HttpWebRequest request =
               (HttpWebRequest)HttpWebRequest.Create(@"http://localhost:32026/Northwind/Northwind.svc/Categories/");
            request.Method = "GET";
            request.Accept = "application/atom+xml";            
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        Action<IList<CategoryModel>> LoadCompletedAction { get; set; }
        Action SuccessAction { get; set; }
        Action ErrorAction { get; set; }

        private void ReadCallback(IAsyncResult result) {

            Deployment.Current.Dispatcher.BeginInvoke(() => {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = request.EndGetResponse(result)
                                                as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK) {
                    var xdoc = XDocument.Load(response.GetResponseStream());

                    var items = new List<CategoryModel>();

                    foreach (var entity in xdoc.Descendants(atom + "entry")) {
                        var properties = entity.Element(atom + "content")
                                               .Element(m + "properties");

                        var category = new CategoryModel() {
                            Id = Convert.ToInt32(properties.Element(d + "CategoryID").Value),
                            Name = properties.Element(d + "CategoryName").Value,
                            Description = properties.Element(d + "Description").Value
                        };
                        items.Add(category);
                    }

                    LoadCompletedAction.Invoke(items);
                }
                else {
                    MessageBox.Show("Exception");
                }
            });

        }
        public void UpdateCategories(CategoryModel model, Action successAction, Action errorAction) {
            throw new NotImplementedException();
        }

        public void SaveCategories(CategoryModel model, Action successAction, Action errorAction) {
            SuccessAction = successAction;
            ErrorAction = errorAction;

            //HttpWebRequest request =
            //(HttpWebRequest)HttpWebRequest.Create(@"http://localhost:32026/Northwind/Northwind.svc/Categories/");
            //request.Method = "GET";
            //request.Accept = "application/atom+xml";
            //request.ContentType = "application/atom+xml;type=entry";
            //request.BeginGetResponse(new AsyncCallback(ReadCallback), request);

            //using (var writer = XmlWriter.Create(request.GetRequestStream())) {
            //    entry.WriteTo(writer);
            //}

            //WebResponse response = request.GetResponse();
            //if (((HttpWebResponse)response).StatusCode == HttpStatusCode.Created /* 201 */ ) {
            //    Console.WriteLine(
            //      "Created category @ Location= {0}", response.Headers["Location"]);
            //}
            //else {
            //    Console.WriteLine("Category creation failed");
            //}
        }

        public void DeleteCategories(int categoryId, Action successAction, Action errorAction) {
            throw new NotImplementedException();
        }
    }
}
