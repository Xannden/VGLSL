﻿using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class PostfixExpressionSyntax : SyntaxNode
	{
		internal PostfixExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PostFixExpression, start)
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