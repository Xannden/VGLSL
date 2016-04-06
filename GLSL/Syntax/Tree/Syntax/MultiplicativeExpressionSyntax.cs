using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class MultiplicativeExpressionSyntax : SyntaxNode
	{
		internal MultiplicativeExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.MultiplicativeExpression, start)
		{
		}

		internal MultiplicativeExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.MultiplicativeExpression, span)
		{
		}

		public TokenSeparatedList<UnaryExpressionSyntax> UnaryExpressions { get; } = new TokenSeparatedList<UnaryExpressionSyntax>();

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.UnaryExpression:
					this.UnaryExpressions.AddNode(node as UnaryExpressionSyntax);
					break;

				case SyntaxType.StarToken:
				case SyntaxType.SlashToken:
				case SyntaxType.AmpersandToken:
					this.UnaryExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}