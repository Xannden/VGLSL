namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ConstructorSyntax : SyntaxNode
	{
		public ConstructorSyntax() : base(SyntaxType.Constructor)
		{
		}

		public TokenSparatedList<AssignmentExpressionSyntax> AssignemntExpressions { get; } = new TokenSparatedList<AssignmentExpressionSyntax>();

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public TypeSyntax Type { get; private set; }

		public SyntaxToken VoidKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Type:
					this.Type = node as TypeSyntax;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.VoidKeyword:
					this.VoidKeyword = node as SyntaxToken;
					break;

				case SyntaxType.AssignmentExpression:
					this.AssignemntExpressions.AddNode(node as AssignmentExpressionSyntax);
					break;

				case SyntaxType.CommaToken:
					this.AssignemntExpressions.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}