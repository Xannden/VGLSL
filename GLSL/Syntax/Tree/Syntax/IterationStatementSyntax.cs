using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class IterationStatementSyntax : SyntaxNode
	{
		internal IterationStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.IterationStatement, start)
		{
		}

		internal IterationStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.IterationStatement, span)
		{
		}

		public SyntaxNode Statement { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.WhileStatement:
				case SyntaxType.DoWhileStatement:
				case SyntaxType.ForStatement:
					this.Statement = node;
					break;
			}
		}
	}
}