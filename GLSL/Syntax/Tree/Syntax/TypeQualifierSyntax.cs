namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class TypeQualifierSyntax : SyntaxNode
	{
		internal TypeQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TypeQualifier, start)
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