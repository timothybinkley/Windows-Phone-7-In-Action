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
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace MicrophoneSample
{
public class XNAFrameworkDispatcherService : IApplicationService
{
    private DispatcherTimer frameworkDispatcherTimer;

    public XNAFrameworkDispatcherService()
    {
        this.frameworkDispatcherTimer = new DispatcherTimer();
        this.frameworkDispatcherTimer.Interval = TimeSpan.FromTicks(333333);
        this.frameworkDispatcherTimer.Tick += frameworkDispatcherTimer_Tick;
        FrameworkDispatcher.Update();
    }
    void frameworkDispatcherTimer_Tick(object sender, EventArgs e) { FrameworkDispatcher.Update(); }

    void IApplicationService.StartService(ApplicationServiceContext context) { this.frameworkDispatcherTimer.Start(); }

    void IApplicationService.StopService() { this.frameworkDispatcherTimer.Stop(); }
}
}
