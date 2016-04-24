using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InitListSyntax : SyntaxNode
	{
		internal InitListSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InitList, start)
		{
		}

		internal InitListSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.InitList, span)
		{
		}

		public TokenSeparatedList<InitializerSyntax> Initializers { get; } = new TokenSeparatedList<InitializerSyntax>();

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LeftBraceToken:
					this.LeftBrace = node as SyntaxToken;
					break;

				case SyntaxType.Initializer:
					this.Initializers.AddNode(node as InitializerSyntax);
					break;

				case SyntaxType.CommaToken:
					this.Initializers.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.RightBraceToken:
					this.RightBrace = node as SyntaxToken;
					break;
			}
		}
	}
}