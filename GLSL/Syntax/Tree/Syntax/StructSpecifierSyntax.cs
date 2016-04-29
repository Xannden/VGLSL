using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StructSpecifierSyntax : SyntaxNode
	{
		private readonly List<StructDeclarationSyntax> structDeclarations = new List<StructDeclarationSyntax>();

		internal StructSpecifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StructSpecifier, start)
		{
		}

		internal StructSpecifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.StructSpecifier, span)
		{
		}

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public IReadOnlyList<StructDeclarationSyntax> StructDeclarations => this.structDeclarations;

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
					this.structDeclarations.Add(node as StructDeclarationSyntax);
					break;

				case SyntaxType.RightBraceToken:
					this.RightBrace = node as SyntaxToken;
					break;
			}
		}
	}
}