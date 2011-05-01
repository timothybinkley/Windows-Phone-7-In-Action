using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using System.Linq;

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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            IDictionary<string, string> queryStrings =
               this.NavigationContext.QueryString;

            string token = null;
            string source = null;
            string action = null;
            if (queryStrings.ContainsKey("token"))
            {
                token = queryStrings["token"];
                source = "extras";
            }
            else if (queryStrings.ContainsKey("FileId"))
            {
                token = queryStrings["FileId"];
                if (queryStrings.ContainsKey("Action"))
                    action = queryStrings["Action"];
                source = "share";
            }

            if (!string.IsNullOrEmpty(token))
            {
                MediaLibrary mediaLib = new MediaLibrary();
                Picture picture = mediaLib.GetPictureFromToken(token);
                currentImage = ImageUtil.GetBitmap(picture.GetImage());
                photoContainer.Fill = new ImageBrush { ImageSource = currentImage };
                imageDetails.Text = string.Format("Image from PhotoHub {0} {1}.\r\nPicture name:\r\n{2}\r\nMedia library token:\r\n{3}",
                    source, action, picture.Name, token);
            }
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
                //using (IsolatedStorageFile storage =
                //IsolatedStorageFile.GetUserStoreForApplication())
                //{
                //    using (IsolatedStorageFileStream stream =
                //        storage.CreateFile(@"customphoto.jpg"))
                //    {
                //        currentImage.SaveJpeg(stream, currentImage.PixelWidth, currentImage.PixelHeight, 0, 100);
                //    }
                //}
                var stream = new MemoryStream();
                currentImage.SaveJpeg(stream, currentImage.PixelWidth, currentImage.PixelHeight, 0, 100);
                stream.Seek(0, 0); 
                var library = new MediaLibrary();
                Picture p = library.SavePicture("customphoto.jpg", stream);
                imageDetails.Text = string.Format("Image saved to media library.\r\nFilename:\r\ncustomphoto.jpg");
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(@"customphoto.jpg"))
                {
                    // load the data file into the contacts list.
                    using (IsolatedStorageFileStream stream =
                        storage.OpenFile(@"customphoto.jpg", FileMode.Open))
                    {
                        currentImage = ImageUtil.GetBitmap(stream);
                        photoContainer.Fill = new ImageBrush { ImageSource = currentImage };
                    }
                }
                imageDetails.Text = string.Format("Image loaded.\r\nFilename:\r\ncustomphoto.jpg");
            }
        }

        private void OpenFromLibrary_Click(object sender, EventArgs e)
        {
            var library = new MediaLibrary();
            var pictures = library.SavedPictures;
            
            // find customphoto.jpeg
            var picture = pictures.FirstOrDefault(item => item.Name == "customphoto.jpg");
            if (picture != null)
            {
                var bitmap = new BitmapImage();
                currentImage = ImageUtil.GetBitmap(picture.GetImage());
                photoContainer.Fill = new ImageBrush { ImageSource = currentImage };
                imageDetails.Text = string.Format("Image from Media Library.\r\nPicture name:\r\n{0}", picture.Name);
            }
            else
            {
                photoContainer.Fill = new SolidColorBrush(Colors.Gray);
                imageDetails.Text = "Choose an image source from the menu.";
            }
        }
    }
}