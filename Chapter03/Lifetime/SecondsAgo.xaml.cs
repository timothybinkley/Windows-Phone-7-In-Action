using System;
using System.Windows.Controls;

namespace Lifetime
{
    public partial class SecondsAgo : UserControl
    {
        public SecondsAgo()
        {
            InitializeComponent();
        }

        public string Label
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        private DateTime secondsAgo;
        public DateTime Value
        {
            get { return secondsAgo; }
            set
            {
                secondsAgo = value;
                seconds.Text = string.Format("{0:N0} seconds ago", (DateTime.Now - secondsAgo).TotalSeconds);
            }
        }
    }
}
