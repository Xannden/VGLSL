namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class ConstantExpressionSyntax : SyntaxNode
	{
		internal ConstantExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ConstantExpression, start)
		{
		}

		public ConditionalExpressionSyntax ConditionalExpression { get; private set; }

		protected override void NewChild(SyntaxNode node)
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