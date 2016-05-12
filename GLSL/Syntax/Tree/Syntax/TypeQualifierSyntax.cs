using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TypeQualifierSyntax : SyntaxNode
	{
		private readonly List<SingleTypeQualifierSyntax> singleTypeQualifiers = new List<SingleTypeQualifierSyntax>();

		internal TypeQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TypeQualifier, start)
		{
		}

		internal TypeQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.TypeQualifier, span)
		{
		}

		public IReadOnlyList<SingleTypeQualifierSyntax> SingleTypeQualifiers => this.singleTypeQualifiers;

		public static bool operator ==(TypeQualifierSyntax left, TypeQualifierSyntax right)
		{
			if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
			{
				return false;
			}

			if (left.singleTypeQualifiers.Count != right.singleTypeQualifiers.Count)
			{
				return false;
			}

			IReadOnlyList<SyntaxToken> leftTokens = left.GetSyntaxTokens();
			IReadOnlyList<SyntaxToken> rightTokens = right.GetSyntaxTokens();

			if (leftTokens.Count != rightTokens.Count)
			{
				return false;
			}

			for (int i = 0; i < left.singleTypeQualifiers.Count; i++)
			{
				if (leftTokens[i].Text != rightTokens[i].Text)
				{
					return false;
				}
			}

			return true;
		}

		public static bool operator !=(TypeQualifierSyntax left, TypeQualifierSyntax right)
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
				case SyntaxType.SingleTypeQualifier:
					this.singleTypeQualifiers.Add(node as SingleTypeQualifierSyntax);
					break;
			}
		}
	}
}