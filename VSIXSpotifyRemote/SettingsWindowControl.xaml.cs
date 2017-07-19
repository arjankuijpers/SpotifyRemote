//------------------------------------------------------------------------------
// <copyright file="SettingsWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace VSIXSpotifyRemote
{
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Settings;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for SettingsWindowControl.
    /// </summary>
    public partial class SettingsWindowControl : UserControl
    {
        System.Windows.Media.Color foregroundColor = new System.Windows.Media.Color();
        System.Windows.Media.Color subOptionsColor = new System.Windows.Media.Color();
        System.Windows.Media.Color explainOptColor = new System.Windows.Media.Color();

        bool initialized = false;

        private string hideButtonTextHiddenActive = "text is hidden when spotify is inactive,\n only button graphics are visible.";
        private string hideButtonTextHiddenInActive = "text is shown when spotify is inactive,\n button graphics and text are visible.";

        private string showPlayListButtonShown = "button is shown in the toolbar.";
        private string showPlayListButtonHidden = "button is not visible in the toolbar.";

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowControl"/> class.
        /// </summary>
        public SettingsWindowControl()
        {
            this.InitializeComponent();
        }

        // On initialization set everything to the correct saved values.
        private void StackPanel_Initialized(object sender, System.EventArgs e)
        {

            UserPreferences.Default.Reload();
            UserPreferences.Default.SettingChanging += Default_SettingChanging;
            SetControlsToSavedValues();


            Microsoft.VisualStudio.PlatformUI.VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
            UpdateUIColors();
            initialized = true;
        }

        private void VSColorTheme_ThemeChanged(Microsoft.VisualStudio.PlatformUI.ThemeChangedEventArgs e)
        {
            UpdateUIColors();
        }

        public void SetControlsToSavedValues()
        {
            System.Console.WriteLine("Set Settings states to control states.");

            if (!initialized)
            {
                System.Console.WriteLine("Not initialized yet.");
                return;
            }

            switch (UserPreferences.Default.TextVisibility)
            {
                case 0:
                    rb_tv0.IsChecked = true;
                    rb_tv1.IsChecked = false;
                    rb_tv2.IsChecked = false;
                    break;
                case 1:
                    rb_tv1.IsChecked = true;
                    break;
                case 2:
                    rb_tv2.IsChecked = true;
                    break;
                default:
                    Debug.Assert(true, "Setting has invalid value.");
                    break;
            }

            ///////////////////

            checkBox_hideText.IsChecked = UserPreferences.Default.HideButtonTextOnInactive;
            if(UserPreferences.Default.HideButtonTextOnInactive)
            {
                label_hideText.Content = hideButtonTextHiddenActive;
            }
            else
            {
                label_hideText.Content = hideButtonTextHiddenInActive;
            }

            checkBox_showPlayListButton.IsChecked = UserPreferences.Default.ShowOpenPlayListButton;
            if(UserPreferences.Default.ShowOpenPlayListButton)
            {
                label_showPlaylistButton.Content = showPlayListButtonShown;
            }
            else
            {
                label_showPlaylistButton.Content = showPlayListButtonHidden;
            }

            ///////////////////


            checkBox_ShowTrackArtist.IsChecked = UserPreferences.Default.showTrackArtistOnChange;
            checkBox_enableInteractiveAnimation.IsChecked = UserPreferences.Default.EnableInteractiveAnimation;

            if(checkBox_ShowTrackArtist.IsChecked ?? true)
            {
                checkBox_enableInteractiveAnimation.IsEnabled = true;
            }
            else
            {
                checkBox_enableInteractiveAnimation.IsEnabled = false;
            }
        }

        public void SaveControlStatesToSettings()
        {
            System.Console.WriteLine("Save control states to settings.");
            if (!initialized)
            {
                System.Console.WriteLine("Not initialized yet.");
                return;
            }


            System.Console.WriteLine("Save control states to settings.");

           
            if (rb_tv0.IsChecked ?? true)
            {
                UserPreferences.Default.TextVisibility = 0;
            }
            else if (rb_tv1.IsChecked ?? true)
            {
                UserPreferences.Default.TextVisibility = 1;
            }
            else if (rb_tv2.IsChecked ?? true)
            {
                UserPreferences.Default.TextVisibility = 2;
            }
            else
            {
                System.Diagnostics.Debug.Assert(true, "Should never reach this condition.");
            }

            UserPreferences.Default.HideButtonTextOnInactive = checkBox_hideText.IsChecked ?? true;
            UserPreferences.Default.ShowOpenPlayListButton = checkBox_showPlayListButton.IsChecked ?? true;
            UserPreferences.Default.showTrackArtistOnChange = checkBox_ShowTrackArtist.IsChecked ?? true;
            UserPreferences.Default.EnableInteractiveAnimation = checkBox_enableInteractiveAnimation.IsChecked ?? true;
            UserPreferences.Default.Save();

            UpdateTextHiddenStates();

            if (checkBox_ShowTrackArtist.IsChecked ?? true)
            {
                checkBox_enableInteractiveAnimation.IsEnabled = true;
            }
            else
            {
                checkBox_enableInteractiveAnimation.IsEnabled = false;
            }
        }

        private void Default_SettingChanging(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            //System.Console.WriteLine("SettingsChanged reloaded to controls.");
            //SetControlsToSavedValues();
        }

        private void rb_tv0_Checked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
        }

        private void rb_tv1_Checked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
        }

        private void rb_tv2_Checked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
        }

        private void checkBox_hideText_Checked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
            SetControlsToSavedValues();
        }

        private void checkBox_hideText_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
            SetControlsToSavedValues();
        }

        private void checkBox_ShowTrackArtist_Checked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
        }

        private void checkBox_ShowTrackArtist_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
        }

        private void checkBox_enableInteractiveAnimation_Checked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
        }

        private void checkBox_enableInteractiveAnimation_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
        }

        private void MyToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetControlsToSavedValues();
        }

        private void checkBox_showPlayListButton_Checked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
            SetControlsToSavedValues();
        }

        private void checkBox_showPlayListButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveControlStatesToSettings();
            SetControlsToSavedValues();
        }

        private void UpdateTextHiddenStates()
        {
            Command1.Instance.SetStartupCommandTextState();
            Command2.Instance.SetStartupCommandTextState();
            Command3.Instance.SetStartupCommandTextState();
            Command4.Instance.SetStartupCommandTextState();
        }



        ///////////////////////////////////////////////////////////////////
        //// Theme


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
                    subOptionsColor = Color.FromRgb(171, 255, 255);
                    explainOptColor = Color.FromRgb(255,255,255);
                    WindowTitle.Foreground = new SolidColorBrush(Color.FromRgb(186, 255, 171));
                    WindowTitle.Background = new SolidColorBrush(ThemeHelper.ToMediaColor(defaultBackground));
                    Background = backgroundCol;
                    //listView.Background = backgroundCol;
                    break;
                case ThemeHelper.eVSTheme.kBlue:
                    foregroundColor = Color.FromRgb(0, 0, 0);
                    subOptionsColor = Color.FromRgb(100,106,106);
                    explainOptColor = Color.FromRgb(0, 0, 0);
                    WindowTitle.Foreground = new SolidColorBrush(Color.FromRgb(83, 114, 76));
                    WindowTitle.Background = new SolidColorBrush(ThemeHelper.ToMediaColor(defaultBackground));
                    Background = backgroundCol;
                    //.Background = backgroundCol;
                    break;
                case ThemeHelper.eVSTheme.kLight:
                    foregroundColor = Color.FromRgb(0, 0, 0);
                    subOptionsColor = Color.FromRgb(100,106,106);
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
                    Dispatcher.BeginInvoke(new System.Action(() => MessageBox.Show("Spotify extension couldnt detect color scheme. \nWould you be so kind to file a bug report?")));
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

            //devInfoLabel.Foreground = scb;
        }

        private void UpdateSettingsSubOptions(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush(color);
            rb_tv0.Foreground = scb;
            rb_tv1.Foreground = scb;
            rb_tv2.Foreground = scb;
            checkBox_hideText.Foreground = scb;
            checkBox_showPlayListButton.Foreground = scb;
            checkBox_ShowTrackArtist.Foreground = scb;
            checkBox_enableInteractiveAnimation.Foreground = scb;

        }

        private void UpdateExplainationSettings(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush(color);
            label_hideText.Foreground = scb;
            label_showPlaylistButton.Foreground = scb;
            additionalInfoEnableAnimation.Foreground = scb;
        }



    }
}

  