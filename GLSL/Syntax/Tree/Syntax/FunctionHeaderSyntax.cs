﻿using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class FunctionHeaderSyntax : SyntaxNode
	{
		internal FunctionHeaderSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.FunctionHeader, start)
		{
		}

		internal FunctionHeaderSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.FunctionHeader, span)
		{
		}

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken LeftParentheses { get; private set; }

		public TokenSeparatedList<ParameterSyntax> Parameters { get; } = new TokenSeparatedList<ParameterSyntax>();

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
					this.Parameters.AddNode(node as ParameterSyntax);
					break;

				case SyntaxType.CommaToken:
					this.Parameters.AddToken(node as SyntaxToken);
					break;

				case SyntaxType.RightParenToken:
					this.RightParentheses = node as SyntaxToken;
					break;
			}
		}
	}
}