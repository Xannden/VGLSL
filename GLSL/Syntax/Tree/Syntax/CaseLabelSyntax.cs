using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class CaseLabelSyntax : SyntaxNode
	{
		internal CaseLabelSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.CaseLabel, start)
		{
		}

		internal CaseLabelSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.CaseLabel, span)
		{
		}

		public SyntaxToken CaseKeyword { get; private set; }

		public SyntaxToken Colon { get; private set; }

		public SyntaxToken DefaultKeyword { get; private set; }

		public ExpressionSyntax Expression { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.CaseKeyword:
					this.CaseKeyword = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.DefaultKeyword:
					this.DefaultKeyword = node as SyntaxToken;
					break;

				case SyntaxType.ColonToken:
					this.Colon = node as SyntaxToken;
					break;
			}
		}
	}
}