using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Command
{
	[Export(typeof(IVsTextViewCreationListener))]
	[Name("GLSL Command Handler")]
	[ContentType(GLSLConstants.ContentType)]
	[TextViewRole(PredefinedTextViewRoles.Editable)]
	internal sealed class GLSLCommandHandlersProvider : IVsTextViewCreationListener
	{
		internal IVsEditorAdaptersFactoryService AdapterService { get; set; }

		public void VsTextViewCreated(IVsTextView textViewAdapter)
		{
			IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
			this.AdapterService = componentModel.GetService<IVsEditorAdaptersFactoryService>();

			ITextView textView = this.AdapterService.GetWpfTextView(textViewAdapter);
			if (textView == null)
			{
				return;
			}

			textView.Properties.GetOrCreateSingletonProperty(() => new GLSLCommandHandlers(textViewAdapter, textView));
		}
	}
}
