using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StructDefinitionSyntax : SyntaxNode
	{
		internal StructDefinitionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StructDefinition, start)
		{
		}

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		public List<StructDeclarationSyntax> StructDeclarations { get; } = new List<StructDeclarationSyntax>();

		public StructDeclaratorSyntax StructDeclarator { get; private set; }

		public TypeNameSyntax TypeName { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
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

				case SyntaxType.StructDeclarator:
					this.StructDeclarator = node as StructDeclaratorSyntax;
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}