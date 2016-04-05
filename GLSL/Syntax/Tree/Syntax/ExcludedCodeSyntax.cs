using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ExcludedCodeSyntax : SyntaxNode
	{
		internal ExcludedCodeSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ExcludedCode, start)
		{
		}

		internal ExcludedCodeSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ExcludedCode, span)
		{
		}

		public List<SyntaxNode> Code { get; } = new List<SyntaxNode>();

		protected override void NewChild(SyntaxNode node)
		{
			this.Code.Add(node);
		}
	}
}