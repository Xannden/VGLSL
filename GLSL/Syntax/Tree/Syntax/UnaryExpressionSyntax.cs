using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class UnaryExpressionSyntax : SyntaxNode
	{
		internal UnaryExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.UnaryExpression, start)
		{
		}

		internal UnaryExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.UnaryExpression, span)
		{
		}

		public PostfixExpressionSyntax PostfixExpression { get; private set; }

		public UnaryExpressionSyntax UnaryExpression { get; private set; }

		public SyntaxToken UnaryOperator { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PlusPlusToken:
				case SyntaxType.MinusMinusToken:
				case SyntaxType.PlusToken:
				case SyntaxType.MinusToken:
				case SyntaxType.ExclamationToken:
				case SyntaxType.TildeToken:
					this.UnaryOperator = node as SyntaxToken;
					break;

				case SyntaxType.UnaryExpression:
					this.UnaryExpression = node as UnaryExpressionSyntax;
					break;

				case SyntaxType.PostfixExpression:
					this.PostfixExpression = node as PostfixExpressionSyntax;
					break;
			}
		}
	}
}