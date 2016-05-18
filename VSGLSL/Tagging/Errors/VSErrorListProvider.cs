using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Errors
{
	[Export(typeof(IWpfTextViewCreationListener))]
	[Name("GLSL ErrorList")]
	[ContentType(GLSLConstants.ContentType)]
	[TextViewRole(PredefinedTextViewRoles.Interactive)]
	internal sealed class VSErrorListProvider : IWpfTextViewCreationListener
	{
		[Import]
		internal SVsServiceProvider ServiceProvider { get; set; }

		public void TextViewCreated(IWpfTextView textView)
		{
			textView.Properties.GetOrCreateSingletonProperty(() => new VSErrorList(this.ServiceProvider, textView));
		}
	}
}
