using System;
using System.Collections.Generic;
using System.Text;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Test.Text
{
	internal sealed class MultiLineSnapshot : Snapshot
	{
		public MultiLineSnapshot(Source source) : base(source)
		{
			this.Lines = new List<TextLine>();
		}

		public MultiLineSnapshot(Source source, List<TextLine> lines) : base(source)
		{
			this.Lines = lines;
		}

		public override int Length
		{
			get
			{
				if (this.Lines.Count <= 0)
				{
					throw new InvalidOperationException("Source must contain a line");
				}

				return this.Lines[this.Lines.Count - 1].Span.End - this.Lines[0].Span.Start;
			}
		}

		public override int LineCount
		{
			get
			{
				return this.Lines.Count;
			}
		}

		public List<TextLine> Lines { get; }

		public override TrackingSpan CreateTrackingSpan(Span span)
		{
			return new TrackingTextSpan(span);
		}

		public override SourceLine GetLineFromLineNumber(int lineNumber)
		{
			return this.Lines[lineNumber];
		}

		public override SourceLine GetLineFromPosition(int position)
		{
			return this.Lines.Find(line => position >= line.Span.Start && position <= line.Span.End);
		}

		public override string GetText(int start, int length)
		{
			StringBuilder builder = new StringBuilder();

			int end = start + length;

			foreach (var line in this.Lines)
			{
				if (start > line.Span.End || end < line.Span.Start)
				{
					continue;
				}

				if (start <= line.Span.Start && end < line.Span.End)
				{
					builder.Append(line.Text.Substring(0, end - line.Span.Start + 1));
				}
				else if (start <= line.Span.Start && end >= line.Span.End)
				{
					builder.Append(line.Text);
				}
				else if (start > line.Span.Start && end >= line.Span.End)
				{
					builder.Append(line.Text.Substring(start - line.Span.Start));
				}
				else
				{
					builder.Append(line.Text.Substring(start - line.Span.Start, end - start));
				}
			}

			return builder.ToString();
		}
	}
}