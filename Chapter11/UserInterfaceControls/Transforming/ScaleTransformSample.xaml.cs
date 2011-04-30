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

namespace UserInterfaceControls.Transforming
{
    public partial class ScaleTransformSample : PhoneApplicationPage
    {
        public ScaleTransformSample()
        {
            InitializeComponent();

            rectangleToScale.RenderTransform = new ScaleTransform
            {
                ScaleX = 0.5D,
                ScaleY = 0.75D,
                CenterX = 30.0D,
                CenterY = 60.0D
            };
        }
    }
}