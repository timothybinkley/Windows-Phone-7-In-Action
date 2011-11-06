using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ChatTcpUnicast {
    public enum MessageSide { 
        Left,
        Right
    }
    public class Message {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string DeviceId { get; set; }
        public string UserName { get; set; }
        public MessageSide Side { get; set; }
    }
}
