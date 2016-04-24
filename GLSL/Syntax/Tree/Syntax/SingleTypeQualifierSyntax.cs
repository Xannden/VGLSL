using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class SingleTypeQualifierSyntax : SyntaxNode
	{
		internal SingleTypeQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.SingleTypeQualifier, span)
		{
		}

		internal SingleTypeQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.SingleTypeQualifier, start)
		{
		}

		public StorageQualifierSyntax StorageQualifier { get; private set; }

		public LayoutQualifierSyntax LayoutQualifier { get; private set; }

		public PrecisionQualifierSyntax PrecisionQualifier { get; private set; }

		public InterpolationQualifierSyntax InterpolationQualifier { get; private set; }

		public InvariantQualifierSyntax InvariantQualifier { get; private set; }

		public PreciseQualifierSyntax PreciseQualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.StorageQualifier:
					this.StorageQualifier = node as StorageQualifierSyntax;
					break;
				case SyntaxType.LayoutQualifier:
					this.LayoutQualifier = node as LayoutQualifierSyntax;
					break;
				case SyntaxType.PrecisionQualifier:
					this.PrecisionQualifier = node as PrecisionQualifierSyntax;
					break;
				case SyntaxType.InterpolationQualifier:
					this.InterpolationQualifier = node as InterpolationQualifierSyntax;
					break;
				case SyntaxType.InvariantQualifier:
					this.InvariantQualifier = node as InvariantQualifierSyntax;
					break;
				case SyntaxType.PreciseQualifier:
					this.PreciseQualifier = node as PreciseQualifierSyntax;
					break;
			}
		}
	}
}
