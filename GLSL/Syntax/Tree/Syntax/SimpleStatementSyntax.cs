namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class SimpleStatementSyntax : SyntaxNode
	{
		internal SimpleStatementSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.SimpleStatement, start)
		{
		}

		public CaseLabelSyntax CaseLabelStatement { get; private set; }

		public DeclarationSyntax Declaration { get; private set; }

		public ExpressionStatementSyntax ExpressionStatement { get; private set; }

		public FunctionStatementSyntax FunctionStatement { get; private set; }

		public IterationStatementSyntax IterationStatement { get; private set; }

		public JumpStatementSyntax JumpStatement { get; private set; }

		public SelectionStatementSyntax SelectionStatement { get; private set; }

		public SwitchStatementSyntax SwitchStatement { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Declaration:
					this.Declaration = node as DeclarationSyntax;
					break;

				case SyntaxType.SelectionStatement:
					this.SelectionStatement = node as SelectionStatementSyntax;
					break;

				case SyntaxType.SwitchStatement:
					this.SwitchStatement = node as SwitchStatementSyntax;
					break;

				case SyntaxType.CaseLabel:
					this.CaseLabelStatement = node as CaseLabelSyntax;
					break;

				case SyntaxType.IterationStatement:
					this.IterationStatement = node as IterationStatementSyntax;
					break;

				case SyntaxType.JumpStatement:
					this.JumpStatement = node as JumpStatementSyntax;
					break;

				case SyntaxType.ExpressionStatement:
					this.ExpressionStatement = node as ExpressionStatementSyntax;
					break;

				case SyntaxType.FunctionStatement:
					this.FunctionStatement = node as FunctionStatementSyntax;
					break;
			}
		}
	}
}