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

namespace ChatTcpUnicast {
    public class TcpSocketClient : IDisposable {
        private const int PORT = 22222;
        private Socket socket;
        private bool disposed = false;

       

         #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.socket.Dispose();
                }
                disposed = true;
            }
        }

        ~TcpSocketClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
