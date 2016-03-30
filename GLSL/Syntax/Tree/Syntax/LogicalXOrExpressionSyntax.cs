namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class LogicalXOrExpressionSyntax : SyntaxNode
	{
		public LogicalXOrExpressionSyntax() : base(SyntaxType.LogicalXOrExpression)
		{
		}

		public TokenSparatedList<LogicalAndExpressionSyntax> LogicalAndExpressions { get; } = new TokenSparatedList<LogicalAndExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LogicalAndExpression:
					this.LogicalAndExpressions.AddNode(node as LogicalAndExpressionSyntax);
					break;

				case SyntaxType.CaretCaretToken:
					this.LogicalAndExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}