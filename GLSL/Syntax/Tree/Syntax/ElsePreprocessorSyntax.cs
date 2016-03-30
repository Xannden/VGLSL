namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ElsePreprocessorSyntax : SyntaxNode
	{
		public ElsePreprocessorSyntax() : base(SyntaxType.ElsePreprocessor)
		{
		}

		public SyntaxToken ElseKeyword { get; private set; }

		public SyntaxToken EndIfKeyword { get; private set; }

		public ExcludedCodeSyntax ExcludedCode { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ElsePreprocessorKeyword:
					this.ElseKeyword = node as SyntaxToken;
					break;

				case SyntaxType.ExcludedCode:
					this.ExcludedCode = node as ExcludedCodeSyntax;
					break;

				case SyntaxType.EndIfPreprocessorKeyword:
					this.EndIfKeyword = node as SyntaxToken;
					break;
			}
		}
	}
}