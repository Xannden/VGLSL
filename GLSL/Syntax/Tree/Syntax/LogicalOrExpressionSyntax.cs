namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class LogicalOrExpressionSyntax : SyntaxNode
	{
		public LogicalOrExpressionSyntax() : base(SyntaxType.LogicalOrExpression)
		{
		}

		public TokenSparatedList<LogicalXOrExpressionSyntax> LogicalXOrExpressions { get; } = new TokenSparatedList<LogicalXOrExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LogicalXOrExpression:
					this.LogicalXOrExpressions.AddNode(node as LogicalXOrExpressionSyntax);
					break;

				case SyntaxType.BarBarToken:
					this.LogicalXOrExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}