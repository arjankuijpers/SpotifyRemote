using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotLocalAPI = SpotifyAPI.Local.SpotifyLocalAPI;

namespace SpotifyRemoteNS
{
    public class SpotifyManager
    {

        private SpotifyAPI.Local.SpotifyLocalAPI spotClient;
        private SpotifyAPI.Web.SpotifyWebAPI spotWeb;
        private bool spotClientConnected = false;

        public SpotifyManager()
        {
            spotClient = null;
            spotWeb = null;
            spotClientConnected = false;
        }
        public bool Initialize()
        {
            Debug.WriteLine("SpotifyManager Initialize.");
            ConnectToSpotifyClient();

            if(!IsSpotifyProcessRunning())
            {
                CommandOpenSpotify.Instance.m_currentTextlabel = CommandOpenSpotify.kSpotifyStartString;
            }
            else
            {
                CommandOpenSpotify.Instance.m_currentTextlabel = CommandOpenSpotify.kSpotifyOpenString;
            }
           
            return true;
        }
        public void Destroy()
        {
            DisconnectSpotifyClient();
        }



        public bool GetSpotifyConnectionStatus()
        {
            if(spotClient != null && spotClientConnected)
            {
                if(spotClient.GetStatus() == null)
                {
                    spotClient = null;
                    spotClientConnected = false;
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ConnectToSpotifyClient()
        {
            Debug.WriteLine("SpotifyManager:: Connect to client.");


            if (SpotLocalAPI.IsSpotifyRunning())
            {
                CommandOpenSpotify.Instance.m_currentTextlabel = CommandOpenSpotify.kSpotifyOpenString;
                if (spotClient == null)
                {
                    spotClient = new SpotLocalAPI();
                }

                bool connected = spotClient.Connect();
#if DEBUG
                Debug.WriteLine("Spotify Client Connected: {0}", connected);
                Debug.Assert(connected);
#endif
                if(connected)
                {
                    RegisterEvents();
                }
                spotClientConnected = connected;

                SettingsManager.GetSettingsManager().ApplyCurrentSettings();
                return connected;
            }

            CommandOpenSpotify.Instance.m_currentTextlabel = CommandOpenSpotify.kSpotifyStartString;
            SettingsManager.GetSettingsManager().ApplyCurrentSettings();
            spotClientConnected = false;
            return false;
        }

        private void DisconnectSpotifyClient()
        {
            spotClient.Dispose();
            spotClient = null;
            spotClientConnected = false;
        }

        private void RegisterEvents()
        {
            Debug.WriteLine("SpotifyManager:: RegisterEvents");
            spotClient.ListenForEvents = true;
            spotClient.OnTrackChange += SpotClient_OnTrackChangeInternal;
            spotClient.OnPlayStateChange += SpotClient_OnPlayStateChangeInternal;
        }

        public void OpenSpotify()
        {
            Debug.WriteLine("SpotifyManager:: OpenSpotify.");
            Process spotifyWindow = null;
            if((spotifyWindow = GetSpotifyWindowProcess()) != null)
            {
                UnsafeNativeMethods.BringProcessToFront(spotifyWindow);
            }
            else if(!SpotifyAPI.Local.SpotifyLocalAPI.IsSpotifyRunning())
            {
                Debug.WriteLine("Spotify not running.");
                SpotifyAPI.Local.SpotifyLocalAPI.RunSpotify();

                System.Threading.Thread.Sleep(300);
                ConnectToSpotifyClient();

                // update command hidden states.

            }
        }

        public void NextTrack()
        {
            Debug.WriteLine("SpotifyManager:: NextTrack.");
            if(!GetSpotifyConnectionStatus())
            {
                ConnectToSpotifyClient();
            }
            if(GetSpotifyConnectionStatus())
            {

                spotClient.Skip();
            }

        }

        public void PreviousTrack()
        {
            Debug.WriteLine("SpotifyManager:: PreviousTrack.");
            if (!GetSpotifyConnectionStatus())
            {
                ConnectToSpotifyClient();
            }
            if (GetSpotifyConnectionStatus())
            {
                spotClient.Previous();
            }
        }

        public void PlayPause()
        {
            Debug.WriteLine("SpotifyManager:: Play/Pause.");
            if (!GetSpotifyConnectionStatus())
            {
                ConnectToSpotifyClient();
            }
            if (GetSpotifyConnectionStatus())
            {
                if(spotClient.GetStatus().Playing)
                {
                    spotClient.Pause();
                }
                else
                {
                    spotClient.Play();
                }
            }
        }


        private bool IsSpotifyProcessRunning()
        {
            Process[] retrevedProc = Process.GetProcessesByName("Spotify");
            Process spotMain = null;
            foreach (Process item in retrevedProc)
            {
                if (item.MainWindowTitle != "")
                {
                    spotMain = item;

                    if (spotMain != null)
                    {
                        return true;
                    }
                    break;
                }
            }
            return false;
        }
        private Process GetSpotifyWindowProcess()
        {
            Process[] retrevedProc = Process.GetProcessesByName("Spotify");
            Process spotMain = null;
            foreach (Process item in retrevedProc)
            {
                if (item.MainWindowTitle != "")
                {
                    spotMain = item;
                    break;
                }
            }

            return spotMain;
        }



        // INTERNAL.
        private void SpotClient_OnTrackChangeInternal(object sender, SpotifyAPI.Local.TrackChangeEventArgs e)
        {
            Debug.WriteLine("SotifyManager:: OnTrackChange");
            OnSpotifyTrackChange(this, e);
        }

        private void SpotClient_OnPlayStateChangeInternal(object sender, SpotifyAPI.Local.PlayStateEventArgs e)
        {
            Debug.WriteLine("SotifyManager:: OnPlayStateChange");
            OnSpotifyPlayStateChange(this, e);
        }
        // END OF INTERNAL.

        public event EventHandler<SpotifyAPI.Local.TrackChangeEventArgs> SpotifyClientTrackChange;
        public event EventHandler<SpotifyAPI.Local.PlayStateEventArgs> SpotifyClientPlayStateChange;

        void OnSpotifyTrackChange(object sender, SpotifyAPI.Local.TrackChangeEventArgs e)
        {
            SpotifyClientTrackChange?.Invoke(this, e);
        }
        void OnSpotifyPlayStateChange(object sender, SpotifyAPI.Local.PlayStateEventArgs e)
        {
            SpotifyClientPlayStateChange?.Invoke(this, e);
        }
    }
   




    // Win32 Window Helper.

    public static class UnsafeNativeMethods
    {
        public static void BringProcessToFront(System.Diagnostics.Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            if (IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }

            SetForegroundWindow(handle);
        }

        private const int SW_RESTORE = 9;

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
    }
}
