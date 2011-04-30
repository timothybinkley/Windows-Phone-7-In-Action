using System;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using System.Windows;

namespace DataStorage
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Contacts;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            ((App)Application.Current).SaveContacts();
        }

        private void ContactsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ContactsListBox.SelectedIndex == -1)
                return;

            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + ContactsListBox.SelectedIndex, UriKind.Relative));

            ContactsListBox.SelectedIndex = -1;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=new", UriKind.Relative));
        }
    }
}