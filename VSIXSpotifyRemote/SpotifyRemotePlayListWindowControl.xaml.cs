//------------------------------------------------------------------------------
// <copyright file="SpotifyRemotePlayListWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


#define LOCAL_PLAY

namespace VSIXSpotifyRemote
{
    using SpotifyAPI.Web.Enums;
    using SpotifyAPI.Web.Models;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    namespace PlayList {
        public class SpotListViewItem
        {
            public Label Name { get; set; }

            public Label Open { get; set; }

            public int playListId { get; set; }
        }

        public class SpotPlayListLabel : Label
        {
            public int playlistId { get; set; }
            public string spotifyTrackId { get; set; }
        }
    }

    /// <summary>
    /// Interaction logic for SpotifyRemotePlayListWindowControl.
    /// </summary>
    public partial class SpotifyRemotePlayListWindowControl : UserControl
    {
        enum ListViewMode
        {
            kPlayList = 0,
            kTrackList = 1
        }


        private static int kMAX_PLAYLISTS = 110;
        private static int kMAX_TRACKS = 110;

        Brush foregroundLabelBrush;
        int selectedIndex = -1;
        string userId;
        ListViewMode lvm = ListViewMode.kPlayList;

        private List<string> playListNames;
        private List<string> playListId;
        private List<string> playListUri;
        private List<PlaylistTrack> listTracksFromPL;


        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyRemotePlayListWindowControl"/> class.
        /// </summary>
        public SpotifyRemotePlayListWindowControl()
        {

            this.InitializeComponent();

            playListNames = new List<string>();
            playListId = new List<string>(kMAX_PLAYLISTS);
            playListUri= new List<string>(kMAX_PLAYLISTS);
            listTracksFromPL = new List<PlaylistTrack>(kMAX_TRACKS);


        }

        private void MyToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RetreivePlayListsFromWeb();
            ShowPlayListsInView();


            listView.Height = this.ActualHeight;// - WindowTitle.ActualHeight;




        }


        void RetreivePlayListsFromWeb()
        {

            PrivateProfile privProfile = Command1Package.spotWeb.GetPrivateProfile();
            userId = privProfile.Id;
            Paging<SimplePlaylist> pPlayLists = Command1Package.spotWeb.GetUserPlaylists(privProfile.Id, kMAX_PLAYLISTS, 0);

            for (int i = 0; i < pPlayLists.Items.Count; i++)
            {
                playListNames.Add(pPlayLists.Items[i].Name);
                playListId.Add(pPlayLists.Items[i].Id);
                playListUri.Add(pPlayLists.Items[i].Uri);
            }

        }

        bool RetreiveTracksFromWeb(string playListId)
        {
            listTracksFromPL.Clear();
            Paging<PlaylistTrack> pagingTracks = Command1Package.spotWeb.GetPlaylistTracks(userId, playListId, "", 900, 0, "");
            if(pagingTracks.Items == null)
            {
                return false;
            }
            listTracksFromPL.AddRange(pagingTracks.Items);
            return true;
            

        }

        void ShowPlayListsInView()
        {
            lvm = ListViewMode.kPlayList;
            ReturnPlaylists.Visibility = Visibility.Collapsed;
            LabelPlayPlayList.Visibility = Visibility.Collapsed;
            RecalculateListView();


            //var gridView = new GridView();
            //listView.View = gridView;
            for (int i = 0; i < playListNames.Count; i++)
            {

                

                Label l = new Label();
                l.Foreground = new SolidColorBrush(Color.FromRgb(171, 255, 255));
                l.Content = playListNames[i];
                l.FontFamily = new FontFamily("Segoe UI Light");

                //gridView.Columns

                PlayList.SpotPlayListLabel openLabel = new PlayList.SpotPlayListLabel();
                openLabel.Content = "Open";
                
                openLabel.MouseUp += OpenLabelClicked;
                openLabel.playlistId = i;

                listView.Items.Add(new PlayList.SpotListViewItem { Name = l, Open = openLabel, playListId = i });
                
            }
            
            foregroundLabelBrush = ((Label)((PlayList.SpotListViewItem)listView.Items[0]).Name).Foreground;


        }

