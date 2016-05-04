using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Xannden.VSGLSL.IntelliSense.Completions
{
	internal class GLSLCompletionCommandHandler : IOleCommandTarget
	{
		private readonly ITextView textView;
		private readonly GLSLCompletionCommandHandlerProvider provider;
		private IOleCommandTarget nextCommandHandler;
		private ICompletionSession session;

		internal GLSLCompletionCommandHandler(IVsTextView textViewAdapter, ITextView textView, GLSLCompletionCommandHandlerProvider provider)
		{
			this.textView = textView;
			this.provider = provider;

			textViewAdapter.AddCommandFilter(this, out this.nextCommandHandler);
		}

		public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			switch ((VSConstants.VSStd2KCmdID)nCmdID)
			{
				case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
					if (this.session == null || this.session.IsDismissed)
					{
						this.TriggerCompletion();
					}

					return VSConstants.S_OK;

				case VSConstants.VSStd2KCmdID.COMPLETEWORD:
					this.CompleteWord();

					return VSConstants.S_OK;

				case VSConstants.VSStd2KCmdID.TYPECHAR:
					return this.TypeChar(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

				case VSConstants.VSStd2KCmdID.RETURN:
				case VSConstants.VSStd2KCmdID.TAB:
					if (this.Done())
					{
						return VSConstants.S_OK;
					}

					break;

				case VSConstants.VSStd2KCmdID.BACKSPACE:
				case VSConstants.VSStd2KCmdID.DELETE:
					this.nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

					if (!this.session?.IsDismissed ?? false)
					{
						this.session.Filter();
					}

					return VSConstants.S_OK;
			}

			return this.nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}

		public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
		{
			if (pguidCmdGroup == VSConstants.VSStd2K)
			{
				for (int i = 0; i < cCmds; i++)
				{
					switch ((VSConstants.VSStd2KCmdID)prgCmds[i].cmdID)
					{
						case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
						case VSConstants.VSStd2KCmdID.COMPLETEWORD:
#pragma warning disable RECS0016 // Bitwise operation on enum which has no [Flags] attribute
							prgCmds[i].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);
#pragma warning restore RECS0016 // Bitwise operation on enum which has no [Flags] attribute
							return VSConstants.S_OK;
					}
				}
			}

			return this.nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}

		private int TypeChar(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			char character = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);

			if (char.IsWhiteSpace(character) || (char.IsPunctuation(character) && character != '_'))
			{
				this.Done();
			}

			int returnValue = this.nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

			if (char.IsLetterOrDigit(character) || character == '_')
			{
				if (this.session?.IsDismissed ?? true)
				{
					this.TriggerCompletion();
				}
				else
				{
					this.session.Filter();
				}

				return VSConstants.S_OK;
			}

			return returnValue;
		}

		private void CompleteWord()
		{
			if (this.session == null || this.session.IsDismissed)
			{
				this.TriggerCompletion();

				if (this.session.SelectedCompletionSet.Completions.Count <= 1)
				{
					this.session.SelectedCompletionSet.SelectBestMatch();

					this.session.Commit();
				}
			}
		}

		private void TriggerCompletion()
		{
			SnapshotPoint point = this.textView.Caret.Position.BufferPosition;

			this.session = this.provider.CompletionBroker.CreateCompletionSession(this.textView, point.Snapshot.CreateTrackingPoint(point.Position, PointTrackingMode.Positive), true);

			this.session.Dismissed += this.OnSessionDismissed;
			this.session.Start();

			this.session.Filter();

			return;
		}

		private void OnSessionDismissed(object sender, EventArgs e)
		{
			this.session.Dismissed -= this.OnSessionDismissed;
			this.session = null;
		}

		private bool Done()
		{
			if (!this.session?.IsDismissed ?? false)
			{
				if (this.session.SelectedCompletionSet.SelectionStatus.IsSelected)
				{
					this.session.Commit();

					return true;
				}

				this.session.Dismiss();
			}

			return false;
		}
	}
}