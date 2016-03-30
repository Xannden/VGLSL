namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class TypeNameSyntax : SyntaxNode
	{
		public TypeNameSyntax() : base(SyntaxType.TypeName)
		{
		}

		public IdentifierSyntax Identifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;
			}
		}
	}
}