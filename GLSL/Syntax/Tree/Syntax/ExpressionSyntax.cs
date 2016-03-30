namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ExpressionSyntax : SyntaxNode
	{
		public ExpressionSyntax() : base(SyntaxType.Expression)
		{
		}

		private TokenSparatedList<AssignmentExpressionSyntax> AssignmentExpressions { get; } = new TokenSparatedList<AssignmentExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AssignmentExpression:
					this.AssignmentExpressions.AddNode(node as AssignmentExpressionSyntax);
					break;

				case SyntaxType.CommaToken:
					this.AssignmentExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}