using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class IdentifierSyntax : SyntaxToken
	{
		internal IdentifierSyntax(SyntaxTree tree, TrackingSpan span, string text, SyntaxTrivia leadingTrivia, SyntaxTrivia trailingTrivia, Snapshot snapshot, bool isMissing) : base(tree, SyntaxType.IdentifierToken, span, text, leadingTrivia, trailingTrivia, snapshot, isMissing)
		{
		}

		internal IdentifierSyntax(string text) : this(null, null, text, null, null, null, false)
		{
		}

		public string Identifier => this.Text;

		public Definition Definition { get; internal set; }
	}
}