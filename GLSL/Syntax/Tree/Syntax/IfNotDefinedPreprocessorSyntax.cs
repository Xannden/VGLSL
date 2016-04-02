namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class IfNotDefinedPreprocessorSyntax : SyntaxNode
	{
		internal IfNotDefinedPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.IfNotDefinedPreprocessor, start)
		{
		}

		public SyntaxToken EndIfKeyword { get; private set; }

		public ExcludedCodeSyntax ExcludedCode { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken IfNotDefinedKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IfNotDefinedPreprocessorKeyword:
					this.IfNotDefinedKeyword = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
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