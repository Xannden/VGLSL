namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class InitDeclaratorListDeclarationSyntax : SyntaxNode
	{
		public InitDeclaratorListDeclarationSyntax() : base(SyntaxType.InitDeclaratorListDeclaration)
		{
		}

		public InitDeclaratorListSyntax InitDeclaratorList { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.InitDeclaratorList:
					this.InitDeclaratorList = node as InitDeclaratorListSyntax;
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}