using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class SelectionStatementSyntax : SyntaxNode
	{
		internal SelectionStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.SelectionStatement, start)
		{
		}

		internal SelectionStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.SelectionStatement, span)
		{
		}

		public SyntaxNode ElseStatement { get; private set; }

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken IfKeyword { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public StatementSyntax Statement { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IfKeyword:
					this.IfKeyword = node as SyntaxToken;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
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

				case SyntaxType.ElseStatement:
					this.ElseStatement = node as ElseStatementSyntax;
					break;
			}
		}
	}
}