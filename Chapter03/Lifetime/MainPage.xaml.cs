using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Lifetime
{
    public partial class MainPage : PhoneApplicationPage
    {
        DateTime pageConstructedTime;
        DateTime navigatedToTime;
        DateTime navigatedFromTime;
        DateTime obscuredTime;
        DateTime unobscuredTime;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            pageConstructedTime = DateTime.Now;

            ((App)Application.Current).RootFrame.Obscured += RootFrame_Obscured;
            ((App)Application.Current).RootFrame.Unobscured += RootFrame_Unobscured;
        }

        public void UpdateUserInterface()
        {
            DateTime now = DateTime.Now;

            // page level times
            pageConstructed.DataContext = (now - pageConstructedTime).TotalSeconds;
            navigatedTo.DataContext = (now - navigatedToTime).TotalSeconds;

            if (navigatedFromTime != DateTime.MinValue)
                navigatedFrom.DataContext = (now - navigatedFromTime).TotalSeconds;

            if (obscuredTime != DateTime.MinValue)
                obscured.DataContext = (now - obscuredTime).TotalSeconds;

            if (unobscuredTime != DateTime.MinValue)
                unobscured.DataContext = (now - unobscuredTime).TotalSeconds;

            // app level times
            var app = (App)Application.Current;

            appConstructed.DataContext = (now - app.AppConstructedTime).TotalSeconds;
            launched.DataContext = (now - app.LaunchedTime).TotalSeconds;

            if (app.DeactivatedTime != DateTime.MinValue)
                deactivated.DataContext = (now - app.DeactivatedTime).TotalSeconds;

            if (app.ActivatedTime != DateTime.MinValue)
                activated.DataContext = (now - app.ActivatedTime).TotalSeconds;

            instancePreserved.Text = app.IsApplicationInstancePreserved.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigatedToTime = DateTime.Now;

            var app = (App)Application.Current;
            bool appInstancePreserved = app.IsApplicationInstancePreserved ?? true;

            if (!appInstancePreserved && State.ContainsKey("NavigatedFromTime"))
            {
                navigatedFromTime = (DateTime)State["NavigatedFromTime"];
            }

            UpdateUserInterface();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (!e.IsNavigationInitiator)
            {
                navigatedFromTime = DateTime.Now;
                State["NavigatedFromTime"] = navigatedFromTime;
            }
            base.OnNavigatedFrom(e);
        }

        private void runOption_Checked(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
            runOption.IsEnabled = false;
        }

        void RootFrame_Unobscured(object sender, EventArgs e)
        {
            unobscuredTime = DateTime.Now;
            UpdateUserInterface();
        }

        void RootFrame_Obscured(object sender, ObscuredEventArgs e)
        {
            obscuredTime = DateTime.Now;
            UpdateUserInterface();
        }

        private void PhoneApplicationPage_Tap(object sender, GestureEventArgs e)
        {
            // perform an action to generate an obscured event.
            var task = new Microsoft.Phone.Tasks.PhoneCallTask();
            task.DisplayName = "Manning Publications Co.";
            task.PhoneNumber = "888 555 1212";
            task.Show();
        }

    }
}