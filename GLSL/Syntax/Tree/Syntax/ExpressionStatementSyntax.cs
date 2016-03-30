namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ExpressionStatementSyntax : SyntaxNode
	{
		public ExpressionStatementSyntax() : base(SyntaxType.ExpressionStatement)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}