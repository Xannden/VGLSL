﻿namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class AssignmentOperatorSyntax : SyntaxNode
	{
		internal AssignmentOperatorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.AssignmentOperator, start)
		{
		}

		public SyntaxToken Operator { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.EqualToken:
				case SyntaxType.StarEqualToken:
				case SyntaxType.SlashEqualToken:
				case SyntaxType.PercentEqualToken:
				case SyntaxType.PlusEqualToken:
				case SyntaxType.MinusEqualToken:
				case SyntaxType.LessThenLessThenEqualToken:
				case SyntaxType.GreaterThenGreaterThenEqualToken:
				case SyntaxType.AmpersandEqualToken:
				case SyntaxType.CaretEqualToken:
				case SyntaxType.BarEqualToken:
					this.Operator = node as SyntaxToken;
					break;
			}
		}
	}
}