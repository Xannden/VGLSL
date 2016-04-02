using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class ProgramSyntax : SyntaxNode
	{
		internal ProgramSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Program, start)
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