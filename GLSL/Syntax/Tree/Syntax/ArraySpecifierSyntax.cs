using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ArraySpecifierSyntax : SyntaxNode
	{
		internal ArraySpecifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ArraySpecifier, start)
		{
		}

		internal ArraySpecifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ArraySpecifier, span)
		{
		}

		public ConstantExpressionSyntax ConstantExpression { get; private set; }

		public SyntaxToken LeftBracket { get; private set; }

		public SyntaxToken RightBracket { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LeftBracketToken:
					this.LeftBracket = node as SyntaxToken;
					break;

				case SyntaxType.ConstantExpression:
					this.ConstantExpression = node as ConstantExpressionSyntax;
					break;

				case SyntaxType.RightBracketToken:
					this.RightBracket = node as SyntaxToken;
					break;
			}
		}
	}
}