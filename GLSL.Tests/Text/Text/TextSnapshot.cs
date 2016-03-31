using System;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Test.Text
{
	internal sealed class TextSnapshot : Snapshot
	{
		private string text;

		public TextSnapshot(Source source, string text) : base(source)
		{
			this.text = text;
		}

		public override int Length => this.text.Length - 1;

		public override int LineCount => 1;

		public override TrackingSpan CreateTrackingSpan(Span span)
		{
			return new TrackingTextSpan(span);
		}

		public override SourceLine GetLineFromLineNumber(int lineNumber)
		{
			if (lineNumber != 0)
			{
				throw new ArgumentOutOfRangeException(nameof(lineNumber));
			}
			else
			{
				return new TextLine(this, 0, this.Length, 0, this.text);
			}
		}

		public override SourceLine GetLineFromPosition(int position)
		{
			if (position < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(position));
			}
			else if (position >= this.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(position));
			}
			else
			{
				return new TextLine(this, 0, this.Length, 0, this.text);
			}
		}

		public override string GetText(int start, int length)
		{
			return this.text;
		}
	}
}