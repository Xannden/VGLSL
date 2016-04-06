using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InitDeclaratorListSyntax : SyntaxNode
	{
		internal InitDeclaratorListSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InitDeclaratorList, start)
		{
		}

		internal InitDeclaratorListSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.InitDeclaratorList, span)
		{
		}

		public TokenSeparatedList<InitPartSyntax> InitParts { get; } = new TokenSeparatedList<InitPartSyntax>();

		public TypeSyntax TypeNode { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		public SyntaxToken Semicolon { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.Type:
					this.TypeNode = node as TypeSyntax;
					break;

				case SyntaxType.InitPart:
					this.InitParts.AddNode(node as InitPartSyntax);
					break;

				case SyntaxType.CommaToken:
					this.InitParts.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.SemicolonToken:
					this.Semicolon = node as SyntaxToken;
					break;
			}
		}
	}
}