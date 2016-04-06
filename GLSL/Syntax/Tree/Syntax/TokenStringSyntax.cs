using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TokenStringSyntax : SyntaxNode
	{
		private List<SyntaxNode> tokens = new List<SyntaxNode>();

		internal TokenStringSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TokenString, start)
		{
		}

		internal TokenStringSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.TokenString, span)
		{
		}

		public IReadOnlyList<SyntaxNode> Tokens => this.tokens;

		internal override void NewChild(SyntaxNode node)
		{
			this.tokens.Add(node);
		}
	}
}