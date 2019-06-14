using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MediaPlayer
{

    public sealed partial class MainPage : Page {


        private List<Artist> artists = new List<Artist>();
        public string keyLargeObject = "large";

        private List<Playlist> playlists = new List<Playlist>();
        private List<Song> allsongs = new List<Song>();
        //private IReadOnlyList<StorageFile> Files;
        private int curSongIndex = 0;
        private int curAlbumIndex = 0;
        private int curArtistIndex = 0;
        private String curListState = "Main";
        private ListViewState listviewstate;
        private Mp3Player mp3Player;

        public MainPage() {
            this.InitializeComponent();
            slist.ItemClick += Slist_ItemClick;
            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;
            mp3Player = new Mp3Player { mPlayer = _mediaPlayer, artis = _Artist, image = _AlbumArt, artists = artists, playlists = playlists, songs = allsongs, title = _Title };
            listviewstate = new ListViewState { slist= slist, mediaplayer = mp3Player };
        }
        
        private void _mediaPlayer_MediaEnded(object sender, RoutedEventArgs e) {
            mp3Player.SongHasEnded();
        }

        private void Slist_ItemClick(object sender, ItemClickEventArgs e) {
            listviewstate.UpdateListView(e.ClickedItem);
    
        }

        async private void Button_Click(object sender, RoutedEventArgs e) {
            
            await SetLocalMedia();
            listviewstate.UpdateMainListView();
            foreach (Artist artist in artists)
            {
                foreach (Album album in artist.Albums)
                {
                    foreach (Song song in album.songs)
                    {
                        allsongs.Add(song);
                    }
                }
            }
        }

        async private Task SetLocalMedia()
        {
            //Open File Explorer to Pick Music Folder
            var openPicker = new FolderPicker();
            openPicker.FileTypeFilter.Add("*");
            StorageFolder musicFolder = await openPicker.PickSingleFolderAsync();

            //List for Artist Folders in Music Folder
            IReadOnlyList<StorageFolder> artsss = await musicFolder.GetFoldersAsync();

            //Lists to Hold Album Folders and Song Files
            List<IReadOnlyList<StorageFolder>> albumFolders = new List<IReadOnlyList<StorageFolder>>();
            List<IReadOnlyList<StorageFile>> songFiles = new List<IReadOnlyList<StorageFile>>();

            //Get Album Folders in Each Artist Folder and Add to List
            foreach (StorageFolder artistfolder in artsss)
            {
                albumFolders.Add(await artistfolder.GetFoldersAsync());
            }

            //Go through each Album Folder and add song to list
            foreach (IReadOnlyList<StorageFolder> albums in albumFolders)
            {
                foreach (StorageFolder songs in albums)
                {
                    songFiles.Add(await songs.GetFilesAsync());
                }
            }

            //Go through each song in list and add to objects
            foreach (IReadOnlyList<StorageFile> Files in songFiles)
            {
                if (Files != null && Files.Count > 0)
                {
                    // Application now has read/write access to the picked file(s)
                    foreach (StorageFile file in Files)
                    {
                        MusicProperties fp = await file.Properties.GetMusicPropertiesAsync();
                        bool artistExists = false;
                        //find artist and add song to album
                        foreach (Artist artist in artists)
                        {
                            if (fp.Artist.Equals(artist.Name))
                            {
                                artist.AddToAlbum(file);
                                artistExists = true;
                                //if (!slist.Items.Any(item => item == artist)) {
                                //    slist.Items.Add(artist);
                                //}
                            }
                        }
                        // if artist doesnt exist create and add song to album
                        if (!artistExists)
                        {
                            artists.Add(new Artist { Name = fp.Artist });
                            artists[artists.Count - 1].AddToAlbum(file);
                            //slist.Items.Add(artists[artists.Count - 1]);

                        }
                    }
                }
            }
            await Save(artists);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            listviewstate.BackButton();
        }

        private List<Artist> Load()
        {
            string file = "D:\\";
            List<Artist> listofa = new List<Artist>();
            XmlSerializer formatter = new XmlSerializer(artists.GetType());
            FileStream aFile = new FileStream(file, FileMode.Open);
            byte[] buffer = new byte[aFile.Length];
            aFile.Read(buffer, 0, (int)aFile.Length);
            MemoryStream stream = new MemoryStream(buffer);
            return (List<Artist>)formatter.Deserialize(stream);
        }

        
        private async void Save(List<Artist> listofa, string filename)
        {


            StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.ReplaceExisting);

            using (var stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Artist));
                TextWriter writer = new StreamWriter(stream, Encoding.UTF8);
                serializer.Serialize(writer, artists);
            }

        }
    }
}
