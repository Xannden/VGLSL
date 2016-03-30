namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class LogicalAndExpressionSyntax : SyntaxNode
	{
		public LogicalAndExpressionSyntax() : base(SyntaxType.LogicalAndExpression)
		{
		}

		public TokenSparatedList<InclusiveOrExpressionSyntax> InclusiveOrExpressions { get; } = new TokenSparatedList<InclusiveOrExpressionSyntax>();

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