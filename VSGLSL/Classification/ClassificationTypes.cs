using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Classification
{
#pragma warning disable SA1201
	internal static class ClassificationTypes
	{
		#region Keyword

		[Export]
		[Name(GLSLConstants.Keyword)]
		[BaseDefinition(PredefinedClassificationTypeNames.Keyword)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLKeywordDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Keyword)]
		[Name(GLSLConstants.Keyword)]
		[UserVisible(true)]
		[BaseDefinition(PredefinedClassificationTypeNames.Keyword)]
		internal class GLSLKeywordFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLKeywordFormatDefinition()
			{
				this.DisplayName = "GLSL Keyword";
			}
		}

		#endregion Keyword

		#region Comment

		[Export]
		[Name(GLSLConstants.Comment)]
		[BaseDefinition(PredefinedClassificationTypeNames.Comment)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLCommentDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Comment)]
		[Name(GLSLConstants.Comment)]
		[UserVisible(true)]
		[Order(After = Priority.Default)]
		internal class GLSLCommentFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLCommentFormatDefinition()
			{
				this.DisplayName = "GLSL Comment";
			}
		}

		#endregion Comment

		#region Literal

		[Export]
		[Name(GLSLConstants.Number)]
		[BaseDefinition(PredefinedClassificationTypeNames.Number)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLNumberLiteralDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Number)]
		[Name(GLSLConstants.Number)]
		[UserVisible(true)]
		internal class GLSLNumberLiteralFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLNumberLiteralFormatDefinition()
			{
				this.DisplayName = "GLSL Number";
			}
		}

		#endregion Literal

		#region Punctuation

		[Export]
		[Name(GLSLConstants.Punctuation)]
		[BaseDefinition(PredefinedClassificationTypeNames.Operator)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLPunctuationDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Punctuation)]
		[Name(GLSLConstants.Punctuation)]
		[UserVisible(true)]
		internal class GLSLPunctuationFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLPunctuationFormatDefinition()
			{
				this.DisplayName = "GLSL Punctuation";
			}
		}

		#endregion Punctuation

		#region Identifier

		[Export]
		[Name(GLSLConstants.Field)]
		[BaseDefinition(PredefinedClassificationTypeNames.Identifier)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLFieldDefinition { get; }

		[Export]
		[Name(GLSLConstants.Function)]
		[BaseDefinition(PredefinedClassificationTypeNames.Identifier)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLFunctionDefinition { get; }

		[Export]
		[Name(GLSLConstants.GlobalVariable)]
		[BaseDefinition(PredefinedClassificationTypeNames.Identifier)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLGlobalVariableDefinition { get; }

		[Export]
		[Name(GLSLConstants.Identifier)]
		[BaseDefinition(PredefinedClassificationTypeNames.Identifier)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLIdentifierDefinition { get; }

		[Export]
		[Name(GLSLConstants.LocalVariable)]
		[BaseDefinition(PredefinedClassificationTypeNames.Identifier)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLLocalVariableDefinition { get; }

		[Export]
		[Name(GLSLConstants.Parameter)]
		[BaseDefinition(PredefinedClassificationTypeNames.Identifier)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLParameterDefinition { get; }

		[Export]
		[Name(GLSLConstants.TypeName)]
		[BaseDefinition(PredefinedClassificationTypeNames.SymbolReference)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLTypeNameDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Field)]
		[Name(GLSLConstants.Field)]
		[UserVisible(true)]
		internal class GLSLFieldFormantDefinition : ClassificationFormatDefinition
		{
			internal GLSLFieldFormantDefinition()
			{
				this.DisplayName = "GLSL Field";
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Function)]
		[Name(GLSLConstants.Function)]
		[UserVisible(true)]
		internal class GLSLFunctionFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLFunctionFormatDefinition()
			{
				this.DisplayName = "GLSL Function";
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.GlobalVariable)]
		[Name(GLSLConstants.GlobalVariable)]
		[UserVisible(true)]
		internal class GLSLGlobalVariableFormantDefinition : ClassificationFormatDefinition
		{
			internal GLSLGlobalVariableFormantDefinition()
			{
				this.DisplayName = "GLSL Global Variable";
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Identifier)]
		[Name(GLSLConstants.Identifier)]
		[UserVisible(true)]
		internal class GLSLIdentifierFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLIdentifierFormatDefinition()
			{
				this.DisplayName = "GLSL Identifier";
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.LocalVariable)]
		[Name(GLSLConstants.LocalVariable)]
		[UserVisible(true)]
		internal class GLSLLocalVariableFormantDefinition : ClassificationFormatDefinition
		{
			internal GLSLLocalVariableFormantDefinition()
			{
				this.DisplayName = "GLSL Local Variable";
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Parameter)]
		[Name(GLSLConstants.Parameter)]
		[UserVisible(true)]
		internal class GLSLParameterFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLParameterFormatDefinition()
			{
				this.DisplayName = "GLSL Parameter";
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.TypeName)]
		[Name(GLSLConstants.TypeName)]
		[UserVisible(true)]
		internal class GLSLTypeNameFormantDefinition : ClassificationFormatDefinition
		{
			internal GLSLTypeNameFormantDefinition()
			{
				this.DisplayName = "GLSL TypeName";
			}
		}

		#endregion Identifier

		#region Preprocessor

		[Export]
		[Name(GLSLConstants.ExcludedCode)]
		[BaseDefinition(PredefinedClassificationTypeNames.ExcludedCode)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLExcludedCodeDefinition { get; }

		[Export]
		[Name(GLSLConstants.Macro)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLMacroDefinition { get; }

		[Export]
		[Name(GLSLConstants.PreprocessorKeyword)]
		[BaseDefinition(PredefinedClassificationTypeNames.PreprocessorKeyword)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLPreprocessorKeywordDefinition { get; }

		[Export]
		[Name(GLSLConstants.PreprocessorText)]
		[BaseDefinition(PredefinedClassificationTypeNames.FormalLanguage)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static ClassificationTypeDefinition GLSLPreprocessorTextDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.ExcludedCode)]
		[Name(GLSLConstants.ExcludedCode)]
		[UserVisible(true)]
		internal class GLSLExcludedCodeFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLExcludedCodeFormatDefinition()
			{
				this.DisplayName = "GLSL Excluded Code";
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Macro)]
		[Name(GLSLConstants.Macro)]
		[UserVisible(true)]
		internal class GLSLMacroFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLMacroFormatDefinition()
			{
				this.DisplayName = "GLSL Macro";
				this.ForegroundColor = Colors.OrangeRed;
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.PreprocessorKeyword)]
		[Name(GLSLConstants.PreprocessorKeyword)]
		[UserVisible(true)]
		internal class GLSLPreprocessorKeywordFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLPreprocessorKeywordFormatDefinition()
			{
				this.DisplayName = "GLSL Preprocessor Keyword";
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.PreprocessorText)]
		[Name(GLSLConstants.PreprocessorText)]
		[UserVisible(true)]
		internal class GLSLPreprocessorTextFormatDefinition : ClassificationFormatDefinition
		{
			internal GLSLPreprocessorTextFormatDefinition()
			{
				this.DisplayName = "GLSL Preprocessor Text";
			}
		}

		#endregion Preprocessor
	}
#pragma warning restore SA1201
}