using System;
using System.Collections.Generic;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Extensions
{
	public static class SyntaxTypeExtensions
	{
		#region Lookup Table
		private static Dictionary<SyntaxType, string> lookupTable = new Dictionary<SyntaxType, string>
		{
			[SyntaxType.None] = string.Empty,
			[SyntaxType.LeftParenToken] = "(",
			[SyntaxType.RightParenToken] = ")",
			[SyntaxType.LeftBracketToken] = "[",
			[SyntaxType.RightBracketToken] = "]",
			[SyntaxType.LeftBraceToken] = "{",
			[SyntaxType.RightBraceToken] = "}",
			[SyntaxType.DotToken] = ".",
			[SyntaxType.CommaToken] = ",",
			[SyntaxType.ColonToken] = ":",
			[SyntaxType.EqualToken] = "=",
			[SyntaxType.SemicolonToken] = ";",
			[SyntaxType.ExclamationToken] = "!",
			[SyntaxType.MinusToken] = "-",
			[SyntaxType.TildeToken] = "~",
			[SyntaxType.PlusToken] = "+",
			[SyntaxType.StarToken] = "*",
			[SyntaxType.SlashToken] = "/",
			[SyntaxType.PercentToken] = "%",
			[SyntaxType.LessThenToken] = "<",
			[SyntaxType.GreaterThenToken] = ">",
			[SyntaxType.VerticalBarToken] = "|",
			[SyntaxType.CaretToken] = "^",
			[SyntaxType.AmpersandToken] = "&",
			[SyntaxType.QuestionToken] = "?",
			[SyntaxType.LessThenLessThenToken] = "<<",
			[SyntaxType.GreaterThenGreaterThenToken] = ">>",
			[SyntaxType.PlusPlusToken] = "++",
			[SyntaxType.MinusMinusToken] = "--",
			[SyntaxType.LessThenEqualToken] = "<=",
			[SyntaxType.GreaterThenEqualToken] = ">=",
			[SyntaxType.EqualEqualToken] = "==",
			[SyntaxType.ExclamationEqualToken] = "!=",
			[SyntaxType.AmpersandAmpersandToken] = "&&",
			[SyntaxType.BarBarToken] = "||",
			[SyntaxType.CaretCaretToken] = "^^",
			[SyntaxType.StarEqualToken] = "+=",
			[SyntaxType.SlashEqualToken] = "/=",
			[SyntaxType.PlusEqualToken] = "+=",
			[SyntaxType.PercentEqualToken] = "%=",
			[SyntaxType.LessThenLessThenEqualToken] = "<<=",
			[SyntaxType.GreaterThenGreaterThenEqualToken] = ">>=",
			[SyntaxType.AmpersandEqualToken] = "&=",
			[SyntaxType.CaretEqualToken] = "^=",
			[SyntaxType.BarEqualToken] = "|=",
			[SyntaxType.MinusEqualToken] = "-=",
			[SyntaxType.AttributeKeyword] = "attribute",
			[SyntaxType.ConstKeyword] = "const",
			[SyntaxType.UniformKeyword] = "uniform",
			[SyntaxType.BufferKeyword] = "buffer",
			[SyntaxType.SharedKeyword] = "shared",
			[SyntaxType.CoherentKeyword] = "coherent",
			[SyntaxType.VolatileKeyword] = "volatile",
			[SyntaxType.RestrictKeyword] = "restrict",
			[SyntaxType.ReadOnlyKeyword] = "readonly",
			[SyntaxType.WriteOnlyKeyword] = "writeonly",
			[SyntaxType.AtomicUIntKeyword] = "atomic_uint",
			[SyntaxType.PreciseKeyword] = "precise",
			[SyntaxType.BreakKeyword] = "break",
			[SyntaxType.ContinueKeyword] = "continue",
			[SyntaxType.DoKeyword] = "do",
			[SyntaxType.ElseKeyword] = "else",
			[SyntaxType.ForKeyword] = "for",
			[SyntaxType.IfKeyword] = "if",
			[SyntaxType.DiscardKeyword] = "discard",
			[SyntaxType.ReturnKeyword] = "return",
			[SyntaxType.SwitchKeyword] = "switch",
			[SyntaxType.CaseKeyword] = "case",
			[SyntaxType.DefaultKeyword] = "default",
			[SyntaxType.SubroutineKeyword] = "subroutine",
			[SyntaxType.CentroidKeyword] = "centroid",
			[SyntaxType.InKeyword] = "in",
			[SyntaxType.OutKeyword] = "out",
			[SyntaxType.InOutKeyword] = "inout",
			[SyntaxType.VaryingKeyword] = "varying",
			[SyntaxType.PatchKeyword] = "patch",
			[SyntaxType.SampleKeyword] = "sample",
			[SyntaxType.BoolKeyword] = "bool",
			[SyntaxType.FloatKeyword] = "float",
			[SyntaxType.DoubleKeyword] = "double",
			[SyntaxType.IntKeyword] = "int",
			[SyntaxType.UIntKeyword] = "uint",
			[SyntaxType.Vec2Keyword] = "vec2",
			[SyntaxType.Vec3Keyword] = "vec3",
			[SyntaxType.Vec4Keyword] = "vec4",
			[SyntaxType.UVec2Keyword] = "uvec2",
			[SyntaxType.UVec3Keyword] = "uvec3",
			[SyntaxType.UVec4Keyword] = "uvec4",
			[SyntaxType.IVec2Keyword] = "ivec2",
			[SyntaxType.IVec3Keyword] = "ivec3",
			[SyntaxType.IVec4Keyword] = "ivec4",
			[SyntaxType.DVec2Keyword] = "dvec2",
			[SyntaxType.DVec3Keyword] = "dvec3",
			[SyntaxType.DVec4Keyword] = "dvec4",
			[SyntaxType.BVec2Keyword] = "bvec2",
			[SyntaxType.BVec3Keyword] = "bvec3",
			[SyntaxType.BVec4Keyword] = "bvec4",
			[SyntaxType.Mat2Keyword] = "mat2",
			[SyntaxType.Mat3Keyword] = "mat3",
			[SyntaxType.Mat4Keyword] = "mat4",
			[SyntaxType.Mat2X2Keyword] = "mat2x2",
			[SyntaxType.Mat2X3Keyword] = "mat2x3",
			[SyntaxType.Mat2X4Keyword] = "mat2x4",
			[SyntaxType.Mat3X2Keyword] = "mat3x2",
			[SyntaxType.Mat3X3Keyword] = "mat3x3",
			[SyntaxType.Mat3X4Keyword] = "mat3x4",
			[SyntaxType.Mat4X2Keyword] = "mat4x2",
			[SyntaxType.Mat4X3Keyword] = "mat4x3",
			[SyntaxType.Mat4X4Keyword] = "mat4x4",
			[SyntaxType.DMat2Keyword] = "dmat2",
			[SyntaxType.DMat3Keyword] = "dmat3",
			[SyntaxType.DMat4Keyword] = "dmat4",
			[SyntaxType.DMat2X2Keyword] = "dmat2x2",
			[SyntaxType.DMat2X3Keyword] = "dmat2x3",
			[SyntaxType.DMat2X4Keyword] = "dmat2x4",
			[SyntaxType.DMat3X2Keyword] = "dmat3x2",
			[SyntaxType.DMat3X3Keyword] = "dmat3x3",
			[SyntaxType.DMat3X4Keyword] = "dmat3x4",
			[SyntaxType.DMat4X2Keyword] = "dmat4x2",
			[SyntaxType.DMat4X3Keyword] = "dmat4x3",
			[SyntaxType.DMat4X4Keyword] = "dmat4x4",
			[SyntaxType.Sampler1DKeyword] = "sampler1D",
			[SyntaxType.Sampler2DKeyword] = "sampler2D",
			[SyntaxType.Sampler3DKeyword] = "sampler3D",
			[SyntaxType.SamplerCubeKeyword] = "samplerCube",
			[SyntaxType.Sampler1DShadowKeyword] = "sampler1DShadow",
			[SyntaxType.Sampler2DShadowKeyword] = "sampler2DShadow",
			[SyntaxType.SamplerCubeShadowKeyword] = "samplerCubeShadow",
			[SyntaxType.Sampler1DArrayKeyword] = "sampler1DArray",
			[SyntaxType.Sampler2DArrayKeyword] = "sampler2DArray",
			[SyntaxType.Sampler1DArrayShadowKeyword] = "sampler1DArrayShadow",
			[SyntaxType.Sampler2DArrayShadowKeyword] = "sampler2DArrayShadow",
			[SyntaxType.ISampler1DKeyword] = "isampler1D",
			[SyntaxType.ISampler2DKeyword] = "isampler2D",
			[SyntaxType.ISampler3DKeyword] = "isampler3D",
			[SyntaxType.ISamplerCubeKeyword] = "isamplerCube",
			[SyntaxType.ISampler1DArrayKeyword] = "isampler1DArray",
			[SyntaxType.ISampler2DArrayKeyword] = "isampler2DArray",
			[SyntaxType.USampler1DKeyword] = "usampler1D",
			[SyntaxType.USampler2DKeyword] = "usampler2D",
			[SyntaxType.USampler3DKeyword] = "usampler3D",
			[SyntaxType.USamplerCubeKeyword] = "usamplerCube",
			[SyntaxType.USampler1DArrayKeyword] = "usampler1DArray",
			[SyntaxType.USampler2DArrayKeyword] = "usampler2DArray",
			[SyntaxType.Sampler2DRectKeyword] = "sampler2DRect",
			[SyntaxType.Sampler2DRectShadowKeyword] = "sampler2DRectShadow",
			[SyntaxType.ISampler2DRectKeyword] = "isampler2DRect",
			[SyntaxType.USampler2DRectKeyword] = "usampler2DRect",
			[SyntaxType.SamplerBufferKeyword] = "samplerBuffer",
			[SyntaxType.ISamplerBufferKeyword] = "isamplerBuffer",
			[SyntaxType.USamplerBufferKeyword] = "usamplerBuffer",
			[SyntaxType.SamplerCubeArrayKeyword] = "samplerCubeArray",
			[SyntaxType.ISamplerCubeArrayKeyword] = "isamplerCubeArray",
			[SyntaxType.USamplerCubeArrayKeyword] = "usamplerCubeArray",
			[SyntaxType.SamplerCubeArrayShadowKeyword] = "samplerCubeArrayShadow",
			[SyntaxType.Sampler2DMSKeyword] = "sampler2DMS",
			[SyntaxType.ISampler2DMSKeyword] = "isampler2DMS",
			[SyntaxType.USampler2DMSKeyword] = "usampler2DMS",
			[SyntaxType.Sampler2DMSArrayKeyword] = "sampler2DMSArray",
			[SyntaxType.ISampler2DMSArrayKeyword] = "isampler2DMSArray",
			[SyntaxType.USampler2DMSArrayKeyword] = "usampler2DMSArray",
			[SyntaxType.Image1DKeyword] = "image1D",
			[SyntaxType.Image2DKeyword] = "image2D",
			[SyntaxType.Image3DKeyword] = "image3D",
			[SyntaxType.IImage1DKeyword] = "iimage1D",
			[SyntaxType.IImage2DKeyword] = "iimage2D",
			[SyntaxType.IImage3DKeyword] = "iimage3D",
			[SyntaxType.UImage1DKeyword] = "uimage1D",
			[SyntaxType.UImage2DKeyword] = "uimage2D",
			[SyntaxType.UImage3DKeyword] = "uimage3D",
			[SyntaxType.Image2DRectKeyword] = "image2DRect",
			[SyntaxType.IImage2DRectKeyword] = "iimage2DRect",
			[SyntaxType.UImage2DRectKeyword] = "uimage2DRect",
			[SyntaxType.ImageCubeKeyword] = "imageCube",
			[SyntaxType.IImageCubeKeyword] = "iimageCube",
			[SyntaxType.UImageCubeKeyword] = "uimageCube",
			[SyntaxType.ImageBufferKeyword] = "imageBuffer",
			[SyntaxType.IImageBufferKeyword] = "iimageBuffer",
			[SyntaxType.UImageBufferKeyword] = "uimageBuffer",
			[SyntaxType.Image1DArrayKeyword] = "image1DArray",
			[SyntaxType.IImage1DArrayKeyword] = "iimage1DArray",
			[SyntaxType.UImage1DArrayKeyword] = "uimage1DArray",
			[SyntaxType.Image2DArrayKeyword] = "image2DArray",
			[SyntaxType.IImage2DArrayKeyword] = "iimage2DArray",
			[SyntaxType.UImage2DArrayKeyword] = "uimage2DArray",
			[SyntaxType.ImageCubeArrayKeyword] = "imageCubeArray",
			[SyntaxType.IImageCubeArrayKeyword] = "iimageCubeArray",
			[SyntaxType.UImageCubeArrayKeyword] = "uimageCubeArray",
			[SyntaxType.Image2DMSKeyword] = "image2DMS",
			[SyntaxType.IImage2DMSKeyword] = "iimage2DMS",
			[SyntaxType.UImage2DMSKeyword] = "uimage2DMS",
			[SyntaxType.Image2DMSArrayKeyword] = "image2DMSArray",
			[SyntaxType.IImage2DMSArrayKeyword] = "iimage2DMSArray",
			[SyntaxType.UImage2DMSArrayKeyword] = "uimage2DMSArray",
			[SyntaxType.VoidKeyword] = "void",
			[SyntaxType.NoPerspectiveKeyword] = "noperspective",
			[SyntaxType.FlatKeyword] = "flat",
			[SyntaxType.SmoothKeyword] = "smooth",
			[SyntaxType.LayoutKeyword] = "layout",
			[SyntaxType.StructKeyword] = "struct",
			[SyntaxType.WhileKeyword] = "while",
			[SyntaxType.InvariantKeyword] = "invariant",
			[SyntaxType.HighPrecisionKeyword] = "highp",
			[SyntaxType.MediumPrecisionKeyword] = "mediump",
			[SyntaxType.LowPrecisionKeyword] = "lowp",
			[SyntaxType.PrecisionKeyword] = "precision",
			[SyntaxType.TrueKeyword] = "true",
			[SyntaxType.FalseKeyword] = "false",
			[SyntaxType.DefinePreprocessorKeyword] = "#define",
			[SyntaxType.PoundToken] = "#",
			[SyntaxType.UndefinePreprocessorKeyword] = "#undefine",
			[SyntaxType.IfPreprocessorKeyword] = "#if",
			[SyntaxType.IfDefinedPreprocessorKeyword] = "#ifdef",
			[SyntaxType.IfNotDefinedPreprocessorKeyword] = "#ifndef",
			[SyntaxType.ElsePreprocessorKeyword] = "#else",
			[SyntaxType.ElseIfPreprocessorKeyword] = "#elseif",
			[SyntaxType.EndIfPreprocessorKeyword] = "#endif",
			[SyntaxType.ErrorPreprocessorKeyword] = "#error",
			[SyntaxType.PragmaPreprocessorKeyword] = "#pragma",
			[SyntaxType.ExtensionPreprocessorKeyword] = "#extension",
			[SyntaxType.VersionPreprocessorKeyword] = "#version",
			[SyntaxType.LinePreprocessorKeyword] = "#line",
			[SyntaxType.CommonKeyword] = "common",
			[SyntaxType.PartitionKeyword] = "partition",
			[SyntaxType.ActiveKeyword] = "active",
			[SyntaxType.ASMKeyword] = "asm",
			[SyntaxType.ClassKeyword] = "class",
			[SyntaxType.UnionKeyword] = "union",
			[SyntaxType.EnumKeyword] = "enum",
			[SyntaxType.TypedefKeyword] = "#typedef",
			[SyntaxType.TemplateKeyword] = "template",
			[SyntaxType.ThisKeyword] = "this",
			[SyntaxType.ResourceKeyword] = "resource",
			[SyntaxType.GotoKeyword] = "goto",
			[SyntaxType.InlineKeyword] = "inline",
			[SyntaxType.NoInlineKeyword] = "noinline",
			[SyntaxType.PublicKeyword] = "public",
			[SyntaxType.StaticKeyword] = "static",
			[SyntaxType.ExternKeyword] = "extern",
			[SyntaxType.ExternalKeyword] = "external",
			[SyntaxType.InterfaceKeyword] = "interface",
			[SyntaxType.LongKeyword] = "long",
			[SyntaxType.ShortKeyword] = "short",
			[SyntaxType.HalfKeyword] = "half",
			[SyntaxType.FixedKeyword] = "fixed",
			[SyntaxType.UnsignedKeyword] = "unsigned",
			[SyntaxType.SuperPKeyword] = "superp",
			[SyntaxType.InputKeyword] = "input",
			[SyntaxType.OutputKeyword] = "output",
			[SyntaxType.HVec2Keyword] = "hvec2",
			[SyntaxType.HVec3Keyword] = "hvec3",
			[SyntaxType.HVec4Keyword] = "hvec4",
			[SyntaxType.FVec2Keyword] = "fvec2",
			[SyntaxType.FVec3Keyword] = "fvec3",
			[SyntaxType.FVec4Keyword] = "fvec4",
			[SyntaxType.Sampler3DRectKeyword] = "sampler3DRect",
			[SyntaxType.FilterKeyword] = "filter",
			[SyntaxType.SizeofKeyword] = "sizeof",
			[SyntaxType.CastKeyword] = "cast",
			[SyntaxType.NamespaceKeyword] = "namespace",
			[SyntaxType.UsingKeyword] = "using"
		};

		#endregion

		public static bool IsComment(this SyntaxType type)
		{
			return type == SyntaxType.LineCommentTrivia || type == SyntaxType.BlockCommentTrivia;
		}

		public static bool IsPreprocessor(this SyntaxType type)
		{
			return type >= SyntaxType.DefinePreprocessorKeyword && type <= SyntaxType.LinePreprocessorKeyword;
		}

		public static bool IsReserved(this SyntaxType type)
		{
			return type >= SyntaxType.CommonKeyword && type <= SyntaxType.UsingKeyword;
		}

		public static bool IsTrivia(this SyntaxType type)
		{
			return type == SyntaxType.WhiteSpaceTrivia || type == SyntaxType.LineCommentTrivia || type == SyntaxType.BlockCommentTrivia || type == SyntaxType.NewLineTrivia;
		}

		public static bool IsType(this SyntaxType type)
		{
			return type >= SyntaxType.BoolKeyword && type <= SyntaxType.VoidKeyword;
		}

		public static bool IsPunctuation(this SyntaxType type)
		{
			return type >= SyntaxType.LeftParenToken && type <= SyntaxType.MinusEqualToken;
		}

		public static bool IsKeyword(this SyntaxType type)
		{
			return type >= SyntaxType.AttributeKeyword && type <= SyntaxType.FalseKeyword;
		}

		public static bool IsNumber(this SyntaxType type)
		{
			return type >= SyntaxType.FloatConstToken && type <= SyntaxType.UIntConstToken;
		}

		public static string GetText(this SyntaxType syntaxType)
		{
			string text;

			if (!lookupTable.TryGetValue(syntaxType, out text))
			{
				return string.Empty;
			}

			return text;
		}

		public static ColoredString ToColoredString(this SyntaxType syntaxType)
		{
			string text = syntaxType.GetText();

			if (!string.IsNullOrEmpty(text))
			{
				return ColoredString.Create(text, ColorType.Keyword);
			}
			else
			{
				return null;
			}
		}

		public static SyntaxType GetSyntaxType(this string text)
		{
			foreach (KeyValuePair<SyntaxType, string> pair in lookupTable)
			{
				if (pair.Value == text)
				{
					return pair.Key;
				}
			}

			throw new ArgumentException($"{text} not found");
		}

		internal static bool Contains(this SyntaxType[] array, SyntaxType type)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == type)
				{
					return true;
				}
			}

			return false;
		}
	}
}