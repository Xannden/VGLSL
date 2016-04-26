using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.BuiltIn;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Extensions
{
	internal static class DefinitionExtensions
	{
		public static ImageSource GetImageSource(this Definition definition, IGlyphService glyphService)
		{
			ImageSource imageSource = null;

			switch (definition.Kind)
			{
				case DefinitionKind.Field:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupField, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionKind.LocalVariable:
				case DefinitionKind.GlobalVariable:
				case DefinitionKind.Parameter:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionKind.Macro:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMacro, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionKind.TypeName:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupStruct, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionKind.Function:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMethod, StandardGlyphItem.GlyphItemPublic);
					break;
			}

			return imageSource;
		}

		public static Image GetIcon(this Definition definition, IGlyphService glyphService)
		{
			ImageSource imageSource = definition.GetImageSource(glyphService);

			if (imageSource != null)
			{
				return new Image
				{
					Source = imageSource
				};
			}

			return null;
		}

		public static TextBlock ToTextBlock(this Definition definition, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry)
		{
			TextBlock block = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};

			block.SetTextProperties(formatMap.DefaultTextProperties);

			if (definition is BuiltInDefinition)
			{
				return GetBuiltInTextBlock(definition, formatMap, typeRegistry);
			}
			else
			{
				UserDefinition userDefinition = definition as UserDefinition;

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
					typeRun.SetTextProperties(formatMap.GetTextProperties(typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage)));
					runs.Add(typeRun);
				}

				List<SyntaxToken> tokens = userDefinition.GetTokens();

				for (int i = 0; i < tokens.Count; i++)
				{
					runs.Add(tokens[i].ToRun(formatMap, typeRegistry.GetClassificationType(GetClassificationName(tokens[i]))));
				}

				block.Inlines.AddRange(runs);
			}

			return block;
		}

		private static List<Run> ToRuns(this IReadOnlyList<Parameter> parameters, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry)
		{
			List<Run> runs = new List<Run>();

			Run spaceRun = new Run(" ");
			spaceRun.SetTextProperties(formatMap.GetTextProperties(typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage)));

			for (int i = 0; i < parameters.Count; i++)
			{
				runs.Add(parameters[i].Type.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Keyword)));
				runs.Add(spaceRun);
				runs.Add(parameters[i].Identifier.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Parameter)));

				if (i != parameters.Count - 1)
				{
					runs.Add(",".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					runs.Add(spaceRun);
				}
			}

			return runs;
		}

		private static string GetClassificationName(SyntaxToken token)
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

		private static TextBlock GetBuiltInTextBlock(Definition definition, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry)
		{
			TextBlock block = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};

			block.SetTextProperties(formatMap.DefaultTextProperties);

			Run spaceRun = new Run(" ");
			spaceRun.SetTextProperties(formatMap.GetTextProperties(typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage)));

			switch (definition.Kind)
			{
				case DefinitionKind.Function:
					BuiltInFunction function = definition as BuiltInFunction;

					block.Inlines.Add(function.ReturnType.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Keyword)));
					block.Inlines.Add(spaceRun);
					block.Inlines.Add(function.Name.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Function)));
					block.Inlines.Add("(".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					block.Inlines.AddRange(function.Parameters.ToRuns(formatMap, typeRegistry));
					block.Inlines.Add(")".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					break;
			}

			return block;
		}
	}
}
