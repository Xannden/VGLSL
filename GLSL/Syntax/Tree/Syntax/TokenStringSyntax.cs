using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TokenStringSyntax : SyntaxNode
	{
		internal TokenStringSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TokenString, start)
		{
		}

		public List<SyntaxNode> Tokens { get; } = new List<SyntaxNode>();

		protected override void NewChild(SyntaxNode node)
		{
			this.Tokens.Add(node);
		}
	}
}