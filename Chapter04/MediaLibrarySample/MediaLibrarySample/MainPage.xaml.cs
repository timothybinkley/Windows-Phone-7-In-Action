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
using System.IO;

namespace MediaLibrarySample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            var lib = new MediaLibrary();

            
//foreach (var album in lib.Albums)
//{
//    var albumModel = new AlbumModel();

//    if (album.HasArt)
//    {
//        albumModel.Art = ConvertToBitmapImage(album.GetAlbumArt());
//    }

//    var thumbnail = album.GetThumbnail();
//    if (thumbnail != null)
//    {
//        albumModel.Thumbnail = ConvertToBitmapImage(thumbnail);
//    }

//    albumModel.Name = album.Name;
//    albumModel.Duration = album.Duration;
//    albumModel.ArtistName = album.Artist != null ? album.Artist.Name : string.Empty;
//    albumModel.GenreName = album.Genre != null ? album.Genre.Name : string.Empty; 
//    albumModel.SongsCount = album.Songs.Count;

//    AlbumsListbox.Items.Add(albumModel);
//}


//foreach (var artist in lib.Artists)
//{
//    artist.n
//   // ArtistsListbox.Items.Add(artist.Name);
//}
            //foreach (var genre in lib.Genres)
            //{
            //    GenresListbox.Items.Add(genre.Name);
            //}
//foreach (var picture in lib.Pictures)
//{
//    Console.WriteLine(picture.Name);
//}

//var rootPicAlbum = lib.RootPictureAlbum;


            //foreach (var playlist in lib.Playlists)
            //{
            //    playlist.
            //    PlaylistsListbox.Items.Add(playlist.Name);
            //}
            foreach (var savedPic in lib.SavedPictures)
            {
                //savedPic.Album
                    //savedPic.Date
                        //savedPic.
                //SavedPicturesListbox.Items.Add(savedPic.Name);
            }
            foreach (var song in lib.Songs)
            {
                
                //SongsPicturesListbox.Items.Add(song.Name);
            }            

        }

        private BitmapImage ConvertToBitmapImage(Stream stream)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }
    }

public class AlbumModel
{       
    public BitmapImage Art { get; set; }
    public BitmapImage Thumbnail { get; set; }
    public string Name { get; set; }
    public int SongsCount { get; set; }
    public string ArtistName { get;  set; }
    public string GenreName { get; set; }
    public TimeSpan Duration { get; set; }
}
}