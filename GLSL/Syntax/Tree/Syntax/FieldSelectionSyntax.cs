namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class FieldSelectionSyntax : SyntaxNode
	{
		public FieldSelectionSyntax() : base(SyntaxType.FieldSelection)
		{
		}

		public SyntaxToken Dot { get; private set; }

		public FunctionCallSyntax FunctionCall { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.DotToken:
					this.Dot = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.FunctionCall:
					this.FunctionCall = node as FunctionCallSyntax;
					break;
			}
		}
	}
}