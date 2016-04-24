using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InitPartSyntax : SyntaxNode
	{
		private List<ArraySpecifierSyntax> arraySpecifiers = new List<ArraySpecifierSyntax>();

		internal InitPartSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.InitPart, start)
		{
		}

		internal InitPartSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.InitPart, span)
		{
		}

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers => this.arraySpecifiers;

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
					this.arraySpecifiers.Add(node as ArraySpecifierSyntax);
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