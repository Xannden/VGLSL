using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.BraceCompletion;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Formatting.BraceCompletion
{
	[Export(typeof(IBraceCompletionContextProvider))]
	[BracePair('{', '}')]
	[BracePair('[', ']')]
	[BracePair('(', ')')]
	[ContentType(GLSLConstants.ContentType)]
	internal sealed class GLSLBraceCompletionContextProvider : IBraceCompletionContextProvider
	{
		public bool TryCreateContext(ITextView textView, SnapshotPoint openingPoint, char openingBrace, char closingBrace, out IBraceCompletionContext context)
		{
			VSSource source = VSSource.GetOrCreate(textView.TextBuffer);
			Snapshot snapshot = source.CurrentSnapshot;

			for (int i = 0; i < source.CommentSpans.Count; i++)
			{
				if (source.CommentSpans[i].GetSpan(snapshot).Contains(openingPoint.Position))
				{
					context = null;
					return false;
				}
			}

			context = new GLSLBraceCompletionContext();
			return true;
		}
	}
}
