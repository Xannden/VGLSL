using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TypeSyntax : SyntaxNode
	{
		internal TypeSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Type, start)
		{
		}

		internal TypeSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Type, span)
		{
		}

		public List<ArraySpecifierSyntax> ArraySpecifiers { get; } = new List<ArraySpecifierSyntax>();

		public TypeNonArraySyntax TypeNonArray { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeNonArray:
					this.TypeNonArray = node as TypeNonArraySyntax;

					break;

				case SyntaxType.ArraySpecifier:
					this.ArraySpecifiers.Add(node as ArraySpecifierSyntax);
					break;
			}
		}
	}
}