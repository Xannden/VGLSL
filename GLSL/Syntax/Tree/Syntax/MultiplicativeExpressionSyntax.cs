namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class MultiplicativeExpressionSyntax : SyntaxNode
	{
		internal MultiplicativeExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.MultiplicativeExpression, start)
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