using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace SilverlightHello
{
    public partial class GreetingPage : PhoneApplicationPage
    {
        public GreetingPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            helloMessage.Text = this.NavigationContext.QueryString["name"];
            base.OnNavigatedTo(e);
        }

        private void Page_BackKeyPress(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Press OK to return to the previous page.", 
                "WP7 in Action", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void navigateBackButton_Click(object sender, System.EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void copyButton_Click(object sender, System.EventArgs e)
        {
            string message = string.Format("Hello {0}!", helloMessage.Text);
            //Clipboard.SetText(message);  
        }
    }
}