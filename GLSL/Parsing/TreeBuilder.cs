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
			SyntaxNode node = this.CreateNode(this.CurrentToken.SyntaxType, this.snapshot.CreateTrackingSpan(this.CurrentToken.Span));

			this.stack.Peek().AddChild(node);

			this.listNode = this.listNode.Next;

			if (node.SyntaxType == SyntaxType.IdentifierToken)
			{
				foreach (SyntaxNode ancestor in node.Ancestors)
				{
					if (ancestor.SyntaxType == SyntaxType.Preprocessor)
					{
						return;
					}
				}
			}
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
				node.Parent.InternalChildren.Remove(node);
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

			this.stack.Peek().AddChild(this.CreateNode(expected, this.snapshot.CreateTrackingSpan(this.CurrentToken.Span), true));
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
			SyntaxNode node = this.CreateNode(type, this.CurrentToken.FullSpan(this.snapshot).Start);

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

		private SyntaxNode CreateNode(SyntaxType type, int start)
		{
			switch (type)
			{
				case SyntaxType.Program:
					return new ProgramSyntax(this.tree, start);

				case SyntaxType.Declaration:
					return new DeclarationSyntax(this.tree, start);

				case SyntaxType.PrecisionDeclaration:
					return new PrecisionDeclarationSyntax(this.tree, start);

				case SyntaxType.DeclarationList:
					return new DeclarationListSyntax(this.tree, start);

				case SyntaxType.ArraySpecifier:
					return new ArraySpecifierSyntax(this.tree, start);

				case SyntaxType.Type:
					return new TypeSyntax(this.tree, start);

				case SyntaxType.TypeNonArray:
					return new TypeNonArraySyntax(this.tree, start);

				case SyntaxType.TypeName:
					return new TypeNameSyntax(this.tree, start);

				case SyntaxType.TypeQualifier:
					return new TypeQualifierSyntax(this.tree, start);

				case SyntaxType.FunctionDefinition:
					return new FunctionDefinitionSyntax(this.tree, start);

				case SyntaxType.Block:
					return new BlockSyntax(this.tree, start);

				case SyntaxType.FunctionHeader:
					return new FunctionHeaderSyntax(this.tree, start);

				case SyntaxType.Parameter:
					return new ParameterSyntax(this.tree, start);

				case SyntaxType.ReturnType:
					return new ReturnTypeSyntax(this.tree, start);

				case SyntaxType.Statement:
					return new StatementSyntax(this.tree, start);

				case SyntaxType.SimpleStatement:
					return new SimpleStatementSyntax(this.tree, start);

				case SyntaxType.SelectionStatement:
					return new SelectionStatementSyntax(this.tree, start);

				case SyntaxType.ElseStatement:
					return new ElseStatementSyntax(this.tree, start);

				case SyntaxType.SwitchStatement:
					return new SwitchStatementSyntax(this.tree, start);

				case SyntaxType.CaseLabel:
					return new CaseLabelSyntax(this.tree, start);

				case SyntaxType.IterationStatement:
					return new IterationStatementSyntax(this.tree, start);

				case SyntaxType.WhileStatement:
					return new WhileStatementSyntax(this.tree, start);

				case SyntaxType.DoWhileStatement:
					return new DoWhileStatementSyntax(this.tree, start);

				case SyntaxType.ForStatement:
					return new ForStatementSyntax(this.tree, start);

				case SyntaxType.JumpStatement:
					return new JumpStatementSyntax(this.tree, start);

				case SyntaxType.ExpressionStatement:
					return new ExpressionStatementSyntax(this.tree, start);

				case SyntaxType.FunctionStatement:
					return new FunctionStatementSyntax(this.tree, start);

				case SyntaxType.Condition:
					return new ConditionSyntax(this.tree, start);

				case SyntaxType.StructDefinition:
					return new StructDefinitionSyntax(this.tree, start);

				case SyntaxType.StructSpecifier:
					return new StructSpecifierSyntax(this.tree, start);

				case SyntaxType.StructDeclaration:
					return new StructDeclarationSyntax(this.tree, start);

				case SyntaxType.StructDeclarator:
					return new StructDeclaratorSyntax(this.tree, start);

				case SyntaxType.Expression:
					return new ExpressionSyntax(this.tree, start);

				case SyntaxType.ConstantExpression:
					return new ConstantExpressionSyntax(this.tree, start);

				case SyntaxType.AssignmentExpression:
					return new AssignmentExpressionSyntax(this.tree, start);

				case SyntaxType.AssignmentOperator:
					return new AssignmentOperatorSyntax(this.tree, start);

				case SyntaxType.ConditionalExpression:
					return new ConditionalExpressionSyntax(this.tree, start);

				case SyntaxType.LogicalOrExpression:
					return new LogicalOrExpressionSyntax(this.tree, start);

				case SyntaxType.LogicalXorExpression:
					return new LogicalXorExpressionSyntax(this.tree, start);

				case SyntaxType.LogicalAndExpression:
					return new LogicalAndExpressionSyntax(this.tree, start);

				case SyntaxType.InclusiveOrExpression:
					return new InclusiveOrExpressionSyntax(this.tree, start);

				case SyntaxType.ExclusiveOrExpression:
					return new ExclusiveOrExpressionSyntax(this.tree, start);

				case SyntaxType.AndExpression:
					return new AndExpressionSyntax(this.tree, start);

				case SyntaxType.EqualityExpression:
					return new EqualityExpressionSyntax(this.tree, start);

				case SyntaxType.RelationalExpression:
					return new RelationalExpressionSyntax(this.tree, start);

				case SyntaxType.ShiftExpression:
					return new ShiftExpressionSyntax(this.tree, start);

				case SyntaxType.AdditiveExpression:
					return new AdditiveExpressionSyntax(this.tree, start);

				case SyntaxType.MultiplicativeExpression:
					return new MultiplicativeExpressionSyntax(this.tree, start);

				case SyntaxType.UnaryExpression:
					return new UnaryExpressionSyntax(this.tree, start);

				case SyntaxType.PostfixExpression:
					return new PostfixExpressionSyntax(this.tree, start);

				case SyntaxType.PostfixExpressionStart:
					return new PostfixExpressionStartSyntax(this.tree, start);

				case SyntaxType.PostfixExpressionContinuation:
					return new PostfixExpressionContinuationSyntax(this.tree, start);

				case SyntaxType.PostfixArrayAccess:
					return new PostfixArrayAccessSyntax(this.tree, start);

				case SyntaxType.PrimaryExpression:
					return new PrimaryExpressionSyntax(this.tree, start);

				case SyntaxType.FunctionCall:
					return new FunctionCallSyntax(this.tree, start);

				case SyntaxType.Constructor:
					return new ConstructorSyntax(this.tree, start);

				case SyntaxType.FieldSelection:
					return new FieldSelectionSyntax(this.tree, start);

				case SyntaxType.InitDeclaratorList:
					return new InitDeclaratorListSyntax(this.tree, start);

				case SyntaxType.InitPart:
					return new InitPartSyntax(this.tree, start);

				case SyntaxType.Initializer:
					return new InitializerSyntax(this.tree, start);

				case SyntaxType.InitList:
					return new InitListSyntax(this.tree, start);

				case SyntaxType.Preprocessor:
					return new PreprocessorSyntax(this.tree, start);

				case SyntaxType.DefinePreprocessor:
					return new DefinePreprocessorSyntax(this.tree, start);

				case SyntaxType.UndefinePreprocessor:
					return new UndefinePreprocessorSyntax(this.tree, start);

				case SyntaxType.IfPreprocessor:
					return new IfPreprocessorSyntax(this.tree, start);

				case SyntaxType.IfDefinedPreprocessor:
					return new IfDefinedPreprocessorSyntax(this.tree, start);

				case SyntaxType.IfNotDefinedPreprocessor:
					return new IfNotDefinedPreprocessorSyntax(this.tree, start);

				case SyntaxType.ElsePreprocessor:
					return new ElsePreprocessorSyntax(this.tree, start);

				case SyntaxType.ElseIfPreprocessor:
					return new ElseIfPreprocessorSyntax(this.tree, start);

				case SyntaxType.EndIfPreprocessor:
					return new EndIfPreprocessorSyntax(this.tree, start);

				case SyntaxType.ErrorPreprocessor:
					return new ErrorPreprocessorSyntax(this.tree, start);

				case SyntaxType.PragmaPreprocessor:
					return new PragmaPreprocessorSyntax(this.tree, start);

				case SyntaxType.ExtensionPreprocessor:
					return new ExtensionPreprocessorSyntax(this.tree, start);

				case SyntaxType.VersionPreprocessor:
					return new VersionPreprocessorSyntax(this.tree, start);

				case SyntaxType.LinePreprocessor:
					return new LinePreprocessorSyntax(this.tree, start);

				case SyntaxType.TokenString:
					return new TokenStringSyntax(this.tree, start);

				case SyntaxType.MacroArguments:
					return new MacroArgumentsSyntax(this.tree, start);

				case SyntaxType.ExcludedCode:
					return new ExcludedCodeSyntax(this.tree, start);
				default:
					return new SyntaxNode(this.tree, type, start);
			}
		}

		private SyntaxNode CreateNode(SyntaxType type, TrackingSpan span, bool isMissing = false)
		{
			switch (type)
			{
				case SyntaxType.Program:
					return new ProgramSyntax(this.tree, span);

				case SyntaxType.Declaration:
					return new DeclarationSyntax(this.tree, span);

				case SyntaxType.PrecisionDeclaration:
					return new PrecisionDeclarationSyntax(this.tree, span);

				case SyntaxType.DeclarationList:
					return new DeclarationListSyntax(this.tree, span);

				case SyntaxType.ArraySpecifier:
					return new ArraySpecifierSyntax(this.tree, span);

				case SyntaxType.Type:
					return new TypeSyntax(this.tree, span);

				case SyntaxType.TypeNonArray:
					return new TypeNonArraySyntax(this.tree, span);

				case SyntaxType.TypeName:
					return new TypeNameSyntax(this.tree, span);

				case SyntaxType.TypeQualifier:
					return new TypeQualifierSyntax(this.tree, span);

				case SyntaxType.FunctionDefinition:
					return new FunctionDefinitionSyntax(this.tree, span);

				case SyntaxType.Block:
					return new BlockSyntax(this.tree, span);

				case SyntaxType.FunctionHeader:
					return new FunctionHeaderSyntax(this.tree, span);

				case SyntaxType.Parameter:
					return new ParameterSyntax(this.tree, span);

				case SyntaxType.ReturnType:
					return new ReturnTypeSyntax(this.tree, span);

				case SyntaxType.Statement:
					return new StatementSyntax(this.tree, span);

				case SyntaxType.SimpleStatement:
					return new SimpleStatementSyntax(this.tree, span);

				case SyntaxType.SelectionStatement:
					return new SelectionStatementSyntax(this.tree, span);

				case SyntaxType.ElseStatement:
					return new ElseStatementSyntax(this.tree, span);

				case SyntaxType.SwitchStatement:
					return new SwitchStatementSyntax(this.tree, span);

				case SyntaxType.CaseLabel:
					return new CaseLabelSyntax(this.tree, span);

				case SyntaxType.IterationStatement:
					return new IterationStatementSyntax(this.tree, span);

				case SyntaxType.WhileStatement:
					return new WhileStatementSyntax(this.tree, span);

				case SyntaxType.DoWhileStatement:
					return new DoWhileStatementSyntax(this.tree, span);

				case SyntaxType.ForStatement:
					return new ForStatementSyntax(this.tree, span);

				case SyntaxType.JumpStatement:
					return new JumpStatementSyntax(this.tree, span);

				case SyntaxType.ExpressionStatement:
					return new ExpressionStatementSyntax(this.tree, span);

				case SyntaxType.FunctionStatement:
					return new FunctionStatementSyntax(this.tree, span);

				case SyntaxType.Condition:
					return new ConditionSyntax(this.tree, span);

				case SyntaxType.StructDefinition:
					return new StructDefinitionSyntax(this.tree, span);

				case SyntaxType.StructSpecifier:
					return new StructSpecifierSyntax(this.tree, span);

				case SyntaxType.StructDeclaration:
					return new StructDeclarationSyntax(this.tree, span);

				case SyntaxType.StructDeclarator:
					return new StructDeclaratorSyntax(this.tree, span);

				case SyntaxType.Expression:
					return new ExpressionSyntax(this.tree, span);

				case SyntaxType.ConstantExpression:
					return new ConstantExpressionSyntax(this.tree, span);

				case SyntaxType.AssignmentExpression:
					return new AssignmentExpressionSyntax(this.tree, span);

				case SyntaxType.AssignmentOperator:
					return new AssignmentOperatorSyntax(this.tree, span);

				case SyntaxType.ConditionalExpression:
					return new ConditionalExpressionSyntax(this.tree, span);

				case SyntaxType.LogicalOrExpression:
					return new LogicalOrExpressionSyntax(this.tree, span);

				case SyntaxType.LogicalXorExpression:
					return new LogicalXorExpressionSyntax(this.tree, span);

				case SyntaxType.LogicalAndExpression:
					return new LogicalAndExpressionSyntax(this.tree, span);

				case SyntaxType.InclusiveOrExpression:
					return new InclusiveOrExpressionSyntax(this.tree, span);

				case SyntaxType.ExclusiveOrExpression:
					return new ExclusiveOrExpressionSyntax(this.tree, span);

				case SyntaxType.AndExpression:
					return new AndExpressionSyntax(this.tree, span);

				case SyntaxType.EqualityExpression:
					return new EqualityExpressionSyntax(this.tree, span);

				case SyntaxType.RelationalExpression:
					return new RelationalExpressionSyntax(this.tree, span);

				case SyntaxType.ShiftExpression:
					return new ShiftExpressionSyntax(this.tree, span);

				case SyntaxType.AdditiveExpression:
					return new AdditiveExpressionSyntax(this.tree, span);

				case SyntaxType.MultiplicativeExpression:
					return new MultiplicativeExpressionSyntax(this.tree, span);

				case SyntaxType.UnaryExpression:
					return new UnaryExpressionSyntax(this.tree, span);

				case SyntaxType.PostfixExpression:
					return new PostfixExpressionSyntax(this.tree, span);

				case SyntaxType.PostfixExpressionStart:
					return new PostfixExpressionStartSyntax(this.tree, span);

				case SyntaxType.PostfixExpressionContinuation:
					return new PostfixExpressionContinuationSyntax(this.tree, span);

				case SyntaxType.PostfixArrayAccess:
					return new PostfixArrayAccessSyntax(this.tree, span);

				case SyntaxType.PrimaryExpression:
					return new PrimaryExpressionSyntax(this.tree, span);

				case SyntaxType.FunctionCall:
					return new FunctionCallSyntax(this.tree, span);

				case SyntaxType.Constructor:
					return new ConstructorSyntax(this.tree, span);

				case SyntaxType.FieldSelection:
					return new FieldSelectionSyntax(this.tree, span);

				case SyntaxType.InitDeclaratorList:
					return new InitDeclaratorListSyntax(this.tree, span);

				case SyntaxType.InitPart:
					return new InitPartSyntax(this.tree, span);

				case SyntaxType.Initializer:
					return new InitializerSyntax(this.tree, span);

				case SyntaxType.InitList:
					return new InitListSyntax(this.tree, span);

				case SyntaxType.Preprocessor:
					return new PreprocessorSyntax(this.tree, span);

				case SyntaxType.DefinePreprocessor:
					return new DefinePreprocessorSyntax(this.tree, span);

				case SyntaxType.UndefinePreprocessor:
					return new UndefinePreprocessorSyntax(this.tree, span);

				case SyntaxType.IfPreprocessor:
					return new IfPreprocessorSyntax(this.tree, span);

				case SyntaxType.IfDefinedPreprocessor:
					return new IfDefinedPreprocessorSyntax(this.tree, span);

				case SyntaxType.IfNotDefinedPreprocessor:
					return new IfNotDefinedPreprocessorSyntax(this.tree, span);

				case SyntaxType.ElsePreprocessor:
					return new ElsePreprocessorSyntax(this.tree, span);

				case SyntaxType.ElseIfPreprocessor:
					return new ElseIfPreprocessorSyntax(this.tree, span);

				case SyntaxType.EndIfPreprocessor:
					return new EndIfPreprocessorSyntax(this.tree, span);

				case SyntaxType.ErrorPreprocessor:
					return new ErrorPreprocessorSyntax(this.tree, span);

				case SyntaxType.PragmaPreprocessor:
					return new PragmaPreprocessorSyntax(this.tree, span);

				case SyntaxType.ExtensionPreprocessor:
					return new ExtensionPreprocessorSyntax(this.tree, span);

				case SyntaxType.VersionPreprocessor:
					return new VersionPreprocessorSyntax(this.tree, span);

				case SyntaxType.LinePreprocessor:
					return new LinePreprocessorSyntax(this.tree, span);

				case SyntaxType.TokenString:
					return new TokenStringSyntax(this.tree, span);

				case SyntaxType.MacroArguments:
					return new MacroArgumentsSyntax(this.tree, span);

				case SyntaxType.ExcludedCode:
					return new ExcludedCodeSyntax(this.tree, span);

				case SyntaxType.IdentifierToken:
					if (isMissing)
					{
						return new IdentifierSyntax(this.tree, span, string.Empty, null, null, this.snapshot, isMissing);
					}

					return new IdentifierSyntax(this.tree, span, this.CurrentToken.Text, this.CurrentToken.LeadingTrivia, this.CurrentToken.TrailingTrivia, this.snapshot, isMissing);

				default:
					if ((type >= SyntaxType.LeftParenToken && type <= SyntaxType.PreprocessorToken) || type == SyntaxType.EOF)
					{
						if (isMissing)
						{
							return new SyntaxToken(this.tree, type, span, string.Empty, null, null, this.snapshot, isMissing);
						}

						return new SyntaxToken(this.tree, type, span, this.CurrentToken.Text, this.CurrentToken.LeadingTrivia, this.CurrentToken.TrailingTrivia, this.snapshot);
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
				case SyntaxType.LogicalXorExpression:
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
				case SyntaxType.PostfixExpression:
				case SyntaxType.Expression:
					return true;

				default:
					return false;
			}
		}
	}
}