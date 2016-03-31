using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Test.Text;

namespace Xannden.GLSL.Test
{
	[TestClass]
	internal class GLSLLexerTests
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

			Assert.AreEqual(SyntaxType.EOF, node.Value.Type);
			Assert.AreEqual(false, node.Value.HasTrailingTrivia);
			Assert.AreEqual(true, node.Value.HasLeadingTrivia);

			Assert.AreEqual(SyntaxType.TriviaList, node.Value.LeadingTrivia.Type);

			SyntaxTriviaList triviaList = node.Value.LeadingTrivia as SyntaxTriviaList;

			Assert.AreEqual(10, triviaList.List.Count);

			Assert.AreEqual(SyntaxType.BlockCommentTrivia, triviaList.List[0].Type);
			Assert.AreEqual(SyntaxType.NewLineTrivia, triviaList.List[1].Type);
			Assert.AreEqual(SyntaxType.NewLineTrivia, triviaList.List[2].Type);
			Assert.AreEqual(SyntaxType.LineCommentTrivia, triviaList.List[3].Type);
			Assert.AreEqual(SyntaxType.NewLineTrivia, triviaList.List[4].Type);
			Assert.AreEqual(SyntaxType.BlockCommentTrivia, triviaList.List[5].Type);
			Assert.AreEqual(SyntaxType.WhitespaceTrivia, triviaList.List[6].Type);
			Assert.AreEqual(SyntaxType.BlockCommentTrivia, triviaList.List[7].Type);
			Assert.AreEqual(SyntaxType.NewLineTrivia, triviaList.List[8].Type);
			Assert.AreEqual(SyntaxType.LineCommentTrivia, triviaList.List[9].Type);
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
				Assert.AreEqual(resultTypes[i], tokens.First.Value.Type);
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
					Assert.AreEqual(SyntaxType.DoubleConstToken, tokens.First.Value.Type);
				}
				else
				{
					Assert.AreEqual(SyntaxType.FloatConstToken, tokens.First.Value.Type);
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
				Assert.AreEqual(resultTypes[i % 3], tokens.First.Value.Type);
			}
		}

		[TestMethod]
		public void LexKeyword()
		{
			SyntaxType[] resultTypes =
			{
				SyntaxType.AttributeKeyword, SyntaxType.ConstKeyword, SyntaxType.UniformKeyword, SyntaxType.VaryingKeyword, SyntaxType.BufferKeyword, SyntaxType.SharedKeyword, SyntaxType.CoherentKeyword, SyntaxType.VolitileKeyword, SyntaxType.RestrictKeyword, SyntaxType.ReadonlyKeyword, SyntaxType.WriteonlyKeyword, SyntaxType.AtomicUIntKeyword, SyntaxType.LayoutKeyword, SyntaxType.CentroidKeyword, SyntaxType.FlatKeyword, SyntaxType.SmoothKeyword, SyntaxType.NoPerspectiveKeyword, SyntaxType.PatchKeyword, SyntaxType.SampleKeyword, SyntaxType.BreakKeyword, SyntaxType.ContinueKeyword, SyntaxType.DoKeyword, SyntaxType.ForKeyword, SyntaxType.WhileKeyword, SyntaxType.SwitchKeyword, SyntaxType.CaseKeyword, SyntaxType.DefaultKeyword, SyntaxType.IfKeyword, SyntaxType.ElseKeyword, SyntaxType.SubroutineKeyword, SyntaxType.InKeyword,
				SyntaxType.OutKeyword, SyntaxType.InOutKeyword, SyntaxType.FloatKeyword, SyntaxType.DoubleKeyword, SyntaxType.IntKeyword, SyntaxType.VoidKeyword, SyntaxType.BoolKeyword, SyntaxType.TrueKeyword, SyntaxType.FalseKeyword, SyntaxType.InvariantKeyword, SyntaxType.PreciseKeyword, SyntaxType.DiscardKeyword, SyntaxType.ReturnKeyword, SyntaxType.Mat2Keyword, SyntaxType.Mat3Keyword, SyntaxType.Mat4Keyword, SyntaxType.DMat2Keyword, SyntaxType.DMat3Keyword, SyntaxType.DMat4Keyword, SyntaxType.Mat2x2Keyword, SyntaxType.Mat2x3Keyword, SyntaxType.Mat2x4Keyword, SyntaxType.Mat3x2Keyword, SyntaxType.Mat3x3Keyword, SyntaxType.Mat3x4Keyword, SyntaxType.Mat4x2Keyword, SyntaxType.Mat4x3Keyword, SyntaxType.Mat4x4Keyword, SyntaxType.DMat2x2Keyword, SyntaxType.DMat2x3Keyword, SyntaxType.DMat2x4Keyword, SyntaxType.DMat3x2Keyword, SyntaxType.DMat3x3Keyword, SyntaxType.DMat3x4Keyword,
				SyntaxType.DMat4x2Keyword, SyntaxType.DMat4x3Keyword, SyntaxType.DMat4x4Keyword, SyntaxType.Vec2Keyword, SyntaxType.Vec3Keyword, SyntaxType.Vec4Keyword, SyntaxType.IVec2Keyword, SyntaxType.IVec3Keyword, SyntaxType.IVec4Keyword, SyntaxType.BVec2Keyword, SyntaxType.BVec3Keyword, SyntaxType.BVec4Keyword, SyntaxType.DVec2Keyword, SyntaxType.DVec3Keyword, SyntaxType.DVec4Keyword, SyntaxType.UIntKeyword, SyntaxType.UVec2Keyword, SyntaxType.UVec3Keyword, SyntaxType.UVec4Keyword, SyntaxType.LowPrecisionKeyword, SyntaxType.MediumPrecisionKeyword, SyntaxType.HighPrecisionKeyword, SyntaxType.PrecisionKeyword, SyntaxType.Sampler1DKeyword, SyntaxType.Sampler2DKeyword, SyntaxType.Sampler3DKeyword, SyntaxType.SamplerCubeKeyword, SyntaxType.Sampler1DShadowKeyword, SyntaxType.Sampler2DShadowKeyword, SyntaxType.SamplerCubeShadowKeyword, SyntaxType.Sampler1DArrayKeyword,
				SyntaxType.Sampler2DArrayKeyword, SyntaxType.Sampler1DArrayShadowKeyword, SyntaxType.Sampler2DArrayShadowKeyword, SyntaxType.ISampler1DKeyword, SyntaxType.ISampler2DKeyword, SyntaxType.ISampler3DKeyword, SyntaxType.ISamplerCubeKeyword, SyntaxType.ISampler1DArrayKeyword, SyntaxType.ISampler2DArrayKeyword, SyntaxType.USampler1DKeyword, SyntaxType.USampler2DKeyword, SyntaxType.USampler3DKeyword, SyntaxType.USamplerCubeKeyword, SyntaxType.USampler1DArrayKeyword, SyntaxType.USampler2DArrayKeyword, SyntaxType.Sampler2DRectKeyword, SyntaxType.Sampler2DRectShadowKeyword, SyntaxType.ISampler2DRectKeyword, SyntaxType.USampler2DRectKeyword, SyntaxType.SamplerBufferKeyword, SyntaxType.ISamplerBufferKeyword, SyntaxType.USamplerBufferKeyword, SyntaxType.Sampler2DMSKeyword, SyntaxType.ISampler2DMSKeyword, SyntaxType.USampler2DMSKeyword, SyntaxType.Sampler2DMSArrayKeyword, SyntaxType.ISampler2DMSArrayKeyword, SyntaxType.USampler2DMSArrayKeyword, SyntaxType.SamplerCubeArrayKeyword,
				SyntaxType.SamplerCubeArrayShadowKeyword, SyntaxType.ISamplerCubeArrayKeyword, SyntaxType.USamplerCubeArrayKeyword, SyntaxType.Image1D, SyntaxType.IImage1D, SyntaxType.UImage1D, SyntaxType.Image2D, SyntaxType.IImage2D, SyntaxType.UImage2D, SyntaxType.Image3D, SyntaxType.IImage3D, SyntaxType.UImage3D, SyntaxType.Image2DRect, SyntaxType.IImage2DRect, SyntaxType.UImage2DRect, SyntaxType.ImageCube, SyntaxType.IImageCube, SyntaxType.UImageCube, SyntaxType.ImageBuffer, SyntaxType.IImageBuffer, SyntaxType.UImageBuffer, SyntaxType.Image1DArray, SyntaxType.IImage1DArray, SyntaxType.UImage1DArray, SyntaxType.Image2DArray, SyntaxType.IImage2DArray, SyntaxType.UImage2DArray, SyntaxType.ImageCubeArray, SyntaxType.IImageCubeArray, SyntaxType.UImageCubeArray, SyntaxType.Image2DMS, SyntaxType.IImage2DMS, SyntaxType.UImage2DMS, SyntaxType.Image2DMSArray, SyntaxType.IImage2DMSArray,
				SyntaxType.UImage2DMSArray, SyntaxType.StructKeyword, SyntaxType.CommonKeyword, SyntaxType.PartitionKeyword, SyntaxType.ActiveKeyword, SyntaxType.ASMKeyword, SyntaxType.ClassKeyword, SyntaxType.UnionKeyword, SyntaxType.EnumKeyword, SyntaxType.TypedefKeyword, SyntaxType.TemplateKeyword, SyntaxType.ThisKeyword, SyntaxType.ResourceKeyword, SyntaxType.GotoKeyword, SyntaxType.InlineKeyword, SyntaxType.NoInlineKeyword, SyntaxType.PublicKeyword, SyntaxType.StaticKeyword, SyntaxType.ExternKeyword, SyntaxType.ExternalKeyword, SyntaxType.InterfaceKeyword, SyntaxType.LongKeyword, SyntaxType.ShortKeyword, SyntaxType.HalfKeyword, SyntaxType.FixedKeyword, SyntaxType.UnsignedKeyword, SyntaxType.SuperPKeyword, SyntaxType.InputKeyword, SyntaxType.OutputKeyword, SyntaxType.HVec2Keyword, SyntaxType.HVec3Keyword, SyntaxType.HVec4Keyword, SyntaxType.FVec2Keyword, SyntaxType.FVec3Keyword, SyntaxType.FVec4Keyword,
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
				Assert.AreEqual(resultTypes[i], tokens.First.Value.Type);
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
				Assert.AreEqual(resultTypes[i], tokens.First.Value.Type);
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
				Assert.AreEqual(types[i], tokens.First.Value.Type);
			}

			source = new TextSource("#unknown", errors);
			tokens = lexer.Run(source.CurrentSnapshot);

			Assert.AreEqual(2, tokens.Count);
			Assert.AreEqual(SyntaxType.InvalidToken, tokens.First.Value.Type);

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
				SyntaxType.LeftParenToken, SyntaxType.RightParenToken, SyntaxType.LeftBracketToken, SyntaxType.RightBracketToken, SyntaxType.LeftBraceToken, SyntaxType.RightBraceToken, SyntaxType.DotToken, SyntaxType.CommaToken, SyntaxType.ColonToken, SyntaxType.EqualToken, SyntaxType.EqualEqualToken, SyntaxType.SemiColonToken, SyntaxType.ExclamationToken, SyntaxType.ExclamationEqualToken, SyntaxType.MinusToken, SyntaxType.MinusMinusToken, SyntaxType.MinusEqualToken, SyntaxType.TildeToken, SyntaxType.PlusToken, SyntaxType.PlusPlusToken, SyntaxType.PlusEqualToken, SyntaxType.StarToken, SyntaxType.StarEqualToken, SyntaxType.SlashToken, SyntaxType.SlashEqualToken, SyntaxType.PercentToken, SyntaxType.PercentEqualToken, SyntaxType.LessThenToken, SyntaxType.LessThenEqualToken, SyntaxType.LessThenLessThenToken, SyntaxType.LessThenLessThenEqualToken, SyntaxType.GreaterThenToken, SyntaxType.GreaterThenEqualToken, SyntaxType.GreaterThenGreaterThenToken,
				SyntaxType.GreaterThenGreaterThenEqualToken, SyntaxType.VerticalBarToken, SyntaxType.BarBarToken, SyntaxType.BarEqualToken, SyntaxType.CaretToken, SyntaxType.CaretCaretToken, SyntaxType.CaretEqualToken, SyntaxType.AmpersandToken, SyntaxType.AmpersandAmpersandToken, SyntaxType.AmpersandEqualToken, SyntaxType.QuestionToken, SyntaxType.EOF
			};
			ErrorHandler errors = new ErrorHandler();
			GLSLLexer lexer = new GLSLLexer();
			TextSource source = new TextSource(line, errors);

			int index = 0;

			foreach (Token token in lexer.Run(source.CurrentSnapshot))
			{
				Assert.AreEqual(expectedTypes[index++], token.Type);
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
			Assert.AreEqual(SyntaxType.IntKeyword, tokens[0].Type);
			Assert.AreEqual(SyntaxType.IdentifierToken, tokens[1].Type);
			Assert.AreEqual("abcdefgh", tokens[1].Text);
			Assert.AreEqual(SyntaxType.SemiColonToken, tokens[2].Type);
			Assert.AreEqual(SyntaxType.IntConstToken, tokens[3].Type);
			Assert.AreEqual("123456789", tokens[3].Text);
		}
	}
}