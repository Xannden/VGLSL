using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class RelationalExpressionSyntax : SyntaxNode
	{
		internal RelationalExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.RelationalExpression, start)
		{
		}

		internal RelationalExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.RelationalExpression, span)
		{
		}

		public TokenSeparatedList<ShiftExpressionSyntax> ShiftExpressions { get; } = new TokenSeparatedList<ShiftExpressionSyntax>();

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ShiftExpression:
					this.ShiftExpressions.AddNode(node as ShiftExpressionSyntax);
					break;

				case SyntaxType.LessThenToken:
				case SyntaxType.GreaterThenToken:
				case SyntaxType.LessThenEqualToken:
				case SyntaxType.GreaterThenEqualToken:
					this.ShiftExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}