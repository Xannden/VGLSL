using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ReturnTypeSyntax : SyntaxNode
	{
		internal ReturnTypeSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ReturnType, start)
		{
		}

		internal ReturnTypeSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ReturnType, span)
		{
		}

		public TypeSyntax TypeSyntax { get; private set; }

		public SyntaxToken VoidKeyword { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.VoidKeyword:
					this.VoidKeyword = node as SyntaxToken;
					break;

				case SyntaxType.Type:
					this.TypeSyntax = node as TypeSyntax;
					break;
			}
		}
	}
}