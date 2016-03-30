using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class TypeSyntax : SyntaxNode
	{
		public TypeSyntax() : base(SyntaxType.Type)
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