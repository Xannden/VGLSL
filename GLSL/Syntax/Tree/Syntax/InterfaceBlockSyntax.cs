using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InterfaceBlockSyntax : SyntaxNode
	{
		private readonly List<StructDeclarationSyntax> structDeclarations = new List<StructDeclarationSyntax>();

		internal InterfaceBlockSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InterfaceBlock, start)
		{
		}

		internal InterfaceBlockSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.InterfaceBlock, span)
		{
		}

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public SyntaxToken Semicolon { get; private set; }

		public IReadOnlyList<StructDeclarationSyntax> StructDeclarations => this.structDeclarations;

		public StructDeclaratorSyntax StructDeclarator { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
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