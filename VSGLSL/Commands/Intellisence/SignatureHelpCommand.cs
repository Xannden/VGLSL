using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace Xannden.VSGLSL.Commands.Intellisence
{
	internal sealed class SignatureHelpCommand : VSCommand<VSConstants.VSStd2KCmdID>
	{
		private ISignatureHelpSession session;

		[Import]
		private ISignatureHelpBroker SignatureHelpBroker { get; set; }

		protected override void Initilize()
		{
			this.AddCommand(VSConstants.VSStd2KCmdID.TYPECHAR, VSConstants.VSStd2KCmdID.PARAMINFO);
		}

		protected override bool IsEnabled(VSConstants.VSStd2KCmdID commandId)
		{
			return true;
		}

		protected override bool Run(VSConstants.VSStd2KCmdID commandId, ref Guid cmdGuid, uint cmdID, uint cmdexecopt, IntPtr vaIn, IntPtr vaOut)
		{
			if (commandId == VSConstants.VSStd2KCmdID.PARAMINFO)
			{
				this.TriggerParameterHelp(this.TextView.Caret.Position.BufferPosition);
			}
			else
			{
				char character = (char)(ushort)Marshal.GetObjectForNativeVariant(vaIn);

				if (character == '(')
				{
					this.TriggerParameterHelp(this.TextView.Caret.Position.BufferPosition - 1);
				}
				else if (character == ')')
				{
					this.session?.Dismiss();
					this.session = null;
				}
			}

			return false;
		}

		private void TriggerParameterHelp(int position)
		{
			ITrackingPoint triggerPoint = this.TextView.TextSnapshot.CreateTrackingPoint(position, PointTrackingMode.Positive);

			this.session?.Dismiss();

			this.session = this.SignatureHelpBroker.TriggerSignatureHelp(this.TextView, triggerPoint, true);
		}
	}
}
