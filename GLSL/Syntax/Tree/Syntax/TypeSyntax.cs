using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TypeSyntax : SyntaxNode
	{
		private List<ArraySpecifierSyntax> arraySpecifiers = new List<ArraySpecifierSyntax>();

		internal TypeSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Type, start)
		{
		}

		internal TypeSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Type, span)
		{
		}

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers => this.arraySpecifiers;

		public TypeNonArraySyntax TypeNonArray { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeNonArray:
					this.TypeNonArray = node as TypeNonArraySyntax;

					break;

				case SyntaxType.ArraySpecifier:
					this.arraySpecifiers.Add(node as ArraySpecifierSyntax);
					break;
			}
		}
	}
}