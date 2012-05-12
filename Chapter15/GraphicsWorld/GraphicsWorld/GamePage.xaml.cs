using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsWorld
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        GamePlayComponent gamePlay = new GamePlayComponent();
        IGamePlayInput input;
        int frameCountAfterMotionStopped;

        UIElementRenderer scoreboardRenderer;
        Vector2 scoreboardPosition = new Vector2(12, 12);

        UIElementRenderer buttonRenderer;
        Vector2 buttonPosition = new Vector2(115, 364);
        
        bool isPaused;

        public static Uri BuildNavigationUri(int input)
        {
            return new Uri("/GamePage.xaml?Input=" + input, UriKind.Relative);
        }

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            scoreboardRenderer = new UIElementRenderer(scoreboard, 456, 60);
            buttonRenderer = new UIElementRenderer(resumeButton, 250, 72);
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            string inputType = this.NavigationContext.QueryString["Input"];
            switch (inputType)
            {
                case "1":
                    input = new ThumbstickInput(spriteBatch);
                    break;
                case "2":
                    input = new ButtonInput(spriteBatch);
                    break;
                case "3":
                    input = new GestureInput(spriteBatch);
                    break;
                case "4":
                    input = new MotionInput(spriteBatch);
                    break;
                default:
                    input = new DemoInput();
                    break;
            }
            input.Initialize(contentManager);             

            gamePlay.Initialize(contentManager, input);

            // Start the timer
            timer.Start();

            input.Start();  

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            input.Stop();

            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            if (isPaused)
                return;

            input.Update();
            gamePlay.Update();
            score.Text = gamePlay.Score.ToString();
            if (!input.MoveForward && !input.MoveBackward
                && !input.TurnLeft && !input.TurnRight)
            {
                if (frameCountAfterMotionStopped > 2)
                {
                    GameTimer.SuppressFrame();
                }
            }
            else
            {
                frameCountAfterMotionStopped = 0;
            }

            if (!gamePlay.IsPlaying && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            scoreboardRenderer.Render();
            if (isPaused) 
                buttonRenderer.Render();

            var device = SharedGraphicsDeviceManager.Current.GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.BlendState = BlendState.AlphaBlend;
            device.Clear(Color.CornflowerBlue); 
            gamePlay.Draw();
            input.Draw();
    
            spriteBatch.Begin();
            spriteBatch.Draw(scoreboardRenderer.Texture, scoreboardPosition, Color.White);
            if (isPaused)
                spriteBatch.Draw(buttonRenderer.Texture, buttonPosition, Color.White);
            spriteBatch.End();
        }
        
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (!isPaused)
            {
                isPaused = true;
                resumeButton.IsEnabled = true;
                e.Cancel = true;
            }
            base.OnBackKeyPress(e);
        }

        private void resume_Click(object sender, RoutedEventArgs e)
        {
            resumeButton.IsEnabled = false;
            isPaused = false;
        }

    }
}
