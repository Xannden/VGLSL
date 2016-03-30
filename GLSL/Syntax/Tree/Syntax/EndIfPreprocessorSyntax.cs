namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class EndIfPreprocessorSyntax : SyntaxNode
	{
		public EndIfPreprocessorSyntax() : base(SyntaxType.EndIfPreprocessor)
		{
		}

		public SyntaxToken EndIfKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.EndIfPreprocessorKeyword:
					this.EndIfKeyword = node as SyntaxToken;
					break;
			}
		}
	}
}