using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class LayoutQualifierIdSyntax : SyntaxNode
	{
		internal LayoutQualifierIdSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.LayoutQualifierId, span)
		{
		}

		internal LayoutQualifierIdSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.LayoutQualifierId, start)
		{
		}

		public SyntaxToken SharedKeyword { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public SyntaxToken Equal { get; private set; }

		public ConstantExpressionSyntax ConstantExpression { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.SharedKeyword:
					this.SharedKeyword = node as SyntaxToken;
					break;
				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;
				case SyntaxType.EqualToken:
					this.Equal = node as SyntaxToken;
					break;
				case SyntaxType.ConstantExpression:
					this.ConstantExpression = node as ConstantExpressionSyntax;
					break;
			}
		}
	}
}