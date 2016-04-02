using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class ExcludedCodeSyntax : SyntaxNode
	{
		internal ExcludedCodeSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ExcludedCode, start)
		{
		}

		public List<SyntaxNode> Code { get; } = new List<SyntaxNode>();

		protected override void NewChild(SyntaxNode node)
		{
			this.Code.Add(node);
		}
	}
}