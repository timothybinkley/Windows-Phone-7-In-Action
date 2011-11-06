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
    public class MessageContentPresenter : ContentControl {
        public DataTemplate RightTemplate { get; set; }
        public DataTemplate LeftTemplate { get; set; }

        protected override void OnContentChanged(object oldContent, object newContent) {
            base.OnContentChanged(oldContent, newContent);

            // apply the required template
            Message message = newContent as Message;
            if (message.Side == MessageSide.Left) {
                ContentTemplate = LeftTemplate;
            }
            else {
                ContentTemplate = RightTemplate;
            }
        }
    }
}
