using Microsoft.Phone.Scheduler;
using System;

namespace NotificationsAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //RemoveUnscheduledAndExpiredNotifications();
            NotifyComplete();
        }

        protected void RemoveUnscheduledAndExpiredNotifications()
        {
            var now = DateTime.Now;
            var notifications = ScheduledActionService.GetActions<ScheduledNotification>();
            foreach (var notification in notifications)
            {
                if (notification.IsScheduled == false || notification.ExpirationTime < now)
                {
                    ScheduledActionService.Remove(notification.Name);
                }
            }
        }
    }
}
