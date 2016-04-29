using System.Collections.Generic;

using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class SwitchStatementSyntax : SyntaxNode
	{
		private readonly List<SimpleStatementSyntax> simpleStatements = new List<SimpleStatementSyntax>();

		internal SwitchStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.SwitchStatement, start)
		{
		}

		internal SwitchStatementSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.SwitchStatement, span)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public IReadOnlyList<SimpleStatementSyntax> SimpleStatements => this.simpleStatements;

		public SyntaxToken SwitchKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.SwitchKeyword:
					this.SwitchKeyword = node as SyntaxToken;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;

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