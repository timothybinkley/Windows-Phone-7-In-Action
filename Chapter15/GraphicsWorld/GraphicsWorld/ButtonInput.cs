using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace GraphicsWorld
{
    public class ButtonInput : IGamePlayInput
    {
        Texture2D spriteSheet;
              
        static readonly Rectangle aSrc = new Rectangle(0, 75, 75, 75);
        static readonly Rectangle bSrc = new Rectangle(75, 75, 75, 75);
        static readonly Rectangle xSrc = new Rectangle(0, 0, 75, 75);
        static readonly Rectangle ySrc = new Rectangle(75, 0, 75, 75);

        static readonly Point centerPoint = new Point(320, 640);

        Rectangle aPosition = new Rectangle(0, 0, 75, 75);
        Rectangle bPosition = new Rectangle(0, 0, 75, 75);
        Rectangle xPosition = new Rectangle(0, 0, 75, 75);
        Rectangle yPosition = new Rectangle(0, 0, 75, 75);

        Rectangle aPosition2 = new Rectangle(0, 0, 65, 65);
        Rectangle bPosition2 = new Rectangle(0, 0, 65, 65);
        Rectangle xPosition2 = new Rectangle(0, 0, 65, 65);
        Rectangle yPosition2 = new Rectangle(0, 0, 65, 65);

        SpriteBatch spriteBatch;

        public ButtonInput(SpriteBatch batch)
        {
            spriteBatch = batch;
        }

        public bool MoveForward { get; protected set; }
        public bool MoveBackward { get; protected set; }
        public bool TurnLeft { get; protected set; }
        public bool TurnRight { get; protected set; }
        
        public void Initialize(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>("buttons");
            
            aPosition.X = centerPoint.X;
            aPosition.Y = centerPoint.Y + 80;
            aPosition2.X = aPosition.X + 5;
            aPosition2.Y = aPosition.Y + 5;

            bPosition.X = centerPoint.X + 80;
            bPosition.Y = centerPoint.Y;
            bPosition2.X = bPosition.X + 5;
            bPosition2.Y = bPosition.Y + 5;

            xPosition.X = centerPoint.X - 80;
            xPosition.Y = centerPoint.Y;
            xPosition2.X = xPosition.X + 5;
            xPosition2.Y = xPosition.Y + 5;

            yPosition.X = centerPoint.X;
            yPosition.Y = centerPoint.Y - 80;
            yPosition2.X = yPosition.X + 5;
            yPosition2.Y = yPosition.Y + 5;
        }

        public void Update()
        {
            MouseState location = Mouse.GetState();

            if (location.LeftButton == ButtonState.Pressed)
            {
                MoveForward = yPosition.Contains(location.X, location.Y);
                MoveBackward = aPosition.Contains(location.X, location.Y);
                TurnLeft = xPosition.Contains(location.X, location.Y);
                TurnRight = bPosition.Contains(location.X, location.Y);
            }
            else
            {
                MoveForward = false;
                MoveBackward = false;
                TurnLeft = false;
                TurnRight = false;
            }
        }

        public void Draw()
        {
            spriteBatch.Begin();

            spriteBatch.Draw(spriteSheet, yPosition, ySrc, Color.White);
            if (MoveForward)
                spriteBatch.Draw(spriteSheet, yPosition2, ySrc, Color.White);

            spriteBatch.Draw(spriteSheet, aPosition, aSrc, Color.White);
            if (MoveBackward)
                spriteBatch.Draw(spriteSheet, aPosition2, aSrc, Color.White);

            spriteBatch.Draw(spriteSheet, xPosition, xSrc, Color.White);
            if (TurnLeft)
                spriteBatch.Draw(spriteSheet, xPosition2, xSrc, Color.White);

            spriteBatch.Draw(spriteSheet, bPosition, bSrc, Color.White);
            if (TurnRight)
                spriteBatch.Draw(spriteSheet, bPosition2, bSrc, Color.White);

            spriteBatch.End();
        }


        public void Start() {}
        
        public void Stop() {}
    }
}
