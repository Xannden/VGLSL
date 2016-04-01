using System.Collections.Generic;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Properties;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Parsing
{
	internal class GLSLParser
	{
		private TreeBuilder builder;
		private ErrorHandler errorHandler;
		private List<IfPreprocessor> preprocessors;
		private Stack<IfPreprocessor> preprocessorStack;
		private GLSLSettings settings;
		private Snapshot snapshot;

		internal GLSLParser(ErrorHandler errorHandler, GLSLSettings settings)
		{
			this.errorHandler = errorHandler;
			this.settings = settings;
		}

		public List<IfPreprocessor> GetPreprocessors()
		{
			return this.preprocessors;
		}

		public SyntaxTree Run(Snapshot snapshot, LinkedList<Token> tokens)
		{
			this.snapshot = snapshot;
			this.builder = new TreeBuilder(snapshot, tokens, this.errorHandler);
			this.preprocessors = new List<IfPreprocessor>();
			this.preprocessorStack = new Stack<IfPreprocessor>();

			this.ParseProgram();

			return this.builder.GetTree();
		}

		private void ParseProgram()
		{
			this.builder.StartNode(SyntaxType.Program);

			while (this.builder.CurrentToken.Type != SyntaxType.EOF)
			{
				if (this.IsPreprocessor())
				{
					this.ParsePreprocessor();
				}
				else if (this.IsFunctionHeader())
				{
					this.ParseFunctionDefinition();
				}
				else
				{
					this.ParseDeclaration();
				}
			}

			this.RequireToken(SyntaxType.EOF);

			this.builder.EndNode();
		}

		#region Type

		private void ParseArraySpecifier()
		{
			while (this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			this.builder.StartNode(SyntaxType.ArraySpecifier);

			this.RequireToken(SyntaxType.LeftBracketToken);

			if (this.builder.CurrentToken?.Type != SyntaxType.RightBracketToken)
			{
				this.ParseConstantExpression();
			}

			this.RequireToken(SyntaxType.RightBracketToken);

			this.builder.EndNode();
		}

		private void ParseInterpolationQualifier()
		{
			this.builder.StartNode(SyntaxType.InterpolationQualifier);

			this.RequireToken(SyntaxType.SmoothKeyword, SyntaxType.FlatKeyword, SyntaxType.NoPerspectiveKeyword);

			this.builder.EndNode();
		}

		private void ParseInvariantQualifier()
		{
			this.builder.StartNode(SyntaxType.InvariantQualifier);

			this.RequireToken(SyntaxType.InvariantKeyword);

			this.builder.EndNode();
		}

		private void ParseLayoutQualifier()
		{
			this.builder.StartNode(SyntaxType.LayoutQualifier);

			this.RequireToken(SyntaxType.LayoutKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.ParseLayoutQualifierID();

			while (this.AcceptToken(SyntaxType.CommaToken))
			{
				this.ParseLayoutQualifierID();
			}

			this.RequireToken(SyntaxType.RightParenToken);

			this.builder.EndNode();
		}

		private void ParseLayoutQualifierID()
		{
			this.builder.StartNode(SyntaxType.LayoutQualifierID);

			if (this.AcceptToken(SyntaxType.IdentifierToken))
			{
				if (this.AcceptToken(SyntaxType.EqualToken))
				{
					this.ParseConstantExpression();
				}
			}
			else
			{
				this.RequireToken(SyntaxType.SharedKeyword);
			}

			this.builder.EndNode();
		}

		private void ParsePreciseQualifier()
		{
			this.builder.StartNode(SyntaxType.PreciseQualifier);

			this.RequireToken(SyntaxType.PreciseKeyword);

			this.builder.EndNode();
		}

		private void ParsePrecisionQualifier()
		{
			this.builder.StartNode(SyntaxType.PrecisionQualifier);

			this.RequireToken(SyntaxType.LowPrecisionKeyword, SyntaxType.MediumPrecisionKeyword, SyntaxType.HighPrecisionKeyword);

			this.builder.EndNode();
		}

		private void ParseStorageQualifier()
		{
			this.builder.StartNode(SyntaxType.StorageQualifier);

			if (this.AcceptToken(SyntaxType.SubroutineKeyword))
			{
				if (this.AcceptToken(SyntaxType.LeftParenToken))
				{
					this.RequireToken(SyntaxType.IdentifierToken);

					while (this.AcceptToken(SyntaxType.CommaToken))
					{
						this.RequireToken(SyntaxType.IdentifierToken);
					}

					this.RequireToken(SyntaxType.RightParenToken);
				}
			}
			else
			{
				this.RequireToken(SyntaxType.ConstKeyword, SyntaxType.InOutKeyword, SyntaxType.InKeyword, SyntaxType.OutKeyword, SyntaxType.CentroidKeyword, SyntaxType.PatchKeyword, SyntaxType.SampleKeyword, SyntaxType.UniformKeyword, SyntaxType.BufferKeyword, SyntaxType.SharedKeyword, SyntaxType.CoherentKeyword, SyntaxType.VolitileKeyword, SyntaxType.RestrictKeyword, SyntaxType.ReadonlyKeyword, SyntaxType.WriteonlyKeyword);
			}

			this.builder.EndNode();
		}

		private void ParseType()
		{
			this.builder.StartNode(SyntaxType.Type);

			this.ParseTypeNonArray();

			while (this.builder.CurrentToken?.Type == SyntaxType.LeftBracketToken)
			{
				this.ParseArraySpecifier();
			}

			this.builder.EndNode();
		}

		private void ParseTypeName()
		{
			this.builder.StartNode(SyntaxType.TypeName);

			this.RequireToken(SyntaxType.IdentifierToken);

			this.builder.EndNode();
		}

		private void ParseTypeNonArray()
		{
			this.builder.StartNode(SyntaxType.TypeNonArray);

			if (this.builder.CurrentToken.Type == SyntaxType.StructKeyword)
			{
				this.ParseStructSpecifier();
			}
			else if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
			{
				this.ParseTypeName();
			}
			else
			{
				this.RequireToken(SyntaxType.IntKeyword, SyntaxType.UIntKeyword, SyntaxType.FloatKeyword, SyntaxType.DoubleKeyword, SyntaxType.Vec2Keyword, SyntaxType.Vec3Keyword, SyntaxType.Vec4Keyword, SyntaxType.UVec2Keyword, SyntaxType.UVec3Keyword, SyntaxType.UVec4Keyword, SyntaxType.IVec2Keyword, SyntaxType.IVec3Keyword, SyntaxType.IVec4Keyword, SyntaxType.DVec2Keyword, SyntaxType.DVec3Keyword, SyntaxType.DVec4Keyword, SyntaxType.BVec2Keyword, SyntaxType.BVec3Keyword, SyntaxType.BVec4Keyword, SyntaxType.Mat2Keyword, SyntaxType.Mat3Keyword, SyntaxType.Mat4Keyword, SyntaxType.Mat2x2Keyword, SyntaxType.Mat2x3Keyword, SyntaxType.Mat2x4Keyword, SyntaxType.Mat3x2Keyword, SyntaxType.Mat3x3Keyword, SyntaxType.Mat3x4Keyword, SyntaxType.Mat4x2Keyword, SyntaxType.Mat4x3Keyword, SyntaxType.Mat4x4Keyword, SyntaxType.DMat2Keyword, SyntaxType.DMat3Keyword, SyntaxType.DMat4Keyword, SyntaxType.DMat2x2Keyword, SyntaxType.DMat2x3Keyword, SyntaxType.DMat2x4Keyword, SyntaxType.DMat3x2Keyword, SyntaxType.DMat3x3Keyword, SyntaxType.DMat3x4Keyword, SyntaxType.DMat4x2Keyword, SyntaxType.DMat4x3Keyword, SyntaxType.DMat4x4Keyword, SyntaxType.Sampler1DKeyword, SyntaxType.Sampler2DKeyword, SyntaxType.Sampler3DKeyword, SyntaxType.SamplerCubeKeyword, SyntaxType.Sampler1DShadowKeyword, SyntaxType.Sampler2DShadowKeyword, SyntaxType.SamplerCubeShadowKeyword, SyntaxType.Sampler1DArrayKeyword, SyntaxType.Sampler2DArrayKeyword, SyntaxType.Sampler1DArrayShadowKeyword, SyntaxType.Sampler2DArrayShadowKeyword, SyntaxType.ISampler1DKeyword, SyntaxType.ISampler2DKeyword, SyntaxType.ISampler3DKeyword, SyntaxType.ISamplerCubeKeyword, SyntaxType.ISampler1DArrayKeyword, SyntaxType.ISampler2DArrayKeyword, SyntaxType.USampler1DKeyword, SyntaxType.USampler2DKeyword, SyntaxType.USampler3DKeyword, SyntaxType.USamplerCubeKeyword, SyntaxType.USampler1DArrayKeyword, SyntaxType.USampler2DArrayKeyword, SyntaxType.Sampler2DRectKeyword, SyntaxType.Sampler2DRectShadowKeyword, SyntaxType.ISampler2DRectKeyword, SyntaxType.USampler2DRectKeyword, SyntaxType.SamplerBufferKeyword, SyntaxType.ISamplerBufferKeyword, SyntaxType.USamplerBufferKeyword, SyntaxType.SamplerCubeArrayKeyword, SyntaxType.SamplerCubeArrayShadowKeyword, SyntaxType.ISamplerCubeArrayKeyword, SyntaxType.USamplerCubeArrayKeyword, SyntaxType.Sampler2DMSKeyword, SyntaxType.ISampler2DMSKeyword, SyntaxType.USampler2DMSKeyword, SyntaxType.Sampler2DMSArrayKeyword, SyntaxType.ISampler2DMSArrayKeyword, SyntaxType.USampler2DMSArrayKeyword, SyntaxType.Image1D, SyntaxType.Image2D, SyntaxType.Image3D, SyntaxType.IImage1D, SyntaxType.IImage2D, SyntaxType.IImage3D, SyntaxType.UImage1D, SyntaxType.UImage2D, SyntaxType.UImage3D, SyntaxType.Image2DRect, SyntaxType.IImage2DRect, SyntaxType.UImage2DRect, SyntaxType.ImageCube, SyntaxType.IImageCube, SyntaxType.UImageCube, SyntaxType.ImageBuffer, SyntaxType.IImageBuffer, SyntaxType.UImageBuffer, SyntaxType.Image1DArray, SyntaxType.IImage1DArray, SyntaxType.UImage1DArray, SyntaxType.Image2DArray, SyntaxType.IImage2DArray, SyntaxType.UImage2DArray, SyntaxType.ImageCubeArray, SyntaxType.IImageCubeArray, SyntaxType.UImageCubeArray, SyntaxType.Image2DMS, SyntaxType.IImage2DMS, SyntaxType.UImage2DMS, SyntaxType.Image2DMSArray, SyntaxType.IImage2DMSArray, SyntaxType.UImage2DMSArray);
			}

			this.builder.EndNode();
		}

		private void ParseTypeQualifier()
		{
			this.builder.StartNode(SyntaxType.TypeQualifier);

			SyntaxType type = this.builder.CurrentToken.Type;

			if (type == SyntaxType.PreciseKeyword)
			{
				this.ParsePreciseQualifier();
			}
			else if (type == SyntaxType.InvariantKeyword)
			{
				this.ParseInvariantQualifier();
			}
			else if (type == SyntaxType.SmoothKeyword || type == SyntaxType.FlatKeyword || type == SyntaxType.NoPerspectiveKeyword)
			{
				this.ParseInterpolationQualifier();
			}
			else if (type == SyntaxType.LowPrecisionKeyword || type == SyntaxType.MediumPrecisionKeyword || type == SyntaxType.HighPrecisionKeyword)
			{
				this.ParsePrecisionQualifier();
			}
			else if (type == SyntaxType.LayoutKeyword)
			{
				this.ParseLayoutQualifier();
			}
			else if (this.IsStorageQualifier(type))
			{
				this.ParseStorageQualifier();
			}

			this.builder.EndNode();
		}

		#endregion Type

		#region Function

		private void ParseBlock()
		{
			BlockSyntax node = this.builder.StartNode(SyntaxType.Block) as BlockSyntax;

			if (this.AcceptToken(SyntaxType.LeftBraceToken))
			{
				while (this.IsSimpleStatement(this.builder.CurrentToken.Type))
				{
					this.ParseSimpleStatement();
				}

				this.RequireToken(SyntaxType.RightBraceToken);
			}
			else
			{
				this.RequireToken(SyntaxType.SemiColonToken, this.GetErrorMessage(), Span.Create(node.TempStart));
			}

			this.builder.EndNode();
		}

		private void ParseFunctionDefinition()
		{
			this.builder.StartNode(SyntaxType.FunctionDefinition);

			this.ParseFunctionHeader();

			this.ParseBlock();

			this.builder.EndNode();
		}

		private void ParseFunctionHeader()
		{
			this.builder.StartNode(SyntaxType.FunctionHeader);

			if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.ParseTypeQualifier();
			}

			this.ParseReturnType();

			this.RequireToken(SyntaxType.IdentifierToken);

			this.RequireToken(SyntaxType.LeftParenToken);

			if (this.IsType(this.builder.CurrentToken.Type) || this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.ParseParameter();

				while (this.AcceptToken(SyntaxType.CommaToken))
				{
					this.ParseParameter();
				}
			}
			else
			{
				this.AcceptToken(SyntaxType.VoidKeyword);
			}

			this.RequireToken(SyntaxType.RightParenToken);

			this.builder.EndNode();
		}

		private void ParseParameter()
		{
			this.builder.StartNode(SyntaxType.Parameter);

			if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.ParseTypeQualifier();
			}

			this.ParseType();

			if (this.AcceptToken(SyntaxType.IdentifierToken))
			{
				while (this.builder.CurrentToken.Type == SyntaxType.LeftBracketToken)
				{
					this.ParseArraySpecifier();
				}
			}
			else
			{
				this.ParseType();
			}

			this.builder.EndNode();
		}

		private void ParseReturnType()
		{
			this.builder.StartNode(SyntaxType.ReturnType);

			if (!this.AcceptToken(SyntaxType.VoidKeyword))
			{
				this.ParseType();
			}

			this.builder.EndNode();
		}

		#endregion Function

		#region Statement

		private void ParseCaseLabel()
		{
			this.builder.StartNode(SyntaxType.CaseLabel);

			if (this.AcceptToken(SyntaxType.CaseKeyword))
			{
				this.ParseExpression();
			}
			else
			{
				this.RequireToken(SyntaxType.DefaultKeyword);
			}

			this.RequireToken(SyntaxType.ColonToken);

			this.builder.EndNode();
		}

		private void ParseCondition()
		{
			this.builder.StartNode(SyntaxType.Condition);

			if (this.IsUnaryExpression(this.builder.CurrentToken.Type))
			{
				this.ParseExpression();
			}
			else
			{
				if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
				{
					this.ParseTypeQualifier();
				}

				this.ParseType();

				this.RequireToken(SyntaxType.IdentifierToken);

				this.RequireToken(SyntaxType.EqualToken);

				this.ParseInitializer();
			}

			this.builder.EndNode();
		}

		private void ParseDoWhileStatement()
		{
			this.builder.StartNode(SyntaxType.WhileStatement);

			this.RequireToken(SyntaxType.DoKeyword);

			this.ParseStatement();

			this.RequireToken(SyntaxType.WhileKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.ParseExpression();

			this.RequireToken(SyntaxType.RightParenToken);

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		private void ParseElseStatement()
		{
			this.builder.StartNode(SyntaxType.ElseStatement);

			this.RequireToken(SyntaxType.ElseKeyword);

			this.ParseStatement();

			this.builder.EndNode();
		}

		private void ParseExpressionStatement()
		{
			this.builder.StartNode(SyntaxType.ExpressionStatement);

			if (this.IsUnaryExpression(this.builder.CurrentToken.Type))
			{
				this.ParseExpression();
			}

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		private void ParseForStatement()
		{
			this.builder.StartNode(SyntaxType.ForStatement);

			this.RequireToken(SyntaxType.ForKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			if (this.IsFunctionHeader())
			{
				this.ParseFunctionHeader();

				this.RequireToken(SyntaxType.SemiColonToken);
			}
			else if (this.IsUnaryExpression(this.builder.CurrentToken.Type) || this.builder.CurrentToken.Type == SyntaxType.SemiColonToken)
			{
				this.ParseExpressionStatement();
			}
			else
			{
				this.ParseDeclaration();
			}

			if (this.IsUnaryExpression(this.builder.CurrentToken.Type) || this.IsType(this.builder.CurrentToken.Type) || this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.ParseCondition();
			}

			this.RequireToken(SyntaxType.SemiColonToken);

			if (this.IsUnaryExpression(this.builder.CurrentToken.Type))
			{
				this.ParseExpression();
			}

			this.RequireToken(SyntaxType.RightParenToken);

			this.ParseStatement();

			this.builder.EndNode();
		}

		private void ParseFunctionStatement()
		{
			this.builder.StartNode(SyntaxType.FunctionStatement);

			this.ParseFunctionHeader();

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		private void ParseIterationStatement()
		{
			this.builder.StartNode(SyntaxType.IterationStatement);
			switch (this.builder.CurrentToken.Type)
			{
				case SyntaxType.WhileKeyword:
					this.ParseWhileStatement();
					break;

				case SyntaxType.DoKeyword:
					this.ParseDoWhileStatement();
					break;

				case SyntaxType.ForKeyword:
					this.ParseForStatement();
					break;
			}

			this.builder.EndNode();
		}

		private void ParseJumpStatement()
		{
			this.builder.StartNode(SyntaxType.JumpStatement);

			if (!this.AcceptToken(SyntaxType.ContinueKeyword, SyntaxType.BreakKeyword, SyntaxType.DiscardKeyword))
			{
				this.RequireToken(SyntaxType.ReturnKeyword);

				if (this.IsUnaryExpression(this.builder.CurrentToken.Type))
				{
					this.ParseExpression();
				}
			}

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		private void ParseSelectionStatement()
		{
			this.builder.StartNode(SyntaxType.SelectionStatement);

			this.RequireToken(SyntaxType.IfKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.ParseExpression();

			this.RequireToken(SyntaxType.RightParenToken);

			this.ParseStatement();

			if (this.builder.CurrentToken.Type == SyntaxType.ElseKeyword)
			{
				this.ParseElseStatement();
			}

			this.builder.EndNode();
		}

		private void ParseSimpleStatement()
		{
			this.builder.StartNode(SyntaxType.SimpleStatement);

			SyntaxType type = this.builder.CurrentToken.Type;

			if (type == SyntaxType.IfKeyword)
			{
				this.ParseSelectionStatement();
			}
			else if (type == SyntaxType.CaseKeyword || type == SyntaxType.DefaultKeyword)
			{
				this.ParseCaseLabel();
			}
			else if (type == SyntaxType.SwitchKeyword)
			{
				this.ParseSwitchStatement();
			}
			else if (type == SyntaxType.WhileKeyword || type == SyntaxType.DoKeyword || type == SyntaxType.ForKeyword)
			{
				this.ParseIterationStatement();
			}
			else if (type == SyntaxType.ContinueKeyword || type == SyntaxType.BreakKeyword || type == SyntaxType.ReturnKeyword || type == SyntaxType.DiscardKeyword)
			{
				this.ParseJumpStatement();
			}
			else if (type == SyntaxType.PrecisionKeyword)
			{
				this.ParseDeclaration();
			}
			else if (this.IsFunctionHeader())
			{
				this.ParseFunctionStatement();
			}
			else if (this.IsDeclaration())
			{
				this.ParseDeclaration();
			}
			else if (this.IsExpressionStatement(type))
			{
				this.ParseExpressionStatement();
			}
			else
			{
				this.builder.Error(SyntaxType.SimpleStatement);
				this.builder.MoveNext();
			}

			this.builder.EndNode();
		}

		private void ParseStatement()
		{
			this.builder.StartNode(SyntaxType.Statement);

			if (this.AcceptToken(SyntaxType.LeftBraceToken))
			{
				while (this.IsSimpleStatement(this.builder.CurrentToken.Type))
				{
					this.ParseSimpleStatement();
				}

				this.RequireToken(SyntaxType.RightBraceToken);
			}
			else
			{
				this.ParseSimpleStatement();
			}

			this.builder.EndNode();
		}

		private void ParseSwitchStatement()
		{
			this.builder.StartNode(SyntaxType.SwitchStatement);

			this.RequireToken(SyntaxType.SwitchKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.ParseExpression();

			this.RequireToken(SyntaxType.RightParenToken);

			this.RequireToken(SyntaxType.LeftBraceToken);

			while (this.IsSimpleStatement(this.builder.CurrentToken.Type))
			{
				this.ParseSimpleStatement();
			}

			this.RequireToken(SyntaxType.RightBraceToken);

			this.builder.EndNode();
		}

		private void ParseWhileStatement()
		{
			this.builder.StartNode(SyntaxType.WhileStatement);

			this.RequireToken(SyntaxType.WhileKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.ParseCondition();

			this.RequireToken(SyntaxType.RightParenToken);

			this.ParseStatement();

			this.builder.EndNode();
		}

		#endregion Statement

		#region Struct

		private void ParseStructDeclaration()
		{
			this.builder.StartNode(SyntaxType.StructDeclaration);

			if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.ParseTypeQualifier();
			}

			this.ParseType();

			this.ParseStructDeclarator();

			while (this.AcceptToken(SyntaxType.CommaToken))
			{
				this.ParseStructDeclarator();
			}

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		private void ParseStructDeclarator()
		{
			this.builder.StartNode(SyntaxType.StructDeclarator);

			this.RequireToken(SyntaxType.IdentifierToken);

			while (this.builder.CurrentToken?.Type == SyntaxType.LeftBracketToken)
			{
				this.ParseArraySpecifier();
			}

			this.builder.EndNode();
		}

		private void ParseStructDefinition()
		{
			this.builder.StartNode(SyntaxType.StructDefinition);

			this.ParseTypeQualifier();

			this.ParseTypeName();

			this.RequireToken(SyntaxType.LeftBraceToken);

			do
			{
				this.ParseStructDeclaration();
			}
			while (this.IsTypeQualifier(this.builder.CurrentToken.Type) || this.IsType(this.builder.CurrentToken.Type));

			this.RequireToken(SyntaxType.RightBraceToken);

			if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
			{
				this.ParseStructDeclarator();
			}

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		private void ParseStructSpecifier()
		{
			this.builder.StartNode(SyntaxType.StructSpecifier);

			this.RequireToken(SyntaxType.StructKeyword);

			if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
			{
				this.ParseTypeName();
			}

			this.RequireToken(SyntaxType.LeftBraceToken);

			do
			{
				this.ParseStructDeclaration();
			}
			while (this.IsTypeQualifier(this.builder.CurrentToken.Type) || this.IsType(this.builder.CurrentToken.Type));

			this.RequireToken(SyntaxType.RightBraceToken);

			this.builder.EndNode();
		}

		#endregion Struct

		#region Expression

		private void ParseAdditiveExpression()
		{
			this.builder.StartNode(SyntaxType.AdditiveExpression);

			this.ParseMultiplicativeExpression();

			while (this.AcceptToken(SyntaxType.PlusToken, SyntaxType.MinusToken))
			{
				this.ParseAdditiveExpression();
			}

			this.builder.EndNode();
		}

		private void ParseAndExpression()
		{
			this.builder.StartNode(SyntaxType.AndExpression);

			this.ParseEqualityExpression();

			while (this.AcceptToken(SyntaxType.AmpersandToken))
			{
				this.ParseAndExpression();
			}

			this.builder.EndNode();
		}

		private void ParseAssignmentExpression()
		{
			this.builder.StartNode(SyntaxType.AssignmentExpression);

			if (this.IsAssignmentExpression())
			{
				this.ParseUnaryExpression();

				this.ParseAssignmentOperator();

				this.ParseAssignmentExpression();
			}
			else
			{
				this.ParseConditionalExpression();
			}

			this.builder.EndNode();
		}

		private void ParseAssignmentOperator()
		{
			this.builder.StartNode(SyntaxType.AssignmentOperator);

			this.RequireToken(SyntaxType.EqualToken, SyntaxType.StarEqualToken, SyntaxType.SlashEqualToken, SyntaxType.PercentEqualToken, SyntaxType.PlusEqualToken, SyntaxType.MinusEqualToken, SyntaxType.LessThenLessThenEqualToken, SyntaxType.GreaterThenGreaterThenEqualToken, SyntaxType.AmpersandEqualToken, SyntaxType.CaretEqualToken, SyntaxType.BarEqualToken);

			this.builder.EndNode();
		}

		private void ParseConditionalExpression()
		{
			this.builder.StartNode(SyntaxType.ConditionalExpression);

			this.ParseLogicalOrExpression();

			if (this.AcceptToken(SyntaxType.QuestionToken))
			{
				this.ParseExpression();

				this.RequireToken(SyntaxType.ColonToken);

				this.ParseAssignmentExpression();
			}

			this.builder.EndNode();
		}

		private void ParseConstantExpression()
		{
			this.builder.StartNode(SyntaxType.ConstantExpression);

			this.ParseConditionalExpression();

			this.builder.EndNode();
		}

		private void ParseConstructor()
		{
			this.builder.StartNode(SyntaxType.Constructor);

			this.ParseType();

			this.RequireToken(SyntaxType.LeftParenToken);

			if (this.IsUnaryExpression(this.builder.CurrentToken.Type))
			{
				this.ParseAssignmentExpression();

				while (this.AcceptToken(SyntaxType.CommaToken))
				{
					this.ParseAssignmentExpression();
				}
			}
			else
			{
				this.AcceptToken(SyntaxType.VoidKeyword);
			}

			this.RequireToken(SyntaxType.RightParenToken);

			this.builder.EndNode();
		}

		private void ParseEqualityExpression()
		{
			this.builder.StartNode(SyntaxType.EqualityExpression);

			this.ParseRelationalExpression();

			while (this.AcceptToken(SyntaxType.EqualEqualToken, SyntaxType.ExclamationEqualToken))
			{
				this.ParseEqualityExpression();
			}

			this.builder.EndNode();
		}

		private void ParseExclusiveOrExpression()
		{
			this.builder.StartNode(SyntaxType.ExclusiveOrExpression);

			this.ParseAndExpression();

			while (this.AcceptToken(SyntaxType.CaretToken))
			{
				this.ParseExclusiveOrExpression();
			}

			this.builder.EndNode();
		}

		private void ParseExpression()
		{
			this.builder.StartNode(SyntaxType.Expression);

			this.ParseAssignmentExpression();

			while (this.AcceptToken(SyntaxType.CommaToken))
			{
				this.ParseAssignmentExpression();
			}

			this.builder.EndNode();
		}

		private void ParseFieldSelection()
		{
			this.builder.StartNode(SyntaxType.FieldSelection);

			this.RequireToken(SyntaxType.DotToken);

			if (this.IsFunctionCall())
			{
				this.ParseFunctionCall();
			}
			else
			{
				this.RequireToken(SyntaxType.IdentifierToken);
			}

			this.builder.EndNode();
		}

		private void ParseFunctionCall()
		{
			this.builder.StartNode(SyntaxType.FunctionCall);

			this.RequireToken(SyntaxType.IdentifierToken);

			this.RequireToken(SyntaxType.LeftParenToken);

			if (this.IsUnaryExpression(this.builder.CurrentToken.Type))
			{
				this.ParseAssignmentExpression();

				while (this.AcceptToken(SyntaxType.CommaToken))
				{
					this.ParseAssignmentExpression();
				}
			}
			else
			{
				this.AcceptToken(SyntaxType.VoidKeyword);
			}

			this.RequireToken(SyntaxType.RightParenToken);

			this.builder.EndNode();
		}

		private void ParseInclusiveOrExpression()
		{
			this.builder.StartNode(SyntaxType.LogicalXOrExpression);

			this.ParseExclusiveOrExpression();

			while (this.AcceptToken(SyntaxType.VerticalBarToken))
			{
				this.ParseInclusiveOrExpression();
			}

			this.builder.EndNode();
		}

		private void ParseLogicalAndExpression()
		{
			this.builder.StartNode(SyntaxType.LogicalAndExpression);

			this.ParseInclusiveOrExpression();

			while (this.AcceptToken(SyntaxType.AmpersandAmpersandToken))
			{
				this.ParseLogicalAndExpression();
			}

			this.builder.EndNode();
		}

		private void ParseLogicalOrExpression()
		{
			this.builder.StartNode(SyntaxType.LogicalOrExpression);

			this.ParseLogicalXOrExpression();

			while (this.AcceptToken(SyntaxType.BarBarToken))
			{
				this.ParseLogicalOrExpression();
			}

			this.builder.EndNode();
		}

		private void ParseLogicalXOrExpression()
		{
			this.builder.StartNode(SyntaxType.LogicalXOrExpression);

			this.ParseLogicalAndExpression();

			while (this.AcceptToken(SyntaxType.CaretCaretToken))
			{
				this.ParseLogicalXOrExpression();
			}

			this.builder.EndNode();
		}

		private void ParseMultiplicativeExpression()
		{
			this.builder.StartNode(SyntaxType.MultiplicativeExpression);

			this.ParseUnaryExpression();

			while (this.AcceptToken(SyntaxType.StarToken, SyntaxType.SlashToken, SyntaxType.PercentToken))
			{
				this.ParseMultiplicativeExpression();
			}

			this.builder.EndNode();
		}

		private void ParsePostFixArrayAccess()
		{
			this.builder.StartNode(SyntaxType.PostFixExpressionStart);

			this.RequireToken(SyntaxType.LeftBracketToken);

			this.ParseExpression();

			this.RequireToken(SyntaxType.RightBracketToken);

			this.builder.EndNode();
		}

		private void ParsePostFixExpression()
		{
			this.builder.StartNode(SyntaxType.PostFixExpression);

			this.ParsePostFixExpressionStart();

			while (this.IsPostFixOperator(this.builder.CurrentToken.Type) || this.builder.CurrentToken.Type == SyntaxType.DotToken || this.builder.CurrentToken.Type == SyntaxType.LeftBracketToken)
			{
				this.ParsePostFixExpressionContinuation();
			}

			this.builder.EndNode();
		}

		private void ParsePostFixExpressionContinuation()
		{
			this.builder.StartNode(SyntaxType.PostFixExpressionContinuation);

			if (this.AcceptToken(SyntaxType.PlusPlusToken, SyntaxType.MinusMinusToken))
			{
			}
			else if (this.builder.CurrentToken.Type == SyntaxType.DotToken)
			{
				this.ParseFieldSelection();
			}
			else if (this.builder.CurrentToken.Type == SyntaxType.LeftBracketToken)
			{
				this.ParsePostFixArrayAccess();
			}

			this.builder.EndNode();
		}

		private void ParsePostFixExpressionStart()
		{
			this.builder.StartNode(SyntaxType.PostFixExpressionStart);

			if (this.IsFunctionCall())
			{
				this.ParseFunctionCall();
			}
			else if (this.IsConstructor())
			{
				this.ParseConstructor();
			}
			else
			{
				this.ParsePrimaryExpression();
			}

			this.builder.EndNode();
		}

		private void ParsePrimaryExpression()
		{
			this.builder.StartNode(SyntaxType.PrimaryExpression);

			if (this.AcceptToken(SyntaxType.LeftParenToken))
			{
				this.ParseExpression();

				this.RequireToken(SyntaxType.RightParenToken);
			}
			else
			{
				this.RequireToken(SyntaxType.IdentifierToken, SyntaxType.IntConstToken, SyntaxType.UIntConstToken, SyntaxType.FloatConstToken, SyntaxType.BoolConstToken, SyntaxType.DoubleConstToken);
			}

			this.builder.EndNode();
		}

		private void ParseRelationalExpression()
		{
			this.builder.StartNode(SyntaxType.RelationalExpression);

			this.ParseShiftExpression();

			while (this.AcceptToken(SyntaxType.LessThenToken, SyntaxType.GreaterThenToken, SyntaxType.LessThenEqualToken, SyntaxType.GreaterThenEqualToken))
			{
				this.ParseRelationalExpression();
			}

			this.builder.EndNode();
		}

		private void ParseShiftExpression()
		{
			this.builder.StartNode(SyntaxType.ShiftExpression);

			this.ParseAdditiveExpression();

			while (this.AcceptToken(SyntaxType.LessThenLessThenToken, SyntaxType.GreaterThenGreaterThenToken))
			{
				this.ParseShiftExpression();
			}

			this.builder.EndNode();
		}

		private void ParseUnaryExpression()
		{
			this.builder.StartNode(SyntaxType.UnaryExpression);

			if (this.AcceptToken(SyntaxType.PlusPlusToken, SyntaxType.MinusMinusToken, SyntaxType.PlusToken, SyntaxType.MinusToken, SyntaxType.ExclamationToken, SyntaxType.TildeToken))
			{
				this.ParseUnaryExpression();
			}
			else
			{
				this.ParsePostFixExpression();
			}

			this.builder.EndNode();
		}

		#endregion Expression

		#region Declaration

		private void ParseDeclaration()
		{
			this.builder.StartNode(SyntaxType.Declaration);

			if (this.builder.CurrentToken.Type == SyntaxType.PrecisionKeyword)
			{
				this.ParsePrecisionDeclaration();
			}
			else if (this.IsStructDefinition())
			{
				this.ParseStructDefinition();
			}
			else if (this.IsInitDeclaratorList())
			{
				this.ParseInitDeclaratorListDeclaration();
			}
			else if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.ParseDeclarationList();
			}
			else
			{
				this.builder.Error(SyntaxType.Declaration);
				this.builder.MoveNext();
			}

			this.builder.EndNode();
		}

		private void ParseDeclarationList()
		{
			this.builder.StartNode(SyntaxType.DeclarationList);

			this.ParseTypeQualifier();

			if (this.AcceptToken(SyntaxType.IdentifierToken))
			{
				while (this.AcceptToken(SyntaxType.CommaToken))
				{
					this.RequireToken(SyntaxType.IdentifierToken);
				}
			}

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		private void ParseInitDeclaratorList()
		{
			this.builder.StartNode(SyntaxType.InitDeclaratorList);

			if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.ParseTypeQualifier();
			}

			this.ParseType();

			if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
			{
				this.ParseInitPart();

				while (this.AcceptToken(SyntaxType.CommaToken))
				{
					this.ParseInitPart();
				}
			}

			this.builder.EndNode();
		}

		private void ParseInitDeclaratorListDeclaration()
		{
			this.builder.StartNode(SyntaxType.InitDeclaratorListDeclaration);

			this.ParseInitDeclaratorList();

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		private void ParseInitializer()
		{
			this.builder.StartNode(SyntaxType.Initializer);

			if (this.builder.CurrentToken.Type == SyntaxType.LeftBraceToken)
			{
				this.ParseInitList();
			}
			else
			{
				this.ParseAssignmentExpression();
			}

			this.builder.EndNode();
		}

		private void ParseInitList()
		{
			this.builder.StartNode(SyntaxType.InitList);

			this.RequireToken(SyntaxType.LeftBraceToken);

			this.ParseInitializer();

			while (this.AcceptToken(SyntaxType.CommaToken))
			{
				if (this.builder.CurrentToken.Type == SyntaxType.RightBraceToken)
				{
					break;
				}
				else
				{
					this.ParseInitializer();
				}
			}

			this.RequireToken(SyntaxType.RightBraceToken);

			this.builder.EndNode();
		}

		private void ParseInitPart()
		{
			this.builder.StartNode(SyntaxType.InitPart);

			this.RequireToken(SyntaxType.IdentifierToken);

			while (this.builder.CurrentToken.Type == SyntaxType.LeftBracketToken)
			{
				this.ParseArraySpecifier();
			}

			if (this.AcceptToken(SyntaxType.EqualToken))
			{
				this.ParseInitializer();
			}

			this.builder.EndNode();
		}

		private void ParsePrecisionDeclaration()
		{
			this.builder.StartNode(SyntaxType.PrecisionDeclaration);

			this.RequireToken(SyntaxType.PrecisionKeyword);

			this.ParsePrecisionQualifier();

			this.ParseType();

			this.RequireToken(SyntaxType.SemiColonToken);

			this.builder.EndNode();
		}

		#endregion Declaration

		#region Preprocessor

		private void ParseDefinePreprocessor()
		{
			this.builder.StartNode(SyntaxType.DefinePreprocessor);

			int line = this.builder.CurrentToken.Line.LineNumber;

			this.PreprocessorRequireToken(SyntaxType.DefinePreprocessorKeyword);

			this.PreprocessorRequireToken(SyntaxType.IdentifierToken);

			if (this.AcceptToken(SyntaxType.LeftParenToken, true))
			{
				if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
				{
					this.ParseMacroArguments();
				}

				this.PreprocessorRequireToken(SyntaxType.RightParenToken);

				this.ParseTokenString(line);
			}
			else
			{
				this.ParseTokenString(line);
			}

			this.builder.EndNode();
		}

		private void ParseElseIfPreprocessor()
		{
			this.builder.StartNode(SyntaxType.ElseIfPreprocessor);

			int line = this.builder.CurrentToken.Line.LineNumber;

			bool isTrue = this.settings.GetPreprocessorValue(this.snapshot, this.builder.CurrentToken.Span.Start);

			this.PreprocessorRequireToken(SyntaxType.ElseIfPreprocessorKeyword);

			this.ParseTokenString(line);

			if (!isTrue)
			{
				this.ParseExcludedCode();
			}

			ElseIfPreprocessorSyntax node = this.builder.EndNode() as ElseIfPreprocessorSyntax;

			if (this.preprocessorStack.Count > 0)
			{
				this.preprocessorStack.Peek().ElsePreprocessors.Add(new Preprocessor(node.ElseIfKeyword, this.GetPreprocessorValue(node.ElseIfKeyword.Span)));
			}
		}

		private void ParseElsePreprocessor()
		{
			this.builder.StartNode(SyntaxType.ElsePreprocessor);

			bool isTrue = this.settings.GetPreprocessorValue(this.snapshot, this.builder.CurrentToken.Span.Start, true);

			this.PreprocessorRequireToken(SyntaxType.ElsePreprocessorKeyword);

			if (!isTrue)
			{
				this.ParseExcludedCode();
			}

			this.AcceptToken(SyntaxType.EndIfPreprocessorKeyword, true);

			ElsePreprocessorSyntax node = this.builder.EndNode() as ElsePreprocessorSyntax;

			if (this.preprocessorStack.Count > 0)
			{
				this.preprocessorStack.Peek().ElsePreprocessors.Add(new Preprocessor(node.ElseKeyword, this.GetPreprocessorValue(node.ElseKeyword.Span, true)));
			}
		}

		private void ParseEndIfPreprocessor()
		{
			this.builder.StartNode(SyntaxType.EndIfPreprocessor);

			this.PreprocessorRequireToken(SyntaxType.EndIfPreprocessorKeyword);

			this.builder.EndNode();

			if (this.preprocessorStack.Count > 0)
			{
				this.preprocessorStack.Pop();
			}
		}

		private void ParseErrorPreprocessor()
		{
			this.builder.StartNode(SyntaxType.ErrorPreprocessor);

			int line = this.builder.CurrentToken.Line.LineNumber;

			this.PreprocessorRequireToken(SyntaxType.ErrorPreprocessorKeyword);

			this.ParseTokenString(line);

			this.builder.EndNode();
		}

		private void ParseExcludedCode()
		{
			this.builder.StartNode(SyntaxType.ExcludedCode);

			int depth = 0;

			while (depth > 0 || (this.builder.CurrentToken.Type != SyntaxType.EndIfPreprocessorKeyword && this.builder.CurrentToken.Type != SyntaxType.ElseIfPreprocessorKeyword && this.builder.CurrentToken.Type != SyntaxType.ElsePreprocessorKeyword))
			{
				if (this.builder.CurrentToken.Type == SyntaxType.IfPreprocessorKeyword)
				{
					depth++;
				}
				else if (this.builder.CurrentToken.Type == SyntaxType.EndIfPreprocessorKeyword && depth > 0)
				{
					depth--;
				}

				this.AcceptToken(SyntaxType.Any, true);
			}

			this.builder.EndNode();
		}

		private void ParseExtensionPreprocessor()
		{
			this.builder.StartNode(SyntaxType.ExtensionPreprocessor);

			this.PreprocessorRequireToken(SyntaxType.ExtensionPreprocessorKeyword);

			this.PreprocessorRequireToken(SyntaxType.IdentifierToken);

			this.PreprocessorRequireToken(SyntaxType.ColonToken);

			this.PreprocessorRequireToken(SyntaxType.IdentifierToken);

			this.builder.EndNode();
		}

		private void ParseIfDefinedPreprocessor()
		{
			this.builder.StartNode(SyntaxType.IfDefinedPreprocessor);

			bool isTrue = this.settings.GetPreprocessorValue(this.snapshot, this.builder.CurrentToken.Span.Start);

			this.PreprocessorRequireToken(SyntaxType.IfDefinedPreprocessorKeyword);

			this.PreprocessorRequireToken(SyntaxType.IdentifierToken);

			if (!isTrue)
			{
				this.ParseExcludedCode();
			}

			if (this.AcceptToken(SyntaxType.EndIfPreprocessorKeyword, true))
			{
				if (this.preprocessorStack.Count > 0)
				{
					this.preprocessorStack.Pop();
				}
			}

			IfDefinedPreprocessorSyntax node = this.builder.EndNode() as IfDefinedPreprocessorSyntax;

			IfPreprocessor preprocessor = new IfPreprocessor(node.IfDefinedKeyword, this.GetPreprocessorValue(node.IfDefinedKeyword.Span));

			this.preprocessors.Add(preprocessor);

			if (node.EndIfKeyword == null)
			{
				this.preprocessorStack.Push(preprocessor);
			}
		}

		private void ParseIfNotDefinedPreprocessor()
		{
			this.builder.StartNode(SyntaxType.IfNotDefinedPreprocessor);

			bool isTrue = this.settings.GetPreprocessorValue(this.snapshot, this.builder.CurrentToken.Span.Start);

			this.PreprocessorRequireToken(SyntaxType.IfNotDefinedPreprocessorKeyword);

			this.PreprocessorRequireToken(SyntaxType.IdentifierToken);

			if (!isTrue)
			{
				this.ParseExcludedCode();
			}

			this.AcceptToken(SyntaxType.EndIfPreprocessorKeyword, true);

			IfNotDefinedPreprocessorSyntax node = this.builder.EndNode() as IfNotDefinedPreprocessorSyntax;

			IfPreprocessor preprocessor = new IfPreprocessor(node.IfNotDefinedKeyword, this.GetPreprocessorValue(node.IfNotDefinedKeyword.Span));

			this.preprocessors.Add(preprocessor);

			if (node.EndIfKeyword == null)
			{
				this.preprocessorStack.Push(preprocessor);
			}
		}

		private void ParseIfPreprocessor()
		{
			this.builder.StartNode(SyntaxType.IfPreprocessor);

			int line = this.builder.CurrentToken.Line.LineNumber;

			bool isTrue = this.settings.GetPreprocessorValue(this.snapshot, this.builder.CurrentToken.Span.Start);

			this.PreprocessorRequireToken(SyntaxType.IfPreprocessorKeyword);

			this.ParseTokenString(line);

			if (!isTrue)
			{
				this.ParseExcludedCode();
			}

			this.AcceptToken(SyntaxType.EndIfPreprocessorKeyword, true);

			IfPreprocessorSyntax node = this.builder.EndNode() as IfPreprocessorSyntax;

			IfPreprocessor preprocessor = new IfPreprocessor(node.IfKeyword, this.GetPreprocessorValue(node.IfKeyword.Span));

			this.preprocessors.Add(preprocessor);

			if (node.EndIfKeyword == null)
			{
				this.preprocessorStack.Push(preprocessor);
			}
		}

		private void ParseLinePreprocessor()
		{
			this.builder.StartNode(SyntaxType.LinePreprocessor);

			this.PreprocessorRequireToken(SyntaxType.LinePreprocessorKeyword);

			this.PreprocessorRequireToken(SyntaxType.IntConstToken);

			this.AcceptToken(SyntaxType.IntConstToken, true);

			this.builder.EndNode();
		}

		private void ParseMacroArguments()
		{
			this.builder.StartNode(SyntaxType.MacroArguments);

			this.PreprocessorRequireToken(SyntaxType.IdentifierToken);

			while (this.AcceptToken(SyntaxType.CommaToken, true))
			{
				this.PreprocessorRequireToken(SyntaxType.IdentifierToken);
			}

			this.builder.EndNode();
		}

		private void ParsePragmaPreprocessor()
		{
			this.builder.StartNode(SyntaxType.PragmaPreprocessor);

			int line = this.builder.CurrentToken.Line.LineNumber;

			this.PreprocessorRequireToken(SyntaxType.PragmaPreprocessorKeyword);

			this.ParseTokenString(line);

			this.builder.EndNode();
		}

		private void ParsePreprocessor()
		{
			this.builder.StartNode(SyntaxType.Preprocessor);

			switch (this.builder.CurrentToken.Type)
			{
				case SyntaxType.ExtensionPreprocessorKeyword:
					this.ParseExtensionPreprocessor();
					break;

				case SyntaxType.VersionPreprocessorKeyword:
					this.ParseVersionPreprocessor();
					break;

				case SyntaxType.LinePreprocessorKeyword:
					this.ParseLinePreprocessor();
					break;

				case SyntaxType.PragmaPreprocessorKeyword:
					this.ParsePragmaPreprocessor();
					break;

				case SyntaxType.ErrorPreprocessorKeyword:
					this.ParseErrorPreprocessor();
					break;

				case SyntaxType.DefinePreprocessorKeyword:
					this.ParseDefinePreprocessor();
					break;

				case SyntaxType.UndefinePreprocessorKeyword:
					this.ParseUnDefinePreprocessor();
					break;

				case SyntaxType.IfPreprocessorKeyword:
					this.ParseIfPreprocessor();
					break;

				case SyntaxType.EndIfPreprocessorKeyword:
					this.ParseEndIfPreprocessor();
					break;

				case SyntaxType.ElseIfPreprocessorKeyword:
					this.ParseElseIfPreprocessor();
					break;

				case SyntaxType.ElsePreprocessorKeyword:
					this.ParseElsePreprocessor();
					break;

				case SyntaxType.IfDefinedPreprocessorKeyword:
					this.ParseIfDefinedPreprocessor();
					break;

				case SyntaxType.IfNotDefinedPreprocessorKeyword:
					this.ParseIfNotDefinedPreprocessor();
					break;

				case SyntaxType.PoundToken:
					this.RequireToken(SyntaxType.PoundToken);
					break;
			}

			this.builder.EndNode();
		}

		private void ParseTokenString(int line)
		{
			this.builder.StartNode(SyntaxType.TokenString);

			while (line == this.builder.CurrentToken.Line.LineNumber)
			{
				this.AcceptToken(SyntaxType.Any, true);
			}

			this.builder.EndNode();
		}

		private void ParseUnDefinePreprocessor()
		{
			this.builder.StartNode(SyntaxType.UndefinePreprocessor);

			this.PreprocessorRequireToken(SyntaxType.UndefinePreprocessorKeyword);

			this.PreprocessorRequireToken(SyntaxType.IdentifierToken);

			this.builder.EndNode();
		}

		private void ParseVersionPreprocessor()
		{
			this.builder.StartNode(SyntaxType.VersionPreprocessor);

			this.PreprocessorRequireToken(SyntaxType.VersionPreprocessorKeyword);

			this.PreprocessorRequireToken(SyntaxType.IntConstToken);

			this.AcceptToken(SyntaxType.IdentifierToken, true);

			this.builder.EndNode();
		}

		#endregion Preprocessor

		#region Helpers

		private bool GetPreprocessorValue(TrackingSpan span, bool defaultValue = false)
		{
			return this.settings.GetPreprocessorValue(this.snapshot, span.GetSpan(this.snapshot).Start, defaultValue);
		}

		private bool IsAssignmentExpression()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();

			while (this.IsUnaryExpression(this.builder.CurrentToken.Type))
			{
				this.builder.MoveNext();
			}

			switch (this.builder.CurrentToken.Type)
			{
				case SyntaxType.EqualToken:
				case SyntaxType.StarEqualToken:
				case SyntaxType.SlashEqualToken:
				case SyntaxType.PercentEqualToken:
				case SyntaxType.PlusEqualToken:
				case SyntaxType.MinusEqualToken:
				case SyntaxType.LessThenLessThenEqualToken:
				case SyntaxType.GreaterThenGreaterThenEqualToken:
				case SyntaxType.AmpersandEqualToken:
				case SyntaxType.CaretEqualToken:
				case SyntaxType.BarEqualToken:
					result = true;
					break;
			}

			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsConstructor()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();

			if (this.IsType(this.builder.CurrentToken.Type))
			{
				this.builder.MoveNext();

				if (this.builder.CurrentToken.Type == SyntaxType.LeftParenToken)
				{
					result = true;
				}
			}

			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsDeclaration()
		{
			if (this.builder.CurrentToken.Type == SyntaxType.PrecisionKeyword || this.builder.IsTypeName(this.builder.CurrentToken))
			{
				return true;
			}

			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();

			if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.builder.MoveNext();

				if (this.builder.CurrentToken.Type == SyntaxType.SemiColonToken || this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
				{
					result = true;
				}
			}

			if (!result && this.IsType(this.builder.CurrentToken.Type))
			{
				this.builder.MoveNext();

				if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
				{
					this.builder.MoveNext();

					if (this.builder.CurrentToken.Type == SyntaxType.EqualToken || this.builder.CurrentToken.Type == SyntaxType.SemiColonToken || this.builder.CurrentToken.Type == SyntaxType.LeftBracketToken)
					{
						result = true;
					}
				}
				else if (this.builder.CurrentToken.Type == SyntaxType.SemiColonToken)
				{
					result = true;
				}
			}

			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsExpressionStatement(SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.PlusPlusToken:
				case SyntaxType.MinusMinusToken:
				case SyntaxType.PlusToken:
				case SyntaxType.MinusToken:
				case SyntaxType.ExclamationToken:
				case SyntaxType.TildeToken:
				case SyntaxType.IdentifierToken:
				case SyntaxType.IntConstToken:
				case SyntaxType.UIntConstToken:
				case SyntaxType.FloatConstToken:
				case SyntaxType.DoubleConstToken:
				case SyntaxType.BoolConstToken:
				case SyntaxType.LeftParenToken:
				case SyntaxType.SemiColonToken:
					return true;

				default:
					return this.IsType(type);
			}
		}

		private bool IsFunctionCall()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();

			if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
			{
				this.builder.MoveNext();

				if (this.builder.CurrentToken.Type == SyntaxType.LeftParenToken)
				{
					result = true;
				}
			}

			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsFunctionHeader()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();

			if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.builder.MoveNext();
			}

			if (this.IsType(this.builder.CurrentToken.Type) || this.builder.CurrentToken.Type == SyntaxType.VoidKeyword)
			{
				this.builder.MoveNext();

				if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
				{
					this.builder.MoveNext();

					if (this.builder.CurrentToken.Type == SyntaxType.LeftParenToken)
					{
						result = true;
					}
				}
			}

			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsInitDeclaratorList()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();

			if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.builder.MoveNext();
			}

			if (this.IsType(this.builder.CurrentToken.Type))
			{
				result = true;
			}

			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsPostFixOperator(SyntaxType type)
		{
			return type == SyntaxType.PlusPlusToken || type == SyntaxType.MinusMinusToken;
		}

		private bool IsPreprocessor()
		{
			return this.builder.CurrentToken?.Type.IsPreprocessor() ?? false;
		}

		private bool IsSimpleStatement(SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.SwitchKeyword:
				case SyntaxType.PrecisionKeyword:
				case SyntaxType.SemiColonToken:
				case SyntaxType.IfKeyword:
				case SyntaxType.CaseKeyword:
				case SyntaxType.DefaultKeyword:
				case SyntaxType.WhileKeyword:
				case SyntaxType.DoKeyword:
				case SyntaxType.ForKeyword:
				case SyntaxType.ContinueKeyword:
				case SyntaxType.BreakKeyword:
				case SyntaxType.ReturnKeyword:
				case SyntaxType.DiscardKeyword:
				case SyntaxType.VoidKeyword:
					return true;

				default:
					if (this.IsType(type) || this.IsTypeQualifier(type) || this.IsUnaryExpression(type))
					{
						return true;
					}
					else
					{
						return false;
					}
			}
		}

		private bool IsStorageQualifier(SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.ConstKeyword:
				case SyntaxType.InOutKeyword:
				case SyntaxType.InKeyword:
				case SyntaxType.OutKeyword:
				case SyntaxType.CentroidKeyword:
				case SyntaxType.PatchKeyword:
				case SyntaxType.SampleKeyword:
				case SyntaxType.UniformKeyword:
				case SyntaxType.BufferKeyword:
				case SyntaxType.SharedKeyword:
				case SyntaxType.CoherentKeyword:
				case SyntaxType.VolitileKeyword:
				case SyntaxType.RestrictKeyword:
				case SyntaxType.ReadonlyKeyword:
				case SyntaxType.WriteonlyKeyword:
				case SyntaxType.SubroutineKeyword:
					return true;

				default:
					return false;
			}
		}

		private bool IsStructDefinition()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();

			if (this.IsTypeQualifier(this.builder.CurrentToken.Type))
			{
				this.builder.MoveNext();

				if (this.builder.CurrentToken.Type == SyntaxType.IdentifierToken)
				{
					this.builder.MoveNext();

					if (this.builder.CurrentToken.Type == SyntaxType.LeftBraceToken)
					{
						result = true;
					}
				}
			}

			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsType(SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.IntKeyword:
				case SyntaxType.UIntKeyword:
				case SyntaxType.FloatKeyword:
				case SyntaxType.DoubleKeyword:
				case SyntaxType.Vec2Keyword:
				case SyntaxType.Vec3Keyword:
				case SyntaxType.Vec4Keyword:
				case SyntaxType.DVec2Keyword:
				case SyntaxType.DVec3Keyword:
				case SyntaxType.DVec4Keyword:
				case SyntaxType.BVec2Keyword:
				case SyntaxType.BVec3Keyword:
				case SyntaxType.BVec4Keyword:
				case SyntaxType.IVec2Keyword:
				case SyntaxType.IVec3Keyword:
				case SyntaxType.IVec4Keyword:
				case SyntaxType.UVec2Keyword:
				case SyntaxType.UVec3Keyword:
				case SyntaxType.UVec4Keyword:
				case SyntaxType.Mat2Keyword:
				case SyntaxType.Mat2x2Keyword:
				case SyntaxType.Mat2x3Keyword:
				case SyntaxType.Mat2x4Keyword:
				case SyntaxType.Mat3Keyword:
				case SyntaxType.Mat3x2Keyword:
				case SyntaxType.Mat3x3Keyword:
				case SyntaxType.Mat3x4Keyword:
				case SyntaxType.Mat4Keyword:
				case SyntaxType.Mat4x2Keyword:
				case SyntaxType.Mat4x3Keyword:
				case SyntaxType.Mat4x4Keyword:
				case SyntaxType.DMat2Keyword:
				case SyntaxType.DMat2x2Keyword:
				case SyntaxType.DMat2x3Keyword:
				case SyntaxType.DMat2x4Keyword:
				case SyntaxType.DMat3Keyword:
				case SyntaxType.DMat3x2Keyword:
				case SyntaxType.DMat3x3Keyword:
				case SyntaxType.DMat3x4Keyword:
				case SyntaxType.DMat4Keyword:
				case SyntaxType.DMat4x2Keyword:
				case SyntaxType.DMat4x3Keyword:
				case SyntaxType.DMat4x4Keyword:
				case SyntaxType.Sampler1DKeyword:
				case SyntaxType.Sampler2DKeyword:
				case SyntaxType.Sampler3DKeyword:
				case SyntaxType.SamplerCubeKeyword:
				case SyntaxType.Sampler1DShadowKeyword:
				case SyntaxType.Sampler2DShadowKeyword:
				case SyntaxType.SamplerCubeShadowKeyword:
				case SyntaxType.Sampler1DArrayKeyword:
				case SyntaxType.Sampler2DArrayKeyword:
				case SyntaxType.Sampler1DArrayShadowKeyword:
				case SyntaxType.Sampler2DArrayShadowKeyword:
				case SyntaxType.ISampler1DKeyword:
				case SyntaxType.ISampler2DKeyword:
				case SyntaxType.ISampler3DKeyword:
				case SyntaxType.ISamplerCubeKeyword:
				case SyntaxType.ISampler1DArrayKeyword:
				case SyntaxType.ISampler2DArrayKeyword:
				case SyntaxType.USampler1DKeyword:
				case SyntaxType.USampler2DKeyword:
				case SyntaxType.USampler3DKeyword:
				case SyntaxType.USamplerCubeKeyword:
				case SyntaxType.USampler1DArrayKeyword:
				case SyntaxType.USampler2DArrayKeyword:
				case SyntaxType.Sampler2DRectKeyword:
				case SyntaxType.Sampler2DRectShadowKeyword:
				case SyntaxType.ISampler2DRectKeyword:
				case SyntaxType.USampler2DRectKeyword:
				case SyntaxType.SamplerBufferKeyword:
				case SyntaxType.ISamplerBufferKeyword:
				case SyntaxType.USamplerBufferKeyword:
				case SyntaxType.SamplerCubeArrayKeyword:
				case SyntaxType.SamplerCubeArrayShadowKeyword:
				case SyntaxType.ISamplerCubeArrayKeyword:
				case SyntaxType.USamplerCubeArrayKeyword:
				case SyntaxType.Sampler2DMSKeyword:
				case SyntaxType.ISampler2DMSKeyword:
				case SyntaxType.USampler2DMSKeyword:
				case SyntaxType.Sampler2DMSArrayKeyword:
				case SyntaxType.ISampler2DMSArrayKeyword:
				case SyntaxType.USampler2DMSArrayKeyword:
				case SyntaxType.Image1D:
				case SyntaxType.Image2D:
				case SyntaxType.Image3D:
				case SyntaxType.IImage1D:
				case SyntaxType.IImage2D:
				case SyntaxType.IImage3D:
				case SyntaxType.UImage1D:
				case SyntaxType.UImage2D:
				case SyntaxType.UImage3D:
				case SyntaxType.Image2DRect:
				case SyntaxType.IImage2DRect:
				case SyntaxType.UImage2DRect:
				case SyntaxType.ImageCube:
				case SyntaxType.IImageCube:
				case SyntaxType.UImageCube:
				case SyntaxType.ImageBuffer:
				case SyntaxType.IImageBuffer:
				case SyntaxType.UImageBuffer:
				case SyntaxType.Image1DArray:
				case SyntaxType.IImage1DArray:
				case SyntaxType.UImage1DArray:
				case SyntaxType.Image2DArray:
				case SyntaxType.IImage2DArray:
				case SyntaxType.UImage2DArray:
				case SyntaxType.ImageCubeArray:
				case SyntaxType.IImageCubeArray:
				case SyntaxType.UImageCubeArray:
				case SyntaxType.Image2DMS:
				case SyntaxType.IImage2DMS:
				case SyntaxType.UImage2DMS:
				case SyntaxType.Image2DMSArray:
				case SyntaxType.IImage2DMSArray:
				case SyntaxType.UImage2DMSArray:
				case SyntaxType.StructKeyword:
					return true;

				case SyntaxType.IdentifierToken:
					if (this.builder.IsTypeName(this.builder.CurrentToken))
					{
						return true;
					}
					else
					{
						return false;
					}

				default:
					return false;
			}
		}

		private bool IsTypeQualifier(SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.ConstKeyword:
				case SyntaxType.InOutKeyword:
				case SyntaxType.InKeyword:
				case SyntaxType.OutKeyword:
				case SyntaxType.CentroidKeyword:
				case SyntaxType.PatchKeyword:
				case SyntaxType.SampleKeyword:
				case SyntaxType.UniformKeyword:
				case SyntaxType.BufferKeyword:
				case SyntaxType.SharedKeyword:
				case SyntaxType.CoherentKeyword:
				case SyntaxType.VolitileKeyword:
				case SyntaxType.RestrictKeyword:
				case SyntaxType.ReadonlyKeyword:
				case SyntaxType.WriteonlyKeyword:
				case SyntaxType.SubroutineKeyword:
				case SyntaxType.LayoutKeyword:
				case SyntaxType.LowPrecisionKeyword:
				case SyntaxType.MediumPrecisionKeyword:
				case SyntaxType.HighPrecisionKeyword:
				case SyntaxType.SmoothKeyword:
				case SyntaxType.FlatKeyword:
				case SyntaxType.NoPerspectiveKeyword:
				case SyntaxType.InvariantKeyword:
				case SyntaxType.PreciseKeyword:
					return true;

				default:
					return false;
			}
		}

		private bool IsUnaryExpression(SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.PlusPlusToken:
				case SyntaxType.MinusMinusToken:
				case SyntaxType.PlusToken:
				case SyntaxType.MinusToken:
				case SyntaxType.ExclamationToken:
				case SyntaxType.TildeToken:
				case SyntaxType.IdentifierToken:
				case SyntaxType.IntConstToken:
				case SyntaxType.UIntConstToken:
				case SyntaxType.FloatConstToken:
				case SyntaxType.BoolConstToken:
				case SyntaxType.DoubleConstToken:
				case SyntaxType.LeftBracketToken:
				case SyntaxType.DotToken:
				case SyntaxType.LeftParenToken:
					return true;

				default:
					if (this.IsType(type))
					{
						ResetPoint resetPoint = this.builder.GetResetPoint();

						this.builder.MoveNext();

						if (this.builder.CurrentToken.Type == SyntaxType.LeftParenToken)
						{
							this.builder.Reset(resetPoint);

							return true;
						}
						else
						{
							this.builder.Reset(resetPoint);

							return false;
						}
					}
					else
					{
						return false;
					}
			}
		}

		private string GetErrorMessage()
		{
			return Resources.Error_MissingSemicolon;
		}

		#endregion Helpers

		#region Token Helpers

		private bool AcceptToken(SyntaxType type, bool skipPreprocessor = false)
		{
			while (!skipPreprocessor && this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			bool result = false;

			if (this.builder.CurrentToken.Type == type || type == SyntaxType.Any)
			{
				this.builder.AddToken();

				result = true;
			}

			while (!skipPreprocessor && this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			return result;
		}

		private bool AcceptToken(params SyntaxType[] types)
		{
			while (this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			bool result = false;

			if (types.Contains(this.builder.CurrentToken.Type))
			{
				this.builder.AddToken();

				result = true;
			}

			while (this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			return result;
		}

		private void RequireToken(SyntaxType type, string errorMessage = "", Span span = null)
		{
			if (!this.AcceptToken(type))
			{
				this.builder.Error(type, errorMessage, span);
			}
		}

		private void RequireToken(params SyntaxType[] types)
		{
			if (!this.AcceptToken(types))
			{
				this.builder.Error(types[0]);
			}
		}

		private void PreprocessorRequireToken(SyntaxType type)
		{
			if (!this.AcceptToken(type, true))
			{
				this.builder.Error(type);
			}
		}

		#endregion Token Helpers
	}
}