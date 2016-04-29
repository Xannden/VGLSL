using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Xannden.GLSL.Errors;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Errors
{
	internal sealed class ErrorTagger : ITagger<ErrorTag>
	{
		private readonly ErrorHandler handler;
		private readonly VSSource source;

		public ErrorTagger(ErrorHandler handler, VSSource source)
		{
			this.handler = handler;
			this.source = source;

			this.source.DoneParsing += this.Source_DoneParsing;
		}

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

		public IEnumerable<ITagSpan<ErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			IReadOnlyList<GLSLError> errors = this.handler.Errors;

			for (int i = 0; i < errors.Count; i++)
			{
				foreach (SnapshotSpan span in spans)
				{
					if (errors[i].Span.Overlaps(span) && errors[i].Span.End < span.Snapshot.Length)
					{
						yield return new TagSpan<ErrorTag>(new SnapshotSpan(span.Snapshot, errors[i].Span.ToVSSpan()), new ErrorTag("syntaxError", errors[i].Message));
					}
				}
			}
		}

		private void Source_DoneParsing(object sender, EventArgs e)
		{
			this.TagsChanged.Invoke(this, new SnapshotSpanEventArgs(this.source.CurrentSnapshot.GetSnapshotSpan(this.source.CurrentSnapshot.Span)));
		}
	}
}