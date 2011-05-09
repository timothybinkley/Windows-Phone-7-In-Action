using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using ExifLib;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace CameraCaptureTaskSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        Stream capturedImage;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                
                var angle = GetAngleFromChosenPhoto(e.ChosenPhoto, e.OriginalFileName);

                if (angle > 0d)
                {
                    capturedImage = RotateStream(e.ChosenPhoto, angle);
                }
                else
                {
                    capturedImage = e.ChosenPhoto;
                }

                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(capturedImage);

                imageControl.Source = bmp;
                
            }
        }
        private int GetAngleFromChosenPhoto(Stream stream, string fileName)
        {
            var angle = 0;

            stream.Position = 0;
            JpegInfo info = ExifReader.ReadJpeg(stream, fileName);

            switch (info.Orientation)
            {
                case ExifOrientation.TopLeft:
                case ExifOrientation.Undefined:
                    angle = 0;
                    break;
                case ExifOrientation.TopRight:
                    angle = 90;
                    break;
                case ExifOrientation.BottomRight:
                    angle = 180;
                    break;
                case ExifOrientation.BottomLeft:
                    angle = 270;
                    break;
            }            
            return angle;
        }
        private Stream RotateStream(Stream stream, int angle)
        {
            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();
            if (angle % 360 == 0) return stream;

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);

            WriteableBitmap wbSource = new WriteableBitmap(bitmap);
            WriteableBitmap wbTarget = null;

            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }
            MemoryStream targetStream = new MemoryStream();
            wbTarget.SaveJpeg(targetStream, wbTarget.PixelWidth, wbTarget.PixelHeight, 0, 100);
            return targetStream;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);
            cameraCaptureTask.Show();
        }

        private void saveToPhoneItem_Click(object sender, EventArgs e)
        {
if (capturedImage != null)
{
    var lib = new MediaLibrary();
    capturedImage.Seek(0, 0);
    Picture p = lib.SavePicture(Guid.NewGuid().ToString(), capturedImage);
    MessageBox.Show("Your photo has been saved.");
}
        }

       // private void 
    }
}