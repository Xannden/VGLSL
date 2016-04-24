using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class InitializerSyntax : SyntaxNode
	{
		internal InitializerSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Initializer, start)
		{
		}

		internal InitializerSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Initializer, span)
		{
		}

		public SyntaxNode Initializer { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.AssignmentExpression:
				case SyntaxType.InitList:
					this.Initializer = node;
					break;
			}
		}
	}
}