using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Commands
{
	internal sealed class GLSLCommandHandlers : IOleCommandTarget
	{
		private readonly IOleCommandTarget nextCommandHandler;
		private readonly GLSLCommandHandlersProvider provider;
		private ITextView textView;

		public GLSLCommandHandlers(GLSLCommandHandlersProvider provider, IVsTextView textViewAdapter, ITextView textView)
		{
			this.provider = provider;
			this.textView = textView;
			textViewAdapter.AddCommandFilter(this, out this.nextCommandHandler);
		}

		public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
			{
				switch ((VSConstants.VSStd2KCmdID)nCmdID)
				{
					case VSConstants.VSStd2KCmdID.COMMENTBLOCK:
					case VSConstants.VSStd2KCmdID.COMMENT_BLOCK:
						this.textView.CommentSelection(true);
						break;
					case VSConstants.VSStd2KCmdID.UNCOMMENTBLOCK:
					case VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK:
						this.textView.CommentSelection(false);
						break;
					case VSConstants.VSStd2KCmdID.QUICKINFO:
						ITrackingPoint triggerPoint = this.textView.TextSnapshot.CreateTrackingPoint(this.textView.Caret.Position.BufferPosition.Position, PointTrackingMode.Negative);

						this.provider.QuickInfoBroker.TriggerQuickInfo(this.textView, triggerPoint, false);
						break;
				}
			}
			else if (pguidCmdGroup == typeof(VSConstants.VSStd97CmdID).GUID)
			{
				switch ((VSConstants.VSStd97CmdID)nCmdID)
				{
					case VSConstants.VSStd97CmdID.GotoDefn:
						break;
				}
			}

			return this.nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}

		public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
		{
#pragma warning disable RECS0016 // Bitwise operation on enum which has no [Flags] attribute
			if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
			{
				for (int i = 0; i < cCmds; i++)
				{
					switch ((VSConstants.VSStd2KCmdID)prgCmds[i].cmdID)
					{
						case VSConstants.VSStd2KCmdID.COMMENT_BLOCK:
						case VSConstants.VSStd2KCmdID.COMMENTBLOCK:
						case VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK:
						case VSConstants.VSStd2KCmdID.UNCOMMENTBLOCK:
						case VSConstants.VSStd2KCmdID.QUICKINFO:

							prgCmds[i].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);
							return VSConstants.S_OK;
					}
				}
			}
			else if (pguidCmdGroup == typeof(VSConstants.VSStd97CmdID).GUID)
			{
				for (int i = 0; i < prgCmds.Length; i++)
				{
					switch ((VSConstants.VSStd97CmdID)prgCmds[i].cmdID)
					{
						case VSConstants.VSStd97CmdID.GotoDefn:
							prgCmds[i].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);

							return VSConstants.S_OK;
					}
				}
			}
#pragma warning restore RECS0016 // Bitwise operation on enum which has no [Flags] attribute

			return this.nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}
	}
}
