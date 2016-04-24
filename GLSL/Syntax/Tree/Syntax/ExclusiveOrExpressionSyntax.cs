using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ExclusiveOrExpressionSyntax : SyntaxNode
	{
		internal ExclusiveOrExpressionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ExclusiveOrExpression, start)
		{
		}

		internal ExclusiveOrExpressionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ExclusiveOrExpression, span)
		{
		}

		public TokenSeparatedList<AndExpressionSyntax> AndExpressions { get; } = new TokenSeparatedList<AndExpressionSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AndExpression:
					this.AndExpressions.AddNode(node as AndExpressionSyntax);
					break;

				case SyntaxType.CaretToken:
					this.AndExpressions.AddToken(node as SyntaxToken);
					break;
			}
		}
	}
}