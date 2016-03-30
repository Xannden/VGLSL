﻿namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class ParameterSyntax : SyntaxNode
	{
		public ParameterSyntax() : base(SyntaxType.Parameter)
		{
		}

		public ArraySpecifierSyntax ArraySpecifier { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public TypeSyntax Type { get; private set; }

		public TypeQualifierSyntax TypeQualifer { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifer = node as TypeQualifierSyntax;
					break;

				case SyntaxType.Type:
					this.Type = node as TypeSyntax;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.ArraySpecifier:
					this.ArraySpecifier = node as ArraySpecifierSyntax;
					break;
			}
		}
	}
}