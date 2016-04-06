using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PostfixExpressionContinuationSyntax : SyntaxNode
	{
		internal PostfixExpressionContinuationSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PostfixExpressionContinuation, start)
		{
		}

		internal PostfixExpressionContinuationSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PostfixExpressionContinuation, span)
		{
		}

		public FieldSelectionSyntax FieldSelection { get; private set; }

		public PostfixArrayAccessSyntax PostfixArrayAccess { get; private set; }

		public SyntaxToken PostfixOperator { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PlusPlusToken:
				case SyntaxType.MinusMinusToken:
					this.PostfixOperator = node as SyntaxToken;
					break;

				case SyntaxType.FieldSelection:
					this.FieldSelection = node as FieldSelectionSyntax;
					break;

				case SyntaxType.PostfixArrayAccess:
					this.PostfixArrayAccess = node as PostfixArrayAccessSyntax;
					break;
			}
		}
	}
}