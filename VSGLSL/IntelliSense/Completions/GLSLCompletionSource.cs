using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Xannden.GLSL.BuiltIn;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Intellisense.Completions
{
	internal sealed class GLSLCompletionSource : ICompletionSource
	{
		private readonly ITextBuffer textBuffer;
		private readonly GLSLCompletionSourceProvider provider;
		private readonly List<Completion> keywords = new List<Completion>();
		private readonly List<Completion> builtIn = new List<Completion>();
		private readonly VSSource source;
		private readonly IClassificationFormatMap formatMap;

		internal GLSLCompletionSource(ITextBuffer textBuffer, GLSLCompletionSourceProvider provider, VSSource source)
		{
			this.textBuffer = textBuffer;
			this.provider = provider;
			this.source = source;
			this.formatMap = this.provider.FormatMap.GetClassificationFormatMap("text");

			ImageSource keywordIcon = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphKeyword, StandardGlyphItem.GlyphItemPublic);

			for (SyntaxType type = SyntaxType.AttributeKeyword; type <= SyntaxType.LinePreprocessorKeyword; type++)
			{
				string text = type.GetText();

				TextBlock textBlock = new TextBlock();

				if (type.IsPreprocessor())
				{
					textBlock.Inlines.Add(text.ToRun(this.formatMap, this.provider.TypeRegistry.GetClassificationType(GLSLConstants.PreprocessorKeyword)));
				}
				else
				{
					textBlock.Inlines.Add(text.ToRun(this.formatMap, this.provider.TypeRegistry.GetClassificationType(GLSLConstants.Keyword)));
				}

				textBlock.Inlines.Add(new Run(" Keyword"));

				this.keywords.Add(new GLSLCompletion(textBlock, text, text + "Keyword", keywordIcon));
			}

			foreach (string key in BuiltInData.Instance.Definitions.Keys)
			{
				this.builtIn.Add(new GLSLCompletion(BuiltInData.Instance.Definitions[key][0].ToTextBlock(this.formatMap, this.provider.TypeRegistry, BuiltInData.Instance.Definitions[key].Count), BuiltInData.Instance.Definitions[key][0], BuiltInData.Instance.Definitions[key][0].GetImageSource(this.provider.GlyphService)));
			}
		}

		public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
		{
			if (session == null || completionSets == null)
			{
				return;
			}

			List<Completion> completions = new List<Completion>(this.keywords);

			SyntaxTree tree = this.source.Tree;
			Snapshot snapshot = this.source.CurrentSnapshot;
			int triggerPoint = session.GetTriggerPoint(this.textBuffer).GetPosition(snapshot);

			if (tree != null)
			{
				List<Definition> definitions = new List<Definition>();

				foreach (string key in tree.Definitions.Keys)
				{
					Definition definition = tree.Definitions[key].Find(def => def.Scope.Contains(snapshot, triggerPoint));

					if (definition != null)
					{
						definitions.Add(definition);
					}
				}

				SyntaxNode node = tree.GetNodeFromPosition(snapshot, triggerPoint);
				Definition currentFunction = null;

				foreach (SyntaxNode ancestor in node.Ancestors)
				{
					if (ancestor.SyntaxType == SyntaxType.FunctionDefinition)
					{
						FunctionDefinitionSyntax function = ancestor as FunctionDefinitionSyntax;

						currentFunction = function.FunctionHeader.Identifier.Definition;
						break;
					}
				}

				for (int i = 0; i < definitions.Count; i++)
				{
					if (definitions[i].Name != currentFunction?.Name)
					{
						completions.Add(new GLSLCompletion(definitions[i].ToTextBlock(this.formatMap, this.provider.TypeRegistry, definitions[i].Overloads.Count - 1), definitions[i], definitions[i].GetImageSource(this.provider.GlyphService)));
					}
				}
			}

			for (int i = 0; i < this.builtIn.Count; i++)
			{
				if (!completions.Contains(completion => completion.DisplayText == this.builtIn[i].DisplayText))
				{
					completions.Add(this.builtIn[i]);
				}
			}

			ITrackingSpan span = this.FindTokenSpanAtPosition(session);

			completions.Sort((first, second) => string.Compare(first.DisplayText, second.DisplayText, System.StringComparison.Ordinal));

			completionSets.Add(new CompletionSet("GLSL", "GLSL", span, completions, null));
		}

		public void Dispose()
		{
		}

		private ITrackingSpan FindTokenSpanAtPosition(ICompletionSession session)
		{
			SnapshotPoint currentPoint = session.TextView.Caret.Position.BufferPosition - 1;
			ITextStructureNavigator navigator = this.provider.NavigatorService.GetTextStructureNavigator(this.textBuffer);
			TextExtent extent = navigator.GetExtentOfWord(currentPoint);
			return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
		}
	}
}