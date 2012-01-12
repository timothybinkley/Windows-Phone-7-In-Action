using Microsoft.Devices.Sensors;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraphicsWorld
{
    public class MotionInput : IGamePlayInput
    {
        Texture2D spriteSheet;

        Motion motionSensor = new Motion();

        readonly float minimumVector = (float)Math.Sin(MathHelper.PiOver4 / 3.0f); // 15 degrees
        readonly float maximumVector = (float)Math.Sin(MathHelper.PiOver4); // 45 degrees

        static readonly Rectangle forwardSrc = new Rectangle(0, 0, 50, 50);
        static readonly Rectangle backwardSrc = new Rectangle(50, 50, 50, 50);
        static readonly Rectangle leftSrc = new Rectangle(0, 50, 50, 50);
        static readonly Rectangle rightSrc = new Rectangle(50, 0, 50, 50);

        Vector2 forwardPos = new Vector2(215.0f, 82.0f);
        Vector2 backwardPos = new Vector2(215.0f, 745.0f);
        Vector2 leftPos = new Vector2(10.0f, 375.0f);
        Vector2 rightPos = new Vector2(425.0f, 375.0f);

        SpriteBatch spriteBatch;

        public MotionInput(SpriteBatch batch)
        {
            spriteBatch = batch;
        }

        public bool MoveForward { get; private set; }
        public bool MoveBackward { get; private set; }
        public bool TurnLeft { get; private set; }
        public bool TurnRight { get; private set; }

        public void Initialize(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>("directionals");
        }

        public void Start()
        {
            motionSensor.Start();
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        public void Stop()
        {
            motionSensor.Stop();
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        public void Update()
        {
            var x = motionSensor.CurrentValue.Gravity.X;
            var y = motionSensor.CurrentValue.Gravity.Y;

            MoveBackward = y < -minimumVector && y >= -maximumVector;
            MoveForward = y > minimumVector && y < maximumVector;
            TurnRight = x > minimumVector && x <= maximumVector;
            TurnLeft = x < -minimumVector && x >= -maximumVector;
        }

        public void Draw()
        {
            spriteBatch.Begin();

            if (MoveForward)
                spriteBatch.Draw(spriteSheet, forwardPos, forwardSrc, Color.Red);

            if (MoveBackward)
                spriteBatch.Draw(spriteSheet, backwardPos, backwardSrc, Color.Red);

            if (TurnLeft)
                spriteBatch.Draw(spriteSheet, leftPos, leftSrc, Color.Red);

            if (TurnRight)
                spriteBatch.Draw(spriteSheet, rightPos, rightSrc, Color.Red);

            spriteBatch.End();
        }
    }
}
