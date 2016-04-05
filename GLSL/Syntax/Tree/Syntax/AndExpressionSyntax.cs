using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class AndExpressionSyntax : SyntaxNode
	{
		internal AndExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.AndExpression, start)
		{
		}

		internal AndExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.AndExpression, span)
		{
		}

		public TokenSparatedList<EqualityExpressionSyntax> EqualityExpressions { get; } = new TokenSparatedList<EqualityExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.EqualityExpression:
					this.EqualityExpressions.AddNode(node as EqualityExpressionSyntax);
					break;

				case SyntaxType.AmpersandToken:
					this.EqualityExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}