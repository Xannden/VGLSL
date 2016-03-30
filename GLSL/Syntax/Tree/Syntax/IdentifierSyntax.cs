namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	internal class IdentifierSyntax : SyntaxToken
	{
		public IdentifierSyntax() : base(SyntaxType.IdentifierToken)
		{
		}

		public string Name => this.Text;
	}
}