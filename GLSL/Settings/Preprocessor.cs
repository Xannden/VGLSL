using Xannden.GLSL.Syntax.Tree;

namespace Xannden.GLSL.Settings
{
	internal class Preprocessor
	{
		internal Preprocessor(SyntaxToken keyword, bool value)
		{
			this.Keyword = keyword;
			this.Value = value;
		}

		public SyntaxToken Keyword { get; }

		public bool Value { get; set; }
	}
}