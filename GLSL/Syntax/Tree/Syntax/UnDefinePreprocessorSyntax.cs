using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class UndefinePreprocessorSyntax : SyntaxNode
	{
		internal UndefinePreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.UndefinePreprocessor, start)
		{
		}

		internal UndefinePreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.UndefinePreprocessor, span)
		{
		}

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken UndefineKeyword { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.UndefinePreprocessorKeyword:
					this.UndefineKeyword = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;
			}
		}
	}
}