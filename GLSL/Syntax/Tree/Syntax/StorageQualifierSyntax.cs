using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StorageQualifierSyntax : SyntaxNode
	{
		internal StorageQualifierSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StorageQualifier, start)
		{
		}

		internal StorageQualifierSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.StorageQualifier, span)
		{
		}

		public SyntaxToken Keyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ConstKeyword:
				case SyntaxType.InOutKeyword:
				case SyntaxType.InKeyword:
				case SyntaxType.OutKeyword:
				case SyntaxType.CentroidKeyword:
				case SyntaxType.PatchKeyword:
				case SyntaxType.SampleKeyword:
				case SyntaxType.UniformKeyword:
				case SyntaxType.BufferKeyword:
				case SyntaxType.SharedKeyword:
				case SyntaxType.CoherentKeyword:
				case SyntaxType.VolatileKeyword:
				case SyntaxType.RestrictKeyword:
				case SyntaxType.ReadOnlyKeyword:
				case SyntaxType.WriteOnlyKeyword:
				case SyntaxType.SubroutineKeyword:
					this.Keyword = node as SyntaxToken;
					break;
			}
		}
	}
}