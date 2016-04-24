using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class VersionPreprocessorSyntax : SyntaxNode
	{
		internal VersionPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.VersionPreprocessor, start)
		{
		}

		internal VersionPreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.VersionPreprocessor, span)
		{
		}

		public SyntaxToken IfKeyword { get; private set; }

		public IdentifierSyntax Profile { get; private set; }

		public SyntaxToken VersionNumber { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IfPreprocessorKeyword:
					this.IfKeyword = node as SyntaxToken;
					break;

				case SyntaxType.IntConstToken:
					this.VersionNumber = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					this.Profile = node as IdentifierSyntax;
					break;
			}
		}
	}
}