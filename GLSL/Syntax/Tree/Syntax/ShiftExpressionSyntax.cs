namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ShiftExpressionSyntax : SyntaxNode
	{
		internal ShiftExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ShiftExpression, start)
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