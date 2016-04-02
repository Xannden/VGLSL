﻿namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class ExpressionStatementSyntax : SyntaxNode
	{
		internal ExpressionStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ExpressionStatement, start)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}