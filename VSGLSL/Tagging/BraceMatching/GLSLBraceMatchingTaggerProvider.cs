using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Xannden.VSGLSL.Tagging.BraceMatching
{
	[Export(typeof(IViewTaggerProvider))]
	[ContentType("text")]
	[TagType(typeof(TextMarkerTag))]
	internal class GLSLBraceMatchingTaggerProvider : IViewTaggerProvider
	{
		public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
		{
			return buffer.Properties.GetOrCreateSingletonProperty(() => new GLSLBraceMatchingTagger(textView)) as ITagger<T>;
		}
	}
}
