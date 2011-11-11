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
using System.Net.Sockets;
using System.Text;
using Microsoft.Phone.Info;

namespace ChatTcpUnicast {
    public class TcpSocketClient : IDisposable {
        private const int PORT = 22222;
        private Socket socket;
        private bool disposed = false;
        private const int MAX_BUFFER_SIZE = 1024;

        public TcpSocketClient(IPAddress ipAddress) {
            IPEndPoint = new IPEndPoint(ipAddress, PORT);
            socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
        }

        public void ConnectAsync(Action success, Action<string> failure) {
            var message = string.Format("{0};{1};", SocketCommands.CONNECT, this.DeviceNameAndId);
            var args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = this.IPEndPoint;
            args.Completed += (__, e) => {
                if (e.SocketError == SocketError.Success) {
                    failure("The remote host is not found.");
                }
                else {
                    success();
                    OnRecieve();
                }
            };
            if (!string.IsNullOrEmpty(message)) {
                var buffer = Encoding.UTF8.GetBytes(message);
                args.SetBuffer(buffer, 0, buffer.Length);
            }
            socket.ConnectAsync(args);
        }


        public delegate void ReceiveHandler(object sender, EventArgs<string> e);
        public event ReceiveHandler Received;
        private void RaiseReceived(string message) {
            if (Received != null) {
                Received(this, new EventArgs<string>(message));
            }
        }

        private void OnRecieve() {
            var receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.RemoteEndPoint = this.IPEndPoint;
            receiveArgs.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);
            var strBdr = new StringBuilder();
            receiveArgs.Completed += (__, result) => {
                var package = Encoding.UTF8.GetString(result.Buffer, 0, result.BytesTransferred);
                if (!string.IsNullOrEmpty(package)) {
                    this.RaiseReceived(package);
                }
                socket.ReceiveAsync(receiveArgs);
            };
            socket.ReceiveAsync(receiveArgs);
        }

        private void SendAsync(string message, Action success, Action<string> failure) {
            if (socket.Connected) {

                var formattedMessage = string.Format("{0};{1};{2}", SocketCommands.TEXT, this.DeviceNameAndId, message);

                var buffer = Encoding.UTF8.GetBytes(formattedMessage);

                var args = new SocketAsyncEventArgs();
                args.RemoteEndPoint = this.IPEndPoint;
                args.SetBuffer(buffer, 0, buffer.Length);
                args.Completed += (__, e) => {
                    if (e.SocketError == SocketError.Success) {
                        failure("Your message can't be sent.");
                    }
                    else {
                        success();
                    }
                };
                socket.SendAsync(args);
            }
        }

        public string DeviceNameAndId {
            get {
                var deviceName = DeviceExtendedProperties.GetValue("DeviceName") as string;
                return deviceName + BitConverter.ToString(DeviceExtendedProperties.GetValue("DeviceUniqueId") as byte[]);
            }
        }

        public IPEndPoint IPEndPoint { get; private set; }

        #region Dispose
        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    this.socket.Dispose();
                }
                disposed = true;
            }
        }

        ~TcpSocketClient() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
