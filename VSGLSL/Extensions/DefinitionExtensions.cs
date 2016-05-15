using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Syntax;
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
					FieldDefinition field = definition as FieldDefinition;

					if (field.TypeQualifiers.Contains(SyntaxType.ConstKeyword))
					{
						imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupConstant, StandardGlyphItem.GlyphItemPublic);
					}
					else
					{
						imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupField, StandardGlyphItem.GlyphItemPublic);
					}

					break;
				case DefinitionKind.LocalVariable:
				case DefinitionKind.GlobalVariable:
					VariableDefinition variable = definition as VariableDefinition;

					if (variable.TypeQualifiers.Contains(SyntaxType.ConstKeyword))
					{
						imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupConstant, StandardGlyphItem.GlyphItemPublic);
					}
					else
					{
						imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
					}

					break;
				case DefinitionKind.Parameter:

					ParameterDefinition parameter = definition as ParameterDefinition;

					if (parameter.TypeQualifiers.Contains(SyntaxType.ConstKeyword))
					{
						imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupConstant, StandardGlyphItem.GlyphItemPublic);
					}
					else
					{
						imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
					}

					break;
				case DefinitionKind.InterfaceBlock:
					InterfaceBlockDefinition interfaceBlock = definition as InterfaceBlockDefinition;

					if (interfaceBlock.TypeQualifiers.Contains(SyntaxType.ConstKeyword))
					{
						imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupConstant, StandardGlyphItem.GlyphItemPublic);
					}
					else
					{
						imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
					}

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

			IReadOnlyList<ColoredString> strings = definition.GetColoredText();

			for (int i = 0; i < strings.Count; i++)
			{
				runs.Add(strings[i].ToRun(formatMap, typeRegistry));
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

			return block;
		}

		public static List<IParameter> GetParameters(this Definition definition, ISignature signature)
		{
			if (definition.Kind != DefinitionKind.Function)
			{
				return null;
			}

			List<IParameter> parameters = new List<IParameter>();

			FunctionDefinition functionDefinition = definition as FunctionDefinition;

			for (int i = 0; i < functionDefinition.Parameters.Count; i++)
			{
				parameters.Add(new GLSLParameter(signature, functionDefinition.Parameters[i], functionDefinition.GetRelativeParameterSpan(i).ToVSSpan()));
			}

			return parameters;
		}

		public static GLSL.Text.Span GetRelativeParameterSpan(this FunctionDefinition definition, int index)
		{
			int start = definition.ToString().IndexOf('(') + 1;

			for (int i = 0; i < index; i++)
			{
				start += definition.Parameters[i].ToString().Length + 2;
			}

			return GLSL.Text.Span.Create(start, start + definition.Parameters[index].ToString().Length - 1);
		}
	}
}
