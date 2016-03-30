namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class TypeQualifierSyntax : SyntaxNode
	{
		public TypeQualifierSyntax() : base(SyntaxType.TypeQualifier)
		{
		}

		public SyntaxNode Qualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.StorageQualifier:
				case SyntaxType.LayoutQualifier:
				case SyntaxType.PrecisionQualifier:
				case SyntaxType.InterpolationQualifier:
				case SyntaxType.InvariantQualifier:
				case SyntaxType.PreciseQualifier:
					this.Qualifier = node;
					break;
			}
		}
	}
}