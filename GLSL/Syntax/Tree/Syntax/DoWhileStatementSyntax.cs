using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class DoWhileStatementSyntax : SyntaxNode
	{
		internal DoWhileStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.DoWhileStatement, start)
		{
		}

		internal DoWhileStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.DoWhileStatement, span)
		{
		}

		public SyntaxToken DoKeyword { get; private set; }

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		public StatementSyntax Statement { get; private set; }

		public SyntaxToken WhileKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.DoKeyword:
					this.DoKeyword = node as SyntaxToken;
					break;

				case SyntaxType.Statement:
					this.Statement = node as StatementSyntax;
					break;

				case SyntaxType.WhileKeyword:
					this.WhileKeyword = node as SyntaxToken;
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

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}