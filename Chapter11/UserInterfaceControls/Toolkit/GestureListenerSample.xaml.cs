using Microsoft.Phone.Controls;

namespace UserInterfaceControls.Toolkit
{
    public partial class GestureListenerSample : PhoneApplicationPage
    {
        public GestureListenerSample()
        {
            InitializeComponent();
        }

        private void gesture_Tap(object sender, GestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel);
            message.Text = string.Format("tap at ({0},{1})", point.X, point.Y);
            flickMessage.Text = null;
        }

        private void gesture_DoubleTap(object sender, GestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel);
            message.Text += string.Format("\r\ndouble tap at ({0},{1})", point.X, point.Y);
            flickMessage.Text = null;
        }

        private void gesture_Hold(object sender, GestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel);
            message.Text = string.Format("hold at ({0},{1})", point.X, point.Y); 
        }

        private void gesture_DragStarted(object sender, DragStartedGestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel);
            message.Text = string.Format("drag started at ({0},{1}) in the {2} direction ", point.X, point.Y, e.Direction);
            flickMessage.Text = null;
        }

        private void gesture_DragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            message.Text = string.Format("drag delta by ({0},{1}) in the {2} direction ", e.HorizontalChange, e.VerticalChange, e.Direction); 
        }

        private void gesture_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            message.Text = string.Format("drag completed with total change ({0},{1}) in the {2} direction ", 
                e.HorizontalChange, e.VerticalChange, e.Direction, e.HorizontalVelocity, e.VerticalVelocity); 
        }

        private void gesture_Flick(object sender, FlickGestureEventArgs e)
        {
            flickMessage.Text = string.Format("flick angle {0} in the {1} direction and velocity ({2},{3}) ", 
                (int)e.Angle, e.Direction, e.HorizontalVelocity, e.VerticalVelocity); 
        }

        private void gesture_PinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel, 0);
            var point2 = e.GetPosition(ContentPanel, 1);
            message.Text = string.Format("pinch started at ({0},{1}) and ({1},{2}) with angle {3} and distance {4}", 
                point.X, point.Y, point2.X, point2.Y, e.Angle, e.Distance);
            flickMessage.Text = null;
        }

        private void gesture_PinchDelta(object sender, PinchGestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel, 0);
            var point2 = e.GetPosition(ContentPanel, 1);
            message.Text = string.Format("pinch delta at ({0},{1}) and ({1},{2}) with angle delta {3} and distance ratio {4}",
                point.X, point.Y, point2.X, point2.Y, e.TotalAngleDelta, e.DistanceRatio); 
        }

        private void gesture_PinchCompleted(object sender, PinchGestureEventArgs e)
        {
            var point = e.GetPosition(ContentPanel, 0);
            var point2 = e.GetPosition(ContentPanel, 1);
            message.Text = string.Format("pinch completed at ({0},{1}) and ({1},{2}) with angle delta {3} and distance ratio {4}",
                point.X, point.Y, point2.X, point2.Y, e.TotalAngleDelta, e.DistanceRatio); 
        }
        
    }
}