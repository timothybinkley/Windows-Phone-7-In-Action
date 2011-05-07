using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace Lifetime
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            pageConstructed.Text = DateTime.Now.ToLongTimeString();

            ((App)Application.Current).RootFrame.Obscured += new EventHandler<ObscuredEventArgs>(RootFrame_Obscured);
            ((App)Application.Current).RootFrame.Unobscured += new EventHandler(RootFrame_Unobscured);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            navigatedTo.Text = DateTime.Now.ToLongTimeString();

            if (State.ContainsKey("NavigatedFromTime"))
                navigatedFrom.Text = ((DateTime)State["NavigatedFromTime"]).ToLongTimeString();

            var app = (App)Application.Current;
            appConstructed.Text = app.AppConstructed.ToLongTimeString();
            launched.Text = app.Launched.ToLongTimeString();

            if (app.Activated != DateTime.MinValue)
                activated.Text = app.Activated.ToLongTimeString();

            if (app.Deactivated != DateTime.MinValue)
                deactivated.Text = app.Deactivated.ToLongTimeString();

            instancePreserved.Text = app.IsAppInstancePreserved.ToString();

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
            unobscured.Text = DateTime.Now.ToLongTimeString();
        }

        void RootFrame_Obscured(object sender, ObscuredEventArgs e)
        {
            obscured.Text = DateTime.Now.ToLongTimeString();
        }



    }
}