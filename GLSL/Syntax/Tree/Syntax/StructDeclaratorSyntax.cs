using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StructDeclaratorSyntax : SyntaxNode
	{
		private readonly List<ArraySpecifierSyntax> arraySpecifiers = new List<ArraySpecifierSyntax>();

		internal StructDeclaratorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.StructDeclarator, start)
		{
		}

		internal StructDeclaratorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.StructDeclarator, span)
		{
		}

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers => this.arraySpecifiers;

		public IdentifierSyntax Identifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.ArraySpecifier:
					this.arraySpecifiers.Add(node as ArraySpecifierSyntax);
					break;
			}
		}
	}
}