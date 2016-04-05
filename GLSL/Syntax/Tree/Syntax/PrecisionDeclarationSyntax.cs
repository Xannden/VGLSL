using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PrecisionDeclarationSyntax : SyntaxNode
	{
		internal PrecisionDeclarationSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.PrecisionDeclaration, start)
		{
		}

		internal PrecisionDeclarationSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.PrecisionDeclaration, span)
		{
		}

		public SyntaxToken PrecisionKeyword { get; private set; }

		public SyntaxNode PrecisionQualifier { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		public TypeSyntax Type { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.PrecisionKeyword:
					this.PrecisionKeyword = node as SyntaxToken;
					break;

				case SyntaxType.PrecisionQualifier:
					this.PrecisionQualifier = node;
					break;

				case SyntaxType.Type:
					this.Type = node as TypeSyntax;
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}