using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MediaPlayer
{
    class ListViewState
    {
        private string State = "Main";
        private Artist curListArtist;
        private Album curListAlbum;
        private Song curlistSong;
        
        //ListView Element
        public ListView slist { get; set; }



        //Mp3Player Class
        public Mp3Player mediaplayer { get; set; }

        

        public void UpdateListView(Object clickedItem)
        {
            mediaplayer.SetListViewState(this);
            // Display all artists || songs || playlists
            if (clickedItem.ToString().Equals("Artists"))
            {
                UpdateArtists();
                Debug.WriteLine(mediaplayer.artists.Count());

            }
            else if (clickedItem.ToString().Equals("Songs"))
            {
                UpdateAllSongs();

            }
            else if (clickedItem.ToString().Equals("Playlists"))
            {
                UpdatePlaylists();
                Debug.WriteLine(mediaplayer.playlists.Count());

            }


            //Click forward logic
            ForwardClickLogic(clickedItem);
        }

        //Song Play
        
        //Xaml UI Updates
        private void UpdateArtists()
        {
            slist.Items.Clear();
            foreach (Artist artist in mediaplayer.artists)
            {
                slist.Items.Add(artist);
                State = "Artists";
            }
        }
        private void UpdateArtistAlbums(List<Album> albums)
        {
            slist.Items.Clear();
            foreach (Album album in albums)
            {
                slist.Items.Add(album);
                State = "Albums";
                mediaplayer.CurrentArtist = album.artist;
                curListArtist = album.artist;
            }
        }
        private void UpdateAlbumSongs(List<Song> songs)
        {
            slist.Items.Clear();
            foreach (Song song in songs)
            {
                slist.Items.Add(song);
                State = "Album";
                mediaplayer.CurrentAlbum = song.Album;
                curListAlbum = song.Album;
            }
        }
        private void UpdateAllSongs()
        {
            slist.Items.Clear();
            foreach (Song song in mediaplayer.songs)
            {
                slist.Items.Add(song);
                State = "Songs";
            }
        }
        private void UpdatePlaylists()
        {
            slist.Items.Clear();
            foreach (Playlist playlist in mediaplayer.playlists)
            {
                slist.Items.Add(playlist);
                State = "Plalists";
            }
        }
        public void UpdateMainListView()
        {
            slist.Items.Clear();
            slist.Items.Add(new Bodge {Name = "Artists" });
            slist.Items.Add(new Bodge { Name = "Songs" });
            slist.Items.Add(new Bodge { Name = "Playlists" });
        }
        
        //Click Forward Logic
        private void ForwardClickLogic(Object clickedItem)
        {
            Debug.WriteLine(clickedItem.GetType());

            if (State.Equals("Artists") && clickedItem is Artist)
            {
                Debug.WriteLine(clickedItem.GetType());
                foreach (Artist artist in mediaplayer.artists)
                {
                    if (clickedItem.Equals(artist))
                    {
                        UpdateArtistAlbums(artist.Albums);
                    }
                }
            }
            if (State.Equals("Albums") && clickedItem is Album)
            {
                foreach (Album album in curListArtist.Albums)
                {
                    if (clickedItem.Equals(album))
                    {
                        UpdateAlbumSongs(album.songs);
                    }
                }
            }
            if (State.Equals("Album") && clickedItem is Song)
            {
                foreach (Song song in curListAlbum.songs)
                {
                    if (clickedItem.Equals(song))
                    {
                        mediaplayer.TriggerSongPlay(song);
                    }
                }
            }
            if (State.Equals("Songs") && clickedItem is Song)
            {
                foreach (Song song in mediaplayer.songs)
                {
                    if (clickedItem.Equals(song))
                    {
                        mediaplayer.TriggerSongPlay(song);
                    }
                }
            }

        }

        //Click back logic
        public void BackButton()
        {
            BackButtonLogic();
        }
        private void BackButtonLogic()
        {
            switch (State)
            {
                case "Main":
                    break;
                case "Artists":
                    UpdateMainListView();
                    break;
                case "Songs":
                    UpdateMainListView();
                    break;
                case "Playlists":
                    UpdateMainListView();
                    break;
                case "Albums":
                    UpdateArtists();
                    break;
                case "Album":
                    UpdateArtistAlbums(curListArtist.Albums);
                    break;
                default:
                    UpdateMainListView();
                    break;
            }
        }

        //get listview state
        public string GetState()
        {
            return State;
        }

    }
}
