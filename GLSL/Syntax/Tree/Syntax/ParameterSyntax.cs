﻿using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ParameterSyntax : SyntaxNode
	{
		internal ParameterSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Parameter, start)
		{
		}

		internal ParameterSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Parameter, span)
		{
		}

		public ArraySpecifierSyntax ArraySpecifier { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public TypeSyntax TypeSyntax { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.Type:
					this.TypeSyntax = node as TypeSyntax;
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