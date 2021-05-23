using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;
using SpotifyRemoteNS.Util;

namespace SpotifyRemoteNS
{
    /// <summary>
    /// Interaction logic for SpotifyRemoteSettingsControl.
    /// </summary>
    public partial class SpotifyRemoteSettingsControl : UserControl
    {

        System.Windows.Media.Color foregroundColor = new System.Windows.Media.Color();
        System.Windows.Media.Color subOptionsColor = new System.Windows.Media.Color();
        System.Windows.Media.Color explainOptColor = new System.Windows.Media.Color();



        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyRemoteSettingsControl"/> class.
        /// </summary>
        public SpotifyRemoteSettingsControl()
        {
            this.InitializeComponent();
        }

        private void SpotifyRemoteSettings_Initialized(object sender, System.EventArgs e)
        {
            Microsoft.VisualStudio.PlatformUI.VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
        }

        private void SpotifyRemoteSettings_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
            UpdateUIColors();
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
                case SettingsManager.EToolbarTextMode.kAllTextVisible:
                    rb_tv0.IsChecked = true;
                    break;
                case SettingsManager.EToolbarTextMode.kOpenTextVisible:
                    rb_tv1.IsChecked = true;
                    break;
                case SettingsManager.EToolbarTextMode.kHideAllText:
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
                case SettingsManager.EToolbarTrackChangeMode.kDisabled:
                    checkBox_ShowTrackArtist.IsChecked = false;
                    checkBox_enableInteractiveAnimation.IsChecked = false;
                    break;
                case SettingsManager.EToolbarTrackChangeMode.kEnabledNoAnimation:
                    checkBox_ShowTrackArtist.IsChecked = true;
                    checkBox_enableInteractiveAnimation.IsChecked = false;
                    break;
                case SettingsManager.EToolbarTrackChangeMode.kEnabledWithAnimation:
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
                sm.SetButtonTextMode(SettingsManager.EToolbarTextMode.kAllTextVisible);
            }
            else if (rb_tv1.IsChecked == true)
            {
                sm.SetButtonTextMode(SettingsManager.EToolbarTextMode.kOpenTextVisible);
            }
            else
            {
                sm.SetButtonTextMode(SettingsManager.EToolbarTextMode.kHideAllText);
            }


            if (checkBox_hideText.IsChecked == true)
            {
                sm.SetHideTextSpotifyInactive(true);
            }
            else
            {
                sm.SetHideTextSpotifyInactive(false);
            }


            if (checkBox_ShowTrackArtist.IsChecked == true && checkBox_enableInteractiveAnimation.IsChecked == true)
            {
                sm.SetTrackChangeMode(SettingsManager.EToolbarTrackChangeMode.kEnabledWithAnimation);
            }
            else if (checkBox_ShowTrackArtist.IsChecked == true && checkBox_enableInteractiveAnimation.IsChecked == false)
            {
                sm.SetTrackChangeMode(SettingsManager.EToolbarTrackChangeMode.kEnabledNoAnimation);
            }
            else
            {
                sm.SetTrackChangeMode(SettingsManager.EToolbarTrackChangeMode.kDisabled);
            }


        }


        ///////////////////////////////////////////////////////////////////
        //// Theme


        private void VSColorTheme_ThemeChanged(Microsoft.VisualStudio.PlatformUI.ThemeChangedEventArgs e)
        {
            UpdateUIColors();
        }
        private void UpdateUIColors()
        {
            var defaultBackground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);
            var defaultForeground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey);

            System.Drawing.Color c = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);

            SolidColorBrush backgroundCol = new SolidColorBrush(ThemeHelper.ToMediaColor(defaultBackground));
            switch (ThemeHelper.GetTheme())
            {
                case ThemeHelper.eVSTheme.kDark:
                    foregroundColor = Color.FromRgb(255, 255, 255);
                    subOptionsColor = Color.FromRgb(130, 203, 247);
                    explainOptColor = Color.FromRgb(255, 255, 255);
                    WindowTitle.Foreground = new SolidColorBrush(Color.FromRgb(186, 255, 171));
                    WindowTitle.Background = new SolidColorBrush(ThemeHelper.ToMediaColor(defaultBackground));
                    Background = backgroundCol;
                    //listView.Background = backgroundCol;
                    break;
                case ThemeHelper.eVSTheme.kBlue:
                    foregroundColor = Color.FromRgb(0, 0, 0);
                    subOptionsColor = Color.FromRgb(100, 106, 106);
                    explainOptColor = Color.FromRgb(0, 0, 0);
                    WindowTitle.Foreground = new SolidColorBrush(Color.FromRgb(83, 114, 76));
                    WindowTitle.Background = new SolidColorBrush(ThemeHelper.ToMediaColor(defaultBackground));
                    Background = backgroundCol;
                    //.Background = backgroundCol;
                    break;
                case ThemeHelper.eVSTheme.kLight:
                    foregroundColor = Color.FromRgb(0, 0, 0);
                    subOptionsColor = Color.FromRgb(100, 106, 106);
                    explainOptColor = Color.FromRgb(0, 0, 0);
                    WindowTitle.Foreground = new SolidColorBrush(Color.FromRgb(83, 114, 76));
                    WindowTitle.Background = backgroundCol;
                    Background = backgroundCol;
                    //listView.Background = backgroundCol;
                    break;
                case ThemeHelper.eVSTheme.kUnknown:
                //break;
                default:
                    byte a = defaultForeground.A;
                    byte r = defaultForeground.R;
                    byte g = defaultForeground.G;
                    byte b = defaultForeground.B;
                    foregroundColor = Color.FromArgb(a, r, g, b);
                    subOptionsColor = Color.FromArgb(a, r, g, b);
                    explainOptColor = Color.FromArgb(a, r, g, b);
                    WindowTitle.Foreground = new SolidColorBrush(foregroundColor);
                    WindowTitle.Background = new SolidColorBrush(ThemeHelper.ToMediaColor(defaultBackground));
                    Dispatcher.BeginInvoke(new System.Action(() => MessageBox.Show("Spotify extension couldn't detect color scheme. \nWould you be so kind to file a bug report?")));
                    break;
            }
            //SetListViewColors(foregroundColor);
            UpdateSettingsTitles(foregroundColor);
            UpdateSettingsSubOptions(subOptionsColor);
            UpdateExplainationSettings(explainOptColor);
        }

        private void UpdateSettingsTitles(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush(color);
            title_textVisibility.Foreground = scb;
            title_hideButText.Foreground = scb;
            title_showPlayListButton.Foreground = scb;
            title_InteractiveInfo.Foreground = scb;

            title_devInfoNotAvailable.Foreground = scb;
            title_devInfo.Foreground = scb;
        }

        private void UpdateSettingsSubOptions(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush(color);
            rb_tv0.Foreground = scb;
            rb_tv1.Foreground = scb;
            rb_tv2.Foreground = scb;
            checkBox_hideText.Foreground = scb;
            checkBox_ShowTrackArtist.Foreground = scb;
            checkBox_enableInteractiveAnimation.Foreground = scb;

        }

        private void UpdateExplainationSettings(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush(color);
            label_hideText.Foreground = scb;
            additionalInfoEnableAnimation.Foreground = scb;
        }


    }
}