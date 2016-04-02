namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class FunctionStatementSyntax : SyntaxNode
	{
		internal FunctionStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FunctionStatement, start)
		{
		}

		public FunctionHeaderSyntax FunctionHeader { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.FunctionHeader:
					this.FunctionHeader = node as FunctionHeaderSyntax;
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}