using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Classification
{
	[SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements must appear in the correct order", Justification = "Elements are ordered incorrectly to help readability", Scope = "class")]
	internal static class ClassificationTypes
	{
		#region Keyword

		[Export]
		[Name(GLSLConstants.Keyword)]
		internal static ClassificationTypeDefinition GLSLKeywordDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Keyword)]
		[Name(GLSLConstants.Keyword)]
		[UserVisible(true)]
		private class GLSLKeywordFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLKeywordFormatDefinition()
			{
				this.DisplayName = "GLSL Keyword";
				this.ForegroundColor = Color.FromRgb(86, 156, 214);
			}
		}

		#endregion Keyword

		#region Comment

		[Export]
		[Name(GLSLConstants.Comment)]
		internal static ClassificationTypeDefinition GLSLCommentDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Comment)]
		[Name(GLSLConstants.Comment)]
		[UserVisible(true)]
		[Order(After = Priority.Default)]
		private class GLSLCommentFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLCommentFormatDefinition()
			{
				this.DisplayName = "GLSL Comment";
				this.ForegroundColor = Color.FromRgb(87, 166, 74);
			}
		}

		#endregion Comment

		#region Literal

		[Export]
		[Name(GLSLConstants.Number)]
		internal static ClassificationTypeDefinition GLSLNumberLiteralDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Number)]
		[Name(GLSLConstants.Number)]
		[UserVisible(true)]
		private class GLSLNumberLiteralFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLNumberLiteralFormatDefinition()
			{
				this.DisplayName = "GLSL Number";
				this.ForegroundColor = Color.FromRgb(181, 206, 168);
			}
		}

		#endregion Literal

		#region Punctuation

		[Export]
		[Name(GLSLConstants.Punctuation)]
		internal static ClassificationTypeDefinition GLSLPunctuationDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Punctuation)]
		[Name(GLSLConstants.Punctuation)]
		[UserVisible(true)]
		private class GLSLPunctuationFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLPunctuationFormatDefinition()
			{
				this.DisplayName = "GLSL Punctuation";
				this.ForegroundColor = Colors.DeepPink;
			}
		}

		#endregion Punctuation

		#region Identifier

		[Export]
		[Name(GLSLConstants.Field)]
		internal static ClassificationTypeDefinition GLSLFieldDefinition { get; }

		[Export]
		[Name(GLSLConstants.Function)]
		internal static ClassificationTypeDefinition GLSLFunctionDefinition { get; }

		[Export]
		[Name(GLSLConstants.GlobalVariable)]
		internal static ClassificationTypeDefinition GLSLGlobalVariableDefinition { get; }

		[Export]
		[Name(GLSLConstants.Identifier)]
		internal static ClassificationTypeDefinition GLSLIdentifierDefinition { get; }

		[Export]
		[Name(GLSLConstants.LocalVariable)]
		internal static ClassificationTypeDefinition GLSLLocalVariableDefinition { get; }

		[Export]
		[Name(GLSLConstants.Parameter)]
		internal static ClassificationTypeDefinition GLSLParameterDefinition { get; }

		[Export]
		[Name(GLSLConstants.TypeName)]
		internal static ClassificationTypeDefinition GLSLTypeNameDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Field)]
		[Name(GLSLConstants.Field)]
		[UserVisible(true)]
		private class GLSLFieldFormantDefinition : ClassificationFormatDefinition
		{
			private GLSLFieldFormantDefinition()
			{
				this.DisplayName = "GLSL Field";
				this.ForegroundColor = Colors.MediumPurple;
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Function)]
		[Name(GLSLConstants.Function)]
		[UserVisible(true)]
		private class GLSLFunctionFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLFunctionFormatDefinition()
			{
				this.DisplayName = "GLSL Function";
				this.ForegroundColor = Colors.Cyan;
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.GlobalVariable)]
		[Name(GLSLConstants.GlobalVariable)]
		[UserVisible(true)]
		private class GLSLGlobalVariableFormantDefinition : ClassificationFormatDefinition
		{
			private GLSLGlobalVariableFormantDefinition()
			{
				this.DisplayName = "GLSL Global Variable";
				this.ForegroundColor = Colors.OrangeRed;
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Identifier)]
		[Name(GLSLConstants.Identifier)]
		[UserVisible(true)]
		private class GLSLIdentifierFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLIdentifierFormatDefinition()
			{
				this.DisplayName = "GLSL Identifier";
				this.ForegroundColor = Colors.PaleGoldenrod;
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.LocalVariable)]
		[Name(GLSLConstants.LocalVariable)]
		[UserVisible(true)]
		private class GLSLLocalVariableFormantDefinition : ClassificationFormatDefinition
		{
			private GLSLLocalVariableFormantDefinition()
			{
				this.DisplayName = "GLSL Local Variable";
				this.ForegroundColor = Colors.Red;
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Parameter)]
		[Name(GLSLConstants.Parameter)]
		[UserVisible(true)]
		private class GLSLParameterFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLParameterFormatDefinition()
			{
				this.DisplayName = "GLSL Parameter";
				this.ForegroundColor = Colors.Purple;
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.TypeName)]
		[Name(GLSLConstants.TypeName)]
		[UserVisible(true)]
		private class GLSLTypeNameFormantDefinition : ClassificationFormatDefinition
		{
			private GLSLTypeNameFormantDefinition()
			{
				this.DisplayName = "GLSL TypeName";
				this.ForegroundColor = Colors.Teal;
			}
		}

		#endregion Identifier

		#region Preprocessor

		[Export]
		[Name(GLSLConstants.ExcludedCode)]
		internal static ClassificationTypeDefinition GLSLExcludedCodeDefinition { get; }

		[Export]
		[Name(GLSLConstants.Macro)]
		internal static ClassificationTypeDefinition GLSLMacroDefinition { get; }

		[Export]
		[Name(GLSLConstants.PreprocessorKeyword)]
		internal static ClassificationTypeDefinition GLSLPreprocessorKeywordDefinition { get; }

		[Export]
		[Name(GLSLConstants.PreprocessorText)]
		internal static ClassificationTypeDefinition GLSLPreprocessorTextDefinition { get; }

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.ExcludedCode)]
		[Name(GLSLConstants.ExcludedCode)]
		[UserVisible(true)]
		private class GLSLExcludedCodeFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLExcludedCodeFormatDefinition()
			{
				this.DisplayName = "GLSL Excluded Code";
				this.ForegroundColor = Color.FromRgb(155, 155, 155);
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.Macro)]
		[Name(GLSLConstants.Macro)]
		[UserVisible(true)]
		private class GLSLMacroFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLMacroFormatDefinition()
			{
				this.DisplayName = "GLSL Macro";
				this.ForegroundColor = Colors.HotPink;
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.PreprocessorKeyword)]
		[Name(GLSLConstants.PreprocessorKeyword)]
		[UserVisible(true)]
		private class GLSLPreprocessorKeywordFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLPreprocessorKeywordFormatDefinition()
			{
				this.DisplayName = "GLSL Preprocessor Keyword";
				this.ForegroundColor = Color.FromRgb(155, 155, 155);
			}
		}

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = GLSLConstants.PreprocessorText)]
		[Name(GLSLConstants.PreprocessorText)]
		[UserVisible(true)]
		private class GLSLPreprocessorTextFormatDefinition : ClassificationFormatDefinition
		{
			private GLSLPreprocessorTextFormatDefinition()
			{
				this.DisplayName = "GLSL Preprocessor Text";
				this.ForegroundColor = Color.FromRgb(220, 220, 220);
			}
		}

		#endregion Preprocessor
	}
}