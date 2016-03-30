namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ErrorPreprocessorSyntax : SyntaxNode
	{
		public ErrorPreprocessorSyntax() : base(SyntaxType.ErrorPreprocessor)
		{
		}

		public SyntaxToken ErrorKeyword { get; private set; }

		public TokenStringSyntax TokenString { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ErrorPreprocessorKeyword:
					this.ErrorKeyword = node as SyntaxToken;
					break;

				case SyntaxType.TokenString:
					this.TokenString = node as TokenStringSyntax;
					break;
			}
		}
	}
}