using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace PhotoEditor
{
    public partial class MainPage : PhoneApplicationPage
    {
        private WriteableBitmap currentImage;
        private Object camera; //private PhotoCamera camera;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Choose_Click(object sender, EventArgs e)
        {
            var task = new PhotoChooserTask();
            task.ShowCamera = true;
            task.Completed += new EventHandler<PhotoResult>(phoneChooserTask_Completed);
            task.Show();
        }

        void phoneChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                currentImage = ImageUtil.GetBitmap(e.ChosenPhoto);
                photoContainer.Fill = new ImageBrush { ImageSource = currentImage };
                imageDetails.Text = string.Format("Image from PhotoChooserTask\r\nOriginal filename:\r\n{0}", e.OriginalFileName);
            }
        }

        private void Capture_Click(object sender, EventArgs e)
        {
            var task = new CameraCaptureTask();
            task.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);
            task.Show();
        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var angle = ImageUtil.GetAngleFromChosenPhoto(e.ChosenPhoto, e.OriginalFileName);
                currentImage = ImageUtil.GetBitmap(e.ChosenPhoto, angle);
                photoContainer.Fill = new ImageBrush { ImageSource = currentImage };
                imageDetails.Text = string.Format("Image from CameraCaptureTask.\r\nImage was rotated {0} degrees.\r\nOriginal filename:\r\n{0}", angle, e.OriginalFileName);
            }
        }



        private void Camera_Click(object sender, EventArgs e)
        {

            if (camera == null)
            {
                currentImage = null;
                imageDetails.Text = string.Format("Choose custom camera again to close camera. Use the hardware buttons to take a picture.");
                camera = new Object(); //    camera = new PhotoCamera();
            //    TODO: wire up the appropriate hardware buttons to call event handlers
            //    camera.ButtonFullPress += camera_ButtonFullPress
            //    camera.CaptureImageCompleted += camera_CaptureImageCompleted
            //    camera.Start();
            //
            //    var brush = new VideoBrush();
            //    brush.SetSource(camera);
                photoContainer.Fill = new SolidColorBrush(Colors.Gray); //    photoContainer.Fill = brush;
            }
            else
            {
                photoContainer.Fill = new SolidColorBrush(Colors.Gray);
                imageDetails.Text = "Choose an image source from the menu.";
            
            //    camera.ButtonFullPress -= camera_ButtonFullPress
            //    camera.CaptureImageCompleted -= camera_CaptureImageCompleted
            //    camera.Stop();
                camera = null;
            }
        }

        //private void camera_ButtonFullPress(object sender, EventArgs e)
        //{
        //    camera.CaptureImageAsync();
        //}

        //void camera_CaptureImageCompleted(object sender, CaptureImageCompletedEventArgs e)
        //{
        //    currentImage = e.Result;
        //    photoContainer.Fill = new ImageBrush { ImageSource = e.Result };
        //    imageDetails.Text = string.Format("Image from PhotoEditor.");
        //    camera.Stop();
        //    camera = null;
        //}

        private void Edit_Click(object sender, EventArgs e)
        {
            if (currentImage != null)
            {
                var transform = new ScaleTransform
                {
                    ScaleX = currentImage.PixelWidth / photoBorder.ActualWidth,
                    ScaleY = currentImage.PixelHeight / photoBorder.ActualHeight
                };
                currentImage.Render(photoBorder, transform);
                currentImage.Invalidate();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (currentImage != null)
            {
                using (IsolatedStorageFile storage =
                IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream =
                        storage.CreateFile(@"customphoto.jpeg"))
                    {
                        currentImage.SaveJpeg(stream, currentImage.PixelWidth, currentImage.PixelHeight, 0, 100);
                    }
                }
                imageDetails.Text = string.Format("Image saved.\r\nFilename:\r\ncustomphoto.jpeg");
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(@"customphoto.jpeg"))
                {
                    // load the data file into the contacts list.
                    using (IsolatedStorageFileStream stream =
                        storage.OpenFile(@"customphoto.jpeg", FileMode.Open))
                    {
                        currentImage = ImageUtil.GetBitmap(stream);
                        photoContainer.Fill = new ImageBrush { ImageSource = currentImage };
                    }
                }
                imageDetails.Text = string.Format("Image loaded.\r\nFilename:\r\ncustomphoto.jpeg");
            }
        }
    }
}