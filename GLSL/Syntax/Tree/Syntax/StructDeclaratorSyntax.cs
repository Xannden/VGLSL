﻿using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StructDeclaratorSyntax : SyntaxNode
	{
		internal StructDeclaratorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StructDeclarator, start)
		{
		}

		internal StructDeclaratorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.StructDeclarator, span)
		{
		}

		public List<ArraySpecifierSyntax> ArraySpecifiers { get; } = new List<ArraySpecifierSyntax>();

		public IdentifierSyntax Identifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.ArraySpecifier:
					this.ArraySpecifiers.Add(node as ArraySpecifierSyntax);
					break;
			}
		}
	}
}