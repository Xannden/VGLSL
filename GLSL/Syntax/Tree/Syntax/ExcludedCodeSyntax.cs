using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ExcludedCodeSyntax : SyntaxNode
	{
		public ExcludedCodeSyntax() : base(SyntaxType.ExcludedCode)
		{
		}

		public List<SyntaxNode> Code { get; } = new List<SyntaxNode>();

		protected override void NewChild(SyntaxNode node)
		{
			this.Code.Add(node);
		}
	}
}