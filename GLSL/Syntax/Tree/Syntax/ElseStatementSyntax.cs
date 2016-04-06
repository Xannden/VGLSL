using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ElseStatementSyntax : SyntaxNode
	{
		internal ElseStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.ElseStatement, start)
		{
		}

		internal ElseStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.ElseStatement, span)
		{
		}

		public SyntaxToken ElseKeyword { get; private set; }

		public StatementSyntax Statement { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.ElseKeyword:
					this.ElseKeyword = node as SyntaxToken;
					break;

				case SyntaxType.Statement:
					this.Statement = node as StatementSyntax;
					break;
			}
		}
	}
}