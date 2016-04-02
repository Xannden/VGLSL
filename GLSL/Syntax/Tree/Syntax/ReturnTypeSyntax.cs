namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ReturnTypeSyntax : SyntaxNode
	{
		internal ReturnTypeSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ReturnType, start)
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