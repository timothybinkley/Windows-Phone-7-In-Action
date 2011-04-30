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

namespace UserInterfaceControls.BuildingTheUI
{
    public partial class ProgressBarSample : PhoneApplicationPage
    {
        public ProgressBarSample()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            progressBar1.IsIndeterminate = !progressBar1.IsIndeterminate;
            progressBar1.Visibility = progressBar1.IsIndeterminate ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}