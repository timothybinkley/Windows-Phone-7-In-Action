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
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;

namespace MediaLibraryPicSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        private PictureCollection picCollection;
        private int index;

        public MainPage()
        {
            InitializeComponent();

            var lib = new MediaLibrary();
            picCollection = lib.Pictures;

            if (picCollection.Count > 0)
            {
                index = 0;
                SetPicture(picCollection[index]);
                SetLabels(picCollection[index]);
            }
        }

        private void SetPicture(Picture pic)
        {
            var bitmap = new BitmapImage();
            bitmap.SetSource(pic.GetImage());
            imageControl.Source = bitmap;
        }
        private void SetLabels(Picture pic)
        {
            nameTextblock.Text = string.Format("Name : {0}", pic.Name);
            dateTextblock.Text = string.Format("Date : {0}", pic.Date.ToShortDateString());
            dimensionTextblock.Text = string.Format("Dimension : {0}x{1}", pic.Width, pic.Height);
            albumTextblock.Text = string.Format("Album : {0}", pic.Album != null ? pic.Album.Name : string.Empty);
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (index < picCollection.Count - 1) index++;
            SetPicture(picCollection[index]);
            SetLabels(picCollection[index]);
        }
        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (index > 0) index--;
            SetPicture(picCollection[index]);
            SetLabels(picCollection[index]);
        }
    }
}