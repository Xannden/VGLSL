using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Preprocessors
{
	internal class PreprocessorSuggestedAction : ISuggestedAction
	{
		private readonly PreprocessorSuggestedActionsSource actionSource;
		private readonly bool newValue;
		private readonly int position;
		private readonly Source source;

		public PreprocessorSuggestedAction(bool newValue, Source source, int position, PreprocessorSuggestedActionsSource actionSource)
		{
			this.newValue = newValue;
			this.source = source;
			this.position = position;
			this.DisplayText = $"Set Preprocessor to {this.newValue.ToString()}";
			this.actionSource = actionSource;
		}

		public string DisplayText { get; }

		public bool HasActionSets => false;

		public bool HasPreview => false;

		public string IconAutomationText => null;

		public ImageMoniker IconMoniker => default(ImageMoniker);

		public string InputGestureText => null;

		public void Dispose()
		{
			// Method intentionally left empty.
		}

		public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult<IEnumerable<SuggestedActionSet>>(null);
		}

		public Task<object> GetPreviewAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult<object>(null);
		}

		public void Invoke(CancellationToken cancellationToken)
		{
			Snapshot snapshot = this.source.CurrentSnapshot;

			IfPreprocessor preprocessor = this.source.Settings.GetPreprocessor(snapshot, this.position);

			if (this.newValue)
			{
				preprocessor.SetAllValues(false);
			}

			preprocessor.GetPreprocessor(snapshot, this.position).Value = this.newValue;

			preprocessor.CheckForElse();

			this.actionSource.Update();

			(this.source as VSSource)?.Parse();
		}

		public bool TryGetTelemetryId(out Guid telemetryId)
		{
			telemetryId = Guid.Empty;
			return false;
		}
	}
}