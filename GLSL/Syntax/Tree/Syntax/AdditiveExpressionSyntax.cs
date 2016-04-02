namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class AdditiveExpressionSyntax : SyntaxNode
	{
		internal AdditiveExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.AdditiveExpression, start)
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