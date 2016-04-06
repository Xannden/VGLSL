using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ProgramSyntax : SyntaxNode
	{
		private List<SyntaxNode> nodes = new List<SyntaxNode>();

		internal ProgramSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Program, start)
		{
		}

		internal ProgramSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Program, span)
		{
		}

		public IReadOnlyList<SyntaxNode> Nodes => this.nodes;

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Declaration:
				case SyntaxType.FunctionDefinition:
					this.nodes.Add(node);
					break;
			}
		}
	}
}