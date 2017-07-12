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

        private string hideButtonTextHiddenActive = "text is hidden when spotify is inactive,&#xD;&#xA; only button graphics are visible.";
        private string hideButtonTextHiddenInActive = "text is shown when spotify is inactive,&#xD;&#xA; button graphics and text are visible.";

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
            SetControlsToSavedValues();
        }



        public void SetControlsToSavedValues()
        {
            System.Console.WriteLine("Set Settings states to control states.");

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

            checkBox_hideText.IsChecked = UserPreferences.Default.HideButtonTextOnInactive;
            if(UserPreferences.Default.HideButtonTextOnInactive)
            {
                label_hideText.Content = hideButtonTextHiddenActive;
            }
            else
            {
                label_hideText.Content = hideButtonTextHiddenInActive;
            }

            checkBox_ShowTrackArtist.IsChecked = UserPreferences.Default.showTrackArtistOnChange;
            checkBox_enableInteractiveAnimation.IsChecked = UserPreferences.Default.EnableInteractiveAnimation;
        }

        public void SaveControlStatesToSettings()
        {
            System.Console.WriteLine("Save control states to settings.");

            if (rb_tv0.IsChecked ?? true)
            {
                UserPreferences.Default.TextVisibility = 0;
            }
            else if (rb_tv1.IsChecked ?? true)
            {
                UserPreferences.Default.TextVisibility = 1;
            }
            else
            {
                UserPreferences.Default.TextVisibility = 2;
            }

            UserPreferences.Default.HideButtonTextOnInactive = checkBox_hideText.IsChecked ?? true;
            UserPreferences.Default.showTrackArtistOnChange = checkBox_ShowTrackArtist.IsChecked ?? true;
            UserPreferences.Default.EnableInteractiveAnimation = checkBox_enableInteractiveAnimation.IsChecked ?? true;
            UserPreferences.Default.Save();
            UserPreferences.Default.SettingChanging += Default_SettingChanging;

        }

        private void Default_SettingChanging(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            System.Console.WriteLine("SettingsChanged reloaded to controls.");
            SetControlsToSavedValues();
        }
    }
}

  