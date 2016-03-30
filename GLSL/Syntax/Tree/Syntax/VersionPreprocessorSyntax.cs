namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class VersionPreprocessorSyntax : SyntaxNode
	{
		public VersionPreprocessorSyntax() : base(SyntaxType.VersionPreprocessor)
		{
		}

		public SyntaxToken IfKeyword { get; private set; }

		public IdentifierSyntax Profile { get; private set; }

		public SyntaxToken VersionNumber { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IfPreprocessorKeyword:
					this.IfKeyword = node as SyntaxToken;
					break;

				case SyntaxType.IntConstToken:
					this.VersionNumber = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					this.Profile = node as IdentifierSyntax;
					break;
			}
		}
	}
}