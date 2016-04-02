using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StructDeclarationSyntax : SyntaxNode
	{
		internal StructDeclarationSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StructDeclaration, start)
		{
		}

		public List<SyntaxToken> Commas { get; } = new List<SyntaxToken>();

		public SyntaxToken SemiColon { get; private set; }

		public TokenSparatedList<StructDeclaratorSyntax> StructDeclarators { get; } = new TokenSparatedList<StructDeclaratorSyntax>();

		public TypeSyntax Type { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.Type:
					this.Type = node as TypeSyntax;
					break;

				case SyntaxType.StructDeclarator:
					this.StructDeclarators.AddNode(node as StructDeclaratorSyntax);
					break;

				case SyntaxType.CommaToken:
					this.StructDeclarators.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}