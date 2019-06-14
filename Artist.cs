using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace MediaPlayer
{
    class Artist{
        public string Name { get; set; }
        public List<Album> Albums = new List<Album>();

        public async void AddToAlbum(StorageFile file) {
            MusicProperties fp = await file.Properties.GetMusicPropertiesAsync();
            //check if album already exists
            bool addedToAlbum = false;
            //find album and add song to it
            foreach(Album album in Albums) {
                if (album.Name.Equals(fp.Album)) {
                    album.AddSong(fp, this, file);
                    addedToAlbum = true;
                } 
            }
            // if song was not added to album create new album and add song
            if (!addedToAlbum) {
                Albums.Add(new Album { artist = this, Name = fp.Album });
                Albums[Albums.Count - 1].AddSong(fp, this, file);
            }
        }
    }
}
