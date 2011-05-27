using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace GraphicsWorld
{
    public class GestureInput : IGamePlayInput
    {
        Texture2D spriteSheet;

        static readonly Rectangle forwardSrc = new Rectangle(0, 0, 50, 50);
        static readonly Rectangle backwardSrc = new Rectangle(50, 50, 50, 50);
        static readonly Rectangle leftSrc = new Rectangle(0, 50, 50, 50);
        static readonly Rectangle rightSrc = new Rectangle(50, 0, 50, 50);

        Vector2 touchPosition;

        private SpriteBatch spriteBatch;

        public bool MoveForward { get; protected set; }
        public bool MoveBackward { get; protected set; }
        public bool TurnLeft { get; protected set; }
        public bool TurnRight { get; protected set; }

        public GestureInput(SpriteBatch batch)
        {
            spriteBatch = batch;
        }

        public void Initialize(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>("directionals");
        }

        public void Update()
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                touchPosition = gs.Position;
                switch (gs.GestureType)
                {
                    case GestureType.HorizontalDrag:
                        TurnLeft = gs.Delta.X < 0;
                        TurnRight = gs.Delta.X > 0;
                        break;

                    case GestureType.VerticalDrag:
                        MoveForward = gs.Delta.Y < 0;
                        MoveBackward = gs.Delta.Y > 0;
                        break;

                    default:  // should only be DragComplete, but catch everything.
                        TurnLeft = false;
                        TurnRight = false;
                        MoveForward = false;
                        MoveBackward = false;
                        break;
                }
            }
        }

        public void Draw()
        {
            spriteBatch.Begin();

            if (MoveForward)
                spriteBatch.Draw(spriteSheet, touchPosition, forwardSrc, Color.Red);

            if (MoveBackward)
                spriteBatch.Draw(spriteSheet, touchPosition, backwardSrc, Color.Red);

            if (TurnLeft)
                spriteBatch.Draw(spriteSheet, touchPosition, leftSrc, Color.Red);

            if (TurnRight)
                spriteBatch.Draw(spriteSheet, touchPosition, rightSrc, Color.Red);

            spriteBatch.End();
       }

        public void Start()
        {
            TouchPanel.EnabledGestures = GestureType.HorizontalDrag
                | GestureType.VerticalDrag | GestureType.DragComplete;
        }

        public void Stop()
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }
    }
}
