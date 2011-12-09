using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WpNotificationService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INotificationService" in both code and config file together.
    [ServiceContract]
    public interface INotificationService {
        [OperationContract]
        IList<NotificationResponse> SendToast(Toast toast);
        [OperationContract]
        IList<NotificationResponse> SendTitle(Title title);
        [OperationContract]
        IList<NotificationResponse> SendRaw(Raw raw);

        [OperationContract]
        void Subscribe(Guid id, string uri);
    }
}
