using System.ComponentModel;

namespace Primer
{
    public class SampleModel : INotifyPropertyChanged
    {
        string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
