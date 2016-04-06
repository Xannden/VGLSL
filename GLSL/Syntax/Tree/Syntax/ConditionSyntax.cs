using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class ConditionSyntax : SyntaxNode
	{
		internal ConditionSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Condition, start)
		{
		}

		internal ConditionSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Condition, span)
		{
		}

		public SyntaxToken Equal { get; private set; }

		public ExpressionSyntax Expression { get; private set; }

		public IdentifierSyntax Identifier { get; private set; }

		public InitializerSyntax Initializer { get; private set; }

		public TypeSyntax TypeSyntax { get; private set; }

		public TypeQualifierSyntax TypeQualifier { get; private set; }

		internal override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Expression:
					this.Expression = node as ExpressionSyntax;
					break;

				case SyntaxType.TypeQualifier:
					this.TypeQualifier = node as TypeQualifierSyntax;
					break;

				case SyntaxType.Type:
					this.TypeSyntax = node as TypeSyntax;
					break;

				case SyntaxType.IdentifierToken:
					this.Identifier = node as IdentifierSyntax;
					break;

				case SyntaxType.EqualToken:
					this.Equal = node as SyntaxToken;
					break;

				case SyntaxType.Initializer:
					this.Initializer = node as InitializerSyntax;
					break;
			}
		}
	}
}