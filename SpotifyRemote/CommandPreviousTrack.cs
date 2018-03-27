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
    internal sealed class CommandPreviousTrack
    {

        public const string commandText = "Previous";

        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4179;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("30159a3f-d07b-4eaa-9337-0aa4c68ae3a3");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;
        private SpotifyManager m_spotifyManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandPreviousTrack"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private CommandPreviousTrack(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;
            SpotifyRemote spotifyRemotePackage = package as SpotifyRemote;
            m_spotifyManager = spotifyRemotePackage.GetSpotifyManager();

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            OleMenuCommand menuItem = null;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }

            SettingsManager setManager = SettingsManager.GetSettingsManager();
            setManager.SetTbButtonPrevious(ref menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static CommandPreviousTrack Instance
        {
            get;
            private set;
        }

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
            Instance = new CommandPreviousTrack(package);
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
            
            // execute
            m_spotifyManager.PreviousTrack();
        }
    }
}
