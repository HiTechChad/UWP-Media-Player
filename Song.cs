using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MediaPlayer
{
    class Song {
        public Artist Artist { get; set; }
        public string Name { get; set; }
        public Album Album { get; set; }
        public StorageFile File { get; set; }
        public TimeSpan Duration { get; set; }
        public readonly string Type = "Song";


        public async void Play(MediaElement mPlayer, Image image, TextBlock title, TextBlock artist)
        {
            var stream = await File.OpenAsync(Windows.Storage.FileAccessMode.Read);
            mPlayer.SetSource(stream, File.ContentType);

            StorageItemThumbnail thumbnail = await File.GetThumbnailAsync(ThumbnailMode.MusicView, 1000);
            BitmapImage art = new BitmapImage();
            art.SetSource(thumbnail);

            mPlayer.Play();
            image.Source = art;
            title.Text = Name;
            artist.Text = Artist.Name;
        }
    }
}
