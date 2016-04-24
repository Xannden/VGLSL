using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class LayoutQualifierSyntax : SyntaxNode
	{
		internal LayoutQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.LayoutQualifier, span)
		{
		}

		internal LayoutQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.LayoutQualifier, start)
		{
		}

		public SyntaxToken LayoutKeyword { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public TokenSeparatedList<LayoutQualifierIdSyntax> LayoutQualifierIds { get; } = new TokenSeparatedList<LayoutQualifierIdSyntax>();

		public SyntaxToken RightParentheses { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LayoutKeyword:
					this.LayoutKeyword = node as SyntaxToken;
					break;
				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;
				case SyntaxType.LayoutQualifierId:
					this.LayoutQualifierIds.AddNode(node as LayoutQualifierIdSyntax);
					break;
				case SyntaxType.CommaToken:
					this.LayoutQualifierIds.AddToken(node as SyntaxToken);
					break;
				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}
