namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class AssignmentExpressionSyntax : SyntaxNode
	{
		public AssignmentExpressionSyntax() : base(SyntaxType.AssignmentExpression)
		{
		}

		public AssignmentExpressionSyntax AssignmentExpression { get; private set; }

		public AssignmentOperatorSyntax AssignmentOperator { get; private set; }

		public ConditionalExpressionSyntax Conditional { get; private set; }

		public UnaryExpressionSyntax UnaryExpression { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ConditionalExpression:
					this.Conditional = node as ConditionalExpressionSyntax;
					break;

				case SyntaxType.UnaryExpression:
					this.UnaryExpression = node as UnaryExpressionSyntax;
					break;

				case SyntaxType.AssignmentOperator:
					this.AssignmentOperator = node as AssignmentOperatorSyntax;
					break;

				case SyntaxType.AssignmentExpression:
					this.AssignmentExpression = node as AssignmentExpressionSyntax;
					break;
			}
		}
	}
}