using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace VoiceRecorder
{
    public class XnaDispatcherService : IApplicationService
    {
        private DispatcherTimer timer;

        public XnaDispatcherService()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromTicks(333333);
            timer.Tick += Timer_Tick;
            FrameworkDispatcher.Update();
        }
        
        void Timer_Tick(object sender, EventArgs e) 
        { 
            FrameworkDispatcher.Update(); 
        }

        public void StartService(ApplicationServiceContext context)
        {
            timer.Start();
        }

        public void StopService()
        {
            timer.Stop();
        }
    }
}
