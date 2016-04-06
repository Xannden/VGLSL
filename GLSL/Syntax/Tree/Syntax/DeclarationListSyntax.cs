using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class DeclarationListSyntax : SyntaxNode
	{
		internal DeclarationListSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.DeclarationList, start)
		{
		}

		internal DeclarationListSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.DeclarationList, span)
		{
		}

		public TokenSeparatedList<IdentifierSyntax> Identifiers { get; } = new TokenSeparatedList<IdentifierSyntax>();

		public SyntaxToken Semicolon { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		internal override void NewChild(SyntaxNode node)
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

				case SyntaxType.SemicolonToken:
					this.Semicolon = node as SyntaxToken;
					break;
			}
		}
	}
}