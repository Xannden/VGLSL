using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Commands
{
	[InheritedExport(typeof(ICommand))]
	internal abstract class VSCommand<T> : IOleCommandTarget, ICommand where T : struct, IComparable
	{
		private IOleCommandTarget nextCommand;
		private Guid commandGroup;
		private List<uint> commandIds = new List<uint>();

		protected VSCommand()
		{
		}

		protected ITextView TextView { get; private set; }

		public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			if (pguidCmdGroup == this.commandGroup && this.commandIds.Contains(nCmdID))
			{
				if (this.Run((T)(object)(int)nCmdID, ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut))
				{
					return VSConstants.S_OK;
				}
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

		public void Create(IVsTextView textViewAdapter, ITextView textView)
		{
			this.TextView = textView;
			this.commandGroup = typeof(T).GUID;

			textViewAdapter.AddCommandFilter(this, out this.nextCommand);

			this.Initilize();
		}

		protected void AddCommand(params T[] commands)
		{
			this.commandIds.AddRange(commands.ConvertAll(item => Convert.ToUInt32(item)));
		}

		protected int RunNextCommand(ref Guid cmdGuid, uint cmdID, uint cmdexecopt, IntPtr vaIn, IntPtr vaOut)
		{
			return this.nextCommand.Exec(ref cmdGuid, cmdID, cmdexecopt, vaIn, vaOut);
		}

		protected abstract bool IsEnabled(T commandId);

		protected abstract void Initilize();

		protected virtual bool Run(T commandId, ref Guid cmdGuid, uint cmdID, uint cmdexecopt, IntPtr vaIn, IntPtr vaOut)
		{
			return this.Run(commandId);
		}

		protected virtual bool Run(T commandId)
		{
			return false;
		}
	}
}