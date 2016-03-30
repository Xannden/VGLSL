﻿namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class PostfixExpressionContinuationSyntax : SyntaxNode
	{
		public PostfixExpressionContinuationSyntax() : base(SyntaxType.PostFixExpressionContinuation)
		{
		}

		public FieldSelectionSyntax FieldSelection { get; private set; }

		public PostfixArrayAccessSyntax PostfixArrayAccess { get; private set; }

		public SyntaxToken PostfixOperator { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PlusPlusToken:
				case SyntaxType.MinusMinusToken:
					this.PostfixOperator = node as SyntaxToken;
					break;

				case SyntaxType.FieldSelection:
					this.FieldSelection = node as FieldSelectionSyntax;
					break;

				case SyntaxType.PostFixArrayAccess:
					this.PostfixArrayAccess = node as PostfixArrayAccessSyntax;
					break;
			}
		}
	}
}