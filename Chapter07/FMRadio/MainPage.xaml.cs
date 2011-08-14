using System;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Radio;
using System.Windows.Navigation;

namespace FMRadioSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void UpdateControls()
        {
            var radio = FMRadio.Instance;
            frequencyTextbox.Text = radio.Frequency.ToString();
            signalStrengthTextblock.Text = string.Format("Signal strength : {0}", radio.SignalStrength);
            powerModeTextblock.Text = string.Format("Power mode : {0}", radio.PowerMode);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool turnOffRadio;
            if(IsolatedStorageSettings.ApplicationSettings.TryGetValue("TurnOffRadio", out turnOffRadio))
                turnOffOnExitCheckbox.IsChecked = turnOffRadio;
            else
                turnOffOnExitCheckbox.IsChecked = true;

            UpdateControls();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool turnOffRadio = turnOffOnExitCheckbox.IsChecked.Value;
            IsolatedStorageSettings.ApplicationSettings["TurnOffRadio"] = turnOffRadio;
            if (turnOffRadio)
            {
                FMRadio.Instance.PowerMode = RadioPowerMode.Off;
            }
        }

        private RadioRegion GetSelectedRadioRegion()
        {
            if ((jpRadioButton.IsChecked != null) && (jpRadioButton.IsChecked.Value))
            {
                return RadioRegion.Japan;
            }
            else if ((euRadioButton.IsChecked != null) && (euRadioButton.IsChecked.Value))
            {
                return RadioRegion.Europe;
            }
            return RadioRegion.UnitedStates;
        }

        private void on_Clicked(object sender, EventArgs e)
        {
            var radio = FMRadio.Instance;
            radio.CurrentRegion = GetSelectedRadioRegion();

            double frequency;
            if( double.TryParse(frequencyTextbox.Text, out frequency))
            {
                try
                {
                    radio.Frequency = frequency;
                    radio.PowerMode = RadioPowerMode.On;
                }
                catch (ArgumentException ex)
                {
                    radio.PowerMode = RadioPowerMode.Off;
                    MessageBox.Show(string.Format("{0} is not a valid radio frequency.", frequency));
                }
            }
            else
            {
                MessageBox.Show(string.Format("{0} is not a valid radio frequency.", frequencyTextbox.Text));
            }

            UpdateControls();
        }

        private void off_Clicked(object sender, EventArgs e)
        {
            FMRadio.Instance.PowerMode = RadioPowerMode.Off;
            UpdateControls();
        }

        private void refresh_Clicked(object sender, EventArgs e)
        {
            // Radio settings might be changed by the UVC while the application is running,
            // or by either the UVC or the Music + Videos Hub while the application is dormant or terminated.
            UpdateControls();
        }
    }
}