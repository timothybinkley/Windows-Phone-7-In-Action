using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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
        private void listen_Click(object sender, RoutedEventArgs e)
        {

            FMRadio myRadio = FMRadio.Instance;
            myRadio.CurrentRegion = RadioRegion.UnitedStates;
            myRadio.Frequency = 91.3;
            Console.WriteLine(myRadio.SignalStrength.ToString());

            myRadio.CurrentRegion = GetSelectedRadioRegion();

            try
            {
                myRadio.Frequency = Convert.ToDouble(frequencyTextbox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            signalStrengthTextblock.Text = string.Format("SignalStrength : {0}", myRadio.SignalStrength);

        }

        private void euRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void frequency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (frequencyTextbox != null)
            {
                //  frequencyTextbox.Text = string.Format("Frequency : {0}", Math.Round(frequencySlider.Value, 1));
            }
        }
    }
}