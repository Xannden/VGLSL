using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ForStatementSyntax : SyntaxNode
	{
		internal ForStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ForStatement, start)
		{
		}

		internal ForStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ForStatement, span)
		{
		}

		public ConditionSyntax Condition { get; private set; }

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxNode ForInitStatement { get; private set; }

		public SyntaxToken ForKeyword { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public SyntaxToken Semicolon { get; private set; }

		public StatementSyntax Statement { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ForKeyword:
					this.ForKeyword = node as SyntaxToken;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.ExpressionStatement:
				case SyntaxType.Declaration:
				case SyntaxType.FunctionStatement:
					this.ForInitStatement = node;
					break;

				case SyntaxType.Condition:
					this.Condition = node as ConditionSyntax;
					break;

				case SyntaxType.SemicolonToken:
					this.Semicolon = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;

				case SyntaxType.Statement:
					this.Statement = node as StatementSyntax;
					break;
			}
		}
	}
}