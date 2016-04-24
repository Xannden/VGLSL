using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class LogicalAndExpressionSyntax : SyntaxNode
	{
		internal LogicalAndExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.LogicalAndExpression, start)
		{
		}

		internal LogicalAndExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.LogicalAndExpression, span)
		{
		}

		public TokenSeparatedList<InclusiveOrExpressionSyntax> InclusiveOrExpressions { get; } = new TokenSeparatedList<InclusiveOrExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.InclusiveOrExpression:
					this.InclusiveOrExpressions.AddNode(node as InclusiveOrExpressionSyntax);
					break;

				case SyntaxType.AmpersandAmpersandToken:
					this.InclusiveOrExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}