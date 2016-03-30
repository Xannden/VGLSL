namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class RelationalExpressionSyntax : SyntaxNode
	{
		public RelationalExpressionSyntax() : base(SyntaxType.RelationalExpression)
		{
		}

		public TokenSparatedList<ShiftExpressionSyntax> ShiftExpressions { get; } = new TokenSparatedList<ShiftExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ShiftExpression:
					this.ShiftExpressions.AddNode(node as ShiftExpressionSyntax);
					break;

				case SyntaxType.LessThenToken:
				case SyntaxType.GreaterThenToken:
				case SyntaxType.LessThenEqualToken:
				case SyntaxType.GreaterThenEqualToken:
					this.ShiftExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}