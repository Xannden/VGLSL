using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PostfixExpressionStartSyntax : SyntaxNode
	{
		internal PostfixExpressionStartSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PostFixExpressionStart, start)
		{
		}

		internal PostfixExpressionStartSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PostFixExpressionStart, span)
		{
		}

		public ConstructorSyntax Constructor { get; private set; }

		public FunctionCallSyntax FunctionCall { get; private set; }

		public PrimaryExpressionSyntax PrimaryExpression { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PrimaryExpression:
					this.PrimaryExpression = node as PrimaryExpressionSyntax;
					break;

				case SyntaxType.FunctionCall:
					this.FunctionCall = node as FunctionCallSyntax;
					break;

				case SyntaxType.Constructor:
					this.Constructor = node as ConstructorSyntax;
					break;
			}
		}
	}
}