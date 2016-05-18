using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.BraceCompletion;

namespace Xannden.VSGLSL.Formatting.BraceCompletion
{
	internal sealed class GLSLBraceCompletionContext : IBraceCompletionContext
	{
		public bool AllowOverType(IBraceCompletionSession session)
		{
			return true;
		}

		public void Finish(IBraceCompletionSession session)
		{
		}

		public void OnReturn(IBraceCompletionSession session)
		{
			ITextSnapshot snapshot = session.SubjectBuffer.CurrentSnapshot;
			SnapshotPoint start = session.OpeningPoint.GetPoint(snapshot);
			SnapshotPoint end = session.ClosingPoint.GetPoint(snapshot);

			for (int i = start.Position + 1; i < end.Position - 1; i++)
			{
				if (!char.IsWhiteSpace(snapshot[i]))
				{
					return;
				}
			}

			session.SubjectBuffer.Insert(end.Position - 1, Environment.NewLine);
		}

		public void Start(IBraceCompletionSession session)
		{
		}
	}
}
