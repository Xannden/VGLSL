using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class InitPartSyntax : SyntaxNode
	{
		internal InitPartSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InitPart, start)
		{
		}

		public List<ArraySpecifierSyntax> ArraySpecifiers { get; } = new List<ArraySpecifierSyntax>();

		public SyntaxToken Equal { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public InitializerSyntax Initializer { get; private set; }

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

				case SyntaxType.EqualToken:
					this.Equal = node as SyntaxToken;
					break;

				case SyntaxType.Initializer:
					this.Initializer = node as InitializerSyntax;
					break;
			}
		}
	}
}