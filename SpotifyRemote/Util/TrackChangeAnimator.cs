using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using SpotifyAPI.Local.Models;

namespace SpotifyRemoteNS.Util
{
    public class TrackChangeAnimator
    {
        OleMenuCommand m_menuCommand;
        Track m_currentTrack;
        string m_standardCommandText;
        bool m_trackChangeEnabled = true;
        bool m_animationEnabled = true;

        internal System.Timers.Timer m_timer = null;

        private const float m_animationTimeWait = 5000; //ms
        private const float m_animationFrameTime = 125; //ms


        public bool Initialize(OleMenuCommand menuCommand, string menuCommandText)
        {
            Debug.WriteLine("TrackChangeAnimator:: Initialize.");
            m_menuCommand = menuCommand;
            m_standardCommandText = menuCommandText;

            return true;
        }


        public void SetTrackChange(bool enabled)
        {
            m_trackChangeEnabled = enabled;
        }

        public void SetAnimationOnChange(bool enabled)
        {
            m_animationEnabled = enabled;
        }

        public void SetStandardCommandText(string text)
        {
            m_standardCommandText = text;
        }

        public void Destroy()
        {

        }

        public void StartTrackChange(Track newTrack)
        {
            Debug.WriteLine("TrackChangeAnimator:: StartTrackChange.");
            if (newTrack == null || newTrack.TrackResource == null ||
                newTrack.TrackResource.Name == null || newTrack.AlbumResource.Name == null)
            {
                Debug.WriteLine("TrackChangeAnimator:: StartTrackChange newTrack contains argument is null.");
                return;
            }

            if (!m_trackChangeEnabled)
            {
                return;
            }

            m_currentTrack = newTrack;
            StartTrackChangeAnimation();



        }

        public void StopTrackChange()
        {
            Debug.WriteLine("TrackChangeAnimator:: Initialize.");
            m_menuCommand.Text = m_standardCommandText;
        }

        //////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////

        private void StartTrackChangeAnimation()
        {
            string trackName = m_currentTrack.TrackResource.Name;
            string artistName = m_currentTrack.ArtistResource.Name;
            Console.WriteLine("Show New track name: " + trackName);
            m_menuCommand.Text = String.Format("{0} - {1}", trackName, artistName);


            if (m_timer != null)
            {
                m_timer.Stop();
                m_timer.Start();
            }
            else
            {
                m_timer = new System.Timers.Timer() { Interval = m_animationTimeWait };
                m_timer.Elapsed += TrackChangeAnimTimerTick;
                m_timer.Start();
            }
        }

        private void TrackChangeAnimTimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            Debug.WriteLine("TrackChangeAnimator:: TrackChange Wait done.");
            m_timer.Stop();
            m_timer.Elapsed -= TrackChangeAnimTimerTick;

            if (!m_animationEnabled)
            {
                m_timer = null;
                m_menuCommand.Text = m_standardCommandText;
                return;
            }

            // Else Animation is enabled.
            m_timer.Elapsed += TimerAnimationTick;
            m_timer.Interval = m_animationFrameTime;
            m_timer.Start();


        }

        private void TimerAnimationTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_menuCommand.Text.Length <= m_standardCommandText.Length)
            {
                m_timer.Stop();
                m_timer.Elapsed -= TimerAnimationTick;
                m_timer = null;
                m_menuCommand.Text = m_standardCommandText;
            }
            else
            {
                if (m_menuCommand.Text.Length > m_standardCommandText.Length)
                {
                    m_menuCommand.Text = m_menuCommand.Text.Remove(m_menuCommand.Text.Length - 1);
                }
                else
                {
                    m_menuCommand.Text += " ";
                }
            }
        }
    }
}
