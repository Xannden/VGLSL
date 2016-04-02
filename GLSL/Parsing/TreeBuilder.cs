using System.Collections.Generic;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Parsing
{
	internal sealed class TreeBuilder
	{
		private ErrorHandler errorHandler;
		private LinkedListNode<Token> listNode;
		private SyntaxNode root;
		private Snapshot snapshot;
		private Stack<SyntaxNode> stack = new Stack<SyntaxNode>();
		private LinkedList<Token> tokens;
		private SyntaxTree tree = new SyntaxTree();

		public TreeBuilder(Snapshot snapshot, LinkedList<Token> tokens, ErrorHandler errorHandler)
		{
			this.snapshot = snapshot;
			this.tokens = tokens;
			this.errorHandler = errorHandler;

			this.listNode = this.tokens.First;
		}

		public Token CurrentToken => this.listNode?.Value;

		public Token NextToken => this.listNode?.Next?.Value;

		public void AddToken()
		{
			this.stack.Peek().AddChild(this.CreateNode(this.CurrentToken.Type, this.CurrentToken.Span));

			this.listNode = this.listNode.Next;
		}

		public SyntaxNode EndNode()
		{
			int end = this.listNode?.Previous?.Value.FullSpan(this.snapshot).End ?? this.tokens.Last.Value.FullSpan(this.snapshot).End;

			if (end < this.stack.Peek().TempStart)
			{
				this.stack.Peek().SetEnd(this.snapshot, this.stack.Peek().TempStart);
			}
			else
			{
				this.stack.Peek().SetEnd(this.snapshot, end);
			}

			SyntaxNode node = this.stack.Peek();

			if (node.Children.Count == 1 && this.IsExpressionNode(node.SyntaxType))
			{
				node.Parent.Children.Remove(node);
				node.Parent.AddChild(node.Children[0]);
			}

			return this.stack.Pop();
		}

		public void Error(SyntaxType expected)
		{
			Span span;

			if (this.stack.Count > 0)
			{
				span = Span.Create(this.stack.Peek().TempStart);
			}
			else
			{
				span = this.CurrentToken.Span;
			}

			this.errorHandler.AddError($"{expected.ToString().Replace("Token", string.Empty).Replace("Keyword", string.Empty)} expected", span);

			this.stack.Peek().AddChild(this.CreateNode(expected, this.CurrentToken.Span, true));
		}

		public void Error(string message)
		{
			this.errorHandler.AddError(message, this.CurrentToken.Span);
		}

		public ResetPoint GetResetPoint()
		{
			return new ResetPoint(this.listNode);
		}

		public SyntaxTree GetTree()
		{
			return new SyntaxTree(this.root);
		}

		public bool IsTypeName(Token token)
		{
			foreach (SyntaxNode ancestor in this.stack.Peek().AncestorsAndSelf)
			{
				if (ancestor.SyntaxType == SyntaxType.TypeName)
				{
					return true;
				}

				foreach (SyntaxNode sibling in ancestor.Siblings)
				{
					StructDefinitionSyntax structDefinition = (sibling as SimpleStatementSyntax)?.Declaration?.StructDefinition;

					if (structDefinition?.TypeName?.Identifier.Name == token.Text)
					{
						return true;
					}
				}
			}

			return false;
		}

		public void MoveNext()
		{
			this.listNode = this.listNode?.Next;
		}

		public void Reset(ResetPoint point)
		{
			this.listNode = point.Node;
		}

		public SyntaxNode StartNode(SyntaxType type)
		{
			SyntaxNode node = this.CreateNode(type, null);

			if (this.stack.Count != 0)
			{
				this.stack.Peek().AddChild(node);
			}
			else
			{
				this.root = node;
			}

			this.stack.Push(node);

			return node;
		}

		private SyntaxNode CreateNode(SyntaxType type, Span span, bool isMissing = false)
		{
			switch (type)
			{
				case SyntaxType.Program:
					return SyntaxNode.Create<ProgramSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Declaration:
					return SyntaxNode.Create<DeclarationSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InitDeclaratorListDeclaration:
					return SyntaxNode.Create<InitDeclaratorListDeclarationSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PrecisionDeclaration:
					return SyntaxNode.Create<PrecisionDeclarationSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.DeclarationList:
					return SyntaxNode.Create<DeclarationListSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ArraySpecifier:
					return SyntaxNode.Create<ArraySpecifierSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Type:
					return SyntaxNode.Create<TypeSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.TypeNonArray:
					return SyntaxNode.Create<TypeNonArraySyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.TypeName:
					return SyntaxNode.Create<TypeNameSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.TypeQualifier:
					return SyntaxNode.Create<TypeQualifierSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FunctionDefinition:
					return SyntaxNode.Create<FunctionDefinitionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Block:
					return SyntaxNode.Create<BlockSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FunctionHeader:
					return SyntaxNode.Create<FunctionHeaderSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Parameter:
					return SyntaxNode.Create<ParameterSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ReturnType:
					return SyntaxNode.Create<ReturnTypeSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Statement:
					return SyntaxNode.Create<StatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.SimpleStatement:
					return SyntaxNode.Create<SimpleStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.SelectionStatement:
					return SyntaxNode.Create<SelectionStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ElseStatement:
					return SyntaxNode.Create<ElseStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.SwitchStatement:
					return SyntaxNode.Create<SwitchStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.CaseLabel:
					return SyntaxNode.Create<CaseLabelSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IterationStatement:
					return SyntaxNode.Create<IterationStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.WhileStatement:
					return SyntaxNode.Create<WhileStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.DoWhileStatement:
					return SyntaxNode.Create<DoWhileStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ForStatement:
					return SyntaxNode.Create<ForStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.JumpStatement:
					return SyntaxNode.Create<JumpStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ExpressionStatement:
					return SyntaxNode.Create<ExpressionStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FunctionStatement:
					return SyntaxNode.Create<FunctionStatementSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Condition:
					return SyntaxNode.Create<ConditionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.StructDefinition:
					return SyntaxNode.Create<StructDefinitionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.StructSpecifier:
					return SyntaxNode.Create<StructSpecifierSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.StructDeclaration:
					return SyntaxNode.Create<StructDeclarationSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.StructDeclarator:
					return SyntaxNode.Create<StructDeclaratorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Expression:
					return SyntaxNode.Create<ExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ConstantExpression:
					return SyntaxNode.Create<ConstantExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.AssignmentExpression:
					return SyntaxNode.Create<AssignmentExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.AssignmentOperator:
					return SyntaxNode.Create<AssignmentOperatorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ConditionalExpression:
					return SyntaxNode.Create<ConditionalExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.LogicalOrExpression:
					return SyntaxNode.Create<LogicalOrExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.LogicalXOrExpression:
					return SyntaxNode.Create<LogicalXOrExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.LogicalAndExpression:
					return SyntaxNode.Create<LogicalAndExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InclusiveOrExpression:
					return SyntaxNode.Create<InclusiveOrExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ExclusiveOrExpression:
					return SyntaxNode.Create<ExclusiveOrExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.AndExpression:
					return SyntaxNode.Create<AndExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.EqualityExpression:
					return SyntaxNode.Create<EqualityExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.RelationalExpression:
					return SyntaxNode.Create<RelationalExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ShiftExpression:
					return SyntaxNode.Create<ShiftExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.AdditiveExpression:
					return SyntaxNode.Create<AdditiveExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.MultiplicativeExpression:
					return SyntaxNode.Create<MultiplicativeExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.UnaryExpression:
					return SyntaxNode.Create<UnaryExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PostFixExpression:
					return SyntaxNode.Create<PostfixExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PostFixExpressionStart:
					return SyntaxNode.Create<PostfixExpressionStartSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PostFixExpressionContinuation:
					return SyntaxNode.Create<PostfixExpressionContinuationSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PostFixArrayAccess:
					return SyntaxNode.Create<PostfixArrayAccessSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PrimaryExpression:
					return SyntaxNode.Create<PrimaryExpressionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FunctionCall:
					return SyntaxNode.Create<FunctionCallSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Constructor:
					return SyntaxNode.Create<ConstructorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FieldSelection:
					return SyntaxNode.Create<FieldSelectionSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InitDeclaratorList:
					return SyntaxNode.Create<InitDeclaratorListSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InitPart:
					return SyntaxNode.Create<InitPartSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Initializer:
					return SyntaxNode.Create<InitializerSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InitList:
					return SyntaxNode.Create<InitListSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Preprocessor:
					return SyntaxNode.Create<PreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.DefinePreprocessor:
					return SyntaxNode.Create<DefinePreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.UndefinePreprocessor:
					return SyntaxNode.Create<UndefinePreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IfPreprocessor:
					return SyntaxNode.Create<IfPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IfDefinedPreprocessor:
					return SyntaxNode.Create<IfDefinedPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IfNotDefinedPreprocessor:
					return SyntaxNode.Create<IfNotDefinedPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ElsePreprocessor:
					return SyntaxNode.Create<ElsePreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ElseIfPreprocessor:
					return SyntaxNode.Create<ElseIfPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.EndIfPreprocessor:
					return SyntaxNode.Create<EndIfPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ErrorPreprocessor:
					return SyntaxNode.Create<ErrorPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PragmaPreprocessor:
					return SyntaxNode.Create<PragmaPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ExtensionPreprocessor:
					return SyntaxNode.Create<ExtensionPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.VersionPreprocessor:
					return SyntaxNode.Create<VersionPreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.LinePreprocessor:
					return SyntaxNode.Create<LinePreprocessorSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.TokenString:
					return SyntaxNode.Create<TokenStringSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.MacroArguments:
					return SyntaxNode.Create<MacroArgumentsSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ExcludedCode:
					return SyntaxNode.Create<ExcludedCodeSyntax>(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IdentifierToken:
					if (isMissing)
					{
						return SyntaxToken.Create<IdentifierSyntax>(this.tree, this.snapshot.CreateTrackingSpan(span), string.Empty, null, null, this.snapshot, isMissing);
					}

					return SyntaxToken.Create<IdentifierSyntax>(this.tree, this.snapshot.CreateTrackingSpan(span), this.CurrentToken.Text, this.CurrentToken.LeadingTrivia, this.CurrentToken.TrailingTrivia, this.snapshot, isMissing);

				default:
					if ((type >= SyntaxType.LeftParenToken && type <= SyntaxType.PreprocessorToken) || type == SyntaxType.EOF)
					{
						if (isMissing)
						{
							return new SyntaxToken(this.tree, type, this.snapshot.CreateTrackingSpan(span), string.Empty, null, null, this.snapshot, isMissing);
						}

						return new SyntaxToken(this.tree, type, this.snapshot.CreateTrackingSpan(span), this.CurrentToken.Text, this.CurrentToken.LeadingTrivia, this.CurrentToken.TrailingTrivia, this.snapshot);
					}
					else
					{
						return new SyntaxNode(this.tree, type, this.CurrentToken.FullSpan(this.snapshot).Start);
					}
			}
		}

		private bool IsExpressionNode(SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.AssignmentExpression:
				case SyntaxType.ConditionalExpression:
				case SyntaxType.LogicalOrExpression:
				case SyntaxType.LogicalXOrExpression:
				case SyntaxType.LogicalAndExpression:
				case SyntaxType.InclusiveOrExpression:
				case SyntaxType.ExclusiveOrExpression:
				case SyntaxType.AndExpression:
				case SyntaxType.EqualityExpression:
				case SyntaxType.RelationalExpression:
				case SyntaxType.ShiftExpression:
				case SyntaxType.AdditiveExpression:
				case SyntaxType.MultiplicativeExpression:
				case SyntaxType.UnaryExpression:
				case SyntaxType.PostFixExpression:
				case SyntaxType.Expression:
					return true;

				default:
					return false;
			}
		}
	}
}