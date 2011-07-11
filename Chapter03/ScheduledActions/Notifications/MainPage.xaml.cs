using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;

namespace Notifications
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            ((App)Application.Current).RootFrame.Unobscured += RootFrame_Unobscured;
            StartNotificationCleanupTask();
        }

        private void StartNotificationCleanupTask()
        {
            // check to see if the task is already scheduled.
            var tasks = ScheduledActionService.GetActions<PeriodicTask>();
            var cleanupTask = ScheduledActionService.Find("Notification clean up") as PeriodicTask;
            if (cleanupTask != null)
            {
                System.Diagnostics.Debug.WriteLine("cleanupTask:\r\n\tIsEnabled: {0}\r\n\tIsEnabled: {1}\r\n\tExpirationTime: {2}"+
                "\r\n\tExit Reason: {3}\r\n\tLast Time: {4}",  
                    cleanupTask.IsEnabled, cleanupTask.IsScheduled, cleanupTask.ExpirationTime, 
                    cleanupTask.LastExitReason, cleanupTask.LastScheduledTime);
                if (!cleanupTask.IsEnabled)
                {
                    ScheduledActionService.Remove("Notification clean up");
                    cleanupTask = CreateCleanupTask();
                }
                if (!cleanupTask.IsScheduled)
                {
                    agentMessage.Text = string.Format("The clean up background task was aborted during its last execution at {0:g}.", cleanupTask.LastScheduledTime);
                    ScheduledActionService.Remove("Notification clean up");
                    cleanupTask = CreateCleanupTask();
                }
            }
            else
            {
                cleanupTask = CreateCleanupTask();
            }

            if(cleanupTask != null) // && cleanupTask.IsEnabled && cleanupTask.IsScheduled && cleanupTask.ExpirationTime > DateTime.Now)
                ScheduledActionService.LaunchForTest("Notification clean up", TimeSpan.FromSeconds(15));            
        }

        private PeriodicTask CreateCleanupTask()
        {
            PeriodicTask cleanupTask = null;
            try
            {
                cleanupTask = new PeriodicTask("Notification clean up");
                cleanupTask.Description = "A background agent responsible for removing expired Windows Phone 7 in Action notifications";
                ScheduledActionService.Add(cleanupTask);
            }
            catch(InvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to add the clean up task: ", ex.Message);
                agentMessage.Text = "Unable to create a new clean up background task. Background tasks may have been disabled in the settings application.";
                cleanupTask = null;
            }
            return cleanupTask;
        }
            

        protected void DisplayScheduledNotifications()
        {
            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            // force the update of cached actions
            foreach (var notification in notifications)
            {
                ScheduledActionService.Find(notification.Name);
            }
            notificationList.ItemsSource = notifications;
        }

        protected void RemoveUnscheduledNotifications()
        {
            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            foreach (var notification in notifications)
            {
                if (notification.IsScheduled == false)
                    ScheduledActionService.Remove(notification.Name);
            }
        }

        protected void RemoveExpiredNotifications()
        {
            var now = DateTime.Now;
            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            foreach (var notification in notifications)
            {
                if (notification.ExpirationTime < now)
                    ScheduledActionService.Remove(notification.Name);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            uriMessage.Text += "NavigationUri: " + e.Uri;
            DisplayScheduledNotifications();
            base.OnNavigatedTo(e);
        }

        void RootFrame_Unobscured(object sender, EventArgs e)
        {
            DisplayScheduledNotifications();
        }

        private void AddAlarm_Click(object sender, EventArgs e)
        {
            int count = 0;
            string alarmName;
            do
            {
                count++;
                alarmName = "LongDayAlarm" + count;
            }
            while (ScheduledActionService.Find(alarmName) != null);

            Alarm alarm = new Alarm(alarmName);
            alarm.BeginTime = DateTime.Now.AddSeconds(300);
            alarm.Content = string.Format("{0} - It's been a long day. Go to bed.", count);
            ScheduledActionService.Add(alarm);
            DisplayScheduledNotifications();
        }

        private void AddReminder_Click(object sender, EventArgs e)
        {
            // todo replace this with a guids or ticks or something so I do not have to 
            // look for a unique name.
            int count = 0;
            string reminderName;
            do
            {
                count++;
                reminderName = "DoNotForgetReminder" + count;
            }
            while (ScheduledActionService.Find(reminderName) != null);  // this way does not work, since find throws when the item is not found!

            Reminder reminder = new Reminder(reminderName);
            reminder.BeginTime = DateTime.Now.AddSeconds(15);
            reminder.ExpirationTime = DateTime.Now.AddSeconds(300);
            reminder.Content = string.Format("{0} - Don't forget that it's been a long day.", count);
            reminder.Title = "Reminder in action " + count;
            reminder.NavigationUri = new Uri("/MainPage.xaml?reminder=" + count, UriKind.Relative);

            ScheduledActionService.Add(reminder);
            DisplayScheduledNotifications();
        }

        private void DeleteNotifications_Click(object sender, EventArgs e)
        {
            RemoveUnscheduledNotifications();
            DisplayScheduledNotifications();
        }

        private void RescheduleNotification_Click(object sender, EventArgs e)
        {
            // todo: implement updating the tasks using replace.
            var notification = notificationList.SelectedItem as ScheduledNotification;
            if (notification != null)
            {
                var foundNotification = ScheduledActionService.Find(notification.Name) as ScheduledNotification;
                //foundNotification.BeginTime = DateTime.Now.AddSeconds(15);
                foundNotification.Content = "*" + foundNotification.Content;
                
                //ScheduledActionService.Replace(foundNotification);
                DisplayScheduledNotifications();
            }
        }

    }
}