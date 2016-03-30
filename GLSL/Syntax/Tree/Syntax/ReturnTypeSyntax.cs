namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ReturnTypeSyntax : SyntaxNode
	{
		public ReturnTypeSyntax() : base(SyntaxType.ReturnType)
		{
		}

		public TypeSyntax Type { get; private set; }

		public SyntaxToken VoidKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.VoidKeyword:
					this.VoidKeyword = node as SyntaxToken;
					break;

				case SyntaxType.Type:
					this.Type = node as TypeSyntax;
					break;
			}
		}
	}
}