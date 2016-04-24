using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ShiftExpressionSyntax : SyntaxNode
	{
		internal ShiftExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ShiftExpression, start)
		{
		}

		internal ShiftExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ShiftExpression, span)
		{
		}

		public TokenSeparatedList<AdditiveExpressionSyntax> AdditiveExpressionSyntax { get; } = new TokenSeparatedList<AdditiveExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AdditiveExpression:
					this.AdditiveExpressionSyntax.AddNode(node as AdditiveExpressionSyntax);
					break;

				case SyntaxType.LessThenLessThenToken:
				case SyntaxType.GreaterThenGreaterThenToken:
					this.AdditiveExpressionSyntax.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}