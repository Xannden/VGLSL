using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class TokenStringSyntax : SyntaxNode
	{
		public TokenStringSyntax() : base(SyntaxType.TokenString)
		{
		}

		public List<SyntaxNode> Tokens { get; } = new List<SyntaxNode>();

		protected override void NewChild(SyntaxNode node)
		{
			this.Tokens.Add(node);
		}
	}
}