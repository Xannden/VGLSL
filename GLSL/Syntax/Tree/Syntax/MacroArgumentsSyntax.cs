using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class MacroArgumentsSyntax : SyntaxNode
	{
		internal MacroArgumentsSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.MacroArguments, start)
		{
		}

		internal MacroArgumentsSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.MacroArguments, span)
		{
		}

		public TokenSeparatedList<IdentifierSyntax> ArgumentList { get; } = new TokenSeparatedList<IdentifierSyntax>();

		internal override void NewChild(SyntaxNode node)
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