using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PostfixArrayAccessSyntax : SyntaxNode
	{
		internal PostfixArrayAccessSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PostfixArrayAccess, start)
		{
		}

		internal PostfixArrayAccessSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PostfixArrayAccess, span)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken LeftBracket { get; private set; }

		public SyntaxToken RightBracket { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LeftBracketToken:
					this.LeftBracket = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.RightBracketToken:
					this.RightBracket = node as SyntaxToken;
					break;
			}
		}
	}
}