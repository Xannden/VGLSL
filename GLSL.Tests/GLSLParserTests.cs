using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Parsing;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Test.Text;

namespace Xannden.GLSL.Tests
{
	[TestClass]
	public sealed class GLSLParserTests
	{
		[TestMethod]
		public void FullParse()
		{
			string[] lines = File.ReadAllLines("test.glsl");

			GLSLLexer lexer = new GLSLLexer();

			MultiLineTextSource source = MultiLineTextSource.FromString(lines, true);

			GLSLParser parser = new GLSLParser(source.Settings);

			LinkedList<Token> tokens = lexer.Run(source.CurrentSnapshot);

			SyntaxTree tree = parser.Run(source.CurrentSnapshot, tokens);

			tree.WriteToXml("tree.xml", source.CurrentSnapshot);
		}
	}
}
