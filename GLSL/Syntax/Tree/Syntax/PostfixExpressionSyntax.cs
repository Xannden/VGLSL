using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PostfixExpressionSyntax : SyntaxNode
	{
		private readonly List<PostfixExpressionContinuationSyntax> postfixExpressionContinuations = new List<PostfixExpressionContinuationSyntax>();

		internal PostfixExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PostfixExpression, start)
		{
		}

		internal PostfixExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PostfixExpression, span)
		{
		}

		public IReadOnlyList<PostfixExpressionContinuationSyntax> PostfixExpressionContinuations => this.postfixExpressionContinuations;

		public PostfixExpressionStartSyntax PostfixExpressionStart { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PostfixExpressionStart:
					this.PostfixExpressionStart = node as PostfixExpressionStartSyntax;
					break;

				case SyntaxType.PostfixExpressionContinuation:
					this.postfixExpressionContinuations.Add(node as PostfixExpressionContinuationSyntax);
					break;
			}
		}
	}
}