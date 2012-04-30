using System.Windows;
using Microsoft.Phone.Controls;

namespace OrientationWithStates
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            this.Loaded += (sender, e) => { System.Diagnostics.Debug.WriteLine("Loaded"); };
            InitializeComponent();
            VisualStateManager.GoToState(this, PageOrientation.Portrait.ToString(), false);
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if ((e.Orientation & PageOrientation.Landscape) == PageOrientation.Landscape)
            {
                VisualStateManager.GoToState(this, "Landscape", true);
            }
            else if ((e.Orientation & PageOrientation.Portrait) == PageOrientation.Portrait)
            {
                VisualStateManager.GoToState(this, "Portrait", true);
            }
        }

    }
}