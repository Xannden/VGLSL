using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.IntelliSense.Completions
{
	internal sealed class GLSLCompletionSource : ICompletionSource
	{
		private ITextBuffer textBuffer;
		private GLSLCompletionSourceProvider provider;
		private List<Completion> keywords;
		private VSSource source;

		internal GLSLCompletionSource(ITextBuffer textBuffer, GLSLCompletionSourceProvider provider, VSSource source)
		{
			this.textBuffer = textBuffer;
			this.provider = provider;
			this.source = source;
			this.keywords = new List<Completion>();

			ImageSource imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphKeyword, StandardGlyphItem.GlyphItemPublic);

			for (SyntaxType type = SyntaxType.AttributeKeyword; type <= SyntaxType.LinePreprocessorKeyword; type++)
			{
				string text = type.GetText();

				this.keywords.Add(new Completion(text, text, text + " Keyword", imageSource, string.Empty));
			}
		}

		public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
		{
			List<Completion> completions = new List<Completion>(this.keywords);

			SyntaxTree tree = this.source.Tree;
			Snapshot snapshot = this.source.CurrentSnapshot;
			int triggerPoint = session.GetTriggerPoint(this.textBuffer).GetPosition(snapshot);

			if (tree != null)
			{
				List<Definition> definitions = tree.Definitions.FindAll(def => def.Scope.Contains(snapshot, triggerPoint));

				foreach (Definition definition in definitions)
				{
					completions.Add(new Completion(definition.Identifier.Identifier, definition.Identifier.Identifier, string.Empty, definition.GetImageSource(this.provider.GlyphService), string.Empty));
				}
			}

			ITrackingSpan span = this.FindTokenSpanAtPosition(session.GetTriggerPoint(this.textBuffer), session);

			completionSets.Add(new CompletionSet("GLSL", "GLSL", span, completions, null));
		}

		public void Dispose()
		{
		}

		private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
		{
			SnapshotPoint currentPoint = session.TextView.Caret.Position.BufferPosition - 1;
			ITextStructureNavigator navigator = this.provider.NavigatorService.GetTextStructureNavigator(this.textBuffer);
			TextExtent extent = navigator.GetExtentOfWord(currentPoint);
			return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
		}
	}
}