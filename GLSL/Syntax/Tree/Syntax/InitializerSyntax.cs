namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class InitializerSyntax : SyntaxNode
	{
		internal InitializerSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Initializer, start)
		{
		}

		public SyntaxNode Initilizer { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AssignmentExpression:
				case SyntaxType.InitList:
					this.Initilizer = node;
					break;
			}
		}
	}
}