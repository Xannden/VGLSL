using System.Windows.Documents;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Extensions
{
	internal static class ColoredStringExtension
	{
		public static Run ToRun(this ColoredString text, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry)
		{
			Run run = new Run(text.Text);
			run.SetTextProperties(formatMap.GetTextProperties(typeRegistry.GetClassificationType(text.GetClassificationType())));

			return run;
		}

		public static string GetClassificationType(this ColoredString text)
		{
			switch (text.Type)
			{
				case ColorType.Keyword:
					return GLSLConstants.Keyword;
				case ColorType.Number:
					return GLSLConstants.Number;
				case ColorType.Identifier:
					return GLSLConstants.Identifier;
				case ColorType.Field:
					return GLSLConstants.Field;
				case ColorType.Function:
					return GLSLConstants.Function;
				case ColorType.Macro:
					return GLSLConstants.Macro;
				case ColorType.ExcludedCode:
					return GLSLConstants.ExcludedCode;
				case ColorType.GlobalVariable:
					return GLSLConstants.GlobalVariable;
				case ColorType.LocalVariable:
					return GLSLConstants.LocalVariable;
				case ColorType.Parameter:
					return GLSLConstants.Parameter;
				case ColorType.PreprocessorKeyword:
					return GLSLConstants.PreprocessorKeyword;
				case ColorType.PreprocessorText:
					return GLSLConstants.PreprocessorText;
				case ColorType.Punctuation:
					return GLSLConstants.Punctuation;
				case ColorType.TypeName:
					return GLSLConstants.TypeName;
				case ColorType.WhiteSpace:
					return PredefinedClassificationTypeNames.WhiteSpace;
				case ColorType.Comment:
					return GLSLConstants.Comment;
				default:
					return PredefinedClassificationTypeNames.FormalLanguage;
			}
		}
	}
}
