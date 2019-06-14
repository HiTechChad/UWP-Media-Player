using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace MediaPlayer
{
    class Playlist
    {
        public string Name { get; set; }
        public readonly string Type = "Playlist";
        public List<Song> songs = new List<Song>();

        public void AddSong(Song song)
        {
            songs.Add(song);
        }
    }
}
