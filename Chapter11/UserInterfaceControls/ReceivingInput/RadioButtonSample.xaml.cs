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
using System.Diagnostics;

namespace UserInterfaceControls.ReceivingInput
{
    public partial class RadioButtonSample : PhoneApplicationPage
    {
        public RadioButtonSample()
        {
            InitializeComponent();
            if (radioButton1.IsChecked.Value)
                Debug.WriteLine("option 1 selected");
            else if (radioButton2.IsChecked.Value)
                Debug.WriteLine("option 2 selected");
            else if (radioButton3.IsChecked.Value)
                Debug.WriteLine("option 3 selected");

        }
    }
}