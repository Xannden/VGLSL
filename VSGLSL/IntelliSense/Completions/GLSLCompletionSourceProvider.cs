using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.IntelliSense.Completions
{
	[Export(typeof(ICompletionSourceProvider))]
	[ContentType(GLSLConstants.ContentType)]
	[Name("GLSL Completions")]
	internal class GLSLCompletionSourceProvider : ICompletionSourceProvider
	{
		[Import]
		internal ITextStructureNavigatorSelectorService NavigatorService { get; private set; }

		[Import]
		internal IGlyphService GlyphService { get; private set; } = null;

		public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
		{
			VSSource source = VSSource.GetOrCreate(textBuffer);

			return new GLSLCompletionSource(textBuffer, this, source);
		}
	}
}
