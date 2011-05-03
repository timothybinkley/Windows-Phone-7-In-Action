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
    public partial class ContextMenuSample : PhoneApplicationPage
    {
        public ContextMenuSample()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem)sender;
            System.Diagnostics.Debug.WriteLine("MenuItem {0} pressed.", item.Header);
        }

    }
}