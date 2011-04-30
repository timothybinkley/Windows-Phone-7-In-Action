using System;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using System.Windows;

namespace DataStorage
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        public DetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                if (selectedIndex == "new")
                {
                    var contact = new ContactInfo();
                    ((App)Application.Current).Contacts.Add(contact);
                    DataContext = contact;
                }
                else
                {
                    int index = int.Parse(selectedIndex);
                    DataContext = ((App)Application.Current).Contacts[index];
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var contact = (ContactInfo)DataContext;
            ((App)Application.Current).Contacts.Remove(contact);
            NavigationService.GoBack();
        }

        
    }
}