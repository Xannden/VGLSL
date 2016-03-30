namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class PragmaPreprocessorSyntax : SyntaxNode
	{
		public PragmaPreprocessorSyntax() : base(SyntaxType.PragmaPreprocessor)
		{
		}

		public SyntaxToken PragamKeyword { get; private set; }

		public TokenStringSyntax TokenString { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PragmaPreprocessorKeyword:
					this.PragamKeyword = node as SyntaxToken;
					break;

				case SyntaxType.TokenString:
					this.TokenString = node as TokenStringSyntax;
					break;
			}
		}
	}
}