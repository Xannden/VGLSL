namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class ExclusiveOrExpressionSyntax : SyntaxNode
	{
		internal ExclusiveOrExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ExclusiveOrExpression, start)
		{
		}

		public TokenSparatedList<AndExpressionSyntax> AndExpressions { get; } = new TokenSparatedList<AndExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AndExpression:
					this.AndExpressions.AddNode(node as AndExpressionSyntax);
					break;

				case SyntaxType.CaretToken:
					this.AndExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}