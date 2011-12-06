using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.IO;

namespace WpNotificationService {

    public class NotificationService : INotificationService {

        string toastMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<wp:Notification " +
                                    "xmlns:wp=\"WPNotification\">" +
                                    "<wp:Toast>" +
                                        "<wp:Text1>{0}</wp:Text1>" +
                                        "<wp:Text2>{1}</wp:Text2>" +
                                        "<wp:Param>{2}</wp:Param>" +
                                    "</wp:Toast>" +
                                "</wp:Notification>";

        string tileMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<wp:Notification " +
                                   "xmlns:wp=\"WPNotification\">" +
                                    "<wp:Tile>" +
                                        "<wp:BackgroundImage>{0}" +
                                          "</wp:BackgroundImage>" +
                                        "<wp:Count>{1}</wp:Count>" +
                                        "<wp:Title>{2}</wp:Title>" +
                                        "<wp:BackBackgroundImage>{3}" +
                                          "</wp:BackBackgroundImage>" +
                                        "<wp:BackTitle>{4}</wp:BackTitle>" +
                                        "<wp:BackContent>{5}" +
                                          "</wp:BackContent>" +
                                    "</wp:Tile> " +
                                "</wp:Notification>";



        string rawMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<root>" +
                                    "<Value1>{0}</Value1>" +
                                    "<Value2>{1}</Value2>" +
                                "</root>";





        private NotificationResponse Post(Uri uri, byte[] msg, int interval, string target) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("X-NotificationClass", interval.ToString());
            request.Headers.Add("X-MessageID", Guid.NewGuid().ToString());
            if (target.Length > 0)
                request.Headers["X-WindowsPhone-Target"] = target;
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.ContentLength = msg.Length;

            using (Stream requestStream = request.GetRequestStream()) {
                requestStream.Write(msg, 0, msg.Length);
            }

            var notificationResponse = new NotificationResponse();
            try {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string notificationStatus = response.Headers["X-NotificationStatus"];
                string subscriptStatus = response.Headers["X-SubscriptionStatus"];
                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                notificationResponse.NotificationStatus = (NotificationStatus)Enum
                    .Parse(typeof(NotificationStatus), notificationStatus, true);
                notificationResponse.SubscriptionStatus = (SubscriptionStatus)Enum
                    .Parse(typeof(SubscriptionStatus), subscriptStatus, true);
                notificationResponse.DeviceConnectionStatus = (DeviceConnectionStatus)Enum
                    .Parse(typeof(DeviceConnectionStatus), deviceConnectionStatus, true);
                notificationResponse.HttpStatusCode = HttpStatusCode.OK;
            }
            catch (WebException ex) {
                notificationResponse.HttpStatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                notificationResponse.NotificationStatus = NotificationStatus.None;
                notificationResponse.SubscriptionStatus = SubscriptionStatus.None;
                notificationResponse.DeviceConnectionStatus = DeviceConnectionStatus.None;
            }
            return notificationResponse;
        }

        private void NotifyAll(List<NotificationResponse> statuses, string message, int interval, string target) {
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
            foreach (var uri in clients.Values) {
                statuses.Add(Post(uri, msg, interval, target));
            }
        }
        static Dictionary<Guid, Uri> clients = new Dictionary<Guid, Uri>();

        public void Subscribe(Guid id, string uri) {
            clients[id] = new Uri(uri);
        }

        public IList<NotificationResponse> SendToast(Toast toast) {
            List<NotificationResponse> statuses = new List<NotificationResponse>();
            string message = string.Format(toastMessage, toast.Text1, toast.Text2, toast.Param);
            NotifyAll(statuses, message, (int)ToastBackingInterval.ImmediateDelivery, "toast");
            return statuses;
        }

        public IList<NotificationResponse> SendTitle(Title title) {
            List<NotificationResponse> statuses = new List<NotificationResponse>();
            string message = string.Format(tileMessage, title.FrontImagePath, title.Count, title.FrontTitle,
                title.BackImagePath, title.BackContent, title.BackTitle);
            NotifyAll(statuses, message, (int)TitleBackingInterval.ImmediateDelivery, "token");
            return statuses;
        }

        public IList<NotificationResponse> SendRaw(Raw raw) {
            List<NotificationResponse> statuses = new List<NotificationResponse>();
            string message = string.Format(rawMessage, raw.Text1, raw.Text2);
            NotifyAll(statuses, message, (int)RawBackingInterval.ImmediateDelivery, string.Empty);
            return statuses;
        }




    }

    [DataContract]
    public class Toast {
        [DataMember]
        public string Text1 { get; set; }
        [DataMember]
        public string Text2 { get; set; }
        [DataMember]
        public string Param { get; set; }
        [DataMember]
        public ToastBackingInterval BackingInterval { get; set; }
    }

    [DataContract]
    public enum ToastBackingInterval {
        [EnumMember]
        ImmediateDelivery = 2,
        [EnumMember]
        DeliveredWithin450Seconds = 12,
        [EnumMember]
        DeliveredWithin900Seconds = 22
    }


    [DataContract]
    public class Title {
        [DataMember]
        public string FrontTitle { get; set; }
        [DataMember]
        public string FrontImagePath { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public string BackTitle { get; set; }
        [DataMember]
        public string BackImagePath { get; set; }
        [DataMember]
        public string BackContent { get; set; }
        [DataMember]
        public TitleBackingInterval BackingInterval { get; set; }
    }
    [DataContract]
    public enum TitleBackingInterval {
        [EnumMember]
        ImmediateDelivery = 1,
        [EnumMember]
        DeliveredWithin450Seconds = 11,
        [EnumMember]
        DeliveredWithin900Seconds = 12
    }
    [DataContract]
    public class Raw {
        [DataMember]
        public string Text1 { get; set; }
        [DataMember]
        public string Text2 { get; set; }
        [DataMember]
        public RawBackingInterval BackingInterval { get; set; }
    }
    [DataContract]
    public enum RawBackingInterval {
        [EnumMember]
        ImmediateDelivery = 3,
        [EnumMember]
        DeliveredWithin450Seconds = 13,
        [EnumMember]
        DeliveredWithin900Seconds = 23
    }


    [DataContract]
    public enum NotificationStatus {
        [EnumMember]
        None,
        [EnumMember]
        Received,
        [EnumMember]
        QueueFull,
        [EnumMember]
        Suppressed,
        [EnumMember]
        Dropped
    }
    [DataContract]
    public enum DeviceConnectionStatus {
        [EnumMember]
        None,
        [EnumMember]
        Connected,
        [EnumMember]
        TemporarilyDisconnected,
        [EnumMember]
        Inactive
    }
    [DataContract]
    public enum SubscriptionStatus {
        [EnumMember]
        None,
        [EnumMember]
        Active,
        [EnumMember]
        Expired
    }

    //http://msdn.microsoft.com/en-us/library/ff941100(v=vs.92).aspx
    [DataContract]
    public class NotificationResponse {
        [DataMember]
        public NotificationStatus NotificationStatus { get; set; }
        [DataMember]
        public DeviceConnectionStatus DeviceConnectionStatus { get; set; }
        [DataMember]
        public SubscriptionStatus SubscriptionStatus { get; set; }
        [DataMember]
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
