using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class DeclarationSyntax : SyntaxNode
	{
		internal DeclarationSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Declaration, start)
		{
		}

		internal DeclarationSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Declaration, span)
		{
		}

		public DeclarationListSyntax DeclarationList { get; private set; }

		public InitDeclaratorListSyntax InitDeclaratorList { get; private set; }

		public PrecisionDeclarationSyntax PrecisionDeclaration { get; private set; }

		public StructDefinitionSyntax StructDefinition { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.InitDeclaratorList:
					this.InitDeclaratorList = node as InitDeclaratorListSyntax;
					break;

				case SyntaxType.PrecisionDeclaration:
					this.PrecisionDeclaration = node as PrecisionDeclarationSyntax;
					break;

				case SyntaxType.StructDefinition:
					this.StructDefinition = node as StructDefinitionSyntax;
					break;

				case SyntaxType.DeclarationList:
					this.DeclarationList = node as DeclarationListSyntax;
					break;
			}
		}
	}
}