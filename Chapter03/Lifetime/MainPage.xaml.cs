using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

namespace Lifetime
{
    public partial class MainPage : PhoneApplicationPage
    {
        DateTime pageConstructedTime;
        DateTime navigatedToTime;
        DateTime navigatedFromTime;
        //DateTime obscuredTime;
        //DateTime unobscuredTime;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            pageConstructedTime = DateTime.Now;

            //((App)Application.Current).RootFrame.Obscured += RootFrame_Obscured;
            //((App)Application.Current).RootFrame.Unobscured += RootFrame_Unobscured;
        }

        public void UpdateUserInterface()
        {
            
            // page level times
            pageConstructed.Value = pageConstructedTime;
            navigatedTo.Value = navigatedToTime;

            if (navigatedFromTime != DateTime.MinValue)
                navigatedFrom.Value = navigatedFromTime;

            //if (obscuredTime != DateTime.MinValue)
            //    obscured.Value = obscuredTime;

            //if (unobscuredTime != DateTime.MinValue)
            //    unobscured.Value = unobscuredTime;

            // app level times
            var app = (App)Application.Current;

            appConstructed.Value = app.AppConstructedTime;
            launched.Value = app.LaunchedTime;

            if (app.DeactivatedTime != DateTime.MinValue)
                deactivated.Value = app.DeactivatedTime;

            if (app.ActivatedTime != DateTime.MinValue)
                activated.Value = app.ActivatedTime;
            
            instancePreserved.Text = app.IsApplicationInstancePreserved.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigatedToTime = DateTime.Now;

            bool appInstancePreserved =  ((App)Application.Current).IsApplicationInstancePreserved ?? true;
            if (!appInstancePreserved && State.ContainsKey("NavigatedFromTime"))
            {
                navigatedFromTime = (DateTime)State["NavigatedFromTime"];
            }
            
            UpdateUserInterface();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.Uri.Equals("app://external/"))
            {
                navigatedFromTime = DateTime.Now;
                State["NavigatedFromTime"] = navigatedFromTime;
            }
            base.OnNavigatedFrom(e);
        }

        //private void runOption_Checked(object sender, RoutedEventArgs e)
        //{
        //    PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
        //    runOption.IsEnabled = false;
        //}

        //void RootFrame_Unobscured(object sender, EventArgs e)
        //{
        //    unobscuredTime = DateTime.Now;
        //    UpdateUserInterface();
        //}

        //void RootFrame_Obscured(object sender, ObscuredEventArgs e)
        //{
        //    obscuredTime = DateTime.Now;
        //    UpdateUserInterface();
        //}



    }
}