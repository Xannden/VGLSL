namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class TypeNonArraySyntax : SyntaxNode
	{
		public TypeNonArraySyntax() : base(SyntaxType.TypeNonArray)
		{
		}

		public SyntaxToken Keyword { get; private set; }

		public StructSpecifierSyntax StructSpecifier { get; private set; }

		public TypeNameSyntax TypeName { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeName:
					this.TypeName = node as TypeNameSyntax;
					break;

				case SyntaxType.StructSpecifier:
					this.StructSpecifier = node as StructSpecifierSyntax;
					break;

				default:
					if (node.SyntaxType.IsType())
					{
						this.Keyword = node as SyntaxToken;
					}

					break;
			}
		}
	}
}