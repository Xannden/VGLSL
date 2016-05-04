using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Parsing
{
	public sealed class GLSLParser
	{
		private readonly GLSLSettings settings;
		private TreeBuilder builder;
		private List<IfPreprocessor> preprocessors;
		private Stack<IfPreprocessor> preprocessorStack;
		private Snapshot snapshot;

		internal GLSLParser(GLSLSettings settings)
		{
			this.settings = settings;
		}

		public IReadOnlyList<IfPreprocessor> Preprocessors => this.preprocessors;

		public SyntaxTree Run(Snapshot snapshot, LinkedList<Token> tokens)
		{
			this.snapshot = snapshot;
			this.builder = new TreeBuilder(snapshot, tokens);
			this.preprocessors = new List<IfPreprocessor>();
			this.preprocessorStack = new Stack<IfPreprocessor>();

			this.ParseProgram();

			return this.builder.GetTree();
		}

		private void ParseProgram()
		{
			this.builder.StartScope();

			this.builder.StartNode(SyntaxType.Program);

			while (this.builder.CurrentToken?.SyntaxType != SyntaxType.EOF)
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

			this.builder.EndScope();
		}

		#region Type

		private ArraySpecifierSyntax ParseArraySpecifier()
		{
			while (this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			this.builder.StartNode(SyntaxType.ArraySpecifier);

			this.RequireToken(SyntaxType.LeftBracketToken);

			if (this.builder.CurrentToken?.SyntaxType != SyntaxType.RightBracketToken)
			{
				this.ParseConstantExpression();
			}

			this.RequireToken(SyntaxType.RightBracketToken);

			return this.builder.EndNode() as ArraySpecifierSyntax;
		}

		private SyntaxNode ParseInterpolationQualifier()
		{
			this.builder.StartNode(SyntaxType.InterpolationQualifier);

			this.RequireToken(SyntaxType.SmoothKeyword, SyntaxType.FlatKeyword, SyntaxType.NoPerspectiveKeyword);

			return this.builder.EndNode();
		}

		private SyntaxNode ParseInvariantQualifier()
		{
			this.builder.StartNode(SyntaxType.InvariantQualifier);

			this.RequireToken(SyntaxType.InvariantKeyword);

			return this.builder.EndNode();
		}

		private SyntaxNode ParseLayoutQualifier()
		{
			this.builder.StartNode(SyntaxType.LayoutQualifier);

			this.RequireToken(SyntaxType.LayoutKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			do
			{
				this.ParseLayoutQualifierID();
			}
			while (this.AcceptToken(SyntaxType.CommaToken));

			this.RequireToken(SyntaxType.RightParenToken);

			return this.builder.EndNode();
		}

		private SyntaxNode ParseLayoutQualifierID()
		{
			this.builder.StartNode(SyntaxType.LayoutQualifierId);

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

			return this.builder.EndNode();
		}

		private SyntaxNode ParsePreciseQualifier()
		{
			this.builder.StartNode(SyntaxType.PreciseQualifier);

			this.RequireToken(SyntaxType.PreciseKeyword);

			return this.builder.EndNode();
		}

		private SyntaxNode ParsePrecisionQualifier()
		{
			this.builder.StartNode(SyntaxType.PrecisionQualifier);

			this.RequireToken(SyntaxType.LowPrecisionKeyword, SyntaxType.MediumPrecisionKeyword, SyntaxType.HighPrecisionKeyword);

			return this.builder.EndNode();
		}

		private SyntaxNode ParseStorageQualifier()
		{
			this.builder.StartNode(SyntaxType.StorageQualifier);

			if (this.AcceptToken(SyntaxType.SubroutineKeyword))
			{
				if (this.AcceptToken(SyntaxType.LeftParenToken))
				{
					do
					{
						this.RequireToken(SyntaxType.IdentifierToken);
					}
					while (this.AcceptToken(SyntaxType.CommaToken));

					this.RequireToken(SyntaxType.RightParenToken);
				}
			}
			else
			{
				this.RequireToken(SyntaxType.ConstKeyword, SyntaxType.InOutKeyword, SyntaxType.InKeyword, SyntaxType.OutKeyword, SyntaxType.CentroidKeyword, SyntaxType.PatchKeyword, SyntaxType.SampleKeyword, SyntaxType.UniformKeyword, SyntaxType.BufferKeyword, SyntaxType.SharedKeyword, SyntaxType.CoherentKeyword, SyntaxType.VolatileKeyword, SyntaxType.RestrictKeyword, SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword);
			}

			return this.builder.EndNode();
		}

		private TypeSyntax ParseType()
		{
			this.builder.StartNode(SyntaxType.Type);

			this.ParseTypeNonArray();

			while (this.builder.CurrentToken?.SyntaxType == SyntaxType.LeftBracketToken)
			{
				this.ParseArraySpecifier();
			}

			return this.builder.EndNode() as TypeSyntax;
		}

		private TypeNameSyntax ParseTypeName()
		{
			SyntaxNode node = this.builder.StartNode(SyntaxType.TypeName);

			IdentifierSyntax identifier = this.RequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

			if (node?.Parent.SyntaxType == SyntaxType.TypeNonArray && identifier != null)
			{
				identifier.Definition = this.builder.FindDefinition(identifier);
			}

			return this.builder.EndNode() as TypeNameSyntax;
		}

		private TypeNonArraySyntax ParseTypeNonArray()
		{
			this.builder.StartNode(SyntaxType.TypeNonArray);

			if (this.builder.CurrentToken?.SyntaxType == SyntaxType.StructKeyword)
			{
				this.ParseStructSpecifier();
			}
			else if (this.builder.CurrentToken?.SyntaxType == SyntaxType.IdentifierToken)
			{
				this.ParseTypeName();
			}
			else
			{
				this.RequireToken(SyntaxType.IntKeyword, SyntaxType.UIntKeyword, SyntaxType.FloatKeyword, SyntaxType.DoubleKeyword, SyntaxType.Vec2Keyword, SyntaxType.Vec3Keyword, SyntaxType.Vec4Keyword, SyntaxType.UVec2Keyword, SyntaxType.UVec3Keyword, SyntaxType.UVec4Keyword, SyntaxType.IVec2Keyword, SyntaxType.IVec3Keyword, SyntaxType.IVec4Keyword, SyntaxType.DVec2Keyword, SyntaxType.DVec3Keyword, SyntaxType.DVec4Keyword, SyntaxType.BVec2Keyword, SyntaxType.BVec3Keyword, SyntaxType.BVec4Keyword, SyntaxType.Mat2Keyword, SyntaxType.Mat3Keyword, SyntaxType.Mat4Keyword, SyntaxType.Mat2X2Keyword, SyntaxType.Mat2X3Keyword, SyntaxType.Mat2X4Keyword, SyntaxType.Mat3X2Keyword, SyntaxType.Mat3X3Keyword, SyntaxType.Mat3X4Keyword, SyntaxType.Mat4X2Keyword, SyntaxType.Mat4X3Keyword, SyntaxType.Mat4X4Keyword, SyntaxType.DMat2Keyword, SyntaxType.DMat3Keyword, SyntaxType.DMat4Keyword, SyntaxType.DMat2X2Keyword, SyntaxType.DMat2X3Keyword, SyntaxType.DMat2X4Keyword, SyntaxType.DMat3X2Keyword, SyntaxType.DMat3X3Keyword, SyntaxType.DMat3X4Keyword, SyntaxType.DMat4X2Keyword, SyntaxType.DMat4X3Keyword, SyntaxType.DMat4X4Keyword, SyntaxType.Sampler1DKeyword, SyntaxType.Sampler2DKeyword, SyntaxType.Sampler3DKeyword, SyntaxType.SamplerCubeKeyword, SyntaxType.Sampler1DShadowKeyword, SyntaxType.Sampler2DShadowKeyword, SyntaxType.SamplerCubeShadowKeyword, SyntaxType.Sampler1DArrayKeyword, SyntaxType.Sampler2DArrayKeyword, SyntaxType.Sampler1DArrayShadowKeyword, SyntaxType.Sampler2DArrayShadowKeyword, SyntaxType.ISampler1DKeyword, SyntaxType.ISampler2DKeyword, SyntaxType.ISampler3DKeyword, SyntaxType.ISamplerCubeKeyword, SyntaxType.ISampler1DArrayKeyword, SyntaxType.ISampler2DArrayKeyword, SyntaxType.USampler1DKeyword, SyntaxType.USampler2DKeyword, SyntaxType.USampler3DKeyword, SyntaxType.USamplerCubeKeyword, SyntaxType.USampler1DArrayKeyword, SyntaxType.USampler2DArrayKeyword, SyntaxType.Sampler2DRectKeyword, SyntaxType.Sampler2DRectShadowKeyword, SyntaxType.ISampler2DRectKeyword, SyntaxType.USampler2DRectKeyword, SyntaxType.SamplerBufferKeyword, SyntaxType.ISamplerBufferKeyword, SyntaxType.USamplerBufferKeyword, SyntaxType.SamplerCubeArrayKeyword, SyntaxType.SamplerCubeArrayShadowKeyword, SyntaxType.ISamplerCubeArrayKeyword, SyntaxType.USamplerCubeArrayKeyword, SyntaxType.Sampler2DMSKeyword, SyntaxType.ISampler2DMSKeyword, SyntaxType.USampler2DMSKeyword, SyntaxType.Sampler2DMSArrayKeyword, SyntaxType.ISampler2DMSArrayKeyword, SyntaxType.USampler2DMSArrayKeyword, SyntaxType.Image1DKeyword, SyntaxType.Image2DKeyword, SyntaxType.Image3DKeyword, SyntaxType.IImage1DKeyword, SyntaxType.IImage2DKeyword, SyntaxType.IImage3DKeyword, SyntaxType.UImage1DKeyword, SyntaxType.UImage2DKeyword, SyntaxType.UImage3DKeyword, SyntaxType.Image2DRectKeyword, SyntaxType.IImage2DRectKeyword, SyntaxType.UImage2DRectKeyword, SyntaxType.ImageCubeKeyword, SyntaxType.IImageCubeKeyword, SyntaxType.UImageCubeKeyword, SyntaxType.ImageBufferKeyword, SyntaxType.IImageBufferKeyword, SyntaxType.UImageBufferKeyword, SyntaxType.Image1DArrayKeyword, SyntaxType.IImage1DArrayKeyword, SyntaxType.UImage1DArrayKeyword, SyntaxType.Image2DArrayKeyword, SyntaxType.IImage2DArrayKeyword, SyntaxType.UImage2DArrayKeyword, SyntaxType.ImageCubeArrayKeyword, SyntaxType.IImageCubeArrayKeyword, SyntaxType.UImageCubeArrayKeyword, SyntaxType.Image2DMSKeyword, SyntaxType.IImage2DMSKeyword, SyntaxType.UImage2DMSKeyword, SyntaxType.Image2DMSArrayKeyword, SyntaxType.IImage2DMSArrayKeyword, SyntaxType.UImage2DMSArrayKeyword);
			}

			return this.builder.EndNode() as TypeNonArraySyntax;
		}

		private TypeQualifierSyntax ParseTypeQualifier()
		{
			this.builder.StartNode(SyntaxType.TypeQualifier);

			do
			{
				this.ParseSingleTypeQualifier();
			}
			while (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType));

			return this.builder.EndNode() as TypeQualifierSyntax;
		}

		private void ParseSingleTypeQualifier()
		{
			this.builder.StartNode(SyntaxType.SingleTypeQualifier);

			SyntaxType type = this.builder.CurrentToken.SyntaxType;

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

		private BlockSyntax ParseBlock()
		{
			this.builder.StartNode(SyntaxType.Block);

			if (this.AcceptToken(SyntaxType.LeftBraceToken))
			{
				while (this.IsSimpleStatement(this.builder.CurrentToken.SyntaxType))
				{
					this.ParseSimpleStatement();
				}

				this.RequireToken(SyntaxType.RightBraceToken);
			}
			else
			{
				this.RequireToken(SyntaxType.SemicolonToken);
			}

			return this.builder.EndNode() as BlockSyntax;
		}

		private FunctionDefinitionSyntax ParseFunctionDefinition()
		{
			this.builder.StartNode(SyntaxType.FunctionDefinition);

			this.ParseFunctionHeader();

			this.ParseBlock();

			this.builder.EndScope();

			return this.builder.EndNode() as FunctionDefinitionSyntax;
		}

		private FunctionHeaderSyntax ParseFunctionHeader()
		{
			SyntaxNode node = this.builder.StartNode(SyntaxType.FunctionHeader);

			if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseTypeQualifier();
			}

			this.ParseReturnType();

			IdentifierSyntax identifier = this.RequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

			if (identifier != null)
			{
				identifier.Definition = this.builder.AddDefinition(node, identifier, DefinitionKind.Function);
			}

			this.builder.StartScope();

			this.RequireToken(SyntaxType.LeftParenToken);

			if (this.IsType(this.builder.CurrentToken.SyntaxType) || this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
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

			return this.builder.EndNode() as FunctionHeaderSyntax;
		}

		private ParameterSyntax ParseParameter()
		{
			SyntaxNode node = this.builder.StartNode(SyntaxType.Parameter);

			if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseTypeQualifier();
			}

			this.ParseType();

			if (this.builder.CurrentToken?.SyntaxType == SyntaxType.IdentifierToken)
			{
				IdentifierSyntax identifier = this.RequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

				if (identifier != null)
				{
					identifier.Definition = this.builder.AddDefinition(node, identifier, DefinitionKind.Parameter);
				}

				while (this.builder.CurrentToken?.SyntaxType == SyntaxType.LeftBracketToken)
				{
					this.ParseArraySpecifier();
				}
			}

			return this.builder.EndNode() as ParameterSyntax;
		}

		private ReturnTypeSyntax ParseReturnType()
		{
			this.builder.StartNode(SyntaxType.ReturnType);

			if (!this.AcceptToken(SyntaxType.VoidKeyword))
			{
				this.ParseType();
			}

			return this.builder.EndNode() as ReturnTypeSyntax;
		}

		#endregion Function

		#region Statement

		private CaseLabelSyntax ParseCaseLabel()
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

			return this.builder.EndNode() as CaseLabelSyntax;
		}

		private ConditionSyntax ParseCondition()
		{
			SyntaxNode node = this.builder.StartNode(SyntaxType.Condition);

			if (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseExpression();
			}
			else
			{
				if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
				{
					this.ParseTypeQualifier();
				}

				this.ParseType();

				IdentifierSyntax identifier = this.RequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

				if (identifier != null)
				{
					identifier.Definition = this.builder.AddDefinition(node, identifier, DefinitionKind.LocalVariable);
				}

				this.RequireToken(SyntaxType.EqualToken);

				this.ParseInitializer();
			}

			return this.builder.EndNode() as ConditionSyntax;
		}

		private DoWhileStatementSyntax ParseDoWhileStatement()
		{
			this.builder.StartNode(SyntaxType.WhileStatement);

			this.builder.StartScope();

			this.RequireToken(SyntaxType.DoKeyword);

			this.ParseStatementNoNewScope();

			this.RequireToken(SyntaxType.WhileKeyword);

			this.builder.EndScope();

			this.RequireToken(SyntaxType.LeftParenToken);

			this.ParseExpression();

			this.RequireToken(SyntaxType.RightParenToken);

			this.RequireToken(SyntaxType.SemicolonToken);

			return this.builder.EndNode() as DoWhileStatementSyntax;
		}

		private ElseStatementSyntax ParseElseStatement()
		{
			this.builder.StartNode(SyntaxType.ElseStatement);

			this.RequireToken(SyntaxType.ElseKeyword);

			this.ParseStatement();

			return this.builder.EndNode() as ElseStatementSyntax;
		}

		private ExpressionStatementSyntax ParseExpressionStatement()
		{
			this.builder.StartNode(SyntaxType.ExpressionStatement);

			if (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseExpression();
			}

			this.RequireToken(SyntaxType.SemicolonToken);

			return this.builder.EndNode() as ExpressionStatementSyntax;
		}

		private ForStatementSyntax ParseForStatement()
		{
			this.builder.StartNode(SyntaxType.ForStatement);

			this.RequireToken(SyntaxType.ForKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.builder.StartScope();

			if (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType) || this.builder.CurrentToken.SyntaxType == SyntaxType.SemicolonToken)
			{
				this.ParseExpressionStatement();
			}
			else
			{
				this.ParseDeclaration();
			}

			if (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType) || this.IsType(this.builder.CurrentToken.SyntaxType) || this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseCondition();
			}

			this.RequireToken(SyntaxType.SemicolonToken);

			if (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseExpression();
			}

			this.RequireToken(SyntaxType.RightParenToken);

			this.ParseStatementNoNewScope();

			this.builder.EndScope();

			return this.builder.EndNode() as ForStatementSyntax;
		}

		private IterationStatementSyntax ParseIterationStatement()
		{
			this.builder.StartNode(SyntaxType.IterationStatement);

			switch (this.builder.CurrentToken?.SyntaxType)
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

			return this.builder.EndNode() as IterationStatementSyntax;
		}

		private JumpStatementSyntax ParseJumpStatement()
		{
			this.builder.StartNode(SyntaxType.JumpStatement);

			if (!this.AcceptToken(SyntaxType.ContinueKeyword, SyntaxType.BreakKeyword, SyntaxType.DiscardKeyword))
			{
				this.RequireToken(SyntaxType.ReturnKeyword);

				if (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType))
				{
					this.ParseExpression();
				}
			}

			this.RequireToken(SyntaxType.SemicolonToken);

			return this.builder.EndNode() as JumpStatementSyntax;
		}

		private SelectionStatementSyntax ParseSelectionStatement()
		{
			this.builder.StartNode(SyntaxType.SelectionStatement);

			this.RequireToken(SyntaxType.IfKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.ParseExpression();

			this.RequireToken(SyntaxType.RightParenToken);

			this.ParseStatement();

			if (this.builder.CurrentToken?.SyntaxType == SyntaxType.ElseKeyword)
			{
				this.ParseElseStatement();
			}

			return this.builder.EndNode() as SelectionStatementSyntax;
		}

		private SimpleStatementSyntax ParseSimpleStatement()
		{
			this.builder.StartNode(SyntaxType.SimpleStatement);

			SyntaxType type = this.builder.CurrentToken.SyntaxType;

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
			else if (type == SyntaxType.PrecisionKeyword || this.IsDeclaration())
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

			return this.builder.EndNode() as SimpleStatementSyntax;
		}

		private StatementSyntax ParseStatement()
		{
			this.builder.StartScope();
			this.builder.StartNode(SyntaxType.Statement);

			if (this.AcceptToken(SyntaxType.LeftBraceToken))
			{
				while (this.IsSimpleStatement(this.builder.CurrentToken.SyntaxType))
				{
					this.ParseSimpleStatement();
				}

				this.RequireToken(SyntaxType.RightBraceToken);
			}
			else
			{
				this.ParseSimpleStatement();
			}

			this.builder.EndScope();

			return this.builder.EndNode() as StatementSyntax;
		}

		private SwitchStatementSyntax ParseSwitchStatement()
		{
			this.builder.StartNode(SyntaxType.SwitchStatement);

			this.RequireToken(SyntaxType.SwitchKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.ParseExpression();

			this.RequireToken(SyntaxType.RightParenToken);

			this.builder.StartScope();

			this.RequireToken(SyntaxType.LeftBraceToken);

			while (this.IsSimpleStatement(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseSimpleStatement();
			}

			this.RequireToken(SyntaxType.RightBraceToken);

			this.builder.EndScope();

			return this.builder.EndNode() as SwitchStatementSyntax;
		}

		private WhileStatementSyntax ParseWhileStatement()
		{
			this.builder.StartNode(SyntaxType.WhileStatement);

			this.RequireToken(SyntaxType.WhileKeyword);

			this.RequireToken(SyntaxType.LeftParenToken);

			this.builder.StartScope();

			this.ParseCondition();

			this.RequireToken(SyntaxType.RightParenToken);

			this.ParseStatementNoNewScope();

			this.builder.EndScope();

			return this.builder.EndNode() as WhileStatementSyntax;
		}

		private StatementSyntax ParseStatementNoNewScope()
		{
			this.builder.StartNode(SyntaxType.Statement);

			if (this.AcceptToken(SyntaxType.LeftBraceToken))
			{
				while (this.IsSimpleStatement(this.builder.CurrentToken.SyntaxType))
				{
					this.ParseSimpleStatement();
				}

				this.RequireToken(SyntaxType.RightBraceToken);
			}
			else
			{
				this.ParseSimpleStatement();
			}

			return this.builder.EndNode() as StatementSyntax;
		}

		#endregion Statement

		#region Struct

		private StructDeclarationSyntax ParseStructDeclaration()
		{
			this.builder.StartNode(SyntaxType.StructDeclaration);

			if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseTypeQualifier();
			}

			this.ParseType();

			do
			{
				StructDeclaratorSyntax declarator = this.ParseStructDeclarator();

				if (declarator?.Identifier != null)
				{
					declarator.Identifier.Definition = this.builder.AddDefinition(declarator, declarator.Identifier, DefinitionKind.Field);
				}
			}
			while (this.AcceptToken(SyntaxType.CommaToken));

			this.RequireToken(SyntaxType.SemicolonToken);

			return this.builder.EndNode() as StructDeclarationSyntax;
		}

		private StructDeclaratorSyntax ParseStructDeclarator()
		{
			this.builder.StartNode(SyntaxType.StructDeclarator);

			this.RequireToken(SyntaxType.IdentifierToken);

			while (this.builder.CurrentToken?.SyntaxType == SyntaxType.LeftBracketToken)
			{
				this.ParseArraySpecifier();
			}

			return this.builder.EndNode() as StructDeclaratorSyntax;
		}

		private StructSpecifierSyntax ParseStructSpecifier()
		{
			this.builder.StartNode(SyntaxType.StructSpecifier);

			this.RequireToken(SyntaxType.StructKeyword);

			if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
			{
				TypeNameSyntax typeName = this.ParseTypeName();

				if (typeName?.Identifier != null)
				{
					typeName.Identifier.Definition = this.builder.AddDefinition(typeName, typeName.Identifier, DefinitionKind.TypeName);
				}
			}

			this.RequireToken(SyntaxType.LeftBraceToken);

			do
			{
				this.ParseStructDeclaration();
			}
			while (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType) || this.IsType(this.builder.CurrentToken.SyntaxType));

			this.RequireToken(SyntaxType.RightBraceToken);

			return this.builder.EndNode() as StructSpecifierSyntax;
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

			if (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType))
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
				FunctionCallSyntax node = this.ParseFunctionCall();

				if (node?.Identifier != null)
				{
					node.Identifier.Definition = null;
				}
			}
			else
			{
				this.RequireToken(SyntaxType.IdentifierToken);

				// TODO: fix this
				// identifier.Definition = this.builder.FindDefinition(identifier);
			}

			this.builder.EndNode();
		}

		private FunctionCallSyntax ParseFunctionCall()
		{
			this.builder.StartNode(SyntaxType.FunctionCall);

			IdentifierSyntax identifier = this.RequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

			if (identifier != null)
			{
				identifier.Definition = this.builder.FindDefinition(identifier);
			}

			this.RequireToken(SyntaxType.LeftParenToken);

			if (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType))
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

			return this.builder.EndNode() as FunctionCallSyntax;
		}

		private void ParseInclusiveOrExpression()
		{
			this.builder.StartNode(SyntaxType.LogicalXorExpression);

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
			this.builder.StartNode(SyntaxType.LogicalXorExpression);

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
			this.builder.StartNode(SyntaxType.PostfixExpressionStart);

			this.RequireToken(SyntaxType.LeftBracketToken);

			this.ParseExpression();

			this.RequireToken(SyntaxType.RightBracketToken);

			this.builder.EndNode();
		}

		private void ParsePostFixExpression()
		{
			this.builder.StartNode(SyntaxType.PostfixExpression);

			this.ParsePostFixExpressionStart();

			while (this.IsPostFixOperator(this.builder.CurrentToken.SyntaxType) || this.builder.CurrentToken.SyntaxType == SyntaxType.DotToken || this.builder.CurrentToken.SyntaxType == SyntaxType.LeftBracketToken)
			{
				this.ParsePostFixExpressionContinuation();
			}

			this.builder.EndNode();
		}

		private void ParsePostFixExpressionContinuation()
		{
			this.builder.StartNode(SyntaxType.PostfixExpressionContinuation);

			if (this.builder.CurrentToken.SyntaxType == SyntaxType.DotToken)
			{
				this.ParseFieldSelection();
			}
			else if (this.builder.CurrentToken.SyntaxType == SyntaxType.LeftBracketToken)
			{
				this.ParsePostFixArrayAccess();
			}
			else
			{
				this.RequireToken(SyntaxType.PlusPlusToken, SyntaxType.MinusMinusToken);
			}

			this.builder.EndNode();
		}

		private void ParsePostFixExpressionStart()
		{
			this.builder.StartNode(SyntaxType.PostfixExpressionStart);

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
				if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
				{
					IdentifierSyntax identifier = this.RequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

					if (identifier != null)
					{
						identifier.Definition = this.builder.FindDefinition(identifier);
					}
				}
				else
				{
					this.RequireToken(SyntaxType.IntConstToken, SyntaxType.UIntConstToken, SyntaxType.FloatConstToken, SyntaxType.BoolConstToken, SyntaxType.DoubleConstToken);
				}
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

			if (this.builder.CurrentToken?.SyntaxType == SyntaxType.PrecisionKeyword)
			{
				this.ParsePrecisionDeclaration();
			}
			else if (this.IsInterfaceBlock())
			{
				this.ParseInterfaceBlock();
			}
			else if (this.IsInitDeclaratorList())
			{
				this.ParseInitDeclaratorList();
			}
			else
			{
				this.builder.Error(SyntaxType.Declaration);
				this.builder.MoveNext();
			}

			this.builder.EndNode();
		}

		private void ParseInitDeclaratorList()
		{
			this.builder.StartNode(SyntaxType.InitDeclaratorList);

			if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseTypeQualifier();
			}

			this.ParseType();

			if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
			{
				do
				{
					this.ParseInitPart();
				}
				while (this.AcceptToken(SyntaxType.CommaToken));
			}

			this.RequireToken(SyntaxType.SemicolonToken);

			this.builder.EndNode();
		}

		private void ParseInitializer()
		{
			this.builder.StartNode(SyntaxType.Initializer);

			if (this.builder.CurrentToken.SyntaxType == SyntaxType.LeftBraceToken)
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
				if (this.builder.CurrentToken.SyntaxType == SyntaxType.RightBraceToken)
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

		private InitPartSyntax ParseInitPart()
		{
			SyntaxNode node = this.builder.StartNode(SyntaxType.InitPart);

			IdentifierSyntax identifier = this.RequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

			if (identifier != null)
			{
				identifier.Definition = this.builder.AddDefinition(node, identifier, this.GetVariableType(node));
			}

			while (this.builder.CurrentToken.SyntaxType == SyntaxType.LeftBracketToken)
			{
				this.ParseArraySpecifier();
			}

			if (this.AcceptToken(SyntaxType.EqualToken))
			{
				this.ParseInitializer();
			}

			return this.builder.EndNode() as InitPartSyntax;
		}

		private void ParsePrecisionDeclaration()
		{
			this.builder.StartNode(SyntaxType.PrecisionDeclaration);

			this.RequireToken(SyntaxType.PrecisionKeyword);

			this.ParsePrecisionQualifier();

			this.ParseType();

			this.RequireToken(SyntaxType.SemicolonToken);

			this.builder.EndNode();
		}

		private InterfaceBlockSyntax ParseInterfaceBlock()
		{
			SyntaxNode node = this.builder.StartNode(SyntaxType.InterfaceBlock);

			this.ParseTypeQualifier();

			if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
			{
				IdentifierSyntax identifier = this.RequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

				if (identifier != null)
				{
					identifier.Definition = this.builder.AddDefinition(node, identifier, DefinitionKind.InterfaceBlock);
				}

				if (this.builder.CurrentToken.SyntaxType == SyntaxType.CommaToken)
				{
					while (this.AcceptToken(SyntaxType.CommaToken))
					{
						this.RequireToken(SyntaxType.IdentifierToken);
					}
				}
				else
				{
					this.RequireToken(SyntaxType.LeftBraceToken);

					do
					{
						this.ParseStructDeclaration();
					}
					while (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType) || this.IsType(this.builder.CurrentToken.SyntaxType));

					this.RequireToken(SyntaxType.RightBraceToken);

					if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
					{
						StructDeclaratorSyntax declarator = this.ParseStructDeclarator();

						if (declarator?.Identifier != null)
						{
							declarator.Identifier.Definition = this.builder.AddDefinition(declarator, declarator.Identifier, this.GetVariableType(declarator));
						}
					}
				}
			}

			this.RequireToken(SyntaxType.SemicolonToken);

			return this.builder.EndNode() as InterfaceBlockSyntax;
		}

		#endregion Declaration

		#region Preprocessor

		private void ParseDefinePreprocessor()
		{
			SyntaxNode node = this.builder.StartNode(SyntaxType.DefinePreprocessor);

			int line = this.builder.CurrentToken.Line.LineNumber;

			this.PreprocessorRequireToken(SyntaxType.DefinePreprocessorKeyword);

			IdentifierSyntax identifier = this.PreprocessorRequireToken(SyntaxType.IdentifierToken) as IdentifierSyntax;

			if (identifier != null)
			{
				identifier.Definition = this.builder.AddDefinition(node, identifier, DefinitionKind.Macro);
			}

			if (this.AcceptTokenPreprocessor(SyntaxType.LeftParenToken))
			{
				if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
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
				this.preprocessorStack.Peek().InternalElsePreprocessors.Add(new Preprocessor(node.ElseIfKeyword, this.GetPreprocessorValue(node.ElseIfKeyword.Span)));
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

			this.AcceptTokenPreprocessor(SyntaxType.EndIfPreprocessorKeyword);

			ElsePreprocessorSyntax node = this.builder.EndNode() as ElsePreprocessorSyntax;

			if (this.preprocessorStack.Count > 0)
			{
				this.preprocessorStack.Peek().InternalElsePreprocessors.Add(new Preprocessor(node.ElseKeyword, this.GetPreprocessorValue(node.ElseKeyword.Span, true)));
			}
		}

		private void ParseEndIfPreprocessor()
		{
			EndIfPreprocessorSyntax endIf = this.builder.StartNode(SyntaxType.EndIfPreprocessor) as EndIfPreprocessorSyntax;

			this.PreprocessorRequireToken(SyntaxType.EndIfPreprocessorKeyword);

			this.builder.EndNode();

			if (this.preprocessorStack.Count > 0)
			{
				this.preprocessorStack.Peek().EndIf = endIf;

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

			while (this.builder.CurrentToken.SyntaxType != SyntaxType.EOF && (depth > 0 || (this.builder.CurrentToken.SyntaxType != SyntaxType.EndIfPreprocessorKeyword && this.builder.CurrentToken.SyntaxType != SyntaxType.ElseIfPreprocessorKeyword && this.builder.CurrentToken.SyntaxType != SyntaxType.ElsePreprocessorKeyword)))
			{
				if (this.builder.CurrentToken.SyntaxType == SyntaxType.IfPreprocessorKeyword)
				{
					depth++;
				}
				else if (this.builder.CurrentToken.SyntaxType == SyntaxType.EndIfPreprocessorKeyword && depth > 0)
				{
					depth--;
				}

				this.AcceptTokenPreprocessor(SyntaxType.Any);
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

			if (this.AcceptTokenPreprocessor(SyntaxType.EndIfPreprocessorKeyword) && this.preprocessorStack.Count > 0)
			{
				this.preprocessorStack.Pop();
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

			this.AcceptTokenPreprocessor(SyntaxType.EndIfPreprocessorKeyword);

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

			this.AcceptTokenPreprocessor(SyntaxType.EndIfPreprocessorKeyword);

			IfPreprocessorSyntax node = this.builder.EndNode() as IfPreprocessorSyntax;

			IfPreprocessor preprocessor = new IfPreprocessor(node?.IfKeyword, this.GetPreprocessorValue(node?.IfKeyword.Span));

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

			this.AcceptTokenPreprocessor(SyntaxType.IntConstToken);

			this.builder.EndNode();
		}

		private void ParseMacroArguments()
		{
			this.builder.StartNode(SyntaxType.MacroArguments);

			this.PreprocessorRequireToken(SyntaxType.IdentifierToken);

			while (this.AcceptTokenPreprocessor(SyntaxType.CommaToken))
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

			switch (this.builder.CurrentToken.SyntaxType)
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
				this.AcceptTokenPreprocessor(SyntaxType.Any);
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

			this.AcceptTokenPreprocessor(SyntaxType.IdentifierToken);

			this.builder.EndNode();
		}

		#endregion Preprocessor

		#region Helpers

		private bool IsPostFixOperator(SyntaxType type)
		{
			return type == SyntaxType.PlusPlusToken || type == SyntaxType.MinusMinusToken;
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
				case SyntaxType.VolatileKeyword:
				case SyntaxType.RestrictKeyword:
				case SyntaxType.ReadOnlyKeyword:
				case SyntaxType.WriteOnlyKeyword:
				case SyntaxType.SubroutineKeyword:
					return true;

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
				case SyntaxType.VolatileKeyword:
				case SyntaxType.RestrictKeyword:
				case SyntaxType.ReadOnlyKeyword:
				case SyntaxType.WriteOnlyKeyword:
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

		private DefinitionKind GetVariableType(SyntaxNode node)
		{
			foreach (SyntaxNode ancestor in node.Ancestors)
			{
				if (ancestor.SyntaxType == SyntaxType.FunctionDefinition)
				{
					return DefinitionKind.LocalVariable;
				}
			}

			return DefinitionKind.GlobalVariable;
		}

		private bool GetPreprocessorValue(TrackingSpan span, bool defaultValue = false)
		{
			return this.settings.GetPreprocessorValue(this.snapshot, span.GetSpan(this.snapshot).Start, defaultValue);
		}

		private bool IsAssignmentExpression()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();
			this.builder.StartTestMode();

			while (this.IsUnaryExpression(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseUnaryExpression();
			}

			switch (this.builder.CurrentToken.SyntaxType)
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

			this.builder.EndTestMode();
			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsConstructor()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();
			this.builder.StartTestMode();

			if (this.IsType(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseType();

				if (this.builder.CurrentToken.SyntaxType == SyntaxType.LeftParenToken)
				{
					result = true;
				}
			}

			this.builder.EndTestMode();
			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsDeclaration()
		{
			if (this.builder.CurrentToken.SyntaxType == SyntaxType.PrecisionKeyword || this.builder.IsTypeName(this.builder.CurrentToken))
			{
				return true;
			}

			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();
			this.builder.StartTestMode();

			if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseTypeQualifier();

				if (this.builder.CurrentToken.SyntaxType == SyntaxType.SemicolonToken || this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken || this.IsType(this.builder.CurrentToken.SyntaxType))
				{
					result = true;
				}
			}
			else if (this.IsType(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseType();

				if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
				{
					this.builder.MoveNext();

					if (this.builder.CurrentToken.SyntaxType == SyntaxType.EqualToken || this.builder.CurrentToken.SyntaxType == SyntaxType.SemicolonToken || this.builder.CurrentToken.SyntaxType == SyntaxType.LeftBracketToken)
					{
						result = true;
					}
				}
				else if (this.builder.CurrentToken.SyntaxType == SyntaxType.SemicolonToken)
				{
					result = true;
				}
			}

			this.builder.EndTestMode();
			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsExpressionStatement(SyntaxType type)
		{
			if (type == SyntaxType.SemicolonToken)
			{
				return true;
			}
			else
			{
				return this.IsUnaryExpression(type);
			}
		}

		private bool IsFunctionCall()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();

			if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
			{
				this.builder.MoveNext();

				if (this.builder.CurrentToken.SyntaxType == SyntaxType.LeftParenToken)
				{
					result = true;
				}
			}

			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsFunctionHeader()
		{
			if (this.builder.CurrentToken == null)
			{
				return false;
			}

			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();
			this.builder.StartTestMode();

			if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseTypeQualifier();
			}

			if (this.IsType(this.builder.CurrentToken.SyntaxType) || this.builder.CurrentToken.SyntaxType == SyntaxType.VoidKeyword)
			{
				this.ParseReturnType();

				if (this.builder.CurrentToken.SyntaxType == SyntaxType.IdentifierToken)
				{
					this.builder.MoveNext();

					if (this.builder.CurrentToken.SyntaxType == SyntaxType.LeftParenToken)
					{
						result = true;
					}
				}
			}

			this.builder.EndTestMode();
			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsInitDeclaratorList()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();
			this.builder.StartTestMode();

			if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseTypeQualifier();
			}

			if (this.IsType(this.builder.CurrentToken.SyntaxType))
			{
				result = true;
			}

			this.builder.EndTestMode();
			this.builder.Reset(resetPoint);

			return result;
		}

		private bool IsPreprocessor()
		{
			return this.builder.CurrentToken?.SyntaxType.IsPreprocessor() ?? false;
		}

		private bool IsSimpleStatement(SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.SwitchKeyword:
				case SyntaxType.PrecisionKeyword:
				case SyntaxType.SemicolonToken:
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

		private bool IsInterfaceBlock()
		{
			bool result = false;
			ResetPoint resetPoint = this.builder.GetResetPoint();
			this.builder.StartTestMode();

			if (this.IsTypeQualifier(this.builder.CurrentToken.SyntaxType))
			{
				this.ParseTypeQualifier();

				if (this.builder.CurrentToken?.SyntaxType == SyntaxType.IdentifierToken)
				{
					this.builder.MoveNext();

					if (this.builder.CurrentToken?.SyntaxType == SyntaxType.LeftBraceToken || this.builder.CurrentToken?.SyntaxType == SyntaxType.SemicolonToken)
					{
						result = true;
					}
				}
				else if (this.builder.CurrentToken?.SyntaxType == SyntaxType.SemicolonToken)
				{
					result = true;
				}
			}

			this.builder.EndTestMode();
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
				case SyntaxType.Mat2X2Keyword:
				case SyntaxType.Mat2X3Keyword:
				case SyntaxType.Mat2X4Keyword:
				case SyntaxType.Mat3Keyword:
				case SyntaxType.Mat3X2Keyword:
				case SyntaxType.Mat3X3Keyword:
				case SyntaxType.Mat3X4Keyword:
				case SyntaxType.Mat4Keyword:
				case SyntaxType.Mat4X2Keyword:
				case SyntaxType.Mat4X3Keyword:
				case SyntaxType.Mat4X4Keyword:
				case SyntaxType.DMat2Keyword:
				case SyntaxType.DMat2X2Keyword:
				case SyntaxType.DMat2X3Keyword:
				case SyntaxType.DMat2X4Keyword:
				case SyntaxType.DMat3Keyword:
				case SyntaxType.DMat3X2Keyword:
				case SyntaxType.DMat3X3Keyword:
				case SyntaxType.DMat3X4Keyword:
				case SyntaxType.DMat4Keyword:
				case SyntaxType.DMat4X2Keyword:
				case SyntaxType.DMat4X3Keyword:
				case SyntaxType.DMat4X4Keyword:
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
				case SyntaxType.Image1DKeyword:
				case SyntaxType.Image2DKeyword:
				case SyntaxType.Image3DKeyword:
				case SyntaxType.IImage1DKeyword:
				case SyntaxType.IImage2DKeyword:
				case SyntaxType.IImage3DKeyword:
				case SyntaxType.UImage1DKeyword:
				case SyntaxType.UImage2DKeyword:
				case SyntaxType.UImage3DKeyword:
				case SyntaxType.Image2DRectKeyword:
				case SyntaxType.IImage2DRectKeyword:
				case SyntaxType.UImage2DRectKeyword:
				case SyntaxType.ImageCubeKeyword:
				case SyntaxType.IImageCubeKeyword:
				case SyntaxType.UImageCubeKeyword:
				case SyntaxType.ImageBufferKeyword:
				case SyntaxType.IImageBufferKeyword:
				case SyntaxType.UImageBufferKeyword:
				case SyntaxType.Image1DArrayKeyword:
				case SyntaxType.IImage1DArrayKeyword:
				case SyntaxType.UImage1DArrayKeyword:
				case SyntaxType.Image2DArrayKeyword:
				case SyntaxType.IImage2DArrayKeyword:
				case SyntaxType.UImage2DArrayKeyword:
				case SyntaxType.ImageCubeArrayKeyword:
				case SyntaxType.IImageCubeArrayKeyword:
				case SyntaxType.UImageCubeArrayKeyword:
				case SyntaxType.Image2DMSKeyword:
				case SyntaxType.IImage2DMSKeyword:
				case SyntaxType.UImage2DMSKeyword:
				case SyntaxType.Image2DMSArrayKeyword:
				case SyntaxType.IImage2DMSArrayKeyword:
				case SyntaxType.UImage2DMSArrayKeyword:
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
						this.builder.StartTestMode();

						this.ParseType();

						this.builder.EndTestMode();

						if (this.builder.CurrentToken.SyntaxType == SyntaxType.LeftParenToken)
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

		#endregion Helpers

		#region Token Helpers

		private bool AcceptTokenPreprocessor(SyntaxType type)
		{
			if (this.builder.CurrentToken?.SyntaxType == type || type == SyntaxType.Any)
			{
				this.builder.AddToken();

				return true;
			}

			return false;
		}

		private bool AcceptTokenPreprocessor(SyntaxType type, out SyntaxToken token)
		{
			if (this.builder.CurrentToken.SyntaxType == type || type == SyntaxType.Any)
			{
				token = this.builder.AddToken();

				return true;
			}

			token = null;

			return false;
		}

		private bool AcceptToken(SyntaxType type, out SyntaxToken token)
		{
			while (this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			bool result = false;

			if (this.builder.CurrentToken?.SyntaxType == type || type == SyntaxType.Any)
			{
				token = this.builder.AddToken();

				result = true;
			}
			else
			{
				token = null;
			}

			while (this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			return result;
		}

		private bool AcceptToken(SyntaxType type)
		{
			while (this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			bool result = false;

			if (this.builder.CurrentToken?.SyntaxType == type || type == SyntaxType.Any)
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

		private bool AcceptToken(params SyntaxType[] types)
		{
			while (this.IsPreprocessor())
			{
				this.ParsePreprocessor();
			}

			bool result = false;

			if (types.Contains(this.builder.CurrentToken.SyntaxType))
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

		private SyntaxNode PreprocessorRequireToken(SyntaxType type)
		{
			SyntaxToken result;

			if (!this.AcceptTokenPreprocessor(type, out result))
			{
				this.builder.Error(type);
			}

			return result;
		}

		private SyntaxToken RequireToken(SyntaxType type)
		{
			SyntaxToken token;

			if (!this.AcceptToken(type, out token))
			{
				this.builder.Error(type);
			}

			return token;
		}

		private void RequireToken(params SyntaxType[] types)
		{
			if (!this.AcceptToken(types))
			{
				this.builder.Error(types[0]);
			}
		}

		#endregion Token Helpers
	}
}