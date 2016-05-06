using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace Xannden.VSGLSL.Commands
{
	internal sealed class QuickInfoCommand : VSCommand<VSConstants.VSStd2KCmdID>
	{
		[Import]
		private IQuickInfoBroker QuickInfoBroker { get; set; }

		protected override void Initilize()
		{
			this.AddCommand(VSConstants.VSStd2KCmdID.QUICKINFO);
		}

		protected override bool IsEnabled(VSConstants.VSStd2KCmdID commandId)
		{
			return true;
		}

		protected override bool Run(VSConstants.VSStd2KCmdID commandId)
		{
			ITrackingPoint triggerPoint = this.TextView.TextSnapshot.CreateTrackingPoint(this.TextView.Caret.Position.BufferPosition.Position, PointTrackingMode.Negative);

			this.QuickInfoBroker.TriggerQuickInfo(this.TextView, triggerPoint, false);

			return true;
		}
	}
}
