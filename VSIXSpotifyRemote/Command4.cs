//------------------------------------------------------------------------------
// <copyright file="Command4.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
//#define NO_ANIM

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;

namespace VSIXSpotifyRemote
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Command4
    {
        private const int kSHOW_TRACK_INTERVAL = 5000; //Milliseconds
        private const int kANIMATE_SPOT_STRING = 125; //ms
#if DEBUG
        private const string kSpotifyOpenString = "Open Spotify(D)";
#else
        private const string kSpotifyOpenString = "Open Spotify";
#endif

#if DEBUG
        private const string kSpotifyStartString = "Start Spotify(D)";
#else
        private const string kSpotifyStartString = "Start Spotify";
#endif

        public static Command4 gCommand4Instance;

        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4179;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("2599fe4a-622d-4cdc-8a4d-a08560938c54");

        private OleMenuCommand myOleCommand;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        internal System.Timers.Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command4"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private Command4(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);

                // var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);

                myOleCommand = new OleMenuCommand(this.MenuItemCallback, menuCommandID);

                //if (null != myOleCommand)
                //{
                //    myOleCommand.Text = "New Text";
                //
                //}
                if (IsSpotifyProcessRunning())
                {
                    if (Command1Package.SpotifyCommandShouldShowText())
                    {
                        myOleCommand.Text = kSpotifyOpenString;
                    }
                    else
                    {
                        myOleCommand.Text = " ";
                    }
                }
                else
                {
                    if (UserPreferences.Default.HideButtonTextOnInactive)
                    {
                        myOleCommand.Text = " ";
                    }
                    else
                    {
                        myOleCommand.Text = kSpotifyStartString;
                    }
                }

                Microsoft.VisualStudio.Shell.ServiceProvider serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider(((EnvDTE.DTE)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE))) as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
                IVsUIShell uiShell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
                uiShell.UpdateCommandUI(0);

                if (IsSpotifyProcessRunning())
                {
                    SpotClientRegisterTrackChange();
                }

                commandService.AddCommand(myOleCommand);

                gCommand4Instance = this;
            }
        }

        public void SetStartupCommandTextState()
        {
            if (!Command1Package.SpotifyCommandShouldShowText())
            {
                myOleCommand.Text = " ";

            }
            else
            {
                if (IsSpotifyProcessRunning())
                {
                    myOleCommand.Text = kSpotifyOpenString;
                }
                else
                {
                    myOleCommand.Text = kSpotifyStartString;
                }
            }
        }

        public void SpotClientRegisterTrackChange()
        {
            if (Command1Package.spotClient != null)
            {
                Command1Package.spotClient.OnTrackChange += SpotClient_OnTrackChange;
                Command1Package.spotClient.ListenForEvents = true;
            }
            else
            {
                Console.WriteLine("SpotClient is null reference, RegisterTrackChange!");
                Debug.Assert(true, "SpotClient null reference.");
            }
        }

        public void SpotClient_OnTrackChange(object sender, SpotifyAPI.Local.TrackChangeEventArgs e)
        {
            if (!UserPreferences.Default.showTrackArtistOnChange)
            {
                Console.WriteLine("ShowTrackArtist is disabled in user preferences.");
                return;
            }

            if(e.NewTrack == null || e.NewTrack.TrackResource == null || 
                e.NewTrack.TrackResource.Name == null || e.NewTrack.AlbumResource.Name == null)
            {
                return;
            }
            string trackName = e.NewTrack.TrackResource.Name;
            string artistName = e.NewTrack.ArtistResource.Name;
            Console.WriteLine("Show New track name: " + trackName);
            myOleCommand.Text = String.Format("{0} - {1}", trackName, artistName);
            Microsoft.VisualStudio.Shell.ServiceProvider serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider(((EnvDTE.DTE)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE))) as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            IVsUIShell uiShell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
            uiShell.UpdateCommandUI(0);
            if (timer != null)
            {
                timer.Stop();
                timer.Start();
            }
            else
            {
                timer = new System.Timers.Timer() { Interval = kSHOW_TRACK_INTERVAL };
                //timer.AutoReset = false;
                timer.Elapsed += TrackChangeTimerTick;
                timer.Start();
            }
        }

        private void TrackChangeTimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("Set Command4 back to original String.");
            timer.Stop();
            timer.Elapsed -= TrackChangeTimerTick;

            // set timer to null, and set string back to original spotify string.
            // done if user disabled animation otherwise execute animation if it is not disabled with define NO_ANIM
            if (!UserPreferences.Default.EnableInteractiveAnimation)
            {
                timer = null;
                if (Command1Package.SpotifyCommandShouldShowText())
                {
                    myOleCommand.Text = kSpotifyOpenString;
                }
                else
                {
                    myOleCommand.Text = " ";
                }
                return;
            }

#if NO_ANIM
            timer = null;
            myOleCommand.Text = kSpotifyOpenString;
#else
            timer.Elapsed += TimerAnimateOriginal;
            timer.Interval = kANIMATE_SPOT_STRING;
            timer.Start();
#endif
        }

        private void TimerAnimateOriginal(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (myOleCommand.Text.Length <= kSpotifyOpenString.Length)
            {
                timer.Stop();
                timer.Elapsed -= TimerAnimateOriginal;
                timer = null;
                if (Command1Package.SpotifyCommandShouldShowText())
                {
                    myOleCommand.Text = kSpotifyOpenString;
                }
                else
                {
                    myOleCommand.Text = " ";
                }
            }
            else
            {
                if (myOleCommand.Text.Length > kSpotifyOpenString.Length)
                {
                    myOleCommand.Text = myOleCommand.Text.Remove(myOleCommand.Text.Length - 1);
                }
                else
                {
                    myOleCommand.Text += " ";
                }
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Command4 Instance
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
            Instance = new Command4(package);
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
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "Command4";

            // Show a message box to prove we were here
            //VsShellUtilities.ShowMessageBox(
            //    this.ServiceProvider,
            //    message,
            //    title,
            //    OLEMSGICON.OLEMSGICON_INFO,
            //    OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            //Process process = Process.Start("Spotify.exe");
            //int id = process.Id;
            //Process tempProc = Process.GetProcessById(id);
            //this.Visible = false;
            //tempProc.WaitForExit();
            //this.Visible = true;

            //myOleCommand.Text = "Test.";

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

            if (spotMain != null)
            {
                WindowHelper.BringProcessToFront(spotMain);
            }
            else
            {
                Console.WriteLine("Spotify not running.");
                if (!SpotifyAPI.Local.SpotifyLocalAPI.IsSpotifyRunning())
                {
                    SpotifyAPI.Local.SpotifyLocalAPI.RunSpotify();
                    System.Threading.Thread.Sleep(300);
                    Command1Package.SpotifyConnect();
                    SpotClientRegisterTrackChange();
                    Command1Package.UpdateCommandsHiddenState();
                }
                else
                {
                    Console.WriteLine("Something went wrong..\n "
                        + "Spotify seems to be running but cant focus.");
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
    }
}

public static class WindowHelper
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