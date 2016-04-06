using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StructDefinitionSyntax : SyntaxNode
	{
		private List<StructDeclarationSyntax> structDeclarations = new List<StructDeclarationSyntax>();

		internal StructDefinitionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StructDefinition, start)
		{
		}

		internal StructDefinitionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.StructDefinition, span)
		{
		}

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public SyntaxToken Semicolon { get; private set; }

		public IReadOnlyList<StructDeclarationSyntax> StructDeclarations => this.structDeclarations;

		public StructDeclaratorSyntax StructDeclarator { get; private set; }

		public TypeNameSyntax TypeName { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		internal override void NewChild(SyntaxNode node)
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
					this.structDeclarations.Add(node as StructDeclarationSyntax);
					break;

				case SyntaxType.RightBraceToken:
					this.RightBrace = node as SyntaxToken;
					break;

				case SyntaxType.StructDeclarator:
					this.StructDeclarator = node as StructDeclaratorSyntax;
					break;

				case SyntaxType.SemicolonToken:
					this.Semicolon = node as SyntaxToken;
					break;
			}
		}
	}
}