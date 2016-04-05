using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ExpressionSyntax : SyntaxNode
	{
		internal ExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Expression, start)
		{
		}

		internal ExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Expression, span)
		{
		}

		private TokenSparatedList<AssignmentExpressionSyntax> AssignmentExpressions { get; } = new TokenSparatedList<AssignmentExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AssignmentExpression:
					this.AssignmentExpressions.AddNode(node as AssignmentExpressionSyntax);
					break;

				case SyntaxType.CommaToken:
					this.AssignmentExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}