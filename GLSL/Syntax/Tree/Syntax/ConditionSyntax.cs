namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ConditionSyntax : SyntaxNode
	{
		public ConditionSyntax() : base(SyntaxType.Condition)
		{
		}

		public SyntaxToken Equal { get; private set; }

		public ExpressionSyntax Expression { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public InitializerSyntax Initilizer { get; private set; }

		public TypeSyntax Type { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.Type:
					this.Type = node as TypeSyntax;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.EqualToken:
					this.Equal = node as SyntaxToken;
					break;

				case SyntaxType.Initializer:
					this.Initilizer = node as InitializerSyntax;
					break;
			}
		}
	}
}