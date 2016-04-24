using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PreciseQualifierSyntax : SyntaxNode
	{
		internal PreciseQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PreciseQualifier, span)
		{
		}

		internal PreciseQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PreciseQualifier, start)
		{
		}

		public SyntaxToken PreciseKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PreciseKeyword:
					this.PreciseKeyword = node as SyntaxToken;
					break;
			}
		}
	}
}
