using Microsoft.VisualStudio;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Commands
{
	internal sealed class UnCommentSelectionCommand : VSCommand<VSConstants.VSStd2KCmdID>
	{
		protected override void Initilize()
		{
			this.AddCommand(VSConstants.VSStd2KCmdID.UNCOMMENTBLOCK, VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK);
		}

		protected override bool IsEnabled(VSConstants.VSStd2KCmdID commandId)
		{
			return true;
		}

		protected override bool Run(VSConstants.VSStd2KCmdID commandId)
		{
			this.TextView.CommentSelection(false);

			return true;
		}
	}
}
