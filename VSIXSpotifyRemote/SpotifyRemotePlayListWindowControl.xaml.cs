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

    /// <summary>
    /// Interaction logic for SpotifyRemotePlayListWindowControl.
    /// </summary>
    public partial class SpotifyRemotePlayListWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyRemotePlayListWindowControl"/> class.
        /// </summary>
        public SpotifyRemotePlayListWindowControl()
        {
            this.InitializeComponent();
        }

        private void MyToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> playListStrings = new List<string>();

            PrivateProfile pp = Command1Package.spotWeb.GetPrivateProfile();


            Paging<SimplePlaylist> ppl = Command1Package.spotWeb.GetUserPlaylists(pp.Id, 20, 0);
            for (int i = 0; i < ppl.Items.Count; i++)
            {
                System.Windows.MessageBox.Show(ppl.Items[i].Name);
                string playListId =  ppl.Items[i].Id;
                Paging<PlaylistTrack> listTracks = Command1Package.spotWeb.GetPlaylistTracks(pp.Id, playListId);

               
                if (listTracks.Total > 0)
                {
                    if (listTracks.Items[0].Track.IsPlayable ?? true)
                    {
                        string trackUri = listTracks.Items[0].Track.Uri;
                        playListStrings.Add(trackUri);
                    }
                    else
                    {
                        MessageBox.Show("Track not available " + listTracks.Items[0].Track.Name);
                    }
                    
                }
                

                

                

            }
            Command1Package.spotWeb.ResumePlayback("", "", playListStrings);





        }

        private void Sa_OnResponseReceivedEvent(SpotifyAPI.Web.Auth.AutorizationCodeAuthResponse response)
        {
            MessageBox.Show("Response: " + response.ToString());
        }



    }
}