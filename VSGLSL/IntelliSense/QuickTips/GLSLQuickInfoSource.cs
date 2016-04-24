using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.IntelliSense.QuickTips
{
	internal class GLSLQuickInfoSource : IQuickInfoSource
	{
		private GLSLQuickInfoSourceProvider provider;
		private Source source;
		private ITextBuffer textBuffer;

		public GLSLQuickInfoSource(GLSLQuickInfoSourceProvider provider, Source source, ITextBuffer textBuffer)
		{
			this.provider = provider;
			this.source = source;
			this.textBuffer = textBuffer;
		}

		public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan)
		{
			applicableToSpan = null;

			if (session == null)
			{
				throw new ArgumentNullException(nameof(session));
			}

			if (quickInfoContent == null)
			{
				throw new ArgumentNullException(nameof(quickInfoContent));
			}

			VSSnapshot snapshot = this.source.CurrentSnapshot as VSSnapshot;

			if (snapshot == null)
			{
				return;
			}

			int triggerPosition = session.GetTriggerPoint(this.textBuffer).GetPosition(snapshot.TextSnapshot);

			SyntaxTree tree = this.source.Tree;
			IdentifierSyntax identifier = tree.GetNodeFromPosition(snapshot, triggerPosition) as IdentifierSyntax;

			if (identifier?.Definition == null)
			{
				return;
			}

			quickInfoContent.Add(new QuickTipPanel(identifier.Definition.GetIcon(this.provider.GlyphService), this.CreateTextBlock(identifier.Definition, snapshot), null));

			applicableToSpan = (identifier.Span as VSTrackingSpan).TrackingSpan;
		}

		public void Dispose()
		{
		}

		private string GetClassificationName(SyntaxToken token, Snapshot snapshot)
		{
			IdentifierSyntax identifier = token as IdentifierSyntax;

			if (token.SyntaxType.IsPreprocessor())
			{
				return GLSLConstants.PreprocessorKeyword;
			}
			else if (identifier?.Definition?.Kind == DefinitionKind.Macro)
			{
				return GLSLConstants.Macro;
			}
			else if (token?.IsExcludedCode() ?? false)
			{
				return GLSLConstants.ExcludedCode;
			}
			else if (token.SyntaxType.IsPunctuation())
			{
				return GLSLConstants.Punctuation;
			}
			else if (token?.IsPreprocessorText() ?? false)
			{
				return GLSLConstants.PreprocessorText;
			}
			else if (token.SyntaxType.IsKeyword())
			{
				return GLSLConstants.Keyword;
			}
			else if (token.SyntaxType.IsNumber())
			{
				return GLSLConstants.Number;
			}
			else if (identifier?.Definition != null)
			{
				switch (identifier.Definition.Kind)
				{
					case DefinitionKind.Field:
						return GLSLConstants.Field;
					case DefinitionKind.Function:
						return GLSLConstants.Function;
					case DefinitionKind.GlobalVariable:
						return GLSLConstants.GlobalVariable;
					case DefinitionKind.LocalVariable:
						return GLSLConstants.LocalVariable;
					case DefinitionKind.Macro:
						return GLSLConstants.Macro;
					case DefinitionKind.Parameter:
						return GLSLConstants.Parameter;
					case DefinitionKind.TypeName:
					case DefinitionKind.InterfaceBlock:
						return GLSLConstants.TypeName;
					default:
						return GLSLConstants.Identifier;
				}
			}
			else if (token.SyntaxType == SyntaxType.IdentifierToken)
			{
				return GLSLConstants.Identifier;
			}

			return PredefinedClassificationTypeNames.FormalLanguage;
		}

		private List<Run> GetRuns(Definition definition, IClassificationFormatMap formatMap, Snapshot snapshot)
		{
			List<Run> runs = new List<Run>();

			Run typeRun = null;

			switch (definition.Kind)
			{
				case DefinitionKind.LocalVariable:
					typeRun = new Run("(local variable) ");
					break;
				case DefinitionKind.Field:
					typeRun = new Run("(field) ");
					break;
				case DefinitionKind.GlobalVariable:
					typeRun = new Run("(global variable) ");
					break;
				case DefinitionKind.Macro:
					typeRun = new Run("(macro) ");
					break;
				case DefinitionKind.Parameter:
					typeRun = new Run("(parameter) ");
					break;
				case DefinitionKind.TypeName:
					typeRun = new Run("(struct) ");
					break;
			}

			if (typeRun != null)
			{
				typeRun.SetTextProperties(formatMap.GetTextProperties(this.provider.TypeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage)));
				runs.Add(typeRun);
			}

			List<SyntaxToken> tokens = definition.GetTokens();

			for (int i = 0; i < tokens.Count; i++)
			{
				runs.Add(tokens[i].ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(tokens[i], snapshot))));
			}

			return runs;
		}

		private TextBlock CreateTextBlock(Definition definition, Snapshot snapshot)
		{
			TextBlock block = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};

			IClassificationFormatMap formatMap = this.provider.FormatMap.GetClassificationFormatMap("text");

			block.SetTextProperties(formatMap.DefaultTextProperties);

			block.Inlines.AddRange(this.GetRuns(definition, formatMap, snapshot));

			return block;
		}
	}
}