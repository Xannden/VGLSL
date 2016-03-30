namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class LinePreprocessorSyntax : SyntaxNode
	{
		public LinePreprocessorSyntax() : base(SyntaxType.LinePreprocessor)
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