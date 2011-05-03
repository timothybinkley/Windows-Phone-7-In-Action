using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Radio;

namespace FMRadioSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            UpdateControls();
        }

        private void UpdateControls()
        {
            var radio = FMRadio.Instance;
            frequencyTextbox.Text = radio.Frequency.ToString();
            signalStrengthTextblock.Text = string.Format("Signal strength : {0}", radio.SignalStrength);
            powerModeTextblock.Text = string.Format("Power mode : {0}", radio.PowerMode);
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (turnOffOnExitCheckbox.IsChecked.HasValue && turnOffOnExitCheckbox.IsChecked.Value)
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
    }
}