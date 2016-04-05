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

		public TokenSparatedList<InitPartSyntax> InitParts { get; } = new TokenSparatedList<InitPartSyntax>();

		public TypeSyntax Type { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

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

				case SyntaxType.InitPart:
					this.InitParts.AddNode(node as InitPartSyntax);
					break;

				case SyntaxType.CommaToken:
					this.InitParts.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}