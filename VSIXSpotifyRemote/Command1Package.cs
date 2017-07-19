//------------------------------------------------------------------------------
// <copyright file="Command1Package.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using EnvDTE80;
using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web;
using System.Windows.Forms;

namespace VSIXSpotifyRemote
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#1110", "#1112", "1.0", IconResourceID = 1400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(Command1Package.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideToolWindow(typeof(SettingsWindow))]
    // --- Microsoft.VisualStudio.VSConstants.UICONTEXT_NoSolution
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    [ProvideToolWindow(typeof(SpotifyRemotePlayListWindow))]
    public sealed class Command1Package : Package
    {
        /// <summary>
        /// Command1Package GUID string.
        /// </summary>
        public const string PackageGuidString = "255a4275-42f0-413a-96f0-6da1d823c63a";






        // for exit.
        public DTE2 ApplicationObject
        {
            get
            {
                if (m_applicationObject == null)
                {
                    // Get an instance of the currently running Visual Studio IDE
                    DTE dte = (DTE)GetService(typeof(DTE));
                    m_applicationObject = dte as DTE2;
                }
                return m_applicationObject;
            }
        }

        public void HandleVisualStudioShutDown()
        {
            Console.WriteLine("Handle Shutdown.");
            spotClient.Dispose();
        }

        public void OnExtensionReady()
        {
#if DEBUG
            Triggers.Default.SettingsShownOnce = false;
#endif
            UpdateCommandsHiddenState();

            // Check for first time opening since installing the extension.
            if (!Triggers.Default.SettingsShownOnce)
            {
                Console.WriteLine("Show settings, first time.");
                Triggers.Default.SettingsShownOnce = true;
                Triggers.Default.Save();

                SettingsWindowCommand.swc.ShowSettings();
            }
        }



        private DTE2 m_applicationObject = null;
        DTEEvents m_packageDTEEvents = null;

        public static SpotifyAPI.Local.SpotifyLocalAPI spotClient;
        public static SpotifyAPI.Web.SpotifyWebAPI spotWeb;
        private static bool spotClientConnected = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="Command1"/> class.
        /// </summary>
        public Command1Package()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.


#if DEBUG
            Console.WriteLine("SpotifyRemote Debug build.");
#endif
            SpotifyConnect();

        }

        public static bool GetSpotifyConnectionStatus()
        {
            return spotClientConnected;
        }

        public static bool SpotifyConnect()
        {
            if (SpotifyAPI.Local.SpotifyLocalAPI.IsSpotifyRunning())
            {
                if (spotClient == null)
                {
                    spotClient = new SpotifyAPI.Local.SpotifyLocalAPI();
                }

                //if (spotWeb == null)
                //{
                //    AuthenticateSpotifyWeb();
                //   // MessageBox.Show(spotWeb.AccessToken);
                //}

                bool connected = spotClient.Connect();

#if DEBUG
                Console.WriteLine("Spotify Client Connected: {0}", connected);
                Debug.Assert(connected);
#endif          
                spotClientConnected = connected;
                return connected;
            }
            spotClientConnected = false;
            return false;
        }


        public static bool CommandShouldShowWhenInactive()
        {
            return UserPreferences.Default.HideButtonTextOnInactive;
        }

        public static bool CommandShouldShowText()
        {
            return (UserPreferences.Default.TextVisibility == 0);
        }

        public static bool SpotifyCommandShouldShowText()
        {
            return ((UserPreferences.Default.TextVisibility == 2) ? false : true);
        }


        public static bool IsSpotifyProcessRunning()
        {
            System.Diagnostics.Process[] retrevedProc = System.Diagnostics.Process.GetProcessesByName("Spotify");
            System.Diagnostics.Process spotMain = null;
            foreach (System.Diagnostics.Process item in retrevedProc)
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

        public static void UpdateCommandsHiddenState()
        {
            if (Command1.Instance == null || Command2.Instance == null || Command3.Instance == null)
            {
                return;
            }

            Command1.Instance.UpdateTextHiddenState();
            Command2.Instance.UpdateTextHiddenState();
            Command3.Instance.UpdateTextHiddenState();
        }

        public static bool isAuthenticated()
        {
            if (spotWeb != null)
            {
                SpotifyAPI.Web.Models.PrivateProfile pp = spotWeb.GetPrivateProfile();
                if(!pp.HasError())
                {
                    return true;
                }
            }

            return false;
        }

        public async static void AuthenticateSpotifyWeb()
        {

            //Check first if we already have a token from Spotify.
            if(ApplicationData.Default.SpotifyToken != "")
            {
                spotWeb = new SpotifyWebAPI();
                spotWeb.AccessToken = ApplicationData.Default.SpotifyToken;
                spotWeb.TokenType = "Bearer";
                SpotifyAPI.Web.Models.PrivateProfile pp = spotWeb.GetPrivateProfile();
                if(!pp.HasError())
                {
                    // success

                    return;
                }
                else
                {
                    //failed, is token invalid ?
#if DEBUG
                    MessageBox.Show("Auth from saved token failed.");
#endif  
                    spotWeb.Dispose();

                }
                
            }


            WebAPIFactory webApiFactory = new WebAPIFactory(
                 "http://localhost",
                 8000,
                 "3a922edff6af43e9be7abb98cf217220",
                 Scope.UserReadPrivate | Scope.PlaylistReadPrivate | Scope.UserLibraryRead | Scope.UserReadRecentlyPlayed | Scope.UserTopRead,
                 TimeSpan.FromSeconds(20)
            );

            try
            {
                //This will open the user's browser and returns once
                //the user is authorized.
                spotWeb = await webApiFactory.GetWebApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (spotWeb == null)
                return;

            Console.WriteLine("Token retreived succesfully from spotify web service.");
            ApplicationData.Default.SpotifyToken = spotWeb.AccessToken;
            ApplicationData.Default.Save();
            return;

        }





        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            SettingsWindowCommand.Initialize(this);
            OpenSettingsCommand.Initialize(this);
            Command1.Initialize(this);
            Command2.Initialize(this);
            Command3.Initialize(this);
            Command4.Initialize(this);



            m_packageDTEEvents = ApplicationObject.Events.DTEEvents;
            m_packageDTEEvents.OnBeginShutdown += new _dispDTEEvents_OnBeginShutdownEventHandler(HandleVisualStudioShutDown);
            m_packageDTEEvents.OnStartupComplete += M_packageDTEEvents_OnStartupComplete;
            SpotifyRemotePlayListWindowCommand.Initialize(this);
            Command5.Initialize(this);
        }

        private void M_packageDTEEvents_OnStartupComplete()
        {
            OnExtensionReady();
        }


        #endregion
    }



}
