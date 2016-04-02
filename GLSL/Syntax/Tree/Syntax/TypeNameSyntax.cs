namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TypeNameSyntax : SyntaxNode
	{
		internal TypeNameSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TypeName, start)
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