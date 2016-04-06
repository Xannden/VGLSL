using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InclusiveOrExpressionSyntax : SyntaxNode
	{
		internal InclusiveOrExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InclusiveOrExpression, start)
		{
		}

		internal InclusiveOrExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.InclusiveOrExpression, span)
		{
		}

		public TokenSeparatedList<ExclusiveOrExpressionSyntax> ExclusiveOrExpressions { get; } = new TokenSeparatedList<ExclusiveOrExpressionSyntax>();

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ExclusiveOrExpression:
					this.ExclusiveOrExpressions.AddNode(node as ExclusiveOrExpressionSyntax);
					break;

				case SyntaxType.VerticalBarToken:
					this.ExclusiveOrExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}