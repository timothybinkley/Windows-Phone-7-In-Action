using System;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;

namespace DataStorage
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            string name;

            if (IsolatedStorageSettings.ApplicationSettings
                .TryGetValue<String>("Name", out name))
            {
                nameTextBox.Text = name;
            }

            if (IsolatedStorageSettings.ApplicationSettings
                 .TryGetValue<String>("Surname", out name))
            {
                surnameTextBox.Text = name;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["Name"] = nameTextBox.Text;
            IsolatedStorageSettings.ApplicationSettings["Surname"] = surnameTextBox.Text;
        }
    }
}