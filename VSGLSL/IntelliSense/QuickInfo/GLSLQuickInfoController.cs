using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.VSGLSL.Intellisense.QuickInfo
{
	internal class GLSLQuickInfoController : IIntellisenseController
	{
		private readonly GLSLQuickInfoControllerProvider provider;
		private readonly IList<ITextBuffer> subjectBuffers;
		private readonly Source source;
		private ITextView textView;

		public GLSLQuickInfoController(GLSLQuickInfoControllerProvider provider, Source source, ITextView textView, IList<ITextBuffer> subjectBuffers)
		{
			this.provider = provider;
			this.textView = textView;
			this.subjectBuffers = subjectBuffers;
			this.source = source;

			textView.MouseHover += this.OnTextViewMouseHover;
		}

		public void Detach(ITextView textView)
		{
			if (textView == null)
			{
				throw new ArgumentNullException(nameof(textView));
			}

			if (textView == this.textView)
			{
				this.textView.MouseHover -= this.OnTextViewMouseHover;
				this.textView = null;
			}
		}

		public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
		{
		}

		public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
		{
		}

		private void OnTextViewMouseHover(object sender, MouseHoverEventArgs e)
		{
			SnapshotPoint? point = this.textView.BufferGraph.MapDownToFirstMatch(new SnapshotPoint(this.textView.TextSnapshot, e.Position), PointTrackingMode.Positive, snapshot => this.subjectBuffers.Contains(snapshot.TextBuffer), PositionAffinity.Predecessor);

			if (point != null)
			{
				ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(point.Value.Position, PointTrackingMode.Positive);

				if (this.provider.QuickInfoBroker.IsQuickInfoActive(this.textView))
				{
					return;
				}

				Snapshot snapshot = this.source.CurrentSnapshot;

				IdentifierSyntax identifier = this.source.Tree?.GetNodeFromPosition(snapshot, point.Value.Position) as IdentifierSyntax;

				if (identifier?.Definition != null)
				{
					this.provider.QuickInfoBroker.TriggerQuickInfo(this.textView, triggerPoint, true);
				}
			}
		}
	}
}