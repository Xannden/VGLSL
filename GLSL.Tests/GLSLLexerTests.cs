using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Test.Text;

namespace Xannden.GLSL.Tests
{
	[TestClass]
	public class GLSLLexerTests
	{
		[TestMethod]
		public void LexComment()
		{
			string[] lines =
			{
				"/*\n",
				"this is in a block comment/* */\n",
				"*/\n",
				"\n",
				"// this is in a comment\n",
				"/* */ /* /* */ \n",
				"Assert.AreEqual(resultTypes[i], lexer.GetTokens().First.Value.Type);*/\n",
				"// this is a line comment \\\n",
				"that continues to the next line \\\n",
				"and still continues \\\n"
			};

			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();

			MultiLineTextSource source = MultiLineTextSource.FromString(lines, errors);

			LinkedList<Token> list = lexer.Run(source.CurrentSnapshot);

			LinkedListNode<Token> node = list.First;

			Assert.AreEqual(1, list.Count);

			Assert.AreEqual(SyntaxType.EOF, node.Value.SyntaxType);
			Assert.AreEqual(false, node.Value.HasTrailingTrivia);
			Assert.AreEqual(true, node.Value.HasLeadingTrivia);

			Assert.AreEqual(SyntaxType.TriviaList, node.Value.LeadingTrivia.SyntaxType);

			SyntaxTriviaList triviaList = node.Value.LeadingTrivia as SyntaxTriviaList;

			Assert.AreEqual(10, triviaList.List.Count);

			Assert.AreEqual(SyntaxType.BlockCommentTrivia, triviaList.List[0].SyntaxType);
			Assert.AreEqual(SyntaxType.NewLineTrivia, triviaList.List[1].SyntaxType);
			Assert.AreEqual(SyntaxType.NewLineTrivia, triviaList.List[2].SyntaxType);
			Assert.AreEqual(SyntaxType.LineCommentTrivia, triviaList.List[3].SyntaxType);
			Assert.AreEqual(SyntaxType.NewLineTrivia, triviaList.List[4].SyntaxType);
			Assert.AreEqual(SyntaxType.BlockCommentTrivia, triviaList.List[5].SyntaxType);
			Assert.AreEqual(SyntaxType.WhiteSpaceTrivia, triviaList.List[6].SyntaxType);
			Assert.AreEqual(SyntaxType.BlockCommentTrivia, triviaList.List[7].SyntaxType);
			Assert.AreEqual(SyntaxType.NewLineTrivia, triviaList.List[8].SyntaxType);
			Assert.AreEqual(SyntaxType.LineCommentTrivia, triviaList.List[9].SyntaxType);
		}

		[TestMethod]
		public void LexDecimal()
		{
			string[] lines = { "1234567890", "1234567890u", "1234567890U" };
			SyntaxType[] resultTypes = { SyntaxType.IntConstToken, SyntaxType.UIntConstToken, SyntaxType.UIntConstToken };
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();

			for (int i = 0; i < lines.Length; i++)
			{
				TextSource source = new TextSource(lines[i], errors);
				LinkedList<Token> tokens = lexer.Run(source.CurrentSnapshot);

				Assert.AreEqual(2, tokens.Count);
				Assert.AreEqual(resultTypes[i], tokens.First.Value.SyntaxType);
			}
		}

