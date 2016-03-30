using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ProgramSyntax : SyntaxNode
	{
		public ProgramSyntax() : base(SyntaxType.Program)
		{
		}

		public List<SyntaxNode> Nodes { get; } = new List<SyntaxNode>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Declaration:
				case SyntaxType.FunctionDefinition:
					this.Nodes.Add(node);
					break;
			}
		}
	}
}