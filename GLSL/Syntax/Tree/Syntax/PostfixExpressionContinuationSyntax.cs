namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class PostfixExpressionContinuationSyntax : SyntaxNode
	{
		internal PostfixExpressionContinuationSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PostFixExpressionContinuation, start)
		{
		}

		public FieldSelectionSyntax FieldSelection { get; private set; }

		public PostfixArrayAccessSyntax PostfixArrayAccess { get; private set; }

		public SyntaxToken PostfixOperator { get; private set; }

		protected override void NewChild(SyntaxNode node)
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

				case SyntaxType.PostFixArrayAccess:
					this.PostfixArrayAccess = node as PostfixArrayAccessSyntax;
					break;
			}
		}
	}
}