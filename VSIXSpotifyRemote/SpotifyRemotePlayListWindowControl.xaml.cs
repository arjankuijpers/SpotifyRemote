//------------------------------------------------------------------------------
// <copyright file="SpotifyRemotePlayListWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace VSIXSpotifyRemote
{
    using SpotifyAPI.Web.Enums;
    using SpotifyAPI.Web.Models;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for SpotifyRemotePlayListWindowControl.
    /// </summary>
    public partial class SpotifyRemotePlayListWindowControl : UserControl
    {

        Brush foregroundLabelBrush;
        int selectedIndex = -1;
        string userId;

        private List<string> playListNames;
        private List<string> playListId;
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyRemotePlayListWindowControl"/> class.
        /// </summary>
        public SpotifyRemotePlayListWindowControl()
        {
            this.InitializeComponent();

            playListNames = new List<string>();
            playListId = new List<string>();
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
            Paging<SimplePlaylist> pPlayLists = Command1Package.spotWeb.GetUserPlaylists(privProfile.Id, 20, 0);

            for (int i = 0; i < pPlayLists.Items.Count; i++)
            {
                playListNames.Add(pPlayLists.Items[i].Name);
                playListId.Add(pPlayLists.Items[i].Id);
            }

        }

        void ShowPlayListsInView()
        {
            for (int i = 0; i < playListNames.Count; i++)
            {
                Label l = new Label();
                l.Foreground = new SolidColorBrush(Color.FromRgb(171, 255, 255));
                l.Content = playListNames[i];
                l.FontFamily = new FontFamily("Segoe UI Light");
                listView.Items.Add(l);
                
            }

            foregroundLabelBrush = ((Label)listView.Items[0]).Foreground;


        }

        void PlayPlayList(string playListId)
        {

            

            Paging<PlaylistTrack> listTracks = Command1Package.spotWeb.GetPlaylistTracks(userId, playListId);
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
                        MessageBox.Show("Track not available " + listTracks.Items[i].Track.Name);
                    }
                }

                Command1Package.spotWeb.ResumePlayback("", "", songURIs);
            }
            
        }


        private void Sa_OnResponseReceivedEvent(SpotifyAPI.Web.Auth.AutorizationCodeAuthResponse response)
        {
            MessageBox.Show("Response: " + response.ToString());
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = listView.SelectedIndex;

            Label l = e.AddedItems[0] as Label;
            l.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            if(e.RemovedItems.Count > 0)
            {
                Label lr = e.RemovedItems[0] as Label;
                lr.Foreground = foregroundLabelBrush;
            }
            
        }

        private void listView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //MessageBox.Show("Clicked: ");
            PlayPlayList(playListId[selectedIndex]);
            
        }

        private void PlayListWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            listView.Height = this.ActualHeight - WindowTitle.ActualHeight;
        }
    }
}