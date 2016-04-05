using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class JumpStatementSyntax : SyntaxNode
	{
		internal JumpStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.JumpStatement, start)
		{
		}

		internal JumpStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.JumpStatement, span)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken Keyword { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ContinueKeyword:
				case SyntaxType.BreakKeyword:
				case SyntaxType.ReturnKeyword:
				case SyntaxType.DiscardKeyword:
					this.Keyword = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}