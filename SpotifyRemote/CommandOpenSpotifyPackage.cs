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

namespace SpotifyRemoteNS
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
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(SpotifyRemote.PackageGuidString)]
    [ProvideToolWindow(typeof(SpotifyRemoteNS.SpotifyRemoteSettings))]
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    public sealed class SpotifyRemote : Package
    {
        /// <summary>
        /// CommandOpenSpotifyPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "c8693e55-5320-47e8-ba93-631dcee86150";


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

        private DTE2 m_applicationObject = null;
        DTEEvents m_packageDTEEvents = null;
        private SpotifyManager m_spotifyManager;




        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyRemote"/> class.
        /// </summary>
        public SpotifyRemote()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.

            m_spotifyManager = new SpotifyManager();
            m_spotifyManager.Initialize();

        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            


            CommandOpenSpotify.Initialize(this);
            base.Initialize();

           

            m_packageDTEEvents = ApplicationObject.Events.DTEEvents;
            m_packageDTEEvents.OnBeginShutdown += SpotifyRemoteDTEEventBeginShutdown;
            m_packageDTEEvents.OnStartupComplete += SpotifyRemoteDTEEventOnStartupComplete;
            CommandNextTrack.Initialize(this);
            CommandPlayPause.Initialize(this);
            CommandPreviousTrack.Initialize(this);
            SpotifyRemoteNS.SpotifyRemoteSettingsCommand.Initialize(this);

        }

        public ref SpotifyManager GetSpotifyManager()
        {
           return ref m_spotifyManager;
        }

        private void SpotifyRemoteDTEEventOnStartupComplete()
        {

           // throw new NotImplementedException();
        }

        private void SpotifyRemoteDTEEventBeginShutdown()
        {
            m_spotifyManager.Destroy();

        }

        #endregion
    }
}