		[TestMethod]
		public void LexFloat()
		{
			string[] lines = { "5.5", "5.", ".5", "5.5e5", "5.e5", ".5e5", "5.5e+5", "5.e+5", ".5e+5", "5.5e-5", "5.e-5", ".5e-5", "5.5e5F", "5.e5F", ".5e5F", "5.5e+5F", "5.e+5F", ".5e+5F", "5.5e-5F", "5.e-5F", ".5e-5F", "5.5e5f", "5.e5f", ".5e5f", "5.5e+5f", "5.e+5f", ".5e+5f", "5.5e-5f", "5.e-5f", ".5e-5f", "5.e5lf", ".5e5lf", "5.5e+5lf", "5.e+5lf", ".5e+5lf", "5.5e-5lf", "5.e-5lf", ".5e-5lf", "5.5e5LF", "5.e5LF", ".5e5LF", "5.5e+5LF", "5.e+5LF", ".5e+5LF", "5.5e-5LF", "5.e-5LF", ".5e-5LF", "5.5f", "5.f", ".5f", "5.5F", "5.F", ".5F", "5.5lf", "5.lf", ".5lf", "5.5LF", "5.LF", ".5LF", "5e5" };
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();

			for (int i = 0; i < lines.Length; i++)
			{
				TextSource source = new TextSource(lines[i], errors);
				LinkedList<Token> tokens = lexer.Run(source.CurrentSnapshot);

				Assert.AreEqual(2, tokens.Count);

				if (lines[i].Contains("lf") || lines[i].Contains("LF"))
				{
					Assert.AreEqual(SyntaxType.DoubleConstToken, tokens.First.Value.SyntaxType);
				}
				else
				{
					Assert.AreEqual(SyntaxType.FloatConstToken, tokens.First.Value.SyntaxType);
				}
			}
		}

		[TestMethod]
		public void LexHex()
		{
			string[] lines = { "0x123456789abcdef", "0x123456789abcdefu", "0x123456789abcedfU", "0x123456789ABCDEF", "0x123456789ABCDEFu", "0x123456789ABCDEFU", "0X123456789abcdef", "0X123456789abcdefu", "0X123456789abcedfU", "0X123456789ABCDEF", "0X123456789ABCDEFu", "0X123456789ABCDEFU" };
			SyntaxType[] resultTypes = { SyntaxType.IntConstToken, SyntaxType.UIntConstToken, SyntaxType.UIntConstToken };
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();

			for (int i = 0; i < lines.Length; i++)
			{
				TextSource source = new TextSource(lines[i], errors);
				LinkedList<Token> tokens = lexer.Run(source.CurrentSnapshot);

				Assert.AreEqual(2, tokens.Count);
				Assert.AreEqual(resultTypes[i % 3], tokens.First.Value.SyntaxType);
			}
		}

