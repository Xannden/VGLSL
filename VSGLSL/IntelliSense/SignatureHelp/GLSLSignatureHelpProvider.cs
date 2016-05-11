using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Intellisense.SignatureHelp
{
	[Export(typeof(ISignatureHelpSourceProvider))]
	[ContentType(GLSLConstants.ContentType)]
	[Name("GLSL Signature Help")]
	internal sealed class GLSLSignatureHelpProvider : ISignatureHelpSourceProvider
	{
		public ISignatureHelpSource TryCreateSignatureHelpSource(ITextBuffer textBuffer)
		{
			return new GLSLSignatureHelpSource(VSSource.GetOrCreate(textBuffer));
		}
	}
}
