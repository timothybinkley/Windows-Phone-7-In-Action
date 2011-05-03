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

namespace UserInterfaceControls.Toolkit
{
    public partial class DateTimePickerSample : PhoneApplicationPage
    {
        public DateTimePickerSample()
        {
            InitializeComponent();
        }

        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e != null)
            {
                System.Diagnostics.Debug.WriteLine("Old date: {0} New Date: {1}", e.OldDateTime, e.NewDateTime);
            }
        }

        private void TimePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e != null)
            {
                System.Diagnostics.Debug.WriteLine("Old time: {0} New time: {1}", e.OldDateTime, e.NewDateTime);
            }
        }
    }
}