		[TestMethod]
		public void LexKeyword()
		{
			SyntaxType[] resultTypes =
			{
				SyntaxType.AttributeKeyword, SyntaxType.ConstKeyword, SyntaxType.UniformKeyword, SyntaxType.VaryingKeyword, SyntaxType.BufferKeyword, SyntaxType.SharedKeyword, SyntaxType.CoherentKeyword, SyntaxType.VolatileKeyword, SyntaxType.RestrictKeyword, SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword, SyntaxType.AtomicUIntKeyword, SyntaxType.LayoutKeyword, SyntaxType.CentroidKeyword, SyntaxType.FlatKeyword, SyntaxType.SmoothKeyword, SyntaxType.NoPerspectiveKeyword, SyntaxType.PatchKeyword, SyntaxType.SampleKeyword, SyntaxType.BreakKeyword, SyntaxType.ContinueKeyword, SyntaxType.DoKeyword, SyntaxType.ForKeyword, SyntaxType.WhileKeyword, SyntaxType.SwitchKeyword, SyntaxType.CaseKeyword, SyntaxType.DefaultKeyword, SyntaxType.IfKeyword, SyntaxType.ElseKeyword, SyntaxType.SubroutineKeyword, SyntaxType.InKeyword,
				SyntaxType.OutKeyword, SyntaxType.InOutKeyword, SyntaxType.FloatKeyword, SyntaxType.DoubleKeyword, SyntaxType.IntKeyword, SyntaxType.VoidKeyword, SyntaxType.BoolKeyword, SyntaxType.TrueKeyword, SyntaxType.FalseKeyword, SyntaxType.InvariantKeyword, SyntaxType.PreciseKeyword, SyntaxType.DiscardKeyword, SyntaxType.ReturnKeyword, SyntaxType.Mat2Keyword, SyntaxType.Mat3Keyword, SyntaxType.Mat4Keyword, SyntaxType.DMat2Keyword, SyntaxType.DMat3Keyword, SyntaxType.DMat4Keyword, SyntaxType.Mat2X2Keyword, SyntaxType.Mat2X3Keyword, SyntaxType.Mat2X4Keyword, SyntaxType.Mat3X2Keyword, SyntaxType.Mat3X3Keyword, SyntaxType.Mat3X4Keyword, SyntaxType.Mat4X2Keyword, SyntaxType.Mat4X3Keyword, SyntaxType.Mat4X4Keyword, SyntaxType.DMat2X2Keyword, SyntaxType.DMat2X3Keyword, SyntaxType.DMat2X4Keyword, SyntaxType.DMat3X2Keyword, SyntaxType.DMat3X3Keyword, SyntaxType.DMat3X4Keyword,
				SyntaxType.DMat4X2Keyword, SyntaxType.DMat4X3Keyword, SyntaxType.DMat4X4Keyword, SyntaxType.Vec2Keyword, SyntaxType.Vec3Keyword, SyntaxType.Vec4Keyword, SyntaxType.IVec2Keyword, SyntaxType.IVec3Keyword, SyntaxType.IVec4Keyword, SyntaxType.BVec2Keyword, SyntaxType.BVec3Keyword, SyntaxType.BVec4Keyword, SyntaxType.DVec2Keyword, SyntaxType.DVec3Keyword, SyntaxType.DVec4Keyword, SyntaxType.UIntKeyword, SyntaxType.UVec2Keyword, SyntaxType.UVec3Keyword, SyntaxType.UVec4Keyword, SyntaxType.LowPrecisionKeyword, SyntaxType.MediumPrecisionKeyword, SyntaxType.HighPrecisionKeyword, SyntaxType.PrecisionKeyword, SyntaxType.Sampler1DKeyword, SyntaxType.Sampler2DKeyword, SyntaxType.Sampler3DKeyword, SyntaxType.SamplerCubeKeyword, SyntaxType.Sampler1DShadowKeyword, SyntaxType.Sampler2DShadowKeyword, SyntaxType.SamplerCubeShadowKeyword, SyntaxType.Sampler1DArrayKeyword,
				SyntaxType.Sampler2DArrayKeyword, SyntaxType.Sampler1DArrayShadowKeyword, SyntaxType.Sampler2DArrayShadowKeyword, SyntaxType.ISampler1DKeyword, SyntaxType.ISampler2DKeyword, SyntaxType.ISampler3DKeyword, SyntaxType.ISamplerCubeKeyword, SyntaxType.ISampler1DArrayKeyword, SyntaxType.ISampler2DArrayKeyword, SyntaxType.USampler1DKeyword, SyntaxType.USampler2DKeyword, SyntaxType.USampler3DKeyword, SyntaxType.USamplerCubeKeyword, SyntaxType.USampler1DArrayKeyword, SyntaxType.USampler2DArrayKeyword, SyntaxType.Sampler2DRectKeyword, SyntaxType.Sampler2DRectShadowKeyword, SyntaxType.ISampler2DRectKeyword, SyntaxType.USampler2DRectKeyword, SyntaxType.SamplerBufferKeyword, SyntaxType.ISamplerBufferKeyword, SyntaxType.USamplerBufferKeyword, SyntaxType.Sampler2DMSKeyword, SyntaxType.ISampler2DMSKeyword, SyntaxType.USampler2DMSKeyword, SyntaxType.Sampler2DMSArrayKeyword, SyntaxType.ISampler2DMSArrayKeyword, SyntaxType.USampler2DMSArrayKeyword, SyntaxType.SamplerCubeArrayKeyword,
				SyntaxType.SamplerCubeArrayShadowKeyword, SyntaxType.ISamplerCubeArrayKeyword, SyntaxType.USamplerCubeArrayKeyword, SyntaxType.Image1DKeyword, SyntaxType.IImage1DKeyword, SyntaxType.UImage1DKeyword, SyntaxType.Image2DKeyword, SyntaxType.IImage2DKeyword, SyntaxType.UImage2DKeyword, SyntaxType.Image3DKeyword, SyntaxType.IImage3DKeyword, SyntaxType.UImage3DKeyword, SyntaxType.Image2DRectKeyword, SyntaxType.IImage2DRectKeyword, SyntaxType.UImage2DRectKeyword, SyntaxType.ImageCubeKeyword, SyntaxType.IImageCubeKeyword, SyntaxType.UImageCubeKeyword, SyntaxType.ImageBufferKeyword, SyntaxType.IImageBufferKeyword, SyntaxType.UImageBufferKeyword, SyntaxType.Image1DArrayKeyword, SyntaxType.IImage1DArrayKeyword, SyntaxType.UImage1DArrayKeyword, SyntaxType.Image2DArrayKeyword, SyntaxType.IImage2DArrayKeyword, SyntaxType.UImage2DArrayKeyword, SyntaxType.ImageCubeArrayKeyword, SyntaxType.IImageCubeArrayKeyword, SyntaxType.UImageCubeArrayKeyword, SyntaxType.Image2DMSKeyword, SyntaxType.IImage2DMSKeyword, SyntaxType.UImage2DMSKeyword, SyntaxType.Image2DMSArrayKeyword, SyntaxType.IImage2DMSArrayKeyword,
				SyntaxType.UImage2DMSArrayKeyword, SyntaxType.StructKeyword, SyntaxType.CommonKeyword, SyntaxType.PartitionKeyword, SyntaxType.ActiveKeyword, SyntaxType.ASMKeyword, SyntaxType.ClassKeyword, SyntaxType.UnionKeyword, SyntaxType.EnumKeyword, SyntaxType.TypedefKeyword, SyntaxType.TemplateKeyword, SyntaxType.ThisKeyword, SyntaxType.ResourceKeyword, SyntaxType.GotoKeyword, SyntaxType.InlineKeyword, SyntaxType.NoInlineKeyword, SyntaxType.PublicKeyword, SyntaxType.StaticKeyword, SyntaxType.ExternKeyword, SyntaxType.ExternalKeyword, SyntaxType.InterfaceKeyword, SyntaxType.LongKeyword, SyntaxType.ShortKeyword, SyntaxType.HalfKeyword, SyntaxType.FixedKeyword, SyntaxType.UnsignedKeyword, SyntaxType.SuperPKeyword, SyntaxType.InputKeyword, SyntaxType.OutputKeyword, SyntaxType.HVec2Keyword, SyntaxType.HVec3Keyword, SyntaxType.HVec4Keyword, SyntaxType.FVec2Keyword, SyntaxType.FVec3Keyword, SyntaxType.FVec4Keyword,
				SyntaxType.Sampler3DRectKeyword, SyntaxType.FilterKeyword, SyntaxType.SizeofKeyword, SyntaxType.CastKeyword, SyntaxType.NamespaceKeyword, SyntaxType.UsingKeyword
			};

			string[] keywords =
			{
				"attribute", "const", "uniform", "varying", "buffer", "shared", "coherent", "volatile", "restrict", "readonly", "writeonly", "atomic_uint", "layout", "centroid", "flat", "smooth", "noperspective", "patch", "sample", "break", "continue", "do", "for", "while", "switch", "case", "default", "if", "else", "subroutine", "in", "out", "inout", "float", "double", "int", "void", "bool", "true", "false", "invariant", "precise", "discard", "return", "mat2", "mat3", "mat4", "dmat2", "dmat3", "dmat4", "mat2x2", "mat2x3", "mat2x4", "mat3x2", "mat3x3", "mat3x4", "mat4x2", "mat4x3", "mat4x4", "dmat2x2", "dmat2x3", "dmat2x4", "dmat3x2", "dmat3x3", "dmat3x4", "dmat4x2", "dmat4x3", "dmat4x4", "vec2", "vec3", "vec4", "ivec2", "ivec3", "ivec4", "bvec2", "bvec3", "bvec4", "dvec2", "dvec3", "dvec4", "uint", "uvec2", "uvec3", "uvec4",
				"lowp", "mediump", "highp", "precision", "sampler1D", "sampler2D", "sampler3D", "samplerCube", "sampler1DShadow", "sampler2DShadow", "samplerCubeShadow", "sampler1DArray", "sampler2DArray", "sampler1DArrayShadow", "sampler2DArrayShadow", "isampler1D", "isampler2D", "isampler3D", "isamplerCube", "isampler1DArray", "isampler2DArray", "usampler1D", "usampler2D", "usampler3D", "usamplerCube", "usampler1DArray", "usampler2DArray", "sampler2DRect", "sampler2DRectShadow", "isampler2DRect", "usampler2DRect", "samplerBuffer", "isamplerBuffer", "usamplerBuffer", "sampler2DMS", "isampler2DMS", "usampler2DMS", "sampler2DMSArray", "isampler2DMSArray", "usampler2DMSArray", "samplerCubeArray", "samplerCubeArrayShadow", "isamplerCubeArray", "usamplerCubeArray",
				"image1D", "iimage1D", "uimage1D", "image2D", "iimage2D", "uimage2D", "image3D", "iimage3D", "uimage3D", "image2DRect", "iimage2DRect", "uimage2DRect", "imageCube", "iimageCube", "uimageCube", "imageBuffer", "iimageBuffer", "uimageBuffer", "image1DArray", "iimage1DArray", "uimage1DArray", "image2DArray", "iimage2DArray", "uimage2DArray", "imageCubeArray", "iimageCubeArray", "uimageCubeArray", "image2DMS", "iimage2DMS", "uimage2DMS", "image2DMSArray", "iimage2DMSArray", "uimage2DMSArray", "struct",
				"common", "partition", "active", "asm", "class", "union", "enum", "typedef", "template", "this", "resource", "goto", "inline", "noinline", "public", "static", "extern", "external", "interface", "long", "short", "half", "fixed", "unsigned", "superp", "input", "output", "hvec2", "hvec3", "hvec4", "fvec2", "fvec3", "fvec4", "sampler3DRect", "filter", "sizeof", "cast", "namespace", "using"
			};
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();

			for (int i = 0; i < keywords.Length; i++)
			{
				TextSource source = new TextSource(keywords[i], errors);
				LinkedList<Token> tokens = lexer.Run(source.CurrentSnapshot);

				Assert.AreEqual(2, tokens.Count);
				Assert.AreEqual(resultTypes[i], tokens.First.Value.SyntaxType);
			}
		}

