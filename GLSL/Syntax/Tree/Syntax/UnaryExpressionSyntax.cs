namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class UnaryExpressionSyntax : SyntaxNode
	{
		public UnaryExpressionSyntax() : base(SyntaxType.UnaryExpression)
		{
		}

		public PostfixExpressionSyntax PostfixExpression { get; private set; }

		public UnaryExpressionSyntax UnaryExpression { get; private set; }

		public SyntaxToken UnaryOperator { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PlusPlusToken:
				case SyntaxType.MinusMinusToken:
				case SyntaxType.PlusToken:
				case SyntaxType.MinusToken:
				case SyntaxType.ExclamationToken:
				case SyntaxType.TildeToken:
					this.UnaryOperator = node as SyntaxToken;
					break;

				case SyntaxType.UnaryExpression:
					this.UnaryExpression = node as UnaryExpressionSyntax;
					break;

				case SyntaxType.PostFixExpression:
					this.PostfixExpression = node as PostfixExpressionSyntax;
					break;
			}
		}
	}
}