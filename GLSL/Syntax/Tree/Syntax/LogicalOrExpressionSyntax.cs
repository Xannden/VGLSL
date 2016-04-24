using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class LogicalOrExpressionSyntax : SyntaxNode
	{
		internal LogicalOrExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.LogicalOrExpression, start)
		{
		}

		internal LogicalOrExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.LogicalOrExpression, span)
		{
		}

		public TokenSeparatedList<LogicalXorExpressionSyntax> LogicalXorExpressions { get; } = new TokenSeparatedList<LogicalXorExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LogicalXorExpression:
					this.LogicalXorExpressions.AddNode(node as LogicalXorExpressionSyntax);
					break;

				case SyntaxType.BarBarToken:
					this.LogicalXorExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}