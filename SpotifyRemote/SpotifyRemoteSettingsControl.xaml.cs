namespace SpotifyRemoteNS
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SpotifyRemoteSettingsControl.
    /// </summary>
    public partial class SpotifyRemoteSettingsControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyRemoteSettingsControl"/> class.
        /// </summary>
        public SpotifyRemoteSettingsControl()
        {
            this.InitializeComponent();
        }

        private void SpotifyRemoteSettings_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        private void SpotifyRemoteSettings_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            SettingsManager sm = SettingsManager.GetSettingsManager();
            sm.ApplyCurrentSettings();
        }



        private void LoadSettings()
        {
            SettingsManager sm = SettingsManager.GetSettingsManager();
            switch (sm.GetButtonTextMode())
            {
                case SettingsManager.eToolbarTextMode.kAllTextVisible:
                    rb_tv0.IsChecked = true;
                    break;
                case SettingsManager.eToolbarTextMode.kOpenTextVisible:
                    rb_tv1.IsChecked = true;
                    break;
                case SettingsManager.eToolbarTextMode.kHideAllText:
                    rb_tv2.IsChecked = true;
                    break;
                default:
                    rb_tv0.IsChecked = true;
                    break;
            }

            if (sm.GetHideTextSpotifyInactive())
            {
                checkBox_hideText.IsChecked = true;
            }
            else
            {
                checkBox_hideText.IsChecked = true;
            }


            switch (sm.GetTrackChangeMode())
            {
                case SettingsManager.eToolbarTrackChangeMode.kDisabled:
                    checkBox_ShowTrackArtist.IsChecked = false;
                    checkBox_enableInteractiveAnimation.IsChecked = false;
                    break;
                case SettingsManager.eToolbarTrackChangeMode.kEnabledNoAnimation:
                    checkBox_ShowTrackArtist.IsChecked = true;
                    checkBox_enableInteractiveAnimation.IsChecked = false;
                    break;
                case SettingsManager.eToolbarTrackChangeMode.kEnabledWithAnimation:
                    checkBox_ShowTrackArtist.IsChecked = true;
                    checkBox_enableInteractiveAnimation.IsChecked = true;
                    break;
                default:
                    checkBox_ShowTrackArtist.IsChecked = true;
                    checkBox_enableInteractiveAnimation.IsChecked = true;
                    break;
            }
        }

        private void SaveSettings()
        {
            SettingsManager sm = SettingsManager.GetSettingsManager();
            if (rb_tv0.IsChecked == true)
            {
                sm.SetButtonTextMode(SettingsManager.eToolbarTextMode.kAllTextVisible);   
            }
            else if(rb_tv1.IsChecked == true)
            {
                sm.SetButtonTextMode(SettingsManager.eToolbarTextMode.kOpenTextVisible);
            }
            else {
                sm.SetButtonTextMode(SettingsManager.eToolbarTextMode.kHideAllText);
            }


            if(checkBox_hideText.IsChecked == true)
            {
                sm.SetHideTextSpotifyInactive(true);
            }
            else
            {
                sm.SetHideTextSpotifyInactive(false);
            }


            if(checkBox_ShowTrackArtist.IsChecked == true && checkBox_enableInteractiveAnimation.IsChecked == true)
            {
                sm.SetTrackChangeMode(SettingsManager.eToolbarTrackChangeMode.kEnabledWithAnimation);
            }
            else if (checkBox_ShowTrackArtist.IsChecked == true && checkBox_enableInteractiveAnimation.IsChecked == false)
            {
                sm.SetTrackChangeMode(SettingsManager.eToolbarTrackChangeMode.kEnabledNoAnimation);
            }
            else
            {
                sm.SetTrackChangeMode(SettingsManager.eToolbarTrackChangeMode.kDisabled);
            }

           
        }

        
    }
}