using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Xannden.GLSL.Text;

namespace Xannden.VSGLSL.Preprocessors
{
	internal class PreprocessorSuggestedActionsSource : ISuggestedActionsSource
	{
		private Source source;
		private ITextView textView;

		public PreprocessorSuggestedActionsSource(Source source, ITextView textView)
		{
			this.source = source;
			this.textView = textView;
		}

		public event EventHandler<EventArgs> SuggestedActionsChanged;

		public void Dispose()
		{
		}

		public IEnumerable<SuggestedActionSet> GetSuggestedActions(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
		{
			Snapshot snapshot = this.source.CurrentSnapshot;

			if (this.source.Settings.IsPreprocessor(snapshot, this.textView.Caret.Position.BufferPosition.Position))
			{
				yield return new SuggestedActionSet(new ISuggestedAction[] { new PreprocessorSuggestedAction(!this.source.Settings.GetPreprocessorValue(snapshot, this.textView.Caret.Position.BufferPosition.Position), this.source, this.textView.Caret.Position.BufferPosition.Position, this) });
			}
		}

		public Task<bool> HasSuggestedActionsAsync(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
		{
			Snapshot snapshot = this.source.CurrentSnapshot;

			return Task.Factory.StartNew(() => this.source.Settings.IsPreprocessor(snapshot, this.textView.Caret.Position.BufferPosition.Position));
		}

		public bool TryGetTelemetryId(out Guid telemetryId)
		{
			telemetryId = Guid.Empty;
			return false;
		}

		internal void Update()
		{
			this.SuggestedActionsChanged?.Invoke(this, new EventArgs());
		}
	}
}