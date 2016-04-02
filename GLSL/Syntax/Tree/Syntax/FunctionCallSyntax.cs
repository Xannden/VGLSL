﻿namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class FunctionCallSyntax : SyntaxNode
	{
		internal FunctionCallSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FunctionCall, start)
		{
		}

		public TokenSparatedList<AssignmentExpressionSyntax> AssignemntExpression { get; } = new TokenSparatedList<AssignmentExpressionSyntax>();

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public SyntaxToken VoidKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.VoidKeyword:
					this.VoidKeyword = node as SyntaxToken;
					break;

				case SyntaxType.AssignmentExpression:
					this.AssignemntExpression.AddNode(node as AssignmentExpressionSyntax);
					break;

				case SyntaxType.CommaToken:
					this.AssignemntExpression.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}