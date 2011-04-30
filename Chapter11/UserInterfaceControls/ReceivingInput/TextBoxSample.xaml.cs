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

namespace UserInterfaceControls.ReceivingInput
{
    public partial class TextBoxSample : PhoneApplicationPage
    {
        public TextBoxSample()
        {
            InitializeComponent();
            multilineTextBox.Text = "this is an example of multi-line editing." +
                "\nThis is line 2\nand this is line 3.";
        }
    }
}