using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax.Semantics;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.IntelliSense.QuickTips
{
	internal class GLSLQuickInfoController : IIntellisenseController
	{
		private GLSLQuickInfoControllerProvider provider;
		private ITextView textView;
		private IList<ITextBuffer> subjectBuffers;
		private Source source;

		public GLSLQuickInfoController(GLSLQuickInfoControllerProvider provider, ITextView textView, IList<ITextBuffer> subjectBuffers)
		{
			this.provider = provider;
			this.textView = textView;
			this.subjectBuffers = subjectBuffers;
			this.source = VSSource.GetOrCreate(textView.TextBuffer);

			textView.MouseHover += this.OnTextViewMouseHover;
		}

		public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
		{
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

				IdentifierSyntax identifier = this.source.Tree.GetNodeFromPosition(snapshot, point.Value.Position) as IdentifierSyntax;

				if (identifier == null)
				{
					return;
				}

				Definition definition = this.source.Tree.Definitions.Find(def => def.Scope.GetSpan(snapshot).Contains(identifier.Span.GetSpan(snapshot)) && identifier.Identifier == def.Identifier.Identifier);

				if (definition != null)
				{
					this.provider.QuickInfoBroker.TriggerQuickInfo(this.textView, triggerPoint, true);
				}
			}
		}
	}
}
