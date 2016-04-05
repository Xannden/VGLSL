using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.IntelliSense.QuickTips
{
	[Export(typeof(IQuickInfoSourceProvider))]
	[Name("GLSL QuickInfo Source")]
	[Order(Before = "Default Quick Info Presenter")]
	[ContentType(GLSLConstants.ContentType)]
	internal class GLSLQuickInfoSourceProvider : IQuickInfoSourceProvider
	{
		[Import]
		internal IGlyphService GlyphService { get; private set; } = null;

		[Import]
		internal IClassificationFormatMapService FormatMap { get; private set; } = null;

		[Import]
		internal IClassificationTypeRegistryService TypeRegistry { get; private set; } = null;

		public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
		{
			return new GLSLQuickInfoSource(this, VSSource.GetOrCreate(textBuffer), textBuffer);
		}
	}
}
