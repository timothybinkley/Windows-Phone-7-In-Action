using Microsoft.Devices.Sensors;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GraphicsWorld
{
    public class MotionInput : IGamePlayInput
    {
        Texture2D spriteSheet;

        Motion motionSensor = new Motion();

        static readonly Rectangle forwardSrc = new Rectangle(0, 0, 50, 50);
        static readonly Rectangle backwardSrc = new Rectangle(50, 50, 50, 50);
        static readonly Rectangle leftSrc = new Rectangle(0, 50, 50, 50);
        static readonly Rectangle rightSrc = new Rectangle(50, 0, 50, 50);

        Vector2 forwardPos;
        Vector2 backwardPos;
        Vector2 leftPos;
        Vector2 rightPos;

        SpriteBatch spriteBatch;

        public MotionInput(SpriteBatch batch)
        {
            spriteBatch = batch;
        }

        public bool MoveForward { get; private set; }
        public bool MoveBackward { get; private set; }
        public bool TurnLeft { get; private set; }
        public bool TurnRight { get; private set; }

        readonly float minimumAngle = MathHelper.ToRadians(30.0f);
        readonly float maximumAngle = MathHelper.ToRadians(330.0f);
        
        public void Initialize(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>("directionals");
            forwardPos.X = TouchPanel.DisplayWidth / 2 - 25;
            forwardPos.Y = 10;

            backwardPos.X = TouchPanel.DisplayWidth / 2 - 25;
            backwardPos.Y = TouchPanel.DisplayHeight - 55;

            leftPos.X = 10;
            leftPos.Y = TouchPanel.DisplayHeight / 2 - 25;

            rightPos.X = TouchPanel.DisplayWidth - 55;
            rightPos.Y = TouchPanel.DisplayHeight / 2 - 25;
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
            var roll = motionSensor.CurrentValue.Attitude.Roll;
            var pitch = motionSensor.CurrentValue.Attitude.Pitch;
            MoveBackward = pitch > minimumAngle && pitch <= MathHelper.Pi;
            MoveForward = pitch > MathHelper.Pi && pitch < maximumAngle;
            TurnRight = roll > minimumAngle && roll <= MathHelper.Pi;
            TurnLeft = roll > MathHelper.Pi && roll < maximumAngle;
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
