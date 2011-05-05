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
    public partial class GestureListenerSample : PhoneApplicationPage
    {
        public GestureListenerSample()
        {
            InitializeComponent();
        }

        private void OnTap(object sender, GestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel);
            message.Text = string.Format("tap at ({0},{1})", point.X, point.Y); 
        }

        private void OnDoubleTap(object sender, GestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel);
            message.Text = string.Format("double tap at ({0},{1})", point.X, point.Y); 
        }

        private void OnHold(object sender, GestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel);
            message.Text = string.Format("hold at ({0},{1})", point.X, point.Y); 
        }

        private void OnDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel);
            message.Text = string.Format("drag started at ({0},{1}) in the {2} direction ", point.X, point.Y, e.Direction); 
        }

        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            message.Text = string.Format("drag delta by ({0},{1}) in the {2} direction ", e.HorizontalChange, e.VerticalChange, e.Direction); 
        }

        private void OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            message.Text = string.Format("drag completed with total change ({0},{1}) in the {2} direction ", 
                e.HorizontalChange, e.VerticalChange, e.Direction, e.HorizontalVelocity, e.VerticalVelocity); 
        }

        private void OnFlick(object sender, FlickGestureEventArgs e)
        {
            message.Text = string.Format("flick angle {0} in the {1} direction and velocity ({2},{3}) ", 
                e.Angle, e.Direction, e.HorizontalVelocity, e.VerticalVelocity); 
        }

        private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel, 0);
            var point2 = e.GetPosition(ContentPanel, 1);
            message.Text = string.Format("pinch started at ({0},{1}) and ({1},{2}) with angle {3} and distance {4}", 
                point.X, point.Y, point2.X, point2.Y, e.Angle, e.Distance); 
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel, 0);
            var point2 = e.GetPosition(ContentPanel, 1);
            message.Text = string.Format("pinch delta at ({0},{1}) and ({1},{2}) with angle delta {3} and distance ration {4}",
                point.X, point.Y, point2.X, point2.Y, e.TotalAngleDelta, e.DistanceRatio); 
        }

        private void OnPinchCompleted(object sender, PinchGestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel, 0);
            var point2 = e.GetPosition(ContentPanel, 1);
            message.Text = string.Format("pinch completed at ({0},{1}) and ({1},{2}) with angle delta {3} and distance ration {4}",
                point.X, point.Y, point2.X, point2.Y, e.TotalAngleDelta, e.DistanceRatio); 

        }

        
    }
}