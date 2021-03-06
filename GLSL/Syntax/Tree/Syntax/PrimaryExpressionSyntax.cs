﻿using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PrimaryExpressionSyntax : SyntaxNode
	{
		internal PrimaryExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PrimaryExpression, start)
		{
		}

		internal PrimaryExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PrimaryExpression, span)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken NumberConstant { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.IntConstToken:
				case SyntaxType.UIntConstToken:
				case SyntaxType.FloatConstToken:
				case SyntaxType.TrueKeyword:
				case SyntaxType.FalseKeyword:
				case SyntaxType.DoubleConstToken:
					this.NumberConstant = node as SyntaxToken;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}