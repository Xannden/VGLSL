using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ConditionalExpressionSyntax : SyntaxNode
	{
		internal ConditionalExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ConditionalExpression, start)
		{
		}

		internal ConditionalExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ConditionalExpression, span)
		{
		}

		public AssignmentExpressionSyntax AssignmentExpression { get; private set; }

		public SyntaxToken Colon { get; private set; }

		public ExpressionSyntax Expression { get; private set; }

		public LogicalOrExpressionSyntax LogicalOrExpression { get; private set; }

		public SyntaxToken QuestionMark { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LogicalOrExpression:
					this.LogicalOrExpression = node as LogicalOrExpressionSyntax;
					break;

				case SyntaxType.QuestionToken:
					this.QuestionMark = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.ColonToken:
					this.Colon = node as SyntaxToken;
					break;

				case SyntaxType.AssignmentExpression:
					this.AssignmentExpression = node as AssignmentExpressionSyntax;
					break;
			}
		}
	}
}