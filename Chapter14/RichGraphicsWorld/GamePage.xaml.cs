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
            bool updateOccurred = gamePlay.Update();
            if (!updateOccurred)
                GameTimer.SuppressFrame();
            
            if (!gamePlay.IsRunning && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            var device = SharedGraphicsDeviceManager.Current.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);

            gamePlay.Draw();
            input.Draw();
        }
    }
}