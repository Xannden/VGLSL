namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ExclusiveOrExpressionSyntax : SyntaxNode
	{
		public ExclusiveOrExpressionSyntax() : base(SyntaxType.ExclusiveOrExpression)
		{
		}

		public TokenSparatedList<AndExpressionSyntax> AndExpressions { get; } = new TokenSparatedList<AndExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AndExpression:
					this.AndExpressions.AddNode(node as AndExpressionSyntax);
					break;

				case SyntaxType.CaretToken:
					this.AndExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}