using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Tasks;

namespace SilverlightAbout
{
    public partial class AboutPage : PhoneApplicationPage
    {
        SavePhoneNumberTask savePhoneNumberTask = new SavePhoneNumberTask();
        SaveEmailAddressTask saveEmailAddressTask = new SaveEmailAddressTask();

        // Constructor
        public AboutPage()
        {
            InitializeComponent();

            savePhoneNumberTask.Completed += savePhoneTask_Completed;
            saveEmailAddressTask.Completed += saveEmailTask_Completed;

            ((App)Application.Current).RootFrame.Obscured += RootFrame_Obscured;
            ((App)Application.Current).RootFrame.Unobscured += RootFrame_Unobscured;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LicenseInformation licenseInfo = new LicenseInformation();
            bool isTrial = licenseInfo.IsTrial();
            if (isTrial)
                HomePageButton.Content = "Buy this application";
            else
                HomePageButton.Content = "Marketplace home";

            base.OnNavigatedTo(e);
        }

        void RootFrame_Unobscured(object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Unobscured...");
        }

        void RootFrame_Obscured(object sender, ObscuredEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Obscured...");
        }
        
        private void SupportPhoneLink_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask task = new PhoneCallTask()
            {
                PhoneNumber = (string)SupportPhoneLink.Content,
                DisplayName = "WP7 In Action Customer Support"
            };
            task.Show();
        }

        private void SupportEmailLink_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask()
            {
                To = (string)SupportEmailLink.Content.ToString(),
                Subject = "WP7 in Action About Application",
                Body = "Support Issue Details:"
            };
            task.Show();
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            SmsComposeTask task = new SmsComposeTask()
            {
                Body = "I like the WP7 in Action About Application, you should try it out!",
            };
            task.Show();
        }

        private void ShareLinkButton_Click(object sender, RoutedEventArgs e)
        {
            ShareLinkTask task = new ShareLinkTask()
            {
                Title = "WP7 in Action", 
                Message = "I like the WP7 in Action About Application, you should try it out!",
                LinkUri = new Uri("http://manning.com/perga")
            };
            task.Show();
        }

        private void ShareStatusButton_Click(object sender, RoutedEventArgs e)
        {
            ShareStatusTask task = new ShareStatusTask()
            {
                Status = "I like the WP7 in Action About Application, you should try it out!",
            };
            task.Show();
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        private void HomePageButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceDetailTask task = new MarketplaceDetailTask();
            task.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceSearchTask task = new MarketplaceSearchTask()
            {
                SearchTerms = "Windows Phone 7 in Action",
                ContentType = MarketplaceContentType.Applications
            };
            task.Show();
        }

        private void BingSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTask task = new SearchTask()
            {
                SearchQuery = "Windows Phone 7 in Action domain:manning.com"
            };
            task.Show();
        }

        private void SavePhoneButton_Click(object sender, RoutedEventArgs e)
        {
            savePhoneNumberTask.PhoneNumber = (string)SupportPhoneLink.Content;
            savePhoneNumberTask.Show();
        }

        private void savePhoneTask_Completed(object sender, TaskEventArgs e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                SavePhoneButton.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveEmailButton_Click(object sender, RoutedEventArgs e)
        {
            saveEmailAddressTask.Email = (string)SupportEmailLink.Content;
            saveEmailAddressTask.Show();
        }

        private void saveEmailTask_Completed(object sender, TaskEventArgs e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                SaveEmailButton.Visibility = Visibility.Collapsed;
            }
        }

    }
}
