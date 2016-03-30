namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ArraySpecifierSyntax : SyntaxNode
	{
		public ArraySpecifierSyntax() : base(SyntaxType.ArraySpecifier)
		{
		}

		public ConstantExpressionSyntax ConstantExpression { get; private set; }

		public SyntaxToken LeftBracket { get; private set; }

		public SyntaxToken RightBracket { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LeftBracketToken:
					this.LeftBracket = node as SyntaxToken;
					break;

				case SyntaxType.ConstantExpression:
					this.ConstantExpression = node as ConstantExpressionSyntax;
					break;

				case SyntaxType.RightBracketToken:
					this.RightBracket = node as SyntaxToken;
					break;
			}
		}
	}
}