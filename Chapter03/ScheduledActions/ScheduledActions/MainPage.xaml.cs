using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;

namespace ScheduledActions
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void AddReminder_Click(object sender, EventArgs e)
        {
            string reminderName = string.Format("Reminder {0}", Guid.NewGuid());

            Reminder reminder = new Reminder(reminderName);
            reminder.BeginTime = DateTime.Now.AddMinutes(1);
            reminder.RecurrenceType = RecurrenceInterval.Daily;
            reminder.Content = "This is a WP7 in Action Reminder";
            reminder.Title = "Reminders in action";
            reminder.NavigationUri = new Uri(
                "/MainPage.xaml?reminder=" + reminderName, UriKind.Relative);

            ScheduledActionService.Add(reminder);
            DisplayScheduledNotifications();
        }

        private void RescheduleNotification_Click(object sender, EventArgs e)
        {
            var notification =
                notificationList.SelectedItem as ScheduledNotification;
            if (notification != null)
            {
                notification.BeginTime = DateTime.Now.AddMinutes(1);
                notification.Content = "*" + notification.Content;

                ScheduledActionService.Replace(notification);
                DisplayScheduledNotifications();
            }
        }


        private void RemoveNotification_Click(object sender, EventArgs e)
        {
            var notification = notificationList.SelectedItem
                as ScheduledNotification;
            if (notification != null)
            {
                ScheduledActionService.Remove(notification.Name);
                DisplayScheduledNotifications();
            }
        }

        protected void DisplayScheduledNotifications()
        {
            var items = new List<ScheduledAction>();
            var notifications = ScheduledActionService.
                GetActions<ScheduledNotification>();
            foreach (var notification in notifications)
            {
                var item = ScheduledActionService.Find(notification.Name);
                items.Add(item);
            }
            notificationList.ItemsSource = items;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            agentMessage.Text = ((App)Application.Current).AgentStatus;
            DisplayScheduledNotifications();
            base.OnNavigatedTo(e);
        }


    }
}