		[TestMethod]
		public void LexOctal()
		{
			string[] lines = { "01234567", "01234567u", "01234567U", "0123456789" };
			SyntaxType[] resultTypes = { SyntaxType.IntConstToken, SyntaxType.UIntConstToken, SyntaxType.UIntConstToken, SyntaxType.InvalidToken };
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();

			for (int i = 0; i < lines.Length; i++)
			{
				TextSource source = new TextSource(lines[i], errors);
				LinkedList<Token> tokens = lexer.Run(source.CurrentSnapshot);

				Assert.AreEqual(2, tokens.Count);
				Assert.AreEqual(resultTypes[i], tokens.First.Value.SyntaxType);
			}
		}

		[TestMethod]
		public void LexPreprocessor()
		{
			string[] preprocessors = { "#define", "#undef", "#if", "#ifdef", "#ifndef", "#else", "#elif", "#endif", "#error", "#pragma", "#extension", "#version", "#line" };
			SyntaxType[] types = { SyntaxType.DefinePreprocessorKeyword, SyntaxType.UndefinePreprocessorKeyword, SyntaxType.IfPreprocessorKeyword, SyntaxType.IfDefinedPreprocessorKeyword, SyntaxType.IfNotDefinedPreprocessorKeyword, SyntaxType.ElsePreprocessorKeyword, SyntaxType.ElseIfPreprocessorKeyword, SyntaxType.EndIfPreprocessorKeyword, SyntaxType.ErrorPreprocessorKeyword, SyntaxType.PragmaPreprocessorKeyword, SyntaxType.ExtensionPreprocessorKeyword, SyntaxType.VersionPreprocessorKeyword, SyntaxType.LinePreprocessorKeyword };
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();
			TextSource source;
			LinkedList<Token> tokens;

			for (int i = 0; i < preprocessors.Length; i++)
			{
				source = new TextSource(preprocessors[i], errors);
				tokens = lexer.Run(source.CurrentSnapshot);

				Assert.AreEqual(2, tokens.Count);
				Assert.AreEqual(types[i], tokens.First.Value.SyntaxType);
			}

			source = new TextSource("#unknown", errors);
			tokens = lexer.Run(source.CurrentSnapshot);

			Assert.AreEqual(2, tokens.Count);
			Assert.AreEqual(SyntaxType.InvalidToken, tokens.First.Value.SyntaxType);

			InvalidToken token = (InvalidToken)tokens.First.Value;

			Assert.AreEqual("#unknown", token.Text);
			Assert.AreEqual(SyntaxType.PreprocessorToken, token.ErrorType);
			Assert.AreEqual("#unknown is not a valid preprocessor", token.ErrorMessage);
		}

