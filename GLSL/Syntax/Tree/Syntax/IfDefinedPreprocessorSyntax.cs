namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class IfDefinedPreprocessorSyntax : SyntaxNode
	{
		public IfDefinedPreprocessorSyntax() : base(SyntaxType.IfDefinedPreprocessor)
		{
		}

		public SyntaxToken EndIfKeyword { get; private set; }

		public ExcludedCodeSyntax ExcludedCode { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken IfDefinedKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IfDefinedPreprocessorKeyword:
					this.IfDefinedKeyword = node as SyntaxToken;
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