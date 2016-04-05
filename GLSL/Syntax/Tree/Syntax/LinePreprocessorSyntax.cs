using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class LinePreprocessorSyntax : SyntaxNode
	{
		internal LinePreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.LinePreprocessor, start)
		{
		}

		internal LinePreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.LinePreprocessor, span)
		{
		}

		public SyntaxToken Line { get; private set; }

		public SyntaxToken LineKeyword { get; private set; }

		public SyntaxToken SourceStringNumber { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LinePreprocessorKeyword:
					this.LineKeyword = node as SyntaxToken;
					break;

				case SyntaxType.IntConstToken:
					if (this.Line != null)
					{
						this.SourceStringNumber = node as SyntaxToken;
					}
					else
					{
						this.Line = node as SyntaxToken;
					}

					break;
			}
		}
	}
}