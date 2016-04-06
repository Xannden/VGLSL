using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class FunctionStatementSyntax : SyntaxNode
	{
		internal FunctionStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FunctionStatement, start)
		{
		}

		internal FunctionStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.FunctionStatement, span)
		{
		}

		public FunctionHeaderSyntax FunctionHeader { get; private set; }

		public SyntaxToken Semicolon { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.FunctionHeader:
					this.FunctionHeader = node as FunctionHeaderSyntax;
					break;

				case SyntaxType.SemicolonToken:
					this.Semicolon = node as SyntaxToken;
					break;
			}
		}
	}
}