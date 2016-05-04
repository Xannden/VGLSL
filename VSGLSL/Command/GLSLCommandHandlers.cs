using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Command
{
	internal sealed class GLSLCommandHandlers : IOleCommandTarget
	{
		private readonly IOleCommandTarget nextCommandHandler;
		private ITextView textView;

		public GLSLCommandHandlers(IVsTextView textViewAdapter, ITextView textView)
		{
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
				}
			}

			return this.nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}

		public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
		{
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
#pragma warning disable RECS0016 // Bitwise operation on enum which has no [Flags] attribute
							prgCmds[i].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);
#pragma warning restore RECS0016 // Bitwise operation on enum which has no [Flags] attribute
							return VSConstants.S_OK;
					}
				}
			}

			return this.nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}
	}
}
