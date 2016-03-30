using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class SwitchStatementSyntax : SyntaxNode
	{
		public SwitchStatementSyntax() : base(SyntaxType.SwitchStatement)
		{
		}

		public ExpressionSyntax Expression { get; private set; }

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public SyntaxToken RightPrentheses { get; private set; }

		public List<SimpleStatementSyntax> SimpleStatements { get; } = new List<SimpleStatementSyntax>();

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
					this.RightPrentheses = node as SyntaxToken;
					break;

				case SyntaxType.LeftBraceToken:
					this.LeftBrace = node as SyntaxToken;
					break;

				case SyntaxType.SimpleStatement:
					this.SimpleStatements.Add(node as SimpleStatementSyntax);
					break;

				case SyntaxType.RightBraceToken:
					this.RightBrace = node as SyntaxToken;
					break;
			}
		}
	}
}