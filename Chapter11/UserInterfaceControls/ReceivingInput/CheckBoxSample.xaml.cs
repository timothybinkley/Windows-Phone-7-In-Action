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
    public partial class CheckBoxSample : PhoneApplicationPage
    {
        public CheckBoxSample()
        {
            InitializeComponent();
            if (!sampleCheckBox.IsChecked.HasValue)
                Debug.WriteLine("sampleCheckBox is indeterminate");
            else if (sampleCheckBox.IsChecked.Value)
                Debug.WriteLine("sampleCheckBox is checked");
            else if (sampleCheckBox.IsChecked.Value)
                Debug.WriteLine("sampleCheckBox is unchecked");
        }
    }
}