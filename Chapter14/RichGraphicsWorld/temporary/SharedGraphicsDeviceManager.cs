using System.Windows;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    public class SharedGraphicsDeviceManager : IApplicationService
    {
        public static SharedGraphicsDeviceManager Current = new SharedGraphicsDeviceManager();

        public GraphicsDevice GraphicsDevice;

        public void SetSharingMode(bool mode) { }


        public void StartService(ApplicationServiceContext context) { }
        public void StopService() { }
    }
}
