using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SpotifyRemoteNS
{
    /// <summary>
    /// Command handler
    /// </summary>
    public class CommandOpenSpotify
    {



#if DEBUG
        public const string kSpotifyOpenString = "Open Spotify(D)";
#else
        public const string kSpotifyOpenString = "Open Spotify";
#endif

#if DEBUG
        public const string kSpotifyStartString = "Start Spotify(D)";
#else
        public const string kSpotifyStartString = "Start Spotify";
#endif










        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("30159a3f-d07b-4eaa-9337-0aa4c68ae3a3");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;
        private SpotifyManager m_spotifyManager;

        private TrackChangeAnimator m_trackChangeAnimator;
        public string m_currentTextlabel = kSpotifyOpenString;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandOpenSpotify"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private CommandOpenSpotify(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            SpotifyRemote spotifyRemotePackage = package as SpotifyRemote;
            m_spotifyManager = spotifyRemotePackage.GetSpotifyManager();
            

            this.package = package;
 
            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            OleMenuCommand menuItem = null;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }


            

            // Initialize TrackChangeAnimator.
            m_trackChangeAnimator = new TrackChangeAnimator();
            m_trackChangeAnimator.Initialize(menuItem, m_currentTextlabel);

            m_spotifyManager.SpotifyClientTrackChange += SpotifyClientTrackChange;


            SettingsManager setManager = SettingsManager.GetSettingsManager();
            setManager.SetTbButtonOpen(ref menuItem);
            setManager.SetTrackChangeAnimation(ref m_trackChangeAnimator);



        }

        private void SpotifyClientTrackChange(object sender, SpotifyAPI.Local.TrackChangeEventArgs e)
        {
            m_trackChangeAnimator.StartTrackChange(e.NewTrack);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static CommandOpenSpotify Instance;

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new CommandOpenSpotify(package);
            SettingsManager setManager = SettingsManager.GetSettingsManager();
            setManager.SetCmdOpenSpotify(ref Instance);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            //string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            //string title = "CommandOpenSpotify";

            //// Show a message box to prove we were here
            //VsShellUtilities.ShowMessageBox(
            //    this.ServiceProvider,
            //    message,
            //    title,
            //    OLEMSGICON.OLEMSGICON_INFO,
            //    OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);


            // execute.
            m_spotifyManager.OpenSpotify();





        }
    }
}
