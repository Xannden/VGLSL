namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class InclusiveOrExpressionSyntax : SyntaxNode
	{
		public InclusiveOrExpressionSyntax() : base(SyntaxType.InclusiveOrExpression)
		{
		}

		public TokenSparatedList<ExclusiveOrExpressionSyntax> ExclusiveOrExpressions { get; } = new TokenSparatedList<ExclusiveOrExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
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