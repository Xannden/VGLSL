namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class FunctionDefinitionSyntax : SyntaxNode
	{
		public FunctionDefinitionSyntax() : base(SyntaxType.FunctionDefinition)
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