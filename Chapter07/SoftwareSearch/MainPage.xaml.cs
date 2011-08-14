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
using System.Windows.Navigation;

namespace SoftwareSearch
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IDictionary<string, string> queryStrings = NavigationContext.QueryString;

            if (queryStrings.ContainsKey("ProductName") && queryStrings.ContainsKey("Category"))
            {
                message.Text = "Launched from Bing Search.\n\n";
                message.Text += string.Format("Product:\n{0}\n\nCategory:\n{1}\n\n",
                    queryStrings["ProductName"], queryStrings["Category"], e.Uri);
            }
            else 
            {
                message.Text = "Normal launch.\n\n";
            }
            message.Text += string.Format("Uri:\n{0}", e.Uri);
        }
    }
}