namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class LogicalOrExpressionSyntax : SyntaxNode
	{
		internal LogicalOrExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.LogicalOrExpression, start)
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