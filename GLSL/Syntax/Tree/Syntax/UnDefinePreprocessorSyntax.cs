﻿namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class UndefinePreprocessorSyntax : SyntaxNode
	{
		public UndefinePreprocessorSyntax() : base(SyntaxType.UndefinePreprocessor)
		{
		}

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken UndefineKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.UndefinePreprocessorKeyword:
					this.UndefineKeyword = node as SyntaxToken;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;
			}
		}
	}
}