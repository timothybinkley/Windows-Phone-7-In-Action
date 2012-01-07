using System.Windows;
using Microsoft.Phone.Controls;

namespace Primer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = new SampleModel { UserName = "Tim" };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SampleModel model = (SampleModel)DataContext;
            MessageBox.Show("You entered : " + model.UserName);
        }
    }
}