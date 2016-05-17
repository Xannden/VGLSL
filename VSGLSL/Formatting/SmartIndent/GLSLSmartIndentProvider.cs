using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Packages;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Formatting.SmartIndent
{
	[Export(typeof(ISmartIndentProvider))]
	[ContentType(GLSLConstants.ContentType)]
	internal sealed class GLSLSmartIndentProvider : ISmartIndentProvider
	{
		public ISmartIndent CreateSmartIndent(ITextView textView)
		{
			return new GLSLSmartIndent(VSSource.GetOrCreate(textView.TextBuffer), textView, GLSLPackage.Instance.Preferences);
		}
	}
}
