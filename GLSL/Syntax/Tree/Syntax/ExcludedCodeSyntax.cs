using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ExcludedCodeSyntax : SyntaxNode
	{
		private List<SyntaxNode> code = new List<SyntaxNode>();

		internal ExcludedCodeSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ExcludedCode, start)
		{
		}

		internal ExcludedCodeSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ExcludedCode, span)
		{
		}

		public IReadOnlyList<SyntaxNode> Code => this.code;

		protected override void NewChild(SyntaxNode node)
		{
			this.code.Add(node);
		}
	}
}