//------------------------------------------------------------------------------
// <copyright file="SettingsWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace VSIXSpotifyRemote
{
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Settings;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SettingsWindowControl.
    /// </summary>
    public partial class SettingsWindowControl : UserControl
    {

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
            initialized = true;
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

        
    }
}

  