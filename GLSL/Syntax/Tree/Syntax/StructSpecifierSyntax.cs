using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StructSpecifierSyntax : SyntaxNode
	{
		internal StructSpecifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StructSpecifier, start)
		{
		}

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public List<StructDeclarationSyntax> StructDeclarations { get; } = new List<StructDeclarationSyntax>();

		public SyntaxToken StructKeyword { get; private set; }

		public TypeNameSyntax TypeName { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.StructKeyword:
					this.StructKeyword = node as SyntaxToken;
					break;

				case SyntaxType.TypeName:
					this.TypeName = node as TypeNameSyntax;
					break;

				case SyntaxType.LeftBraceToken:
					this.LeftBrace = node as SyntaxToken;
					break;

				case SyntaxType.StructDeclaration:
					this.StructDeclarations.Add(node as StructDeclarationSyntax);
					break;

				case SyntaxType.RightBraceToken:
					this.RightBrace = node as SyntaxToken;
					break;
			}
		}
	}
}