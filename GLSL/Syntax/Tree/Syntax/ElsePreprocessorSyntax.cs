namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class ElsePreprocessorSyntax : SyntaxNode
	{
		internal ElsePreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ElsePreprocessor, start)
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