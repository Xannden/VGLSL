﻿namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class EndIfPreprocessorSyntax : SyntaxNode
	{
		internal EndIfPreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.EndIfPreprocessor, start)
		{
		}

		public SyntaxToken EndIfKeyword { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.EndIfPreprocessorKeyword:
					this.EndIfKeyword = node as SyntaxToken;
					break;
			}
		}
	}
}