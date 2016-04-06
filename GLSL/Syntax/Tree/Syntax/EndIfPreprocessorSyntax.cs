using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class EndIfPreprocessorSyntax : SyntaxNode
	{
		internal EndIfPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.EndIfPreprocessor, start)
		{
		}

		internal EndIfPreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.EndIfPreprocessor, span)
		{
		}

		public SyntaxToken EndIfKeyword { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.EndIfPreprocessorKeyword:
					this.EndIfKeyword = node as SyntaxToken;
					break;
			}
		}
	}
}