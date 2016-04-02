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

		public SyntaxTree GetTree(List<DefinePreprocessorSyntax> macroDefinitions)
		{
			this.tree.SetMacroDefinitions(macroDefinitions);

			return this.tree;
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
				this.tree.Root = node;
			}

			this.stack.Push(node);

			return node;
		}

		private SyntaxNode CreateNode(SyntaxType type, Span span, bool isMissing = false)
		{
			switch (type)
			{
				case SyntaxType.Program:
					return new ProgramSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Declaration:
					return new DeclarationSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InitDeclaratorListDeclaration:
					return new InitDeclaratorListDeclarationSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PrecisionDeclaration:
					return new PrecisionDeclarationSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.DeclarationList:
					return new DeclarationListSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ArraySpecifier:
					return new ArraySpecifierSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Type:
					return new TypeSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.TypeNonArray:
					return new TypeNonArraySyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.TypeName:
					return new TypeNameSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.TypeQualifier:
					return new TypeQualifierSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FunctionDefinition:
					return new FunctionDefinitionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Block:
					return new BlockSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FunctionHeader:
					return new FunctionHeaderSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Parameter:
					return new ParameterSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ReturnType:
					return new ReturnTypeSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Statement:
					return new StatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.SimpleStatement:
					return new SimpleStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.SelectionStatement:
					return new SelectionStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ElseStatement:
					return new ElseStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.SwitchStatement:
					return new SwitchStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.CaseLabel:
					return new CaseLabelSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IterationStatement:
					return new IterationStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.WhileStatement:
					return new WhileStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.DoWhileStatement:
					return new DoWhileStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ForStatement:
					return new ForStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.JumpStatement:
					return new JumpStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ExpressionStatement:
					return new ExpressionStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FunctionStatement:
					return new FunctionStatementSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Condition:
					return new ConditionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.StructDefinition:
					return new StructDefinitionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.StructSpecifier:
					return new StructSpecifierSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.StructDeclaration:
					return new StructDeclarationSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.StructDeclarator:
					return new StructDeclaratorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Expression:
					return new ExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ConstantExpression:
					return new ConstantExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.AssignmentExpression:
					return new AssignmentExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.AssignmentOperator:
					return new AssignmentOperatorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ConditionalExpression:
					return new ConditionalExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.LogicalOrExpression:
					return new LogicalOrExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.LogicalXOrExpression:
					return new LogicalXOrExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.LogicalAndExpression:
					return new LogicalAndExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InclusiveOrExpression:
					return new InclusiveOrExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ExclusiveOrExpression:
					return new ExclusiveOrExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.AndExpression:
					return new AndExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.EqualityExpression:
					return new EqualityExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.RelationalExpression:
					return new RelationalExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ShiftExpression:
					return new ShiftExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.AdditiveExpression:
					return new AdditiveExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.MultiplicativeExpression:
					return new MultiplicativeExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.UnaryExpression:
					return new UnaryExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PostFixExpression:
					return new PostfixExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PostFixExpressionStart:
					return new PostfixExpressionStartSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PostFixExpressionContinuation:
					return new PostfixExpressionContinuationSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PostFixArrayAccess:
					return new PostfixArrayAccessSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PrimaryExpression:
					return new PrimaryExpressionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FunctionCall:
					return new FunctionCallSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Constructor:
					return new ConstructorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.FieldSelection:
					return new FieldSelectionSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InitDeclaratorList:
					return new InitDeclaratorListSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InitPart:
					return new InitPartSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Initializer:
					return new InitializerSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.InitList:
					return new InitListSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.Preprocessor:
					return new PreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.DefinePreprocessor:
					return new DefinePreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.UndefinePreprocessor:
					return new UndefinePreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IfPreprocessor:
					return new IfPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IfDefinedPreprocessor:
					return new IfDefinedPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IfNotDefinedPreprocessor:
					return new IfNotDefinedPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ElsePreprocessor:
					return new ElsePreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ElseIfPreprocessor:
					return new ElseIfPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.EndIfPreprocessor:
					return new EndIfPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ErrorPreprocessor:
					return new ErrorPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.PragmaPreprocessor:
					return new PragmaPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ExtensionPreprocessor:
					return new ExtensionPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.VersionPreprocessor:
					return new VersionPreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.LinePreprocessor:
					return new LinePreprocessorSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.TokenString:
					return new TokenStringSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.MacroArguments:
					return new MacroArgumentsSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.ExcludedCode:
					return new ExcludedCodeSyntax(this.tree, this.CurrentToken.FullSpan(this.snapshot).Start);

				case SyntaxType.IdentifierToken:
					if (isMissing)
					{
						return new IdentifierSyntax(this.tree, this.snapshot.CreateTrackingSpan(span), string.Empty, null, null, this.snapshot, isMissing);
					}

					return new IdentifierSyntax(this.tree, this.snapshot.CreateTrackingSpan(span), this.CurrentToken.Text, this.CurrentToken.LeadingTrivia, this.CurrentToken.TrailingTrivia, this.snapshot, isMissing);

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