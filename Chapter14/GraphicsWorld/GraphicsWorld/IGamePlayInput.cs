using Microsoft.Xna.Framework.Content;

namespace GraphicsWorld
{
    public interface IGamePlayInput
    {
        bool MoveForward { get; }
        bool MoveBackward { get; }
        bool TurnLeft { get; }
        bool TurnRight { get; }

        void Initialize(ContentManager content);
        void Start();
        void Stop();
        void Update();
        void Draw();
    }

}
