namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class InitializerSyntax : SyntaxNode
	{
		public InitializerSyntax() : base(SyntaxType.Initializer)
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