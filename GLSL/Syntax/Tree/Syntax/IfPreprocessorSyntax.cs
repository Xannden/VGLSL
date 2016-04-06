using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class IfPreprocessorSyntax : SyntaxNode
	{
		internal IfPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.IfPreprocessor, start)
		{
		}

		internal IfPreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.IfPreprocessor, span)
		{
		}

		public SyntaxToken EndIfKeyword { get; private set; }

		public ExcludedCodeSyntax ExcludedCode { get; private set; }

		public SyntaxToken IfKeyword { get; private set; }

		public TokenStringSyntax TokenString { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IfPreprocessorKeyword:
					this.IfKeyword = node as SyntaxToken;
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