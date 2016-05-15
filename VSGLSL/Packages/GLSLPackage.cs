using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Packages
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
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[ProvideService(typeof(GLSLLanguageInfo))]
	[ProvideLanguageService(typeof(GLSLLanguageInfo), GLSLConstants.Name, 106, EnableLineNumbers = true, ShowCompletion = true, EnableAdvancedMembersOption = true, RequestStockColors = true)]
	[ProvideLanguageExtension(typeof(GLSLLanguageInfo), ".glsl")]
	[ProvideEditorFactory(typeof(GLSLEditorFactoryWithoutEncoding), 101)]
	[ProvideEditorFactory(typeof(GLSLEditorFactoryWithEncoding), 102)]
	[ProvideEditorLogicalView(typeof(GLSLEditorFactoryWithoutEncoding), VSConstants.LOGVIEWID.TextView_string)]
	[ProvideEditorLogicalView(typeof(GLSLEditorFactoryWithEncoding), VSConstants.LOGVIEWID.TextView_string)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), ".glsl", 50)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), ".glsl", 49)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), ".*", 2)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), ".*", 1)]
	[Guid(GLSLConstants.GLSLPackageString)]
	public sealed class GLSLPackage : Package
	{
		private GLSLLanguageInfo languageInfo;

		public static GLSLPackage Instance { get; private set; }

		internal GLSLPreferences Preferences { get; private set; }

		public new object GetService(Type serviceType)
		{
			return base.GetService(serviceType);
		}

		protected override void Initialize()
		{
			base.Initialize();

			Instance = this;

			this.languageInfo = new GLSLLanguageInfo();
			((IServiceContainer)this).AddService(typeof(GLSLLanguageInfo), this.languageInfo, true);

			this.RegisterEditorFactory(new GLSLEditorFactoryWithoutEncoding(this));
			this.RegisterEditorFactory(new GLSLEditorFactoryWithEncoding(this));

			IVsTextManager textManager = (IVsTextManager)this.GetService(typeof(SVsTextManager));

			LANGPREFERENCES[] lanaguagePreferences = new LANGPREFERENCES[1];
			lanaguagePreferences[0].guidLang = typeof(GLSLLanguageInfo).GUID;
			ErrorHandler.ThrowOnFailure(textManager.GetUserPreferences(null, null, lanaguagePreferences, null));
			this.Preferences = new GLSLPreferences(lanaguagePreferences[0]);

			Guid events2Guid = typeof(IVsTextManagerEvents2).GUID;
			IConnectionPoint connectionPoint;
			((IConnectionPointContainer)textManager).FindConnectionPoint(ref events2Guid, out connectionPoint);

			uint cookie;
			connectionPoint.Advise(this.Preferences, out cookie);
		}
	}
}