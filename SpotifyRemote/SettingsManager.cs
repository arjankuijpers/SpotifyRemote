using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics;

namespace SpotifyRemoteNS
{
    public class SettingsManager   
    {
        public enum eToolbarTextMode
        {
            kAllTextVisible = 0,
            kOpenTextVisible = 1,
            kHideAllText = 3,
        }

        public enum eToolbarTrackChangeMode
        {
            kDisabled = 0,
            kEnabledNoAnimation = 1,
            kEnabledWithAnimation = 2
        }


        //settings manager.
        static SettingsManager sm_settingsManager;

        CommandOpenSpotify m_cmdOpenSpotify;
        OleMenuCommand m_tbButtonOpen;
        OleMenuCommand m_tbButtonPrevious;
        OleMenuCommand m_tbButtonPlayPause;
        OleMenuCommand m_tbButtonNext;

        TrackChangeAnimator m_trackChangeAnimator;
        SpotifyManager m_spotifyManager;

        eToolbarTextMode m_textMode;
        eToolbarTrackChangeMode m_trackChangeMode;
        bool m_hideTextSpotifyInactive;



        public static ref SettingsManager GetSettingsManager()
        {
            return ref sm_settingsManager;
        }
        public static void ProvideSettingsManager(ref SettingsManager settingsManager)
        {
            sm_settingsManager = settingsManager;
        }

        public void SetSpotifyManager(ref SpotifyManager spotifyManager)
        {
            m_spotifyManager = spotifyManager;
        }
        ///////////////////////////////////////////

        // Toolbar Text Mode
        public void SetButtonTextMode(eToolbarTextMode textMode)
        {
            m_textMode = textMode;
        }

        public eToolbarTextMode GetButtonTextMode()
        {
            return m_textMode;
        }

        // Toolbar Text Mode End
        ///////////////////////////////////////////

        public void SetTrackChangeMode(eToolbarTrackChangeMode trackChangeMode)
        {
            m_trackChangeMode = trackChangeMode;
        }

        public eToolbarTrackChangeMode GetTrackChangeMode()
        {
            return m_trackChangeMode;
        }

        public void SetHideTextSpotifyInactive(bool enabled)
        {
            m_hideTextSpotifyInactive = enabled;
        }

        public bool GetHideTextSpotifyInactive()
        {
            return m_hideTextSpotifyInactive;
        }



        public void ReadSettingsFromFile()
        {
            Debug.WriteLine("SettingsManager:: ReadSettingsFromFile");
            UserSettings.Default.Reload();
            m_textMode = (eToolbarTextMode)UserSettings.Default.TextToolbarVisibleMode;
            m_trackChangeMode = (eToolbarTrackChangeMode)UserSettings.Default.TrackInfoOnChangeMode;
            m_hideTextSpotifyInactive = UserSettings.Default.TextToolbarHiddenOnInactive;

        }

        public void SaveSettingsToFile()
        {
            Debug.WriteLine("SettingsManager:: SaveSettingsToFile");

            UserSettings.Default.TextToolbarVisibleMode = (ushort)m_textMode;
            UserSettings.Default.TrackInfoOnChangeMode = (ushort)m_trackChangeMode;
            UserSettings.Default.TextToolbarHiddenOnInactive = m_hideTextSpotifyInactive;
            UserSettings.Default.Save();
        }


        public void ApplyCurrentSettings()
        {

            switch (m_textMode)
            {
                case eToolbarTextMode.kAllTextVisible:
                    m_tbButtonOpen.Text = m_cmdOpenSpotify.m_currentTextlabel;
                    m_tbButtonPrevious.Text = CommandPreviousTrack.commandText;
                    m_tbButtonPlayPause.Text = CommandPlayPause.commandText;
                    m_tbButtonNext.Text = CommandNextTrack.commandText;
                    break;
                case eToolbarTextMode.kOpenTextVisible:
                    m_tbButtonOpen.Text = m_cmdOpenSpotify.m_currentTextlabel;
                    m_tbButtonPrevious.Text = " ";
                    m_tbButtonPlayPause.Text = " ";
                    m_tbButtonNext.Text = " ";
                    break;
                case eToolbarTextMode.kHideAllText:
                    m_tbButtonOpen.Text = " ";
                    m_tbButtonPrevious.Text = " ";
                    m_tbButtonPlayPause.Text = " ";
                    m_tbButtonNext.Text = " ";
                    break;
                default:
                    m_tbButtonOpen.Text = m_cmdOpenSpotify.m_currentTextlabel;
                    m_tbButtonPrevious.Text = CommandPreviousTrack.commandText;
                    m_tbButtonPlayPause.Text = CommandPlayPause.commandText;
                    m_tbButtonNext.Text = CommandNextTrack.commandText;
                    Debug.WriteLine("WARNING m_textMode has unrecognized mode.");
                    break;
            }

            if (m_hideTextSpotifyInactive)
            {
                if (!m_spotifyManager.GetSpotifyConnectionStatus())
                {
                    m_tbButtonOpen.Text = " ";
                    m_tbButtonPrevious.Text = " ";
                    m_tbButtonPlayPause.Text = " ";
                    m_tbButtonNext.Text = " ";
                }
            }


            switch (m_trackChangeMode)
            {
                case eToolbarTrackChangeMode.kDisabled:
                    m_trackChangeAnimator.SetTrackChange(false);
                    m_trackChangeAnimator.SetAnimationOnChange(false);
                    break;
                case eToolbarTrackChangeMode.kEnabledNoAnimation:
                    m_trackChangeAnimator.SetTrackChange(true);
                    m_trackChangeAnimator.SetAnimationOnChange(false);
                    break;
                case eToolbarTrackChangeMode.kEnabledWithAnimation:
                    m_trackChangeAnimator.SetTrackChange(true);
                    m_trackChangeAnimator.SetAnimationOnChange(true);
                    break;
                default:
                    m_trackChangeAnimator.SetTrackChange(true);
                    m_trackChangeAnimator.SetAnimationOnChange(true);
                    Debug.WriteLine("WARNING m_trackChangeMode has unrecognized mode.");
                    break;
            }

            
            
           
        }


        public void SetCmdOpenSpotify(ref CommandOpenSpotify commandOpenSpotify)
        {
            m_cmdOpenSpotify = commandOpenSpotify;
        }

        public void SetTbButtonOpen(ref OleMenuCommand buttonOpen)
        {
            m_tbButtonOpen = buttonOpen;
        }

        public void SetTbButtonPrevious(ref OleMenuCommand buttonPrev)
        {
            m_tbButtonPrevious = buttonPrev;
        }

        public void SetTbButtonPlayPause(ref OleMenuCommand buttonPausePlay)
        {
            m_tbButtonPlayPause = buttonPausePlay;
        }

        public void SetTbButtonNext(ref OleMenuCommand buttonNext)
        {
            m_tbButtonNext = buttonNext;
        }

        public void SetTrackChangeAnimation(ref TrackChangeAnimator trChangeAnimator)
        {
            m_trackChangeAnimator = trChangeAnimator;
        }
    }
}
