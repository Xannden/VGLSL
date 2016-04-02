namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class FunctionDefinitionSyntax : SyntaxNode
	{
		internal FunctionDefinitionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FunctionDefinition, start)
		{
		}

		public BlockSyntax Block { get; private set; }

		public FunctionHeaderSyntax FunctionHeader { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.FunctionHeader:
					this.FunctionHeader = node as FunctionHeaderSyntax;
					break;

				case SyntaxType.Block:
					this.Block = node as BlockSyntax;
					break;
			}
		}
	}
}