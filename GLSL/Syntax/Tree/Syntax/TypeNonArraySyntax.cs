using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TypeNonArraySyntax : SyntaxNode
	{
		internal TypeNonArraySyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TypeNonArray, start)
		{
		}

		internal TypeNonArraySyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.TypeNonArray, span)
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