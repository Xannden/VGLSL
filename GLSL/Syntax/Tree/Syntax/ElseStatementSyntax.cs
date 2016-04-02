namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class ElseStatementSyntax : SyntaxNode
	{
		internal ElseStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ElseStatement, start)
		{
		}

		public SyntaxToken ElseKeyword { get; private set; }

		public StatementSyntax Statement { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ElseKeyword:
					this.ElseKeyword = node as SyntaxToken;
					break;

				case SyntaxType.Statement:
					this.Statement = node as StatementSyntax;
					break;
			}
		}
	}
}