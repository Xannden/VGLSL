using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class StatementSyntax : SyntaxNode
	{
		private List<SimpleStatementSyntax> simpleStatements = new List<SimpleStatementSyntax>();

		internal StatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Statement, start)
		{
		}

		internal StatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Statement, span)
		{
		}

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public IReadOnlyList<SimpleStatementSyntax> SimpleStatements => this.simpleStatements;

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LeftBraceToken:
					this.LeftBrace = node as SyntaxToken;
					break;

				case SyntaxType.SimpleStatement:
					this.simpleStatements.Add(node as SimpleStatementSyntax);
					break;

				case SyntaxType.RightBraceToken:
					this.RightBrace = node as SyntaxToken;
					break;
			}
		}
	}
}