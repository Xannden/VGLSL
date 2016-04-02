namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class LogicalXOrExpressionSyntax : SyntaxNode
	{
		internal LogicalXOrExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.LogicalXOrExpression, start)
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