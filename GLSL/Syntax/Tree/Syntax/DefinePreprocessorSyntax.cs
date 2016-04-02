namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class DefinePreprocessorSyntax : SyntaxNode
	{
		internal DefinePreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.DefinePreprocessor, start)
		{
		}

		public MacroArgumentsSyntax Arguments { get; private set; }

		public SyntaxToken DefineKeyword { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public TokenStringSyntax TokenString { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.DefinePreprocessorKeyword:
					this.DefineKeyword = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.MacroArguments:
					this.Arguments = node as MacroArgumentsSyntax;
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;

				case SyntaxType.TokenString:
					this.TokenString = node as TokenStringSyntax;
					break;
			}
		}
	}
}