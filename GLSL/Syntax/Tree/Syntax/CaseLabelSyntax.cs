namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class CaseLabelSyntax : SyntaxNode
	{
		public CaseLabelSyntax() : base(SyntaxType.CaseLabel)
		{
		}

		public SyntaxToken CaseKeyword { get; private set; }

		public SyntaxToken Colon { get; private set; }

		public SyntaxToken DefaultKeyword { get; private set; }

		public ExpressionSyntax Expression { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.CaseKeyword:
					this.CaseKeyword = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.DefaultKeyword:
					this.DefaultKeyword = node as SyntaxToken;
					break;

				case SyntaxType.ColonToken:
					this.Colon = node as SyntaxToken;
					break;
			}
		}
	}
}