using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Commands
{
	internal class GoToDefinitionCommand : VSCommand<VSConstants.VSStd97CmdID>
	{
		private readonly VSSource source;

		internal GoToDefinitionCommand(IVsTextView textViewAdapter, ITextView textView) : base(textViewAdapter, textView, VSConstants.VSStd97CmdID.GotoDefn)
		{
			this.source = VSSource.GetOrCreate(textView.TextBuffer);
		}

		protected override bool IsEnabled(VSConstants.VSStd97CmdID commandId)
		{
			return true;
		}

		protected override void Run()
		{
			SyntaxTree tree = this.source.Tree;

			if (tree == null)
			{
				return;
			}

			VSSnapshot snapshot = this.source.CurrentSnapshot as VSSnapshot;
			int position = this.TextView.Caret.Position.BufferPosition.Position;

			IdentifierSyntax identifier = tree.GetNodeFromPosition(snapshot, position) as IdentifierSyntax;

			Span span = identifier?.Definition.Span?.GetSpan(snapshot);

			if (span == null)
			{
				return;
			}

			this.TextView.Caret.MoveTo(new Microsoft.VisualStudio.Text.SnapshotPoint(snapshot.TextSnapshot, span.Start));
		}
	}
}
