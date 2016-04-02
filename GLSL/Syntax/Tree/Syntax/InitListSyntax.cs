namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InitListSyntax : SyntaxNode
	{
		internal InitListSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InitList, start)
		{
		}

		public TokenSparatedList<InitializerSyntax> Initilizers { get; } = new TokenSparatedList<InitializerSyntax>();

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
					this.Initilizers.AddNode(node as InitializerSyntax);
					break;

				case SyntaxType.CommaToken:
					this.Initilizers.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.RightBraceToken:
					this.RightBrace = node as SyntaxToken;
					break;
			}
		}
	}
}