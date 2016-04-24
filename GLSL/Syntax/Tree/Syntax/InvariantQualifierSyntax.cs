using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InvariantQualifierSyntax : SyntaxNode
	{
		internal InvariantQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.InvariantQualifier, span)
		{
		}

		internal InvariantQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InvariantQualifier, start)
		{
		}

		public SyntaxToken InvariantKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.InvariantKeyword:
					this.InvariantKeyword = node as SyntaxToken;
					break;
			}
		}
	}
}
