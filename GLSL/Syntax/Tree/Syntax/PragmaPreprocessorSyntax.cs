using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PragmaPreprocessorSyntax : SyntaxNode
	{
		internal PragmaPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PragmaPreprocessor, start)
		{
		}

		internal PragmaPreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PragmaPreprocessor, span)
		{
		}

		public SyntaxToken PragmaKeyword { get; private set; }

		public TokenStringSyntax TokenString { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PragmaPreprocessorKeyword:
					this.PragmaKeyword = node as SyntaxToken;
					break;

				case SyntaxType.TokenString:
					this.TokenString = node as TokenStringSyntax;
					break;
			}
		}
	}
}