namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class MultiplicativeExpressionSyntax : SyntaxNode
	{
		public MultiplicativeExpressionSyntax() : base(SyntaxType.MultiplicativeExpression)
		{
		}

		public TokenSparatedList<UnaryExpressionSyntax> UnaryExpressions { get; } = new TokenSparatedList<UnaryExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.UnaryExpression:
					this.UnaryExpressions.AddNode(node as UnaryExpressionSyntax);
					break;

				case SyntaxType.StarToken:
				case SyntaxType.SlashToken:
				case SyntaxType.AmpersandToken:
					this.UnaryExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}