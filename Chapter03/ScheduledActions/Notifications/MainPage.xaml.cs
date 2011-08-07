using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using System.Collections.Generic;

namespace Notifications
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            ((App)Application.Current).RootFrame.Unobscured += RefreshNotifications_Click;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            agentMessage.Text = ((App)Application.Current).AgentStatus;
            uriMessage.Text += "NavigationUri: " + e.Uri;
            DisplayScheduledNotifications();
            base.OnNavigatedTo(e);
        }

        private void AddAlarm_Click(object sender, EventArgs e)
        {
            string alarmName = string.Format("Alarm {0}", Guid.NewGuid());
            Alarm alarm = new Alarm(alarmName);
            alarm.BeginTime = DateTime.Now.AddMinutes(1);
            alarm.RecurrenceType = RecurrenceInterval.Daily;
            alarm.Content = "This is a WP7 in Action Alarm";
            alarm.Sound = new Uri("wp7iaAlarm.wma", UriKind.Relative);
            ScheduledActionService.Add(alarm);
            DisplayScheduledNotifications();
        }

        private void AddReminder_Click(object sender, EventArgs e)
        {
            string reminderName = string.Format("Reminder {0}", Guid.NewGuid());
            Reminder reminder = new Reminder(reminderName);
            reminder.BeginTime = DateTime.Now.AddMinutes(1);
            reminder.Content = "This is a WP7 in Action Reminder";
            reminder.Title = "Reminders in action";
            reminder.NavigationUri = new Uri("/MainPage.xaml?reminder=" + reminderName, UriKind.Relative);

            ScheduledActionService.Add(reminder);
            DisplayScheduledNotifications();
        }

        private void RescheduleNotification_Click(object sender, EventArgs e)
        {
            var notification = notificationList.SelectedItem as ScheduledNotification;
            if (notification != null)
            {
                notification.BeginTime = DateTime.Now.AddMinutes(1);
                notification.ExpirationTime = DateTime.Now.AddMinutes(30);
                notification.Content = "*" + notification.Content;
                notification.RecurrenceType = RecurrenceInterval.None;
                ScheduledActionService.Replace(notification);
                DisplayScheduledNotifications();
            }
        }

        private void RemoveNotification_Click(object sender, EventArgs e)
        {
            var notification = notificationList.SelectedItem as ScheduledNotification;
            if (notification != null)
            {
                ScheduledActionService.Remove(notification.Name);
                DisplayScheduledNotifications();
            }
        }

        private void RefreshNotifications_Click(object sender, EventArgs e)
        {
            DisplayScheduledNotifications();
        }

        protected void DisplayScheduledNotifications()
        {
            var items = new List<ScheduledAction>();
            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            foreach (var notification in notifications)
            {
                var item = ScheduledActionService.Find(notification.Name);
                items.Add(item);
            }
            notificationList.ItemsSource = items;
        }

    }
}