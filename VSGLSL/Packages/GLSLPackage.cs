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
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]

	[ProvideService(typeof(GLSLLanguageInfo))]
	[ProvideLanguageService(typeof(GLSLLanguageInfo), GLSLConstants.Name, 106, EnableLineNumbers = true, ShowCompletion = true, EnableAdvancedMembersOption = true, RequestStockColors = true)]
	[ProvideLanguageExtension(typeof(GLSLLanguageInfo), ".glsl")]

	[ProvideEditorFactory(typeof(GLSLEditorFactoryWithoutEncoding), 101)]
	[ProvideEditorFactory(typeof(GLSLEditorFactoryWithEncoding), 102)]

	[ProvideEditorLogicalView(typeof(GLSLEditorFactoryWithoutEncoding), VSConstants.LOGVIEWID.TextView_string)]
	[ProvideEditorLogicalView(typeof(GLSLEditorFactoryWithEncoding), VSConstants.LOGVIEWID.TextView_string)]

	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), GLSLConstants.VertexExtension, 50)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), GLSLConstants.VertexExtension, 49)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), GLSLConstants.FragmentExtension, 50)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), GLSLConstants.FragmentExtension, 49)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), GLSLConstants.GeometryExtension, 50)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), GLSLConstants.GeometryExtension, 49)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), GLSLConstants.ComputeExtension, 50)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), GLSLConstants.ComputeExtension, 49)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), GLSLConstants.TessellationControlExtension, 50)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), GLSLConstants.TessellationControlExtension, 49)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), GLSLConstants.TessellationEvaluationExtension, 50)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), GLSLConstants.TessellationEvaluationExtension, 49)]

	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithoutEncoding), ".*", 2)]
	[ProvideEditorExtension(typeof(GLSLEditorFactoryWithEncoding), ".*", 1)]

	[Guid(GLSLConstants.GLSLPackageString)]
	public sealed class GLSLPackage : Package
	{
		private GLSLLanguageInfo languageInfo;
		private uint cookie;
		private IConnectionPoint connectionPoint;

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
			((IConnectionPointContainer)textManager).FindConnectionPoint(ref events2Guid, out this.connectionPoint);

			this.connectionPoint.Advise(this.Preferences, out this.cookie);
		}

		protected override void Dispose(bool disposing)
		{
			this.connectionPoint.Unadvise(this.cookie);

			base.Dispose(disposing);
		}
	}
}