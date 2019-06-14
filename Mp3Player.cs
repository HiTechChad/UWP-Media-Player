using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MediaPlayer
{
    class Mp3Player
    {
        public Artist CurrentArtist = null;
        public Album CurrentAlbum = null;
        public Song CurrantSong = null;
        public Playlist CurrentPlaylist = null;
        private int CurrantSongIndex = 0;


        public List<Artist> artists { get; set; }
        public List<Song> songs { get; set; }
        public List<Playlist> playlists { get; set; }

        //Xaml UI elements
        public ListViewState listViewState;
        public MediaElement mPlayer { get; set; }
        public Image image { get; set; }
        public TextBlock title { get; set; }
        public TextBlock artis { get; set; }

        public bool Repeat = false;
        public bool Shuffel = false;

        public void SetListViewState(ListViewState listview)
        {
            listViewState = listview;
        }

        public void SongHasEnded()
        {
            if (listViewState.GetState().Equals("Album"))
            {
                TriggerSongPlay(CurrentAlbum.songs[NextSongLogic()]);
            }
            if (listViewState.GetState().Equals("Songs"))
            {
                TriggerSongPlay(songs[NextSongLogic()]);
            }

        }
        public void TriggerSongPlay(Song song)
        {
            CurrantSong = song;
            CurrantSongIndex = GetSongIndex(CurrantSong);
            song.Play(mPlayer, image, title, artis);
        }

        private int NextSongLogic()
        {
            if (Repeat)
            {
                //do nothing
            }
            else if (Shuffel)
            {
                if (listViewState.GetState().Equals("Songs"))
                {
                    Random random = new Random();
                    CurrantSongIndex = random.Next(0, songs.Count() - 1);
                }
                if (listViewState.GetState().Equals("Album"))
                {
                    Random random = new Random();
                    CurrantSongIndex = random.Next(0, CurrentAlbum.songs.Count() - 1);
                }

            }
            else
            {
                if (listViewState.GetState().Equals("Songs") && CurrantSongIndex < songs.Count() - 1)
                {
                    ++CurrantSongIndex;
                }
                else if (listViewState.GetState().Equals("Album") && CurrantSongIndex < CurrentAlbum.songs.Count() - 1)
                {
                    ++CurrantSongIndex;
                }
                else
                {
                    CurrantSongIndex = 0;
                }
            }
            return CurrantSongIndex;
        }
        private int GetSongIndex(Song song)
        {
            int index = 0;
            if (listViewState.GetState().Equals("Album"))
            {
                index = CurrentAlbum.songs.IndexOf(song);
            }
            if (listViewState.GetState().Equals("Songs"))
            {
                index = songs.IndexOf(song);
            }
            return index;
        }
    }
}
