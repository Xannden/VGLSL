namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class AdditiveExpressionSyntax : SyntaxNode
	{
		public AdditiveExpressionSyntax() : base(SyntaxType.AdditiveExpression)
		{
		}

		public TokenSparatedList<MultiplicativeExpressionSyntax> MultiplicativeExpressions { get; } = new TokenSparatedList<MultiplicativeExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.MultiplicativeExpression:
					this.MultiplicativeExpressions.AddNode(node as MultiplicativeExpressionSyntax);
					break;

				case SyntaxType.PlusToken:
				case SyntaxType.MinusToken:
					this.MultiplicativeExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}