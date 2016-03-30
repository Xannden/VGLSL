namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class IterationStatementSyntax : SyntaxNode
	{
		public IterationStatementSyntax() : base(SyntaxType.IterationStatement)
		{
		}

		public SyntaxNode Statement { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.WhileStatement:
				case SyntaxType.DoWhileStatement:
				case SyntaxType.ForStatement:
					this.Statement = node;
					break;
			}
		}
	}
}