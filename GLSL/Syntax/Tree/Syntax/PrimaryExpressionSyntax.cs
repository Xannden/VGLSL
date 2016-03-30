namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class PrimaryExpressionSyntax : SyntaxNode
	{
		public PrimaryExpressionSyntax() : base(SyntaxType.PrimaryExpression)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken NumberConstant { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.IntConstToken:
				case SyntaxType.UIntConstToken:
				case SyntaxType.FloatConstToken:
				case SyntaxType.BoolConstToken:
				case SyntaxType.DoubleConstToken:
					this.NumberConstant = node as SyntaxToken;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}