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