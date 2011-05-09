using System;
using System.Windows;
using System.Windows.Navigation;
using ImageTools;
using ImageTools.Filtering;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using ImageTools.IO.Jpeg;

namespace PhotoExtrasAppExtSampple
{
    public partial class MainPage : PhoneApplicationPage
    {
public MainPage()
{
    Decoders.AddDecoder<BmpDecoder>();
    Decoders.AddDecoder<PngDecoder>();
    Decoders.AddDecoder<GifDecoder>();
    Decoders.AddDecoder<JpegDecoder>();

    InitializeComponent();
}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var queryStrings = this.NavigationContext.QueryString;

            progressbarPanel.Visibility = Visibility.Visible;
            prograssbar.IsIndeterminate = true;

            if (queryStrings.ContainsKey("token"))
            {
                MediaLibrary mediaLib = new MediaLibrary(); 
                Picture picture = mediaLib.GetPictureFromToken(queryStrings["token"]);
                var stream = picture.GetImage();

                ExtendedImage image = new ExtendedImage();
                image.LoadingFailed += new EventHandler<UnhandledExceptionEventArgs>(image_LoadingFailed);
                image.LoadingCompleted += new EventHandler(image_LoadingCompleted);
                image.SetSource(stream);
                
                ImageContainer.Source = image;
            }            
        }

        void image_LoadingCompleted(object sender, EventArgs e)
        {   
          Dispatcher.BeginInvoke(() => {
              progressbarPanel.Visibility = Visibility.Collapsed;
              prograssbar.IsIndeterminate = false;
          }
          );
        }

private void image_LoadingFailed(object sender, UnhandledExceptionEventArgs e)
{
    Dispatcher.BeginInvoke(() => MessageBox.Show(e.ExceptionObject.ToString()));
}

private void ApplyGrayscaleItem_Click(object sender, EventArgs e)
{
    ImageContainer.Filter = new GrayscaleBT709();
}

private void ApplySepiaItem_Click(object sender, EventArgs e)
{
    ImageContainer.Filter = new Sepia();
}

private void ApplyInvertItem_Click(object sender, EventArgs e)
{
    ImageContainer.Filter = new Inverter();
}
    }
}