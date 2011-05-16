using System;
using System.Windows.Threading;

namespace Microsoft.Xna.Framework
{
    public sealed class GameTimerEventArgs : EventArgs
    {
        public TimeSpan ElapsedTime { get; internal set; }
        public bool IsRunningSlowly { get; internal set; }
        public TimeSpan TotalTime { get; internal set; }
    }

    public sealed class GameTimer : IDisposable
    {
        public GameTimer()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(33) };
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Update(this, null);
            Draw(this, null);
        }

        public event EventHandler<GameTimerEventArgs> Draw;
        public event EventHandler<EventArgs> FrameAction;
        public event EventHandler<GameTimerEventArgs> Update;

        public int FrameActionOrder { get; set; }
        public TimeSpan UpdateInterval { get; set; }
        public int UpdateOrder { get; set; }
        public int DrawOrder { get; set; }

        public static void ResetElapsedTime() { }
        public static void SuppressFrame() { }

        DispatcherTimer timer;

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
        public void Dispose() { }
    }

}
