using System;

namespace Xannden.GLSL.Text
{
	public sealed class Span
	{
		private Span(int start, int end)
		{
			this.Start = start;
			this.End = end;
		}

		public int End { get; }

		public int Length => this.End - this.Start + 1;

		public int Start { get; }

		public static bool operator !=(Span span, Span other)
		{
			return span?.Start != other?.Start || span?.End != other?.End;
		}

		public static bool operator ==(Span span, Span other)
		{
			return span?.Start == other?.Start && span?.End == other?.End;
		}

		public static Span operator -(Span span, int value)
		{
			return Create(span.Start - value, span.End - value);
		}

		public static Span operator +(Span span, int value)
		{
			return Create(span.Start + value, span.End + value);
		}

		public static Span Create(int start, int end)
		{
			if (start > end)
			{
				throw new ArgumentOutOfRangeException(nameof(end), $"end position must be after start position Start={start} End={end}");
			}

			return new Span(start, end);
		}

		public static Span Create(int position)
		{
			if (position < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(position), $"position must be greater then 0");
			}

			return new Span(position, position);
		}

		public bool Contains(int position)
		{
			return position >= this.Start && position <= this.End;
		}

		public bool Contains(Span span)
		{
			if (span == null)
			{
				throw new ArgumentNullException(nameof(span));
			}

			return this.Start <= span.Start && this.End >= span.End;
		}

		public override bool Equals(object obj)
		{
			Span other = obj as Span;

			if (other == null)
			{
				return false;
			}

			return this == other;
		}

		public override int GetHashCode()
		{
			return this.Start.GetHashCode() ^ this.End.GetHashCode();
		}

		public bool Overlaps(Span span)
		{
			if (span == null)
			{
				throw new ArgumentNullException(nameof(span));
			}

			if ((span.Start >= this.Start && span.Start <= this.End) || (span.End <= this.End && span.End >= this.Start))
			{
				return true;
			}

			return false;
		}

		public override string ToString()
		{
			return $"[{this.Start},{this.End}]";
		}
	}
}