using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace GraphicsWorld
{
    public class ThumbstickInput : IGamePlayInput
    {
        
        Texture2D thumbsprite;
        Vector2 centerPoint = new Vector2(115, 685);
        Vector2 sensitivity = new Vector2();
        Vector2 restPosition = new Vector2();
        Vector4 bounds = new Vector4();

        int touchId;

        SpriteBatch spriteBatch;

        public ThumbstickInput(SpriteBatch batch)
        {
            spriteBatch = batch;
        }

        public bool MoveForward { get; private set; }
        public bool MoveBackward { get; private set; }
        public bool TurnLeft { get; private set; }
        public bool TurnRight { get; private set; }

        public void Initialize(ContentManager content)
        {
            thumbsprite = content.Load<Texture2D>("thumbstick");
            sensitivity.X = thumbsprite.Width / 4;
            sensitivity.Y = thumbsprite.Height / 4;

            restPosition.X = centerPoint.X - thumbsprite.Width / 2;
            restPosition.Y = centerPoint.Y - thumbsprite.Height / 2;

            bounds.X = centerPoint.X - thumbsprite.Width;
            bounds.Y = centerPoint.Y - thumbsprite.Height;
            bounds.Z = centerPoint.X + thumbsprite.Width;
            bounds.W = centerPoint.Y + thumbsprite.Height;
        }

        public void Update()
        {
            MoveForward = false;
            MoveBackward = false;
            TurnLeft = false;
            TurnRight = false;

            TouchCollection touches = TouchPanel.GetState();
            for (int index = 0; index < touches.Count; index++)
            {
                TouchLocation location = touches[index];
                if (location.State == TouchLocationState.Pressed
                    && touchId == 0
                    && location.Position.X > bounds.X
                    && location.Position.X < bounds.Z
                    && location.Position.Y > bounds.Y
                    && location.Position.Y < bounds.W)
                {
                    touchId = location.Id;
                }
                else if (location.Id == touchId)
                {
                    if (location.State == TouchLocationState.Moved)
                    {
                        MoveForward = location.Position.Y < (centerPoint.Y - sensitivity.Y);
                        MoveBackward = location.Position.Y > (centerPoint.Y + sensitivity.Y);
                        TurnLeft = location.Position.X < (centerPoint.X - sensitivity.X);
                        TurnRight = location.Position.X > (centerPoint.X + sensitivity.X);
                    }
                    else if (location.State == TouchLocationState.Released)
                    {
                        touchId = 0;
                    }
                }
            }
        }

        public void Draw()
        {
            Vector2 position = restPosition;
            if (MoveForward) position.Y -= thumbsprite.Height / 6;
            if (MoveBackward) position.Y += thumbsprite.Height / 6;
            if (TurnLeft) position.X -= thumbsprite.Width / 6;
            if (TurnRight) position.X += thumbsprite.Width / 6;

            spriteBatch.Begin();
            spriteBatch.Draw(thumbsprite, restPosition, Color.Black);
            spriteBatch.Draw(thumbsprite, position, Color.White);
            spriteBatch.End();
        }



        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }
    }
}
