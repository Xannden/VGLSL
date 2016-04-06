using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class FunctionCallSyntax : SyntaxNode
	{
		internal FunctionCallSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FunctionCall, start)
		{
		}

		internal FunctionCallSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.FunctionCall, span)
		{
		}

		public TokenSeparatedList<AssignmentExpressionSyntax> AssignmentExpression { get; } = new TokenSeparatedList<AssignmentExpressionSyntax>();

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public SyntaxToken VoidKeyword { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.VoidKeyword:
					this.VoidKeyword = node as SyntaxToken;
					break;

				case SyntaxType.AssignmentExpression:
					this.AssignmentExpression.AddNode(node as AssignmentExpressionSyntax);
					break;

				case SyntaxType.CommaToken:
					this.AssignmentExpression.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}