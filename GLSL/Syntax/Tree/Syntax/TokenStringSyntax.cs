using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TokenStringSyntax : SyntaxNode
	{
		internal TokenStringSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TokenString, start)
		{
		}

		internal TokenStringSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.TokenString, span)
		{
		}

		public List<SyntaxNode> Tokens { get; } = new List<SyntaxNode>();

		protected override void NewChild(SyntaxNode node)
		{
			this.Tokens.Add(node);
		}
	}
}