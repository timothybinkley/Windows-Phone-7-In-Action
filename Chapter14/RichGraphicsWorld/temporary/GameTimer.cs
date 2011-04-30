using System;

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
        public GameTimer(){}

        public event EventHandler<GameTimerEventArgs> Draw;
        public event EventHandler<EventArgs> FrameAction;
        public event EventHandler<GameTimerEventArgs> Update;

        public int FrameActionOrder { get; set; }
        public TimeSpan UpdateInterval { get; set; }
        public int UpdateOrder { get; set; }
        public int DrawOrder { get; set; }

        public static void ResetElapsedTime(){}
        public static void SuppressFrame(){}

        public void Start(){}
        public void Stop(){}
        public void Dispose(){}
    }

}
