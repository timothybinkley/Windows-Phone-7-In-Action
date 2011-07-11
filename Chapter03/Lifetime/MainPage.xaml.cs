using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace Lifetime
{
    public partial class MainPage : PhoneApplicationPage
    {
        DateTime navigatedToTime;
        DateTime navigatedFromTime;
        DateTime pageConstructedTime;
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
            var app = (App)Application.Current;
            DateTime now = DateTime.Now;

            // page level times
            pageConstructed.Text = string.Format("{0:N0} seconds ago", (now - pageConstructedTime).TotalSeconds);
            navigatedTo.Text = string.Format("{0:N0} seconds ago", (now - navigatedToTime).TotalSeconds);

            if (navigatedFromTime != DateTime.MinValue)
                navigatedFrom.Text = string.Format("{0:N0} seconds ago", (now - navigatedFromTime).TotalSeconds);

            if (obscuredTime != DateTime.MinValue)
                obscured.Text = string.Format("{0:N0} seconds ago", (now - obscuredTime).TotalSeconds);

            if (unobscuredTime != DateTime.MinValue)
                unobscured.Text = string.Format("{0:N0} seconds ago", (now - unobscuredTime).TotalSeconds);

            // app level times
            appConstructed.Text = string.Format("{0:N0} seconds ago", (now - app.AppConstructedTime).TotalSeconds);
            launched.Text = string.Format("{0:N0} seconds ago", (now - app.LaunchedTime).TotalSeconds);

            if (app.ActivatedTime != DateTime.MinValue)
                activated.Text = string.Format("{0:N0} seconds ago", (now - app.ActivatedTime).TotalSeconds);

            if (app.DeactivatedTime != DateTime.MinValue)
                deactivated.Text = string.Format("{0:N0} seconds ago", (now - app.DeactivatedTime).TotalSeconds);

            instancePreserved.Text = app.IsApplicationInstancePreserved.ToString();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            navigatedToTime = DateTime.Now;

            if (State.ContainsKey("NavigatedFromTime"))
                navigatedFromTime = (DateTime)State["NavigatedFromTime"];

            UpdateUserInterface();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            State["NavigatedFromTime"] = DateTime.Now;
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



    }
}