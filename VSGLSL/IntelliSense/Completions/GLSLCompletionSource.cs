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
using Xannden.GLSL.Settings;
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
		private static List<Completion> keywords = new List<Completion>();
		private static Dictionary<ShaderType, List<Completion>> builtIn = new Dictionary<ShaderType, List<Completion>>();

		private readonly ITextBuffer textBuffer;
		private readonly GLSLCompletionSourceProvider provider;
		private readonly VSSource source;
		private readonly IClassificationFormatMap formatMap;

		internal GLSLCompletionSource(ITextBuffer textBuffer, GLSLCompletionSourceProvider provider, VSSource source)
		{
			this.textBuffer = textBuffer;
			this.provider = provider;
			this.source = source;
			this.formatMap = provider.FormatMap.GetClassificationFormatMap("text");
		}

		public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
		{
			if (session == null || completionSets == null)
			{
				return;
			}

			List<Completion> completions = new List<Completion>(keywords);

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
						int overloads = definitions[i].Overloads.Count;

						if (BuiltInData.Instance.Definitions.ContainsKey(definitions[i].Name.Text))
						{
							overloads += BuiltInData.Instance.Definitions[definitions[i].Name.Text].Count;
						}

						completions.Add(new GLSLCompletion(definitions[i].ToTextBlock(this.formatMap, this.provider.TypeRegistry, definitions[i].Overloads.Count - 1), definitions[i], definitions[i].GetImageSource(this.provider.GlyphService)));
					}
				}
			}

			for (int i = 0; i < builtIn.Count; i++)
			{
				if (!completions.Contains(completion => completion.DisplayText == builtIn[this.source.Type][i].DisplayText))
				{
					completions.Add(builtIn[this.source.Type][i]);
				}
			}

			ITrackingSpan span = this.FindTokenSpanAtPosition(session);

			completions.Sort((first, second) => string.Compare(first.DisplayText, second.DisplayText, System.StringComparison.Ordinal));

			completionSets.Add(new CompletionSet("GLSL", "GLSL", span, completions, null));
		}

		public void Dispose()
		{
		}

		internal static void LoadDefaultCompletions(IGlyphService glyphService, IClassificationFormatMapService formatMapService, IClassificationTypeRegistryService typeRegistry)
		{
			IClassificationFormatMap formatMap = formatMapService.GetClassificationFormatMap("text");

			ImageSource keywordIcon = glyphService.GetGlyph(StandardGlyphGroup.GlyphKeyword, StandardGlyphItem.GlyphItemPublic);

			for (SyntaxType syntaxType = SyntaxType.AttributeKeyword; syntaxType <= SyntaxType.LinePreprocessorKeyword; syntaxType++)
			{
				string text = syntaxType.GetText();

				TextBlock textBlock = new TextBlock();

				if (syntaxType.IsPreprocessor())
				{
					textBlock.Inlines.Add(text.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.PreprocessorKeyword)));
				}
				else
				{
					textBlock.Inlines.Add(text.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Keyword)));
				}

				textBlock.Inlines.Add(new Run(" Keyword"));

				keywords.Add(new GLSLCompletion(textBlock, text, text + "Keyword", keywordIcon));
			}

			for (int j = 1; j <= 32; j *= 2)
			{
				builtIn.Add((ShaderType)j, new List<Completion>());
			}

			foreach (List<Definition> definitions in BuiltInData.Instance.Definitions.Values)
			{
				for (int i = 0; i < definitions.Count; i++)
				{
					GLSLCompletion completion = new GLSLCompletion(definitions[i].ToTextBlock(formatMap, typeRegistry, definitions.Count), definitions[i], definitions[i].GetImageSource(glyphService));

					if (definitions[i].ShaderType.HasFlag<ShaderType>(ShaderType.Compute))
					{
						builtIn[ShaderType.Compute].Add(completion);
					}

					if (definitions[i].ShaderType.HasFlag<ShaderType>(ShaderType.Vertex))
					{
						builtIn[ShaderType.Vertex].Add(completion);
					}

					if (definitions[i].ShaderType.HasFlag<ShaderType>(ShaderType.Geometry))
					{
						builtIn[ShaderType.Geometry].Add(completion);
					}

					if (definitions[i].ShaderType.HasFlag<ShaderType>(ShaderType.TessellationControl))
					{
						builtIn[ShaderType.TessellationControl].Add(completion);
					}

					if (definitions[i].ShaderType.HasFlag<ShaderType>(ShaderType.TessellationEvaluation))
					{
						builtIn[ShaderType.TessellationEvaluation].Add(completion);
					}

					if (definitions[i].ShaderType.HasFlag<ShaderType>(ShaderType.Fragment))
					{
						builtIn[ShaderType.Fragment].Add(completion);
					}
				}
			}
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