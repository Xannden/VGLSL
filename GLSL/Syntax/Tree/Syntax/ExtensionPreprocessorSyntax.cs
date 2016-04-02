namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class ExtensionPreprocessorSyntax : SyntaxNode
	{
		internal ExtensionPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ExtensionPreprocessor, start)
		{
		}

		public IdentifierSyntax Behavior { get; private set; }

		public SyntaxToken Colon { get; private set; }

		public SyntaxToken ExtensionKeyword { get; private set; }

		public IdentifierSyntax ExtensionName { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ExtensionPreprocessorKeyword:
					this.ExtensionKeyword = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					if (this.ExtensionName == null)
					{
						this.ExtensionName = node as IdentifierSyntax;
					}
					else
					{
						this.Behavior = node as IdentifierSyntax;
					}

					break;

				case SyntaxType.ColonToken:
					this.Colon = node as SyntaxToken;
					break;
			}
		}
	}
}