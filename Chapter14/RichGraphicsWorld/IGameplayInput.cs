using System;

namespace RichGraphicsWorld
{
    public interface IGameplayInput  
    {
        bool MoveForward { get; }
        bool MoveBackward { get; }
        bool TurnLeft { get; }
        bool TurnRight { get; }

        void Start();
        void Update();
        void Draw();
        void Stop();

    }
}
