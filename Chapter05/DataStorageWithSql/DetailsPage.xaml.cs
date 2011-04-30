using System;
using System.Windows;
using Microsoft.Phone.Controls;

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
                    var contact = ((App)Application.Current).Model.CreateContact();
                    DataContext = contact;
                }
                else
                {
                    int index = int.Parse(selectedIndex);
                    DataContext = ((App)Application.Current).Model.Contacts[index];
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var contact = (ContactInfo)DataContext;
            ((App)Application.Current).Model.DeleteContact(contact);
            NavigationService.GoBack();
        }
    }
}