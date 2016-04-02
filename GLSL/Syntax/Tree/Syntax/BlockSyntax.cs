using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class BlockSyntax : SyntaxNode
	{
		internal BlockSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Block, start)
		{
		}

		public SyntaxToken LeftBrace { get; private set; }

		public SyntaxToken RightBrace { get; private set; }

		public SyntaxToken SemiColon { get; private set; }

		public List<SimpleStatementSyntax> SimpleStatements { get; } = new List<SimpleStatementSyntax>();

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.LeftBraceToken:
					this.LeftBrace = node as SyntaxToken;
					break;

				case SyntaxType.SimpleStatement:
					this.SimpleStatements.Add(node as SimpleStatementSyntax);
					break;

				case SyntaxType.RightBraceToken:
					this.RightBrace = node as SyntaxToken;
					break;

				case SyntaxType.SemiColonToken:
					this.SemiColon = node as SyntaxToken;
					break;
			}
		}
	}
}