using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InterpolationQualifierSyntax : SyntaxNode
	{
		internal InterpolationQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.InterpolationQualifier, span)
		{
		}

		internal InterpolationQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InterpolationQualifier, start)
		{
		}

		public SyntaxToken Qualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.SmoothKeyword:
				case SyntaxType.FlatKeyword:
				case SyntaxType.NoPerspectiveKeyword:
					this.Qualifier = node as SyntaxToken;
					break;
			}
		}
	}
}
