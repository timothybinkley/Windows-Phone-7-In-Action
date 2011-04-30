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

namespace RichGraphicsWorld
{
    public class ServiceProvider : IServiceProvider
    {

        public object GetService(Type serviceType)
        {
            System.Diagnostics.Debug.WriteLine("Looking for service of type: " + serviceType.Name);
            return null;
        }
    }
}
