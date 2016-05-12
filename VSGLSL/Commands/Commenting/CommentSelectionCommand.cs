using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Commands
{
	internal sealed class CommentSelectionCommand : VSCommand<VSConstants.VSStd2KCmdID>
	{
		public CommentSelectionCommand(IVsTextView textViewAdapter, ITextView textView) : base(textViewAdapter, textView)
		{
			this.AddCommand(VSConstants.VSStd2KCmdID.COMMENTBLOCK, VSConstants.VSStd2KCmdID.COMMENT_BLOCK);
		}

		protected override bool IsEnabled(VSConstants.VSStd2KCmdID commandId)
		{
			return true;
		}

		protected override bool Run(VSConstants.VSStd2KCmdID commandId)
		{
			this.TextView.CommentSelection(true);

			return true;
		}
	}
}
