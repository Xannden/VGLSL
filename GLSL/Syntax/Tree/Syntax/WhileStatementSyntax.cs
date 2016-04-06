using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class WhileStatementSyntax : SyntaxNode
	{
		internal WhileStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.WhileStatement, start)
		{
		}

		internal WhileStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.WhileStatement, span)
		{
		}

		public ConditionSyntax Condition { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public StatementSyntax Statement { get; private set; }

		public SyntaxToken WhileKeyword { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.WhileKeyword:
					this.WhileKeyword = node as SyntaxToken;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.Condition:
					this.Condition = node as ConditionSyntax;
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