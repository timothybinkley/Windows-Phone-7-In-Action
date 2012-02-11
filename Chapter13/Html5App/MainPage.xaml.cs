using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Navigation;
using System.Windows;
using System.ComponentModel;

namespace Html5App
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            webBrowser.Navigate(new Uri("MainPage.html", UriKind.Relative));
        }
        
        private void about_Click(object sender, EventArgs e)
        {
            var task = new WebBrowserTask
            {
                Uri = new Uri("http://www.manning.com/perga", UriKind.Absolute)
            };
            task.Show();
        }

        private void webBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            webBrowser.NavigateToString("<html><body>I'm having trouble loading " +
                "the web applicaiton right now. Are you in airplane mode?<br/>" +
                "<a href='" + e.Uri.ToString() + "'>Try again.</a>" +
                "</body></html>");
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, CancelEventArgs e)
        {
            if (webBrowser.Source != null && webBrowser.Source.OriginalString != "MainPage.html")
            {
                webBrowser.Navigate(new Uri("MainPage.html", UriKind.Relative));
                e.Cancel = true;
            }
        }

        private void webBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value == "chooseAddress")
            {
                var task = new AddressChooserTask();
                task.Completed += task_Completed;
                task.Show();
            }
        }

        void task_Completed(object sender, AddressResult e)
        {
            string message;
            if (e.Error != null || e.TaskResult != TaskResult.OK)
                message = "No address chosen";
            else
                message = e.Address.Replace("\r\n", ",");
            webBrowser.InvokeScript("addressChooserCompleted", message);
        }

    }
}