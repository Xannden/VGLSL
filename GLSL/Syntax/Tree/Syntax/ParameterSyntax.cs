using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ParameterSyntax : SyntaxNode
	{
		private readonly List<ArraySpecifierSyntax> arraySpecifiers = new List<ArraySpecifierSyntax>();

		internal ParameterSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Parameter, start)
		{
		}

		internal ParameterSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Parameter, span)
		{
		}

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers => this.arraySpecifiers;

		public IdentifierSyntax Identifier { get; private set; }

		public TypeSyntax TypeSyntax { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
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
					this.arraySpecifiers.Add(node as ArraySpecifierSyntax);
					break;
			}
		}
	}
}