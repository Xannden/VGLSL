namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class IterationStatementSyntax : SyntaxNode
	{
		internal IterationStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.IterationStatement, start)
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