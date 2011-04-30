using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Windows;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RichGraphicsWorld
{
    public partial class GamePage : PhoneApplicationPage
    {
        GameTimer timer;
        GamePlayComponent gamePlay;
        SpriteBatch spriteBatch;

        public GamePage()
        {
            InitializeComponent();
            gamePlay = new GamePlayComponent();

            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.SetSharingMode(true);
            ContentManager content = ((App)(Application.Current)).Content;
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            gamePlay.Initialize(content, null);

            timer.Start();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }
                
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}