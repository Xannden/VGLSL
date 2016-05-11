using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.BuiltIn;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Intellisense.SignatureHelp;

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
				case DefinitionKind.InterfaceBlock:
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

		public static TextBlock ToTextBlock(this Definition definition, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry, int overloads = 0)
		{
			TextBlock block = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};

			block.SetTextProperties(formatMap.DefaultTextProperties);

			if (definition is BuiltInDefinition)
			{
				return GetBuiltInTextBlock(definition, formatMap, typeRegistry, overloads);
			}
			else
			{
				UserDefinition userDefinition = definition as UserDefinition;

				List<Run> runs = new List<Run>();

				Run typeRun = null;

				switch (definition.Kind)
				{
					case DefinitionKind.LocalVariable:
						typeRun = "local variable".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage));
						break;
					case DefinitionKind.Field:
						typeRun = "field".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage));
						break;
					case DefinitionKind.GlobalVariable:
						typeRun = "global variable".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage));
						break;
					case DefinitionKind.Macro:
						typeRun = "macro".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage));
						break;
					case DefinitionKind.Parameter:
						typeRun = "parameter".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage));
						break;
					case DefinitionKind.TypeName:
						typeRun = "struct".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage));
						break;
					case DefinitionKind.Function:
					case DefinitionKind.InterfaceBlock:
						break;
				}

				if (typeRun != null)
				{
					runs.Add("(".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					runs.Add(typeRun);
					runs.Add(")".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					runs.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
				}

				IReadOnlyList<SyntaxToken> tokens = userDefinition.GetTokens();

				for (int i = 0; i < tokens.Count; i++)
				{
					runs.AddRange(tokens[i].ToRuns(formatMap, typeRegistry));
				}

				if (overloads > 0)
				{
					runs.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
					runs.Add("(+".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					runs.Add(overloads.ToString().ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Number)));
					runs.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
					runs.Add("overloads".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Identifier)));
					runs.Add(")".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
				}

				block.Inlines.AddRange(runs);
			}

			return block;
		}

		public static string GetContent(this Definition definition)
		{
			StringBuilder builder = new StringBuilder();

			if (definition is UserDefinition)
			{
				UserDefinition userDefinition = definition as UserDefinition;

				IReadOnlyList<SyntaxToken> tokens = userDefinition.GetTokens();

				for (int i = 0; i < tokens.Count; i++)
				{
					if (tokens[i].HasLeadingTrivia)
					{
						builder.Append(tokens[i].LeadingTrivia.GetTextAndReplaceNewLines(string.Empty));
					}

					builder.Append(tokens[i].Text);

					if (tokens[i].HasTrailingTrivia)
					{
						builder.Append(tokens[i].TrailingTrivia.GetTextAndReplaceNewLines(" "));
					}
				}
			}
			else
			{
				builder.Append(definition.ToString());
			}

			return builder.ToString();
		}

		public static List<IParameter> GetParameters(this Definition definition, ISignature signature, Snapshot snapshot)
		{
			if (definition.Kind != DefinitionKind.Function)
			{
				return null;
			}

			List<IParameter> parameters = new List<IParameter>();

			if (definition is UserDefinition)
			{
				FunctionDefinition functionDefinition = definition as FunctionDefinition;

				for (int i = 0; i < functionDefinition.Parameters.Count; i++)
				{
					parameters.Add(new GLSLParameter(signature, functionDefinition.Parameters[i], functionDefinition.GetRelativeParameterSpan(i, snapshot).ToVSSpan()));
				}
			}
			else
			{
				BuiltInFunction builtInFunction = definition as BuiltInFunction;

				for (int i = 0; i < builtInFunction.Parameters.Count; i++)
				{
					parameters.Add(new GLSLParameter(signature, builtInFunction.Parameters[i], builtInFunction.GetRelativeParamterSpan(i).ToVSSpan()));
				}
			}

			return parameters;
		}

		private static List<Run> ToRuns(this IReadOnlyList<Parameter> parameters, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry)
		{
			List<Run> runs = new List<Run>();

			for (int i = 0; i < parameters.Count; i++)
			{
				if (parameters[i].IsOptional)
				{
					runs.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
					runs.Add("[".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
				}

				if (i != 0)
				{
					runs.Add(",".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					runs.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
				}

				if (!string.IsNullOrEmpty(parameters[i].TypeQualifier))
				{
					runs.Add(parameters[i].TypeQualifier.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Keyword)));
					runs.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
				}

				runs.Add(parameters[i].VariableType.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Keyword)));
				runs.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
				runs.Add(parameters[i].Identifier.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Parameter)));

				if (parameters[i].ArraySize > 0)
				{
					runs.Add("[".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					runs.Add(parameters[i].ArraySize.ToString().ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Number)));
					runs.Add("]".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
				}

				if (parameters[i].IsOptional)
				{
					runs.Add("]".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
				}
			}

			return runs;
		}

		private static TextBlock GetBuiltInTextBlock(Definition definition, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry, int overloads)
		{
			TextBlock block = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};

			block.SetTextProperties(formatMap.DefaultTextProperties);

			switch (definition.Kind)
			{
				case DefinitionKind.Function:
					BuiltInFunction function = definition as BuiltInFunction;

					block.Inlines.Add(function.ReturnType.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Keyword)));
					block.Inlines.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
					block.Inlines.Add(function.Name.ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Function)));
					block.Inlines.Add("(".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					block.Inlines.AddRange(function.Parameters.ToRuns(formatMap, typeRegistry));
					block.Inlines.Add(")".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));

					if (overloads > 0)
					{
						block.Inlines.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
						block.Inlines.Add("(+".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
						block.Inlines.Add(overloads.ToString().ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Number)));
						block.Inlines.Add(" ".ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
						block.Inlines.Add("overloads".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Identifier)));
						block.Inlines.Add(")".ToRun(formatMap, typeRegistry.GetClassificationType(GLSLConstants.Punctuation)));
					}

					break;
			}

			return block;
		}
	}
}
