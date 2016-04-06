using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ConstantExpressionSyntax : SyntaxNode
	{
		internal ConstantExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ConstantExpression, start)
		{
		}

		internal ConstantExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ConstantExpression, span)
		{
		}

		public ConditionalExpressionSyntax ConditionalExpression { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ConditionalExpression:
					this.ConditionalExpression = node as ConditionalExpressionSyntax;
					break;
			}
		}
	}
}