using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class FieldSelectionSyntax : SyntaxNode
	{
		internal FieldSelectionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FieldSelection, start)
		{
		}

		internal FieldSelectionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.FieldSelection, span)
		{
		}

		public SyntaxToken Dot { get; private set; }

		public FunctionCallSyntax FunctionCall { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.DotToken:
					this.Dot = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.FunctionCall:
					this.FunctionCall = node as FunctionCallSyntax;
					break;
			}
		}
	}
}