namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class AssignmentOperatorSyntax : SyntaxNode
	{
		public AssignmentOperatorSyntax() : base(SyntaxType.AssignmentOperator)
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