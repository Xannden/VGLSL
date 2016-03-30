namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class EqualityExpressionSyntax : SyntaxNode
	{
		public EqualityExpressionSyntax() : base(SyntaxType.EqualityExpression)
		{
		}

		public TokenSparatedList<RelationalExpressionSyntax> RelationalExpression { get; } = new TokenSparatedList<RelationalExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.RelationalExpression:
					this.RelationalExpression.AddNode(node as RelationalExpressionSyntax);
					break;

				case SyntaxType.EqualEqualToken:
				case SyntaxType.ExclamationEqualToken:
					this.RelationalExpression.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}