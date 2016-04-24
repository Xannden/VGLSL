using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TypeNameSyntax : SyntaxNode
	{
		internal TypeNameSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.TypeName, start)
		{
		}

		internal TypeNameSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.TypeName, span)
		{
		}

		public IdentifierSyntax Identifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;
			}
		}
	}
}