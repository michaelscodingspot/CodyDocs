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
using System.ComponentModel;

namespace CodyDocs
{
    public class OptionPageGrid : DialogPage
    {
        private bool _enableCodyDocs = true;

        [Category("Features")]
        [DisplayName("Enable CodyDocs")]
        [Description("Toggle to enable CodyDocs documentation")]
        public bool EnableCodyDocs
        {
            get { return _enableCodyDocs; }
            set { _enableCodyDocs = value; }
        }
    }
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
    [Guid(OptionsPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionPageGrid),
        "CodyDocs", "General", 0, 0, true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    public sealed class OptionsPackage : Package
    {
        /// <summary>
        /// OptionsPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "05f18475-73cf-40ad-abd7-bb727f987a0d";

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPackage"/> class.
        /// </summary>
        public OptionsPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            EnableDisableCodyDocsCommand.Initialize(this);

            //OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            //if (null != mcs)
            //{
            //    // Create the command for the menu item. 
            //    CommandID menuCommandID = new CommandID(new Guid("e0e79a86-61bf-4d09-8291-709475b1ab60"), (int)0x0100);
            //    //CommandID menuCommandID = new CommandID(GuidList.guidMenuPackage1CmdSet, (int)PkgCmdIDList.cmdidMyCommand);
            //    MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
            //    mcs.AddCommand(menuItem);

            //    CommandID id = new CommandID(Microsoft.VisualStudio.VSConstants.GUID_VSStandardCommandSet97, (int)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.Toolbox);

            //    OleMenuCommand command = new OleMenuCommand(MenuItemCallback, id);
            //    command.BeforeQueryStatus += new EventHandler(MenuItemCallback);

            //    mcs.AddCommand(command);
            //}
        }

        #endregion
    }
}
