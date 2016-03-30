namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ShiftExpressionSyntax : SyntaxNode
	{
		public ShiftExpressionSyntax() : base(SyntaxType.ShiftExpression)
		{
		}

		public TokenSparatedList<AdditiveExpressionSyntax> AdditiveExpressionSyntax { get; } = new TokenSparatedList<AdditiveExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AdditiveExpression:
					this.AdditiveExpressionSyntax.AddNode(node as AdditiveExpressionSyntax);
					break;

				case SyntaxType.LessThenLessThenToken:
				case SyntaxType.GreaterThenGreaterThenToken:
					this.AdditiveExpressionSyntax.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}