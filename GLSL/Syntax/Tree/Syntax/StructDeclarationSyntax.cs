using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StructDeclarationSyntax : SyntaxNode
	{
		internal StructDeclarationSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StructDeclaration, start)
		{
		}

		internal StructDeclarationSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.StructDeclaration, span)
		{
		}

		public SyntaxToken Semicolon { get; private set; }

		public TokenSeparatedList<StructDeclaratorSyntax> StructDeclarators { get; } = new TokenSeparatedList<StructDeclaratorSyntax>();

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		public TypeSyntax TypeSyntax { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.Type:
					this.TypeSyntax = node as TypeSyntax;
					break;

				case SyntaxType.StructDeclarator:
					this.StructDeclarators.AddNode(node as StructDeclaratorSyntax);
					break;

				case SyntaxType.CommaToken:
					this.StructDeclarators.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.SemicolonToken:
					this.Semicolon = node as SyntaxToken;
					break;
			}
		}
	}
}