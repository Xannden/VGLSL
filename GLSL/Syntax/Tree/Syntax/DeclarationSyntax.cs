﻿namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class DeclarationSyntax : SyntaxNode
	{
		public DeclarationSyntax() : base(SyntaxType.Declaration)
		{
		}

		public DeclarationListSyntax DeclarationList { get; private set; }

		public InitDeclaratorListDeclarationSyntax InitDeclaratorListDeclaration { get; private set; }

		public PrecisionDeclarationSyntax PrecisionDeclaration { get; private set; }

		public StructDefinitionSyntax StructDefinition { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.InitDeclaratorListDeclaration:
					this.InitDeclaratorListDeclaration = node as InitDeclaratorListDeclarationSyntax;
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