using System.Collections.Generic;
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

		public static bool operator ==(ArraySpecifierSyntax left, ArraySpecifierSyntax right)
		{
			IReadOnlyList<SyntaxToken> leftTokens = left?.GetSyntaxTokens();
			IReadOnlyList<SyntaxToken> rightTokens = right?.GetSyntaxTokens();

			if (leftTokens != null && rightTokens != null && leftTokens.Count != rightTokens.Count)
			{
				return false;
			}

			for (int i = 0; i < leftTokens.Count; i++)
			{
				if (leftTokens[i].Text != rightTokens[i].Text)
				{
					return false;
				}
			}

			return true;
		}

		public static bool operator !=(ArraySpecifierSyntax left, ArraySpecifierSyntax right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

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