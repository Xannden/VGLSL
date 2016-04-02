using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class FunctionHeaderSyntax : SyntaxNode
	{
		internal FunctionHeaderSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FunctionHeader, start)
		{
		}

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public List<ParameterSyntax> Parameters { get; } = new List<ParameterSyntax>();

		public ReturnTypeSyntax ReturnType { get; private set; }

		public SyntaxToken RightParentheses { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.ReturnType:
					this.ReturnType = node as ReturnTypeSyntax;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.LeftParenToken:
					this.LeftParentheses = node as SyntaxToken;
					break;

				case SyntaxType.Parameter:
					this.Parameters.Add(node as ParameterSyntax);
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}