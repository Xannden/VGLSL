using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ElseIfPreprocessorSyntax : SyntaxNode
	{
		internal ElseIfPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ElseIfPreprocessor, start)
		{
		}

		internal ElseIfPreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ElseIfPreprocessor, span)
		{
		}

		public SyntaxToken ElseIfKeyword { get; private set; }

		public SyntaxToken EndIfKeyword { get; private set; }

		public ExcludedCodeSyntax ExcludedCode { get; private set; }

		public TokenStringSyntax TokenString { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ElseIfPreprocessorKeyword:
					this.ElseIfKeyword = node as SyntaxToken;
					break;

				case SyntaxType.TokenString:
					this.TokenString = node as TokenStringSyntax;
					break;

				case SyntaxType.ExcludedCode:
					this.ExcludedCode = node as ExcludedCodeSyntax;
					break;

				case SyntaxType.EndIfPreprocessorKeyword:
					this.EndIfKeyword = node as SyntaxToken;
					break;
			}
		}
	}
}