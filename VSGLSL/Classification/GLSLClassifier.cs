using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Classification
{
	internal sealed class GLSLClassifier : IClassifier
	{
		private IClassificationTypeRegistryService classificationTypeRegistryService;
		private GLSLLexer lexer = new GLSLLexer();
		private VSSource source;

		internal GLSLClassifier(VSSource source, IClassificationTypeRegistryService classificationTypeRegistryService)
		{
			this.source = source;
			this.classificationTypeRegistryService = classificationTypeRegistryService;

			this.source.DoneParsing += this.Source_DoneParsing;
		}

		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			LinkedList<Token> tokens = this.lexer.Run(new VSSnapshot(this.source, span.Snapshot), span.Start.GetContainingLine().ExtentIncludingLineBreak.Span.ToGLSLSpan());
			List<ClassificationSpan> spans = new List<ClassificationSpan>();
			IReadOnlyList<GLSL.Text.TrackingSpan> commentSpans = this.source.CommentSpans;
			VSSnapshot currentSnapshot = new VSSnapshot(this.source, span.Snapshot);

			SyntaxTree tree = this.source.Tree;

			if (tree == null)
			{
				return spans;
			}

			foreach (Token token in tokens)
			{
				if (!span.OverlapsWith(token.Span.ToVSSpan()))
				{
					continue;
				}

				SyntaxNode node = tree?.GetNodeFromPosition(this.source.CurrentSnapshot, token.Span.Start);

				IdentifierSyntax identifier = node as IdentifierSyntax;

				string classificationName = string.Empty;

				if (token.SyntaxType.IsPreprocessor())
				{
					classificationName = GLSLConstants.PreprocessorKeyword;
				}
				else if (identifier?.Definition?.DefinitionType == DefinitionType.Macro)
				{
					classificationName = GLSLConstants.Macro;
				}
				else if (node?.IsExcludedCode() ?? false)
				{
					classificationName = GLSLConstants.ExcludedCode;
				}
				else if (token.SyntaxType.IsPunctuation())
				{
					classificationName = GLSLConstants.Punctuation;
				}
				else if (node?.IsPreprocessorText() ?? false)
				{
					classificationName = GLSLConstants.PreprocessorText;
				}
				else if (token.SyntaxType.IsKeyword())
				{
					classificationName = GLSLConstants.Keyword;
				}
				else if (token.SyntaxType.IsNumber())
				{
					classificationName = GLSLConstants.Number;
				}
				else if (identifier?.Definition != null)
				{
					switch (identifier.Definition.DefinitionType)
					{
						case DefinitionType.Field:
							classificationName = GLSLConstants.Field;
							break;
						case DefinitionType.Function:
							classificationName = GLSLConstants.Function;
							break;
						case DefinitionType.GlobalVariable:
							classificationName = GLSLConstants.GlobalVariable;
							break;
						case DefinitionType.LocalVariable:
							classificationName = GLSLConstants.LocalVariable;
							break;
						case DefinitionType.Macro:
							classificationName = GLSLConstants.Macro;
							break;
						case DefinitionType.Parameter:
							classificationName = GLSLConstants.Parameter;
							break;
						case DefinitionType.TypeName:
							classificationName = GLSLConstants.TypeName;
							break;
						default:
							classificationName = GLSLConstants.Identifier;
							break;
					}
				}
				else if (token.SyntaxType == SyntaxType.IdentifierToken)
				{
					classificationName = GLSLConstants.Identifier;
				}

				if (!string.IsNullOrEmpty(classificationName))
				{
					SnapshotSpan snapshotSpan = new SnapshotSpan(span.Snapshot, token.Span.ToVSSpan());

					spans.Add(new ClassificationSpan(snapshotSpan, this.classificationTypeRegistryService.GetClassificationType(classificationName)));
				}
			}

			this.ColorComments(span, spans, currentSnapshot);

			return spans;
		}

		private void ColorComments(SnapshotSpan span, List<ClassificationSpan> spans, VSSnapshot snapshot)
		{
			IReadOnlyList<GLSL.Text.TrackingSpan> commentSpans = this.source.CommentSpans;

			for (int i = 0; i < commentSpans.Count; i++)
			{
				Span commentSpan = commentSpans[i].GetSpan(snapshot).ToVSSpan();
				if (span.IntersectsWith(commentSpan))
				{
					spans.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, commentSpan), this.classificationTypeRegistryService.GetClassificationType(GLSLConstants.Comment)));
				}
			}
		}

		private void Source_DoneParsing(object sender, EventArgs e)
		{
			this.ClassificationChanged.Invoke(this, new ClassificationChangedEventArgs(this.source.CurrentSnapshot.GetSnapshotSpan(this.source.CurrentSnapshot.Span)));
		}
	}
}