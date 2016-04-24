using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class FunctionDefinitionSyntax : SyntaxNode
	{
		internal FunctionDefinitionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FunctionDefinition, start)
		{
		}

		internal FunctionDefinitionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.FunctionDefinition, span)
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