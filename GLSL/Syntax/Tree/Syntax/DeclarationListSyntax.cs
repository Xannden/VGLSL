namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class DeclarationListSyntax : SyntaxNode
	{
		public DeclarationListSyntax() : base(SyntaxType.DeclarationList)
		{
		}

		public TokenSparatedList<IdentifierSyntax> Identifiers { get; } = new TokenSparatedList<IdentifierSyntax>();

		public SyntaxToken SemiColon { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifiers.AddNode(node as IdentifierSyntax);
					break;

				case SyntaxType.CommaToken:
					this.Identifiers.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}