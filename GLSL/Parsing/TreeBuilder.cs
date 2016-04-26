using System.Collections.Generic;
using Xannden.GLSL.BuiltIn;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
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
		private Stack<Scope> scope = new Stack<Scope>();
		private List<Definition> definitions = new List<Definition>();
		private int testModeLayer = 0;

		public TreeBuilder(Snapshot snapshot, LinkedList<Token> tokens, ErrorHandler errorHandler)
		{
			this.snapshot = snapshot;
			this.tokens = tokens;
			this.errorHandler = errorHandler;

			this.listNode = this.tokens.First;
		}

		public Token CurrentToken => this.listNode?.Value;

		public SyntaxToken AddToken()
		{
			if (this.testModeLayer <= 0)
			{
				SyntaxToken node = this.CreateToken(this.CurrentToken.SyntaxType, this.snapshot.CreateTrackingSpan(this.CurrentToken.Span));

				this.stack.Peek().AddChild(node);

				this.listNode = this.listNode.Next;

				return node;
			}
			else
			{
				this.listNode = this.listNode.Next;
				return null;
			}
		}

		public SyntaxNode StartNode(SyntaxType type)
		{
			if (this.testModeLayer <= 0)
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
			else
			{
				return null;
			}
		}

		public SyntaxNode EndNode()
		{
			if (this.testModeLayer <= 0)
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
			else
			{
				return null;
			}
		}

		public void StartScope()
		{
			if (this.testModeLayer <= 0)
			{
				this.scope.Push(new Scope(this.snapshot.CreateTrackingPoint(this.CurrentToken.Span.Start), this.snapshot.CreateTrackingPoint(this.snapshot.Length)));
			}
		}

		public void EndScope()
		{
			if (this.testModeLayer <= 0)
			{
				this.scope.Peek().End.SetPosition(this.snapshot, this.listNode?.Previous.Value.Span.End ?? this.tokens.Last.Value.Span.End);
				this.scope.Pop();
			}
		}

		public void Error(SyntaxType expected)
		{
			if (this.testModeLayer <= 0)
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

				this.errorHandler.AddError($"{expected.GetText()} expected", span);

				SyntaxNode node = new SyntaxNode(this.tree, expected, this.snapshot.CreateTrackingSpan(this.CurrentToken.Span));

				node.IsMissing = true;

				this.stack.Peek().AddChild(node);
			}
		}

		public void Error(string message)
		{
			if (this.testModeLayer <= 0)
			{
				this.errorHandler.AddError(message, this.CurrentToken.Span);
			}
		}

		public SyntaxTree GetTree()
		{
			this.tree.Definitions = this.definitions;

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
					InterfaceBlockSyntax interfaceBlock = (sibling as SimpleStatementSyntax)?.Declaration?.InterfaceBlock;

					if (interfaceBlock?.Identifier?.Identifier == token.Text)
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

		public Definition AddDefinition(SyntaxNode node, IdentifierSyntax identifier, DefinitionKind type)
		{
			if (this.testModeLayer > 0)
			{
				return null;
			}

			if (identifier == null || node == null)
			{
				return null;
			}

			Scope definitionScope = new Scope(this.snapshot.CreateTrackingPoint(identifier.Span.GetSpan(this.snapshot).Start), this.scope.Peek().End);
			Definition definition = null;

			switch (type)
			{
				case DefinitionKind.Function:
					definition = new FunctionDefinition(node as FunctionHeaderSyntax, definitionScope, identifier, string.Empty);
					break;
				case DefinitionKind.Parameter:
					definition = new ParameterDefinition(node as ParameterSyntax, definitionScope, identifier, string.Empty);
					(this.definitions.FindLast(def => def.Kind == DefinitionKind.Function) as FunctionDefinition)?.AddParameter(definition as ParameterDefinition);
					break;
				case DefinitionKind.Field:
					definition = new FieldDefinition(node as StructDeclaratorSyntax, definitionScope, identifier, string.Empty);
					break;
				case DefinitionKind.GlobalVariable:
				case DefinitionKind.LocalVariable:
					definition = new VariableDefinition(node, definitionScope, identifier, string.Empty, type);
					break;
				case DefinitionKind.Macro:
					definition = new MacroDefinition(node as DefinePreprocessorSyntax, definitionScope, identifier, string.Empty);
					break;
				case DefinitionKind.TypeName:
					definition = new TypeNameDefinition(definitionScope, identifier, string.Empty);
					break;
				case DefinitionKind.InterfaceBlock:
					definition = new InterfaceBlockDefinition(node as InterfaceBlockSyntax, definitionScope, identifier, string.Empty);
					break;
			}

			this.definitions.Add(definition);

			return definition;
		}

		public Definition FindDefinition(IdentifierSyntax identifier)
		{
			if (this.testModeLayer > 0)
			{
				return null;
			}

			Definition definition = this.definitions.FindLast(def => def.Scope.Contains(this.snapshot, identifier.Span) && def.Name == identifier.Identifier);

			if (definition == null)
			{
				definition = BuiltInData.Instance.Definitions.Find(def => def.Name == identifier.Identifier);
			}

			return definition;
		}

		public ResetPoint GetResetPoint()
		{
			return new ResetPoint(this.listNode);
		}

		public void Reset(ResetPoint point)
		{
			this.listNode = point.Node;
		}

		public void StartTestMode()
		{
			this.testModeLayer++;
		}

		public void EndTestMode()
		{
			this.testModeLayer--;
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

				case SyntaxType.SingleTypeQualifier:
					return new SingleTypeQualifierSyntax(this.tree, start);

				case SyntaxType.StorageQualifier:
					return new StorageQualifierSyntax(this.tree, start);

				case SyntaxType.LayoutQualifier:
					return new LayoutQualifierSyntax(this.tree, start);

				case SyntaxType.LayoutQualifierId:
					return new LayoutQualifierIdSyntax(this.tree, start);

				case SyntaxType.PrecisionQualifier:
					return new PrecisionQualifierSyntax(this.tree, start);

				case SyntaxType.InterpolationQualifier:
					return new InterpolationQualifierSyntax(this.tree, start);

				case SyntaxType.InvariantQualifier:
					return new InvariantQualifierSyntax(this.tree, start);

				case SyntaxType.PreciseQualifier:
					return new PreciseQualifierSyntax(this.tree, start);

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

				case SyntaxType.Condition:
					return new ConditionSyntax(this.tree, start);

				case SyntaxType.InterfaceBlock:
					return new InterfaceBlockSyntax(this.tree, start);

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

		private SyntaxToken CreateToken(SyntaxType type, TrackingSpan span, bool isMissing = false)
		{
			switch (type)
			{
				case SyntaxType.IdentifierToken:
					if (isMissing)
					{
						return new IdentifierSyntax(this.tree, span, string.Empty, null, null, this.snapshot, isMissing);
					}

					return new IdentifierSyntax(this.tree, span, this.CurrentToken.Text, this.CurrentToken.LeadingTrivia, this.CurrentToken.TrailingTrivia, this.snapshot, isMissing);

				default:
					if (isMissing)
					{
						return new SyntaxToken(this.tree, type, span, string.Empty, null, null, this.snapshot, isMissing);
					}

					return new SyntaxToken(this.tree, type, span, this.CurrentToken.Text, this.CurrentToken.LeadingTrivia, this.CurrentToken.TrailingTrivia, this.snapshot);
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