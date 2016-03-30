namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ConstantExpressionSyntax : SyntaxNode
	{
		public ConstantExpressionSyntax() : base(SyntaxType.ConstantExpression)
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