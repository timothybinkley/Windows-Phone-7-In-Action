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
using System.Collections.ObjectModel;

namespace ChatTcpUnicast {
    public class MockMainViewModel : MainPageViewModel {
        public MockMainViewModel() {
            Messages = new ObservableCollection<Message> {
                new Message() { DeviceId ="1", Text = "Dude! Are you allright?'", Side = MessageType.Left, UserName = "Leo", Timestamp = DateTime.Now },
                new Message() { DeviceId ="2", Text = "John joined the chat.", Side = MessageType.Notification, UserName = "Me", Timestamp = DateTime.Now },
                new Message() { DeviceId ="2", Text = "He was so drunk!", Side = MessageType.Left, UserName = "John", Timestamp = DateTime.Now },
                new Message() { DeviceId ="2", Text = "I wasn't that drunk last night!", Side = MessageType.Right, UserName = "Me", Timestamp = DateTime.Now },
                new Message() { DeviceId ="2", Text = "Dude, you were hugging an old man with a breard screeming 'DUMBLEDORE YOU'RE ALIVE'", 
                    Side = MessageType.Left, UserName = "Leo", Timestamp = DateTime.Now },                
                 new Message() { DeviceId ="2", Text = "haha.", Side = MessageType.Right, UserName = "Me", Timestamp = DateTime.Now }
            };

        }
        
    }
}
