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

		public InitDeclaratorListSyntax InitDeclaratorList { get; private set; }

		public PrecisionDeclarationSyntax PrecisionDeclaration { get; private set; }

		public InterfaceBlockSyntax InterfaceBlock { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.InitDeclaratorList:
					this.InitDeclaratorList = node as InitDeclaratorListSyntax;
					break;

				case SyntaxType.PrecisionDeclaration:
					this.PrecisionDeclaration = node as PrecisionDeclarationSyntax;
					break;

				case SyntaxType.InterfaceBlock:
					this.InterfaceBlock = node as InterfaceBlockSyntax;
					break;
			}
		}
	}
}