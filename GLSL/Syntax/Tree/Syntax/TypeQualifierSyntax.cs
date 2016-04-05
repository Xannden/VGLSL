using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TypeQualifierSyntax : SyntaxNode
	{
		internal TypeQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TypeQualifier, start)
		{
		}

		internal TypeQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.TypeQualifier, span)
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