using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class EqualityExpressionSyntax : SyntaxNode
	{
		internal EqualityExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.EqualityExpression, start)
		{
		}

		internal EqualityExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.EqualityExpression, span)
		{
		}

		public TokenSparatedList<RelationalExpressionSyntax> RelationalExpression { get; } = new TokenSparatedList<RelationalExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.RelationalExpression:
					this.RelationalExpression.AddNode(node as RelationalExpressionSyntax);
					break;

				case SyntaxType.EqualEqualToken:
				case SyntaxType.ExclamationEqualToken:
					this.RelationalExpression.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}