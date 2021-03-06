﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Lexing
{
	public sealed class GLSLLexer
	{
		private readonly List<TrackingSpan> commentSpans = new List<TrackingSpan>();
		private readonly Dictionary<string, SyntaxType> keywords;
		private readonly List<SyntaxTrivia> leadingTriva = new List<SyntaxTrivia>();
		private readonly Dictionary<string, SyntaxType> preprocessors;
		private readonly StringBuilder tokenBuilder = new StringBuilder();
		private readonly List<SyntaxTrivia> trailingTriva = new List<SyntaxTrivia>();
		private TokenInfo info;
		private int invalidTokens = 0;
		private bool isLeadingTrivia = false;
		private TextNavigator navigator;
		private Snapshot snapshot;
		private LinkedList<Token> tokens;

		public GLSLLexer()
		{
			this.preprocessors = new Dictionary<string, SyntaxType>
			{
				["#define"] = SyntaxType.DefinePreprocessorKeyword,
				["#undef"] = SyntaxType.UndefinePreprocessorKeyword,
				["#if"] = SyntaxType.IfPreprocessorKeyword,
				["#ifdef"] = SyntaxType.IfDefinedPreprocessorKeyword,
				["#ifndef"] = SyntaxType.IfNotDefinedPreprocessorKeyword,
				["#else"] = SyntaxType.ElsePreprocessorKeyword,
				["#elif"] = SyntaxType.ElseIfPreprocessorKeyword,
				["#endif"] = SyntaxType.EndIfPreprocessorKeyword,
				["#error"] = SyntaxType.ErrorPreprocessorKeyword,
				["#pragma"] = SyntaxType.PragmaPreprocessorKeyword,
				["#extension"] = SyntaxType.ExtensionPreprocessorKeyword,
				["#version"] = SyntaxType.VersionPreprocessorKeyword,
				["#line"] = SyntaxType.LinePreprocessorKeyword,
				["#"] = SyntaxType.PoundToken
			};

			this.keywords = new Dictionary<string, SyntaxType>
			{
				["attribute"] = SyntaxType.AttributeKeyword,
				["const"] = SyntaxType.ConstKeyword,
				["uniform"] = SyntaxType.UniformKeyword,
				["varying"] = SyntaxType.VaryingKeyword,
				["buffer"] = SyntaxType.BufferKeyword,
				["shared"] = SyntaxType.SharedKeyword,
				["coherent"] = SyntaxType.CoherentKeyword,
				["volatile"] = SyntaxType.VolatileKeyword,
				["restrict"] = SyntaxType.RestrictKeyword,
				["readonly"] = SyntaxType.ReadOnlyKeyword,
				["writeonly"] = SyntaxType.WriteOnlyKeyword,
				["atomic_uint"] = SyntaxType.AtomicUIntKeyword,
				["layout"] = SyntaxType.LayoutKeyword,
				["centroid"] = SyntaxType.CentroidKeyword,
				["flat"] = SyntaxType.FlatKeyword,
				["smooth"] = SyntaxType.SmoothKeyword,
				["noperspective"] = SyntaxType.NoPerspectiveKeyword,
				["patch"] = SyntaxType.PatchKeyword,
				["sample"] = SyntaxType.SampleKeyword,
				["break"] = SyntaxType.BreakKeyword,
				["continue"] = SyntaxType.ContinueKeyword,
				["do"] = SyntaxType.DoKeyword,
				["for"] = SyntaxType.ForKeyword,
				["while"] = SyntaxType.WhileKeyword,
				["switch"] = SyntaxType.SwitchKeyword,
				["case"] = SyntaxType.CaseKeyword,
				["default"] = SyntaxType.DefaultKeyword,
				["if"] = SyntaxType.IfKeyword,
				["else"] = SyntaxType.ElseKeyword,
				["subroutine"] = SyntaxType.SubroutineKeyword,
				["in"] = SyntaxType.InKeyword,
				["out"] = SyntaxType.OutKeyword,
				["inout"] = SyntaxType.InOutKeyword,
				["float"] = SyntaxType.FloatKeyword,
				["double"] = SyntaxType.DoubleKeyword,
				["int"] = SyntaxType.IntKeyword,
				["void"] = SyntaxType.VoidKeyword,
				["bool"] = SyntaxType.BoolKeyword,
				["true"] = SyntaxType.TrueKeyword,
				["false"] = SyntaxType.FalseKeyword,
				["invariant"] = SyntaxType.InvariantKeyword,
				["precise"] = SyntaxType.PreciseKeyword,
				["discard"] = SyntaxType.DiscardKeyword,
				["return"] = SyntaxType.ReturnKeyword,
				["mat2"] = SyntaxType.Mat2Keyword,
				["mat3"] = SyntaxType.Mat3Keyword,
				["mat4"] = SyntaxType.Mat4Keyword,
				["dmat2"] = SyntaxType.DMat2Keyword,
				["dmat3"] = SyntaxType.DMat3Keyword,
				["dmat4"] = SyntaxType.DMat4Keyword,
				["mat2x2"] = SyntaxType.Mat2X2Keyword,
				["mat2x3"] = SyntaxType.Mat2X3Keyword,
				["mat2x4"] = SyntaxType.Mat2X4Keyword,
				["mat3x2"] = SyntaxType.Mat3X2Keyword,
				["mat3x3"] = SyntaxType.Mat3X3Keyword,
				["mat3x4"] = SyntaxType.Mat3X4Keyword,
				["mat4x2"] = SyntaxType.Mat4X2Keyword,
				["mat4x3"] = SyntaxType.Mat4X3Keyword,
				["mat4x4"] = SyntaxType.Mat4X4Keyword,
				["dmat2x2"] = SyntaxType.DMat2X2Keyword,
				["dmat2x3"] = SyntaxType.DMat2X3Keyword,
				["dmat2x4"] = SyntaxType.DMat2X4Keyword,
				["dmat3x2"] = SyntaxType.DMat3X2Keyword,
				["dmat3x3"] = SyntaxType.DMat3X3Keyword,
				["dmat3x4"] = SyntaxType.DMat3X4Keyword,
				["dmat4x2"] = SyntaxType.DMat4X2Keyword,
				["dmat4x3"] = SyntaxType.DMat4X3Keyword,
				["dmat4x4"] = SyntaxType.DMat4X4Keyword,
				["vec2"] = SyntaxType.Vec2Keyword,
				["vec3"] = SyntaxType.Vec3Keyword,
				["vec4"] = SyntaxType.Vec4Keyword,
				["ivec2"] = SyntaxType.IVec2Keyword,
				["ivec3"] = SyntaxType.IVec3Keyword,
				["ivec4"] = SyntaxType.IVec4Keyword,
				["bvec2"] = SyntaxType.BVec2Keyword,
				["bvec3"] = SyntaxType.BVec3Keyword,
				["bvec4"] = SyntaxType.BVec4Keyword,
				["dvec2"] = SyntaxType.DVec2Keyword,
				["dvec3"] = SyntaxType.DVec3Keyword,
				["dvec4"] = SyntaxType.DVec4Keyword,
				["uint"] = SyntaxType.UIntKeyword,
				["uvec2"] = SyntaxType.UVec2Keyword,
				["uvec3"] = SyntaxType.UVec3Keyword,
				["uvec4"] = SyntaxType.UVec4Keyword,
				["lowp"] = SyntaxType.LowPrecisionKeyword,
				["mediump"] = SyntaxType.MediumPrecisionKeyword,
				["highp"] = SyntaxType.HighPrecisionKeyword,
				["precision"] = SyntaxType.PrecisionKeyword,
				["sampler1D"] = SyntaxType.Sampler1DKeyword,
				["sampler2D"] = SyntaxType.Sampler2DKeyword,
				["sampler3D"] = SyntaxType.Sampler3DKeyword,
				["samplerCube"] = SyntaxType.SamplerCubeKeyword,
				["sampler1DShadow"] = SyntaxType.Sampler1DShadowKeyword,
				["sampler2DShadow"] = SyntaxType.Sampler2DShadowKeyword,
				["samplerCubeShadow"] = SyntaxType.SamplerCubeShadowKeyword,
				["sampler1DArray"] = SyntaxType.Sampler1DArrayKeyword,
				["sampler2DArray"] = SyntaxType.Sampler2DArrayKeyword,
				["sampler1DArrayShadow"] = SyntaxType.Sampler1DArrayShadowKeyword,
				["sampler2DArrayShadow"] = SyntaxType.Sampler2DArrayShadowKeyword,
				["isampler1D"] = SyntaxType.ISampler1DKeyword,
				["isampler2D"] = SyntaxType.ISampler2DKeyword,
				["isampler3D"] = SyntaxType.ISampler3DKeyword,
				["isamplerCube"] = SyntaxType.ISamplerCubeKeyword,
				["isampler1DArray"] = SyntaxType.ISampler1DArrayKeyword,
				["isampler2DArray"] = SyntaxType.ISampler2DArrayKeyword,
				["usampler1D"] = SyntaxType.USampler1DKeyword,
				["usampler2D"] = SyntaxType.USampler2DKeyword,
				["usampler3D"] = SyntaxType.USampler3DKeyword,
				["usamplerCube"] = SyntaxType.USamplerCubeKeyword,
				["usampler1DArray"] = SyntaxType.USampler1DArrayKeyword,
				["usampler2DArray"] = SyntaxType.USampler2DArrayKeyword,
				["sampler2DRect"] = SyntaxType.Sampler2DRectKeyword,
				["sampler2DRectShadow"] = SyntaxType.Sampler2DRectShadowKeyword,
				["isampler2DRect"] = SyntaxType.ISampler2DRectKeyword,
				["usampler2DRect"] = SyntaxType.USampler2DRectKeyword,
				["samplerBuffer"] = SyntaxType.SamplerBufferKeyword,
				["isamplerBuffer"] = SyntaxType.ISamplerBufferKeyword,
				["usamplerBuffer"] = SyntaxType.USamplerBufferKeyword,
				["sampler2DMS"] = SyntaxType.Sampler2DMSKeyword,
				["isampler2DMS"] = SyntaxType.ISampler2DMSKeyword,
				["usampler2DMS"] = SyntaxType.USampler2DMSKeyword,
				["sampler2DMSArray"] = SyntaxType.Sampler2DMSArrayKeyword,
				["isampler2DMSArray"] = SyntaxType.ISampler2DMSArrayKeyword,
				["usampler2DMSArray"] = SyntaxType.USampler2DMSArrayKeyword,
				["samplerCubeArray"] = SyntaxType.SamplerCubeArrayKeyword,
				["samplerCubeArrayShadow"] = SyntaxType.SamplerCubeArrayShadowKeyword,
				["isamplerCubeArray"] = SyntaxType.ISamplerCubeArrayKeyword,
				["usamplerCubeArray"] = SyntaxType.USamplerCubeArrayKeyword,
				["image1D"] = SyntaxType.Image1DKeyword,
				["image2D"] = SyntaxType.Image2DKeyword,
				["image3D"] = SyntaxType.Image3DKeyword,
				["iimage1D"] = SyntaxType.IImage1DKeyword,
				["iimage2D"] = SyntaxType.IImage2DKeyword,
				["iimage3D"] = SyntaxType.IImage3DKeyword,
				["uimage1D"] = SyntaxType.UImage1DKeyword,
				["uimage2D"] = SyntaxType.UImage2DKeyword,
				["uimage3D"] = SyntaxType.UImage3DKeyword,
				["image2DRect"] = SyntaxType.Image2DRectKeyword,
				["iimage2DRect"] = SyntaxType.IImage2DRectKeyword,
				["uimage2DRect"] = SyntaxType.UImage2DRectKeyword,
				["imageCube"] = SyntaxType.ImageCubeKeyword,
				["iimageCube"] = SyntaxType.IImageCubeKeyword,
				["uimageCube"] = SyntaxType.UImageCubeKeyword,
				["imageBuffer"] = SyntaxType.ImageBufferKeyword,
				["iimageBuffer"] = SyntaxType.IImageBufferKeyword,
				["uimageBuffer"] = SyntaxType.UImageBufferKeyword,
				["image1DArray"] = SyntaxType.Image1DArrayKeyword,
				["iimage1DArray"] = SyntaxType.IImage1DArrayKeyword,
				["uimage1DArray"] = SyntaxType.UImage1DArrayKeyword,
				["image2DArray"] = SyntaxType.Image2DArrayKeyword,
				["iimage2DArray"] = SyntaxType.IImage2DArrayKeyword,
				["uimage2DArray"] = SyntaxType.UImage2DArrayKeyword,
				["imageCubeArray"] = SyntaxType.ImageCubeArrayKeyword,
				["iimageCubeArray"] = SyntaxType.IImageCubeArrayKeyword,
				["uimageCubeArray"] = SyntaxType.UImageCubeArrayKeyword,
				["image2DMS"] = SyntaxType.Image2DMSKeyword,
				["iimage2DMS"] = SyntaxType.IImage2DMSKeyword,
				["uimage2DMS"] = SyntaxType.UImage2DMSKeyword,
				["image2DMSArray"] = SyntaxType.Image2DMSArrayKeyword,
				["iimage2DMSArray"] = SyntaxType.IImage2DMSArrayKeyword,
				["uimage2DMSArray"] = SyntaxType.UImage2DMSArrayKeyword,
				["struct"] = SyntaxType.StructKeyword,
				["common"] = SyntaxType.CommonKeyword,
				["partition"] = SyntaxType.PartitionKeyword,
				["active"] = SyntaxType.ActiveKeyword,
				["asm"] = SyntaxType.ASMKeyword,
				["class"] = SyntaxType.ClassKeyword,
				["union"] = SyntaxType.UnionKeyword,
				["enum"] = SyntaxType.EnumKeyword,
				["typedef"] = SyntaxType.TypedefKeyword,
				["template"] = SyntaxType.TemplateKeyword,
				["this"] = SyntaxType.ThisKeyword,
				["resource"] = SyntaxType.ResourceKeyword,
				["goto"] = SyntaxType.GotoKeyword,
				["inline"] = SyntaxType.InlineKeyword,
				["noinline"] = SyntaxType.NoInlineKeyword,
				["public"] = SyntaxType.PublicKeyword,
				["static"] = SyntaxType.StaticKeyword,
				["extern"] = SyntaxType.ExternKeyword,
				["external"] = SyntaxType.ExternalKeyword,
				["interface"] = SyntaxType.InterfaceKeyword,
				["long"] = SyntaxType.LongKeyword,
				["short"] = SyntaxType.ShortKeyword,
				["half"] = SyntaxType.HalfKeyword,
				["fixed"] = SyntaxType.FixedKeyword,
				["unsigned"] = SyntaxType.UnsignedKeyword,
				["superp"] = SyntaxType.SuperPKeyword,
				["input"] = SyntaxType.InputKeyword,
				["output"] = SyntaxType.OutputKeyword,
				["hvec2"] = SyntaxType.HVec2Keyword,
				["hvec3"] = SyntaxType.HVec3Keyword,
				["hvec4"] = SyntaxType.HVec4Keyword,
				["fvec2"] = SyntaxType.FVec2Keyword,
				["fvec3"] = SyntaxType.FVec3Keyword,
				["fvec4"] = SyntaxType.FVec4Keyword,
				["sampler3DRect"] = SyntaxType.Sampler3DRectKeyword,
				["filter"] = SyntaxType.FilterKeyword,
				["sizeof"] = SyntaxType.SizeofKeyword,
				["cast"] = SyntaxType.CastKeyword,
				["namespace"] = SyntaxType.NamespaceKeyword,
				["using"] = SyntaxType.UsingKeyword
			};
		}

		public IReadOnlyList<TrackingSpan> CommentSpans => this.commentSpans;

		public LinkedList<Token> Run(Snapshot snapshot, Span span)
		{
			this.info = new TokenInfo(0, null, string.Empty, SyntaxType.None, 0, string.Empty, SyntaxType.None);
			this.invalidTokens = 0;
			this.isLeadingTrivia = false;
			this.leadingTriva.Clear();
			this.navigator = new TextNavigator(snapshot, span);
			this.snapshot = snapshot;
			this.tokenBuilder.Clear();
			this.tokens = new LinkedList<Token>();
			this.trailingTriva.Clear();
			this.commentSpans.Clear();

			this.LexSource();

			return this.tokens;
		}

		public LinkedList<Token> Run(Snapshot snapshot)
		{
			return this.Run(snapshot, null);
		}

		private void CreateToken()
		{
			if ((!this.info.SyntaxType.IsTrivia() || (this.trailingTriva.Count > 0 && this.isLeadingTrivia)) && this.trailingTriva.Count > 0)
			{
				this.tokens.Last.Value.TrailingTrivia = this.CreateTrivia(this.trailingTriva);
				this.trailingTriva.Clear();
			}

			if (this.info.Start > this.info.End)
			{
				Debugger.Break();
			}

			Span span = Span.Create(this.info.Start, this.info.End);

			if (this.info.SyntaxType == SyntaxType.InvalidToken)
			{
				this.invalidTokens++;
				this.tokens.AddLast(new InvalidToken(this.info.ExtraType, span, this.info.Line, this.info.Text, this.CreateTrivia(this.leadingTriva), this.info.ExtraText));
			}
			else if (this.info.SyntaxType.IsTrivia())
			{
				if (this.isLeadingTrivia || this.tokens.Count <= 0)
				{
					this.leadingTriva.Add(new SyntaxTrivia(this.info.SyntaxType, this.snapshot.CreateTrackingSpan(span), this.info.Text));
				}
				else
				{
					this.trailingTriva.Add(new SyntaxTrivia(this.info.SyntaxType, this.snapshot.CreateTrackingSpan(span), this.info.Text));

					if (this.info.SyntaxType == SyntaxType.NewLineTrivia)
					{
						this.isLeadingTrivia = true;
					}
				}
			}
			else
			{
				this.tokens.AddLast(new Token(this.info.SyntaxType, span, this.info.Line, this.info.Text, this.CreateTrivia(this.leadingTriva)));
			}

			if (!this.info.SyntaxType.IsTrivia())
			{
				this.leadingTriva.Clear();
				this.trailingTriva.Clear();
				this.isLeadingTrivia = false;
			}

			this.info.SyntaxType = SyntaxType.None;
			this.info.Start = 0;
			this.info.End = 0;
			this.info.Line = null;
		}

		private SyntaxTrivia CreateTrivia(List<SyntaxTrivia> trivia)
		{
			if (trivia.Count > 1)
			{
				return SyntaxTriviaList.Create(trivia, this.snapshot);
			}
			else if (trivia.Count == 1)
			{
				return trivia[0];
			}
			else
			{
				return null;
			}
		}

		private void End()
		{
			this.info.End = this.navigator.Position - 1;
			this.info.Text = this.tokenBuilder.ToString();
			this.tokenBuilder.Clear();
		}

		private void LexSource()
		{
			char character = this.navigator.PeekChar();

			while (character != TextNavigator.EndCharacter)
			{
				switch (character)
				{
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
					case 'g':
					case 'h':
					case 'i':
					case 'j':
					case 'k':
					case 'l':
					case 'm':
					case 'n':
					case 'o':
					case 'p':
					case 'q':
					case 'r':
					case 's':
					case 't':
					case 'u':
					case 'v':
					case 'w':
					case 'x':
					case 'y':
					case 'z':
					case 'A':
					case 'B':
					case 'C':
					case 'D':
					case 'E':
					case 'F':
					case 'G':
					case 'H':
					case 'I':
					case 'J':
					case 'K':
					case 'L':
					case 'M':
					case 'N':
					case 'O':
					case 'P':
					case 'Q':
					case 'R':
					case 'S':
					case 'T':
					case 'U':
					case 'V':
					case 'W':
					case 'X':
					case 'Y':
					case 'Z':
					case '_':
						this.ScanIdentifierOrKeyword();
						break;

					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						this.ScanNumericLiteral();
						break;

					case '(':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();
						this.info.SyntaxType = SyntaxType.LeftParenToken;

						this.End();
						break;

					case ')':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();
						this.info.SyntaxType = SyntaxType.RightParenToken;

						this.End();
						break;

					case '[':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();
						this.info.SyntaxType = SyntaxType.LeftBracketToken;

						this.End();
						break;

					case ']':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();
						this.info.SyntaxType = SyntaxType.RightBracketToken;

						this.End();
						break;

					case '{':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();
						this.info.SyntaxType = SyntaxType.LeftBraceToken;

						this.End();
						break;

					case '}':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();
						this.info.SyntaxType = SyntaxType.RightBraceToken;

						this.End();
						break;

					case '.':
						char next = this.navigator.PeekChar(1);

						// Check to see if this is a float literal that starts with a period
						if (next >= '0' && next <= '9')
						{
							this.ScanNumericLiteral();
						}
						else
						{
							this.Start();

							this.tokenBuilder.Append(character);
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.DotToken;

							this.End();
						}

						break;

					case ',':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						this.info.SyntaxType = SyntaxType.CommaToken;

						this.End();
						break;

					case ':':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						this.info.SyntaxType = SyntaxType.ColonToken;

						this.End();
						break;

					case '=':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.EqualEqualToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.EqualToken;
						}

						this.End();
						break;

					case ';':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						this.info.SyntaxType = SyntaxType.SemicolonToken;

						this.End();
						break;

					case '!':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.ExclamationEqualToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.ExclamationToken;
						}

						this.End();
						break;

					case '-':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.MinusEqualToken;
						}
						else if (this.navigator.PeekChar() == '-')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.MinusMinusToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.MinusToken;
						}

						this.End();
						break;

					case '~':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						this.info.SyntaxType = SyntaxType.TildeToken;

						this.End();
						break;

					case '+':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.PlusEqualToken;
						}
						else if (this.navigator.PeekChar() == '+')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.PlusPlusToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.PlusToken;
						}

						this.End();
						break;

					case '*':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.StarEqualToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.StarToken;
						}

						this.End();
						break;

					case '/':
						this.Start();

						this.navigator.Advance();

						switch (this.navigator.PeekChar())
						{
							case '=':
								this.tokenBuilder.Append("/=");
								this.navigator.Advance();

								this.info.SyntaxType = SyntaxType.SlashEqualToken;
								break;

							case '/':
								this.navigator.Advance();

								this.ScanLineCommentTriva();

								this.info.SyntaxType = SyntaxType.LineCommentTrivia;
								break;

							case '*':
								this.navigator.Advance();

								this.ScanBlockCommentTrivia();

								this.info.SyntaxType = SyntaxType.BlockCommentTrivia;
								break;

							default:
								this.tokenBuilder.Append("/");

								this.info.SyntaxType = SyntaxType.SlashToken;
								break;
						}

						this.End();
						break;

					case '%':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.PercentEqualToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.PercentToken;
						}

						this.End();
						break;

					case '<':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.LessThenEqualToken;
						}
						else if (this.navigator.PeekChar() == '<')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							if (this.navigator.PeekChar() == '=')
							{
								this.tokenBuilder.Append(this.navigator.PeekChar());
								this.navigator.Advance();

								this.info.SyntaxType = SyntaxType.LessThenLessThenEqualToken;
							}
							else
							{
								this.info.SyntaxType = SyntaxType.LessThenLessThenToken;
							}
						}
						else
						{
							this.info.SyntaxType = SyntaxType.LessThenToken;
						}

						this.End();
						break;

					case '>':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.GreaterThenEqualToken;
						}
						else if (this.navigator.PeekChar() == '>')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							if (this.navigator.PeekChar() == '=')
							{
								this.tokenBuilder.Append(this.navigator.PeekChar());
								this.navigator.Advance();

								this.info.SyntaxType = SyntaxType.GreaterThenGreaterThenEqualToken;
							}
							else
							{
								this.info.SyntaxType = SyntaxType.GreaterThenGreaterThenToken;
							}
						}
						else
						{
							this.info.SyntaxType = SyntaxType.GreaterThenToken;
						}

						this.End();
						break;

					case '|':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.BarEqualToken;
						}
						else if (this.navigator.PeekChar() == '|')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.BarBarToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.VerticalBarToken;
						}

						this.End();
						break;

					case '^':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.CaretEqualToken;
						}
						else if (this.navigator.PeekChar() == '^')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.CaretCaretToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.CaretToken;
						}

						this.End();
						break;

					case '&':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						if (this.navigator.PeekChar() == '=')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.AmpersandEqualToken;
						}
						else if (this.navigator.PeekChar() == '&')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar());
							this.navigator.Advance();

							this.info.SyntaxType = SyntaxType.AmpersandAmpersandToken;
						}
						else
						{
							this.info.SyntaxType = SyntaxType.AmpersandToken;
						}

						this.End();
						break;

					case '?':
						this.Start();

						this.tokenBuilder.Append(character);
						this.navigator.Advance();

						this.info.SyntaxType = SyntaxType.QuestionToken;

						this.End();
						break;

					case ' ':
					case '\t':
						this.Start();

						this.info.SyntaxType = SyntaxType.WhiteSpaceTrivia;

						this.ScanWhiteSpaceTriva();

						this.End();
						break;

					case '\n':
					case '\r':
						this.Start();

						this.info.SyntaxType = SyntaxType.NewLineTrivia;

						this.ScanNewLineTriva();

						this.End();
						break;

					case '#':
						this.Start();

						this.tokenBuilder.Append('#');
						this.navigator.Advance();

						this.ScanPreprocessor();

						this.End();
						break;

					default:
						this.navigator.Advance();
						break;
				}

				if (this.info.SyntaxType != SyntaxType.None)
				{
					this.CreateToken();
				}

				character = this.navigator.PeekChar();
			}

			this.tokens.AddLast(new Token(SyntaxType.EOF, Span.Create(this.snapshot.Span.End, this.snapshot.Span.End), this.navigator.Line, string.Empty, this.CreateTrivia(this.leadingTriva)));

			this.leadingTriva.Clear();
			this.trailingTriva.Clear();
		}

		private void ScanBlockCommentTrivia()
		{
			int layer = 0;

			while (this.navigator.PeekChar() != TextNavigator.EndCharacter)
			{
				if (!this.navigator.CurrentLineContains("*/"))
				{
					this.navigator.MoveToNextLine();
					continue;
				}

				if (this.navigator.PeekChar() == '*')
				{
					this.navigator.Advance();

					if (this.navigator.PeekChar() == '/')
					{
						this.navigator.Advance();

						if (layer <= 0)
						{
							break;
						}
						else
						{
							layer--;
						}
					}
				}
				else if (this.navigator.PeekChar() == '/')
				{
					this.navigator.Advance();

					if (this.navigator.PeekChar() == '*')
					{
						this.navigator.Advance();

						layer++;
					}
				}
				else
				{
					this.navigator.Advance();
				}
			}

			this.End();

			this.tokenBuilder.Append(this.navigator.GetText(this.info.Start, this.navigator.Position - this.info.Start - 1));

			this.commentSpans.Add(this.snapshot.CreateTrackingSpan(Span.Create(this.info.Start, this.navigator.Position - 1)));
		}

		private void ScanIdentifierOrKeyword()
		{
			this.Start();

			char character;
			while (true)
			{
				character = this.navigator.PeekChar();

				if (char.IsLetterOrDigit(character) || character == '_')
				{
					this.tokenBuilder.Append(character);
					this.navigator.Advance();
				}
				else if (this.navigator.IsLineContinuation())
				{
					this.navigator.MoveToNextLine();
				}
				else
				{
					break;
				}
			}

			this.End();

			SyntaxType type;

			this.info.SyntaxType = this.keywords.TryGetValue(this.info.Text, out type) ? type : SyntaxType.IdentifierToken;
		}

		private void ScanLineCommentTriva()
		{
			this.navigator.MoveToEndOfLine();

			this.tokenBuilder.Append(this.navigator.GetText(this.info.Start, this.navigator.Position - this.info.Start - 1));

			int start = this.info.Start;

			while (this.navigator.IsLineContinuation())
			{
				this.navigator.MoveToNextLine();

				if (this.navigator.Position == this.navigator.EndPosition)
				{
					break;
				}

				start = this.navigator.Position;

				this.navigator.MoveToEndOfLine();

				this.tokenBuilder.Append(this.navigator.GetText(start, this.navigator.Position - start - 1));
			}

			this.commentSpans.Add(this.snapshot.CreateTrackingSpan(Span.Create(this.info.Start, this.navigator.Position)));
		}

		private void ScanNewLineTriva()
		{
			if (this.navigator.PeekChar() == '\r')
			{
				this.tokenBuilder.Append(this.navigator.PeekChar());

				this.navigator.Advance();

				if (this.navigator.PeekChar() == '\n')
				{
					this.tokenBuilder.Append(this.navigator.PeekChar());

					this.navigator.Advance();
				}
			}
			else
			{
				this.tokenBuilder.Append(this.navigator.PeekChar());

				this.navigator.Advance();
			}
		}

		private void ScanNumericLiteral()
		{
			this.Start();

			bool isHex = false;
			bool isOctal = false;
			bool isFloat = false;

			if (this.navigator.PeekChar() == '0' && (this.navigator.PeekChar(1) == 'x' || this.navigator.PeekChar(1) == 'X'))
			{
				isHex = true;
				this.navigator.Advance(2);
			}
			else if (this.navigator.PeekChar() == '0')
			{
				isOctal = true;
				this.navigator.Advance();

				if (this.navigator.PeekChar() < '0' || this.navigator.PeekChar() > '9')
				{
					isOctal = false;
					this.tokenBuilder.Append('0');
				}
			}

			char character;

			while (true)
			{
				if (this.navigator.IsLineContinuation())
				{
					this.navigator.MoveToNextLine();
				}

				character = this.navigator.PeekChar();

				if (isHex)
				{
					if (char.IsDigit(character) || (char.ToLowerInvariant(character) >= 'a' && char.ToLowerInvariant(character) <= 'f'))
					{
						this.tokenBuilder.Append(character);
						this.navigator.Advance();
					}
					else
					{
						break;
					}
				}
				else if (isOctal)
				{
					if (character >= '0' && character <= '7')
					{
						this.tokenBuilder.Append(character);
						this.navigator.Advance();
					}
					else if (character > '7' && character <= '9')
					{
						if (this.info.SyntaxType != SyntaxType.InvalidToken)
						{
							this.info.SyntaxType = SyntaxType.InvalidToken;
							this.info.ExtraText = "Octal Literals can only contain the numbers 0-7";
						}

						this.tokenBuilder.Append(character);
						this.navigator.Advance();
					}
					else if (character == '.' || char.ToLowerInvariant(character) == 'e' || char.ToLowerInvariant(character) == 'f' || (char.ToLowerInvariant(character) == 'l' && char.ToLowerInvariant(this.navigator.PeekChar(1)) == 'f'))
					{
						isFloat = true;
						isOctal = false;
						this.info.SyntaxType = SyntaxType.None;
						this.info.ExtraText = string.Empty;
					}
					else
					{
						break;
					}
				}
				else if (isFloat)
				{
					if (character == '.' || (character >= '0' && character <= '9'))
					{
						this.tokenBuilder.Append(character);
						this.navigator.Advance();
					}
					else if (character == 'e' || character == 'E')
					{
						this.tokenBuilder.Append(character);

						if (this.navigator.PeekChar(1) == '-' || this.navigator.PeekChar(1) == '+')
						{
							this.tokenBuilder.Append(this.navigator.PeekChar(1));
							this.navigator.Advance(2);
						}
						else
						{
							this.navigator.Advance();
						}
					}
					else if (character == 'f' || character == 'F')
					{
						this.tokenBuilder.Append(character);
						this.navigator.Advance();
						break;
					}
					else if ((character == 'l' && this.navigator.PeekChar(1) == 'f') || (character == 'L' && this.navigator.PeekChar(1) == 'F'))
					{
						this.tokenBuilder.Append(character);
						this.tokenBuilder.Append(this.navigator.PeekChar(1));
						this.navigator.Advance(2);

						this.info.SyntaxType = SyntaxType.DoubleConstToken;
						break;
					}
					else
					{
						break;
					}
				}
				else
				{
					if (character >= '0' && character <= '9')
					{
						this.tokenBuilder.Append(character);
						this.navigator.Advance();
					}
					else if (character == '.' || char.ToLowerInvariant(character) == 'e' || char.ToLowerInvariant(character) == 'f' || (char.ToLowerInvariant(character) == 'l' && char.ToLowerInvariant(this.navigator.PeekChar(1)) == 'f'))
					{
						isFloat = true;
					}
					else
					{
						break;
					}
				}
			}

			if (isFloat)
			{
				if (this.info.SyntaxType != SyntaxType.DoubleConstToken)
				{
					this.info.SyntaxType = SyntaxType.FloatConstToken;
				}

				this.End();
				return;
			}

			if (character == 'u' || character == 'U')
			{
				this.navigator.Advance();

				if (this.info.SyntaxType == SyntaxType.InvalidToken)
				{
					this.info.ExtraType = SyntaxType.UIntConstToken;
				}
				else
				{
					this.info.SyntaxType = SyntaxType.UIntConstToken;
				}
			}
			else
			{
				if (this.info.SyntaxType == SyntaxType.InvalidToken)
				{
					this.info.ExtraType = SyntaxType.IntConstToken;
				}
				else
				{
					this.info.SyntaxType = SyntaxType.IntConstToken;
				}
			}

			this.End();
		}

		private void ScanPreprocessor()
		{
			char character = this.navigator.PeekChar();

			while (character >= 'a' && character <= 'z')
			{
				this.tokenBuilder.Append(character);

				this.navigator.Advance();

				if (this.navigator.IsLineContinuation())
				{
					this.navigator.MoveToNextLine();
				}

				character = this.navigator.PeekChar();
			}

			SyntaxType type;

			if (this.tokenBuilder.ToString() == "#")
			{
				this.tokenBuilder.Clear();
				this.info.SyntaxType = SyntaxType.None;
			}
			else if (this.preprocessors.TryGetValue(this.tokenBuilder.ToString(), out type))
			{
				this.info.SyntaxType = type;
			}
			else
			{
				this.info.SyntaxType = SyntaxType.InvalidToken;
				this.info.ExtraType = SyntaxType.PreprocessorToken;
				this.info.ExtraText = $"{this.tokenBuilder.ToString()} is not a valid preprocessor";
			}
		}

		private void ScanWhiteSpaceTriva()
		{
			while (this.navigator.PeekChar() == ' ' || this.navigator.PeekChar() == '\t')
			{
				this.tokenBuilder.Append(this.navigator.PeekChar());
				this.navigator.Advance();
			}
		}

		private void Start()
		{
			this.info.Start = this.navigator.Position;
			this.info.Line = this.navigator.Line;
		}

		private struct TokenInfo
		{
			internal int End;
			internal string ExtraText;
			internal SyntaxType ExtraType;
			internal SourceLine Line;
			internal int Start;
			internal string Text;
			internal SyntaxType SyntaxType;

			public TokenInfo(int start, SourceLine line, string text, SyntaxType type, int end, string extraText, SyntaxType extratype)
			{
				this.End = end;
				this.ExtraText = extraText;
				this.ExtraType = extratype;
				this.Line = line;
				this.Start = start;
				this.Text = text;
				this.SyntaxType = type;
			}
		}
	}
}