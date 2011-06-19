using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;

namespace UserData
{
    public partial class MainPage : PhoneApplicationPage
    {
        Contacts contacts;
        Appointments appointments = new Appointments();

        public MainPage()
        {
            InitializeComponent();
            contacts = new Contacts();
            contacts.SearchCompleted += contacts_SearchCompleted;
            appointments.SearchCompleted += appointments_SearchCompleted;
        }

        #region contacts pivot item code

        private void searchContacts_Click(object sender, RoutedEventArgs e)
        {
            var filterKind = FilterKind.DisplayName;
            if (phoneSearch.IsChecked.Value)
                filterKind = FilterKind.PhoneNumber;
            else if (emailSearch.IsChecked.Value)
                filterKind = FilterKind.EmailAddress;

            string filter = filterBox.Text;

            contacts.SearchAsync(filter, filterKind, null);
        }

        void contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            searchContactsResult.Text = string.Format("{0} contacts found", e.Results.Count());
            if (e.Results.Count() > 1)
                searchContactsResult.Text += string.Format(", displaying the first match");
            contactLayout.DataContext = e.Results.FirstOrDefault();
        }

        #endregion

        #region appointments pivot item code

        private void searchAppointments_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(1).AddSeconds(-1);

            if (weekSearch.IsChecked.Value)
                end = end.AddDays(7);
            else if (monthSearch.IsChecked.Value)
                end = end.AddDays(30);

            appointments.SearchAsync(start, end, null);
        }

        void appointments_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {
            //var results = e.Results.ToList();
            var results = FakeAppointment.FakeResults().ToList();
            apptsResult.Text = string.Format("{0} appointments found", results.Count); // e.Results.Count()
            if (results.Count > 1) // e.Results.Count() > 1)
                apptsResult.Text += string.Format(", displaying the first match");
            appointmentLayout.DataContext = results.FirstOrDefault();  // e.Results.FirstOrDefault();
        }

        #endregion

        #region choosers pivot item code

        //PhoneNumberChooserTask phoneChooser = new PhoneNumberChooserTask();
        //EmailAddressChooserTask emailChooser = new EmailAddressChooserTask();
        //AddressChooserTask addressChooser = new AddressChooserTask();

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //phoneChooser.Completed += phoneChooser_Completed;
            //emailChooser.Completed += emailChooser_Completed;
            //addressChooser.Completed += addressChooser_Completed;
        }

        /*
        private void ChoosePhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            phoneChooser.Show();
        }

        void phoneChooser_Completed(object sender, PhoneNumberResult e)
        {
            if (e.Error != null)
                chooserResult.Text = e.Error.Message;
            else if (e.TaskResult == TaskResult.Cancel)
                chooserResult.Text = "user canceled";
            else if (e.TaskResult == TaskResult.None)
                chooserResult.Text = "no result";
            else if (e.TaskResult == TaskResult.OK)
                chooserResult.Text = string.Format("Phone Number for {0}\r\n{1}", e.DisplayName, e.PhoneNumber);
        }

        private void ChooseEmailAddress_Click(object sender, RoutedEventArgs e)
        {
            emailChooser.Show();
        }

        void emailChooser_Completed(object sender, EmailResult e)
        {
            if (e.Error != null)
                chooserResult.Text = e.Error.Message;
            else if (e.TaskResult == TaskResult.Cancel)
                chooserResult.Text = "user canceled";
            else if (e.TaskResult == TaskResult.None)
                chooserResult.Text = "no result";
            else if (e.TaskResult == TaskResult.OK)
                chooserResult.Text = string.Format("Email Address for {0}\r\n{1}", e.DisplayName, e.Email);
        }

        private void ChooseStreetAddress_Click(object sender, RoutedEventArgs e)
        {
            addressChooser.Show();
        }

        void addressChooser_Completed(object sender, AddressResult e)
        {
            if (e.Error != null)
                chooserResult.Text = e.Error.Message;
            else if (e.TaskResult == TaskResult.Cancel)
                chooserResult.Text = "user canceled";
            else if (e.TaskResult == TaskResult.None)
                chooserResult.Text = "no result";
            else if (e.TaskResult == TaskResult.OK)
                chooserResult.Text = string.Format("Street Address for {0}\r\n{1}", e.DisplayName, e.Address);
        }
        */
        #endregion

    }
}