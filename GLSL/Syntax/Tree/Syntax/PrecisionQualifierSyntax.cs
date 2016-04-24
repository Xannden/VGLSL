using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PrecisionQualifierSyntax : SyntaxNode
	{
		internal PrecisionQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PrecisionQualifier, span)
		{
		}

		internal PrecisionQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PrecisionQualifier, start)
		{
		}

		public SyntaxToken Qualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LowPrecisionKeyword:
				case SyntaxType.MediumPrecisionKeyword:
				case SyntaxType.HighPrecisionKeyword:
					this.Qualifier = node as SyntaxToken;
					break;
			}
		}
	}
}