using System.IO.IsolatedStorage;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Panorama
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int selectedIndex;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("selection", out selectedIndex))
            {
                panorama.DefaultItem = panorama.Items[selectedIndex];
            }
        }

        private void panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["selection"] = panorama.SelectedIndex;
        }

    }
}