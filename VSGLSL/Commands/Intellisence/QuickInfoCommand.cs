using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Xannden.VSGLSL.Commands
{
	internal sealed class QuickInfoCommand : VSCommand<VSConstants.VSStd2KCmdID>
	{
		private readonly IQuickInfoBroker quickInfoBroker;

		internal QuickInfoCommand(IVsTextView textViewAdapter, ITextView textView, IQuickInfoBroker quickInfoBroker) : base(textViewAdapter, textView, VSConstants.VSStd2KCmdID.QUICKINFO)
		{
			this.quickInfoBroker = quickInfoBroker;
		}

		protected override bool IsEnabled(VSConstants.VSStd2KCmdID commandId)
		{
			return true;
		}

		protected override void Run()
		{
			ITrackingPoint triggerPoint = this.TextView.TextSnapshot.CreateTrackingPoint(this.TextView.Caret.Position.BufferPosition.Position, PointTrackingMode.Negative);

			this.quickInfoBroker.TriggerQuickInfo(this.TextView, triggerPoint, false);
		}
	}
}
