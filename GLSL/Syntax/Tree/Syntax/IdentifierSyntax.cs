using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class IdentifierSyntax : SyntaxToken
	{
		internal IdentifierSyntax(SyntaxTree tree, TrackingSpan span, string text, SyntaxTrivia leadingTrivia, SyntaxTrivia trailingTrivia, Snapshot snapshot, bool isMissing = false) : base(tree, SyntaxType.IdentifierToken, span, text, leadingTrivia, trailingTrivia, snapshot, isMissing)
		{
		}

		public string Name => this.Text;
	}
}