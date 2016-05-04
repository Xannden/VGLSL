using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Commands
{
	internal sealed class CommentSelectionCommand : VSCommand<VSConstants.VSStd2KCmdID>
	{
		internal CommentSelectionCommand(IVsTextView textViewAdapter, ITextView textView) : base(textViewAdapter, textView, VSConstants.VSStd2KCmdID.COMMENTBLOCK, VSConstants.VSStd2KCmdID.COMMENT_BLOCK)
		{
		}

		protected override bool IsEnabled(VSConstants.VSStd2KCmdID commandId)
		{
			return true;
		}

		protected override void Run()
		{
			this.TextView.CommentSelection(true);
		}
	}
}
