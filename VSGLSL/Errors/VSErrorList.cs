using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Errors
{
	internal sealed class VSErrorList : IDisposable
	{
		private readonly ErrorListProvider errorListProvider;
		private readonly IServiceProvider provider;
		private readonly VSSource source;

		public VSErrorList(IServiceProvider provider, ITextView textView)
		{
			this.source = VSSource.GetOrCreate(textView.TextBuffer);
			this.provider = provider;
			this.errorListProvider = new ErrorListProvider(provider);

			this.source.DoneParsing += this.OnDoneParsing;
			textView.Closed += this.OnClosed;
		}

		public void Dispose()
		{
			this.errorListProvider.Tasks.Clear();
			this.errorListProvider.Dispose();
		}

		private void OnDoneParsing(object sender, EventArgs e)
		{
			this.errorListProvider.Tasks.Clear();

			IReadOnlyList<GLSLError> errors = this.source.Tree?.Errors;
			Snapshot snapshot = this.source.CurrentSnapshot;

			for (int i = 0; i < errors.Count; i++)
			{
				this.AddError(errors[i], snapshot);
			}
		}

		private void AddError(GLSLError error, Snapshot snapshot)
		{
			SourceLine line = snapshot.GetLineFromPosition(error.Span.Start);

			ErrorTask task = new ErrorTask
			{
				Text = error.Message,
				Line = line.LineNumber,
				Column = error.Span.Start - line.Span.Start,
				Category = TaskCategory.CodeSense,
				ErrorCategory = TaskErrorCategory.Error,
				Priority = TaskPriority.Low,
				Document = snapshot.FileName
			};

			task.Navigate += this.OnNavigate;

			this.errorListProvider.Tasks.Add(task);
		}

		private void OnClosed(object sender, EventArgs e)
		{
			this.errorListProvider.Tasks.Clear();
		}

		private void OnNavigate(object sender, EventArgs e)
		{
			ErrorTask task = sender as ErrorTask;

			this.provider.NavigateTo(task.Document, task.Line, task.Column);
		}
	}
}