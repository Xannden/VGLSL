﻿using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Commands
{
	internal sealed class CompletionCommand : VSCommand<VSConstants.VSStd2KCmdID>
	{
		private ICompletionSession session;
		private ICompletionBroker completionBroker;
		private Source source;

		internal CompletionCommand(IVsTextView textViewAdapter, ITextView textView, ICompletionBroker completionBroker) : base(textViewAdapter, textView)
		{
			this.source = VSSource.GetOrCreate(textView.TextBuffer);

			this.completionBroker = completionBroker;

			this.AddCommand(VSConstants.VSStd2KCmdID.SHOWMEMBERLIST, VSConstants.VSStd2KCmdID.COMPLETEWORD, VSConstants.VSStd2KCmdID.TYPECHAR, VSConstants.VSStd2KCmdID.RETURN, VSConstants.VSStd2KCmdID.TAB, VSConstants.VSStd2KCmdID.BACKSPACE, VSConstants.VSStd2KCmdID.DELETE);
		}

		protected override bool IsEnabled(VSConstants.VSStd2KCmdID commandId)
		{
			return true;
		}

		protected override bool Run(VSConstants.VSStd2KCmdID commandId, ref Guid cmdGuid, uint cmdID, uint cmdexecopt, IntPtr vaIn, IntPtr vaOut)
		{
			switch (commandId)
			{
				case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
					if (this.session == null || this.session.IsDismissed)
					{
						this.TriggerCompletion();
					}

					return true;

				case VSConstants.VSStd2KCmdID.COMPLETEWORD:
					this.CompleteWord();

					return true;

				case VSConstants.VSStd2KCmdID.TYPECHAR:
					return this.TypeChar(ref cmdGuid, cmdID, cmdexecopt, vaIn, vaOut);

				case VSConstants.VSStd2KCmdID.RETURN:
				case VSConstants.VSStd2KCmdID.TAB:
					return this.Done();

				case VSConstants.VSStd2KCmdID.BACKSPACE:
				case VSConstants.VSStd2KCmdID.DELETE:
					this.RunNextCommand(ref cmdGuid, cmdID, cmdexecopt, vaIn, vaOut);

					if (!this.session?.IsDismissed ?? false)
					{
						this.session.Filter();
					}

					return true;
			}

			return false;
		}

		private bool TypeChar(ref Guid cmdGuid, uint cmdID, uint cmdexecopt, IntPtr vaIn, IntPtr vaOut)
		{
			char character = (char)(ushort)Marshal.GetObjectForNativeVariant(vaIn);

			if (char.IsWhiteSpace(character) || (char.IsPunctuation(character) && character != '_'))
			{
				return this.Done();
			}

			int returnValue = this.RunNextCommand(ref cmdGuid, cmdID, cmdexecopt, vaIn, vaOut);

			Snapshot snapshot = this.source.CurrentSnapshot;

			if ((char.IsLetterOrDigit(character) || character == '_') && !this.source.CommentSpans.Contains(span => span.GetSpan(snapshot).Contains(this.TextView.Caret.Position.BufferPosition)))
			{
				if (this.session?.IsDismissed ?? true)
				{
					this.TriggerCompletion();
				}
				else
				{
					this.session.Filter();
				}

				return true;
			}

			return returnValue == VSConstants.S_OK;
		}

		private void CompleteWord()
		{
			if (this.session == null || this.session.IsDismissed)
			{
				this.TriggerCompletion();

				if (this.session?.SelectedCompletionSet.Completions.Count <= 1)
				{
					this.session.SelectedCompletionSet.SelectBestMatch();

					this.session.Commit();
				}
			}
		}

		private void TriggerCompletion()
		{
			SnapshotPoint point = this.TextView.Caret.Position.BufferPosition;

			this.session = this.completionBroker.CreateCompletionSession(this.TextView, point.Snapshot.CreateTrackingPoint(point.Position, PointTrackingMode.Positive), true);

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