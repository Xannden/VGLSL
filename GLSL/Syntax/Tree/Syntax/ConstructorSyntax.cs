using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ConstructorSyntax : SyntaxNode
	{
		internal ConstructorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Constructor, start)
		{
		}

		internal ConstructorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Constructor, span)
		{
		}

		public TokenSeparatedList<AssignmentExpressionSyntax> AssignmentExpressions { get; } = new TokenSeparatedList<AssignmentExpressionSyntax>();

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public TypeSyntax TypeNode { get; private set; }

		public SyntaxToken VoidKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Type:
					this.TypeNode = node as TypeSyntax;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.VoidKeyword:
					this.VoidKeyword = node as SyntaxToken;
					break;

				case SyntaxType.AssignmentExpression:
					this.AssignmentExpressions.AddNode(node as AssignmentExpressionSyntax);
					break;

				case SyntaxType.CommaToken:
					this.AssignmentExpressions.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}