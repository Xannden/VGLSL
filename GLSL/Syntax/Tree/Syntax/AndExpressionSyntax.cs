namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class AndExpressionSyntax : SyntaxNode
	{
		public AndExpressionSyntax() : base(SyntaxType.AndExpression)
		{
		}

		public TokenSparatedList<EqualityExpressionSyntax> EqualityExpressions { get; } = new TokenSparatedList<EqualityExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.EqualityExpression:
					this.EqualityExpressions.AddNode(node as EqualityExpressionSyntax);
					break;

				case SyntaxType.AmpersandToken:
					this.EqualityExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}