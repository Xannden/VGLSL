using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class LogicalXorExpressionSyntax : SyntaxNode
	{
		internal LogicalXorExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.LogicalXorExpression, start)
		{
		}

		internal LogicalXorExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.LogicalXorExpression, span)
		{
		}

		public TokenSeparatedList<LogicalAndExpressionSyntax> LogicalAndExpressions { get; } = new TokenSeparatedList<LogicalAndExpressionSyntax>();

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LogicalAndExpression:
					this.LogicalAndExpressions.AddNode(node as LogicalAndExpressionSyntax);
					break;

				case SyntaxType.CaretCaretToken:
					this.LogicalAndExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}