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

		public SyntaxToken Semicolon { get; private set; }

		public TypeSyntax TypeSyntax { get; private set; }

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
					this.TypeSyntax = node as TypeSyntax;
					break;

				case SyntaxType.SemicolonToken:
					this.Semicolon = node as SyntaxToken;
					break;
			}
		}
	}
}