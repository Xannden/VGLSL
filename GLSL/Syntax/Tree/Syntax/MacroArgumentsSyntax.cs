namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class MacroArgumentsSyntax : SyntaxNode
	{
		internal MacroArgumentsSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.MacroArguments, start)
		{
		}

		public TokenSparatedList<IdentifierSyntax> ArgumentList { get; } = new TokenSparatedList<IdentifierSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.ArgumentList.AddNode(node as IdentifierSyntax);
					break;

				case SyntaxType.CommaToken:
					this.ArgumentList.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}