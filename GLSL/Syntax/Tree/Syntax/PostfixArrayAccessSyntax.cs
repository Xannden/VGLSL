namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class PostfixArrayAccessSyntax : SyntaxNode
	{
		public PostfixArrayAccessSyntax() : base(SyntaxType.PostFixArrayAccess)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken LeftBracket { get; private set; }

		public SyntaxToken RightBracket { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LeftBracketToken:
					this.LeftBracket = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.RightBracketToken:
					this.RightBracket = node as SyntaxToken;
					break;
			}
		}
	}
}