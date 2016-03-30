using System;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			SyntaxNode root = new SyntaxNode(SyntaxType.LeftBraceToken);
			root.AddChild(new SyntaxNode(SyntaxType.IntKeyword));
			root.AddChild(new SyntaxNode(SyntaxType.IdentifierToken));
			root.AddChild(new SyntaxNode(SyntaxType.SemiColonToken));
			root.AddChild(new SyntaxNode(SyntaxType.RightBraceToken));

			SyntaxTree tree = new SyntaxTree(root);

			tree.DebugWrite(Console.Out);
		}
	}
}
