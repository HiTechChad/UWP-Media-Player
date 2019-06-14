using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace MediaPlayer
{
    class Album{

        public List<Song> songs = new List<Song>();
        public Artist artist { get; set; }
        public string Name { get; set; }
        public readonly string Type = "Album";

        public void AddSong(MusicProperties fp, Artist artist, StorageFile file) {
            

            songs.Add(new Song { Artist=artist, Album=this, Name=fp.Title, Duration=fp.Duration, File=file});
        }
    }
}
