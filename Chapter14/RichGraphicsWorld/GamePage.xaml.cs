using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Windows;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Navigation;

namespace RichGraphicsWorld
{
    public partial class GamePage : PhoneApplicationPage
    {
        GameTimer timer;
        //GamePlayComponent gamePlay = new GamePlayComponent();
        TemporaryGamePlayComponent gamePlay = new TemporaryGamePlayComponent();
        DemoInput input;
        bool drawCalledAfterMotionStopped = false;

        public GamePage()
        {
            InitializeComponent();

            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.SetSharingMode(true);
            ContentManager content = ((App)(Application.Current)).Content;
            //spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            input = new DemoInput();
            gamePlay.Initialize(content, input);
            
            timer.Start();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }
                
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            input.Update();
            gamePlay.Update();
            if (!input.MoveForward && !input.MoveBackward
                && !input.TurnLeft && !input.TurnRight)
            {
                if (drawCalledAfterMotionStopped)
                {
                    GameTimer.SuppressFrame();
                }
            }
            else
            {
                drawCalledAfterMotionStopped = false;
            }
            
            if (!gamePlay.IsPlaying && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            var device = SharedGraphicsDeviceManager.Current.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);
            
            drawCalledAfterMotionStopped = true;
            gamePlay.Draw();
            input.Draw();
        }
    }
}