		[TestMethod]
		public void LexSymbols()
		{
			string line = "( ) [ ] { } . , : = == ; ! != - -- -= ~ + ++ += * *= / /= % %= < <= << <<= > >= >> >>= | || |= ^ ^^ ^= & && &= ?";
			SyntaxType[] expectedTypes =
			{
				SyntaxType.LeftParenToken, SyntaxType.RightParenToken, SyntaxType.LeftBracketToken, SyntaxType.RightBracketToken, SyntaxType.LeftBraceToken, SyntaxType.RightBraceToken, SyntaxType.DotToken, SyntaxType.CommaToken, SyntaxType.ColonToken, SyntaxType.EqualToken, SyntaxType.EqualEqualToken, SyntaxType.SemicolonToken, SyntaxType.ExclamationToken, SyntaxType.ExclamationEqualToken, SyntaxType.MinusToken, SyntaxType.MinusMinusToken, SyntaxType.MinusEqualToken, SyntaxType.TildeToken, SyntaxType.PlusToken, SyntaxType.PlusPlusToken, SyntaxType.PlusEqualToken, SyntaxType.StarToken, SyntaxType.StarEqualToken, SyntaxType.SlashToken, SyntaxType.SlashEqualToken, SyntaxType.PercentToken, SyntaxType.PercentEqualToken, SyntaxType.LessThenToken, SyntaxType.LessThenEqualToken, SyntaxType.LessThenLessThenToken, SyntaxType.LessThenLessThenEqualToken, SyntaxType.GreaterThenToken, SyntaxType.GreaterThenEqualToken, SyntaxType.GreaterThenGreaterThenToken,
				SyntaxType.GreaterThenGreaterThenEqualToken, SyntaxType.VerticalBarToken, SyntaxType.BarBarToken, SyntaxType.BarEqualToken, SyntaxType.CaretToken, SyntaxType.CaretCaretToken, SyntaxType.CaretEqualToken, SyntaxType.AmpersandToken, SyntaxType.AmpersandAmpersandToken, SyntaxType.AmpersandEqualToken, SyntaxType.QuestionToken, SyntaxType.EOF
			};
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();
			TextSource source = new TextSource(line, errors);

			int index = 0;

			foreach (Token token in lexer.Run(source.CurrentSnapshot))
			{
				Assert.AreEqual(expectedTypes[index++], token.SyntaxType);
			}

			Assert.AreEqual(expectedTypes.Length, index);
		}

		[TestMethod]
		public void LineContinuation()
		{
			string[] lines =
			{
				"in\\\n",
				"t ab\\\n",
				"cde\\\n",
				"fgh;",
				"1234\\\n",
				"56789"
			};
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();

			MultiLineTextSource source = MultiLineTextSource.FromString(lines, errors);

			List<Token> tokens = lexer.Run(source.CurrentSnapshot).ToList();

			Assert.AreEqual(5, tokens.Count);
			Assert.AreEqual(SyntaxType.IntKeyword, tokens[0].SyntaxType);
			Assert.AreEqual(SyntaxType.IdentifierToken, tokens[1].SyntaxType);
			Assert.AreEqual("abcdefgh", tokens[1].Text);
			Assert.AreEqual(SyntaxType.SemicolonToken, tokens[2].SyntaxType);
			Assert.AreEqual(SyntaxType.IntConstToken, tokens[3].SyntaxType);
			Assert.AreEqual("123456789", tokens[3].Text);
		}
	}
}
