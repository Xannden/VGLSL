using System;
using System.Globalization;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Commands
{
	internal abstract class VSCommand<T> : IOleCommandTarget where T : struct, IComparable
	{
		private IOleCommandTarget nextCommand;
		private Guid commandGroup;
		private uint[] commandIds;

		protected VSCommand(IVsTextView textViewAdapter, ITextView textView, params T[] commandIds) : this(textViewAdapter, textView, commandIds.ConvertAll(item => Convert.ToUInt32(item, CultureInfo.InvariantCulture)))
		{
		}

		protected VSCommand(IVsTextView textViewAdapter, ITextView textView, params uint[] commandIds)
		{
			this.TextView = textView;
			this.commandGroup = typeof(T).GUID;
			this.commandIds = commandIds;

			textViewAdapter.AddCommandFilter(this, out this.nextCommand);
		}

		protected ITextView TextView { get; }

		public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			if (pguidCmdGroup == this.commandGroup && this.commandIds.Contains(nCmdID))
			{
				this.Run();

				return VSConstants.S_OK;
			}

			return this.nextCommand.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}

		public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
		{
			if (pguidCmdGroup != this.commandGroup)
			{
				return this.nextCommand.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
			}

#pragma warning disable RECS0016
			for (int i = 0; i < prgCmds.Length; i++)
			{
				if (this.commandIds.Contains(prgCmds[i].cmdID))
				{
					if (this.IsEnabled((T)(object)(int)prgCmds[i].cmdID))
					{
						prgCmds[i].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);

						return VSConstants.S_OK;
					}

					prgCmds[i].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED;
				}
			}
#pragma warning restore RECS0016

			return this.nextCommand.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}

		protected abstract bool IsEnabled(T commandId);

		protected abstract void Run();
	}
}