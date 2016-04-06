using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class AdditiveExpressionSyntax : SyntaxNode
	{
		internal AdditiveExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.AdditiveExpression, start)
		{
		}

		internal AdditiveExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.AdditiveExpression, span)
		{
		}

		public TokenSeparatedList<MultiplicativeExpressionSyntax> MultiplicativeExpressions { get; } = new TokenSeparatedList<MultiplicativeExpressionSyntax>();

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.MultiplicativeExpression:
					this.MultiplicativeExpressions.AddNode(node as MultiplicativeExpressionSyntax);
					break;

				case SyntaxType.PlusToken:
				case SyntaxType.MinusToken:
					this.MultiplicativeExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}