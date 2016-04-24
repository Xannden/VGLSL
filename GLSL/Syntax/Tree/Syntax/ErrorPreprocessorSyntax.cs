using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ErrorPreprocessorSyntax : SyntaxNode
	{
		internal ErrorPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ErrorPreprocessor, start)
		{
		}

		internal ErrorPreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ErrorPreprocessor, span)
		{
		}

		public SyntaxToken ErrorKeyword { get; private set; }

		public TokenStringSyntax TokenString { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ErrorPreprocessorKeyword:
					this.ErrorKeyword = node as SyntaxToken;
					break;

				case SyntaxType.TokenString:
					this.TokenString = node as TokenStringSyntax;
					break;
			}
		}
	}
}