        void ShowTracksInListView()
        {
            lvm = ListViewMode.kTrackList;
            ReturnPlaylists.Visibility = Visibility.Visible;
            LabelPlayPlayList.Visibility = Visibility.Visible;
            RecalculateListView();


            //var gridView = new GridView();
            //listView.View = gridView;
            for (int i = 0; i < listTracksFromPL.Count; i++)
            {



                Label l = new Label();
                l.Foreground = new SolidColorBrush(Color.FromRgb(171, 255, 255));
                l.Content = listTracksFromPL[i].Track.Name;
                l.FontFamily = new FontFamily("Segoe UI Light");

                //gridView.Columns

                PlayList.SpotPlayListLabel openLabel = new PlayList.SpotPlayListLabel();
                openLabel.Content = "Play";

                openLabel.MouseUp += OpenLabelClicked;
                openLabel.playlistId = 0;
                openLabel.spotifyTrackId = listTracksFromPL[i].Track.Uri;

                listView.Items.Add(new PlayList.SpotListViewItem { Name = l, Open = openLabel, playListId = i });

            }

            foregroundLabelBrush = ((Label)((PlayList.SpotListViewItem)listView.Items[0]).Name).Foreground;


        }

        private void OpenLabelClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(lvm == ListViewMode.kPlayList)
            {
                
                bool successful = RetreiveTracksFromWeb(playListId[((PlayList.SpotPlayListLabel)e.Source).playlistId]);
                if(successful)
                {
                    listView.Items.Clear();
                    ShowTracksInListView();
                }
                else
                {
                    MessageBox.Show("Sorry couldn't show your playlist songs.\nTry again later.");
                }
            }
            else
            {
                Command1Package.spotClient.PlayURL(((PlayList.SpotPlayListLabel)e.Source).spotifyTrackId);
            }
            


        }

        void PlayPlayList(string playListId)
        {

            Paging<PlaylistTrack> listTracks = Command1Package.spotWeb.GetPlaylistTracks(userId, playListId, "", kMAX_TRACKS);
            List<string> songURIs = new List<string>();


            if (listTracks.Total > 0)
            {
                for (int i = 0; i < listTracks.Items.Count; i++)
                {
                    if (listTracks.Items[i].Track.IsPlayable ?? true)
                    {
                        string trackUri = listTracks.Items[i].Track.Uri;
                        songURIs.Add(trackUri);
                    }
                    else
                    {
                        MessageBox.Show("Track not available: " + listTracks.Items[i].Track.Name);
                    }
                }

                Command1Package.spotWeb.ResumePlayback("", "", songURIs);
            }
            
        }
        void PlayPlayListUri(string playListUri)
        {
            Command1Package.spotClient.PlayURL(playListUri);

        }


        private void Sa_OnResponseReceivedEvent(SpotifyAPI.Web.Auth.AutorizationCodeAuthResponse response)
        {
            MessageBox.Show("Response: " + response.ToString());
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = listView.SelectedIndex;
            if(listView.Items.Count == 0)
            {
                return;
            }

            Label l = ((PlayList.SpotListViewItem)e.AddedItems[0]).Name as Label;
            l.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            if(e.RemovedItems.Count > 0)
            {
                Label lr = ((PlayList.SpotListViewItem)e.RemovedItems[0]).Name as Label;
                lr.Foreground = foregroundLabelBrush;
            }
            
        }

        private void listView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //MessageBox.Show("Clicked: ");
            if (lvm == ListViewMode.kPlayList)
            {
#if LOCAL_PLAY
                PlayPlayListUri(playListUri[selectedIndex]);
#else
             PlayPlayList(playListId[selectedIndex]);
#endif
            }
            else
            {
                PlayPlayListUri(listTracksFromPL[selectedIndex].Track.Uri);
            }
            



        }

        private void PlayListWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecalculateListView();
        }

        private void RecalculateListView()
        {
            if (lvm == ListViewMode.kPlayList)
            {
                listView.Height = this.ActualHeight - WindowTitle.ActualHeight;
            }
            else
            {
                listView.Height = (this.ActualHeight - WindowTitle.ActualHeight) - ReturnPlaylists.ActualHeight;
            }
        }

        private void ReturnPlaylists_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            listView.Items.Clear();
            ShowPlayListsInView();
        }
    }
}