using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PostfixExpressionSyntax : SyntaxNode
	{
		internal PostfixExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PostFixExpression, start)
		{
		}

		internal PostfixExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PostFixExpression, span)
		{
		}

		public List<PostfixExpressionContinuationSyntax> PostfixExpressionContinuations { get; } = new List<PostfixExpressionContinuationSyntax>();

		public PostfixExpressionStartSyntax PostfixExpressionStart { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PostFixExpressionStart:
					this.PostfixExpressionStart = node as PostfixExpressionStartSyntax;
					break;

				case SyntaxType.PostFixExpressionContinuation:
					this.PostfixExpressionContinuations.Add(node as PostfixExpressionContinuationSyntax);
					break;
			}
		}
	}
}