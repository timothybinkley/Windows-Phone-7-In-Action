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
    public partial class MatrixTransformSample : PhoneApplicationPage
    {
        public MatrixTransformSample()
        {
            InitializeComponent();

            double angle = Math.Sqrt(2.0D) / 2.0D;
            Matrix matrix = new Matrix
            {
                M11 = angle,
                M22 = angle,
                M21 = angle,
                M12 = -angle
            };

            rectangleToRotate.RenderTransform = new MatrixTransform { Matrix = matrix };
        }
    }
}