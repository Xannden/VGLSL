using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class PreprocessorSyntax : SyntaxNode
	{
		internal PreprocessorSyntax(SyntaxTree tree, int start) : base(tree, SyntaxType.Preprocessor, start)
		{
		}

		internal PreprocessorSyntax(SyntaxTree tree, TrackingSpan span) : base(tree, SyntaxType.Preprocessor, span)
		{
		}

		public DefinePreprocessorSyntax DefinePreprocessor { get; private set; }

		public ElseIfPreprocessorSyntax ElseIfPreprocessor { get; private set; }

		public ElsePreprocessorSyntax ElsePreprocessor { get; private set; }

		public EndIfPreprocessorSyntax EndIfPreprocessor { get; private set; }

		public ErrorPreprocessorSyntax ErrorPreprocessor { get; private set; }

		public ExtensionPreprocessorSyntax ExtensionPreprocessor { get; private set; }

		public IfDefinedPreprocessorSyntax IfDefinedPreprocessor { get; private set; }

		public IfNotDefinedPreprocessorSyntax IfNotDefinedPreprocessor { get; private set; }

		public IfPreprocessorSyntax IfPreprocessor { get; private set; }

		public LinePreprocessorSyntax LinePreprocessor { get; private set; }

		public PragmaPreprocessorSyntax PragmaPreprocessor { get; private set; }

		public UndefinePreprocessorSyntax UnDefinePreprocessor { get; private set; }

		public VersionPreprocessorSyntax VersionPreprocessor { get; private set; }

		protected override void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.DefinePreprocessor:
					this.DefinePreprocessor = node as DefinePreprocessorSyntax;
					break;

				case SyntaxType.UndefinePreprocessor:
					this.UnDefinePreprocessor = node as UndefinePreprocessorSyntax;
					break;

				case SyntaxType.IfPreprocessor:
					this.IfPreprocessor = node as IfPreprocessorSyntax;
					break;

				case SyntaxType.IfDefinedPreprocessor:
					this.IfDefinedPreprocessor = node as IfDefinedPreprocessorSyntax;
					break;

				case SyntaxType.IfNotDefinedPreprocessor:
					this.IfNotDefinedPreprocessor = node as IfNotDefinedPreprocessorSyntax;
					break;

				case SyntaxType.ElsePreprocessor:
					this.ElsePreprocessor = node as ElsePreprocessorSyntax;
					break;

				case SyntaxType.ElseIfPreprocessor:
					this.ElseIfPreprocessor = node as ElseIfPreprocessorSyntax;
					break;

				case SyntaxType.EndIfPreprocessor:
					this.EndIfPreprocessor = node as EndIfPreprocessorSyntax;
					break;

				case SyntaxType.ErrorPreprocessor:
					this.ErrorPreprocessor = node as ErrorPreprocessorSyntax;
					break;

				case SyntaxType.PragmaPreprocessor:
					this.PragmaPreprocessor = node as PragmaPreprocessorSyntax;
					break;

				case SyntaxType.ExtensionPreprocessor:
					this.ExtensionPreprocessor = node as ExtensionPreprocessorSyntax;
					break;

				case SyntaxType.VersionPreprocessor:
					this.VersionPreprocessor = node as VersionPreprocessorSyntax;
					break;

				case SyntaxType.LinePreprocessor:
					this.LinePreprocessor = node as LinePreprocessorSyntax;
					break;
			}
		}
	}
}