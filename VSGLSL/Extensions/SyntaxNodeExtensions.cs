using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Extensions
{
	internal static class SyntaxNodeExtensions
	{
		public static List<Run> ToRuns(this SyntaxToken token, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry)
		{
			List<Run> runs = new List<Run>();

			if (token.HasLeadingTrivia)
			{
				runs.Add(token.LeadingTrivia.GetTextAndReplaceNewLines(string.Empty).ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
			}

			runs.Add(token.Text.ToRun(formatMap, typeRegistry.GetClassificationType(token.GetClassificationName())));

			if (token.HasTrailingTrivia)
			{
				runs.Add(token.TrailingTrivia.GetTextAndReplaceNewLines(" ").ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
			}

			return runs;
		}

		public static List<Run> ToRunsInSpan(this SyntaxNode node, Snapshot snapshot, GLSL.Text.Span span, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry)
		{
			List<Run> runs = new List<Run>();

			foreach (SyntaxToken token in node.GetSyntaxTokens())
			{
				if (span.Contains(token.Span.GetSpan(snapshot)))
				{
					runs.AddRange(token.ToRuns(formatMap, typeRegistry));
				}
			}

			return runs;
		}

		public static string GetClassificationName(this SyntaxToken token)
		{
			IdentifierSyntax identifier = token as IdentifierSyntax;

			if (token.SyntaxType.IsPreprocessor())
			{
				return GLSLConstants.PreprocessorKeyword;
			}

			if (identifier?.Definition?.Kind == DefinitionKind.Macro)
			{
				return GLSLConstants.Macro;
			}

			if (token?.IsExcludedCode() ?? false)
			{
				return GLSLConstants.ExcludedCode;
			}

			if (token.SyntaxType.IsPunctuation())
			{
				return GLSLConstants.Punctuation;
			}

			if (token?.IsPreprocessorText() ?? false)
			{
				return GLSLConstants.PreprocessorText;
			}

			if (token.SyntaxType.IsKeyword())
			{
				return GLSLConstants.Keyword;
			}

			if (token.SyntaxType.IsNumber())
			{
				return GLSLConstants.Number;
			}

			if (identifier?.Definition != null)
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

			if (identifier?.Parent.SyntaxType == SyntaxType.FieldSelection)
			{
				return GLSLConstants.Field;
			}

			if (token.SyntaxType == SyntaxType.IdentifierToken)
			{
				return GLSLConstants.Identifier;
			}

			return PredefinedClassificationTypeNames.FormalLanguage;
		}
	}
}
