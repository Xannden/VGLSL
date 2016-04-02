namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InclusiveOrExpressionSyntax : SyntaxNode
	{
		internal InclusiveOrExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InclusiveOrExpression, start)
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