namespace Xannden.GLSL.Text
{
	public abstract class SourceLine
	{
		protected SourceLine(Snapshot snapshot, int start, int end, int lineNumber)
		{
			this.Snapshot = snapshot;

			if (end < start)
			{
				end = start;
			}

			this.Span = Span.Create(start, end);
			this.LineNumber = lineNumber;
		}

		protected SourceLine(Snapshot snapshot)
		{
			this.Snapshot = snapshot;
		}

		public abstract int Length { get; }

		public int LineNumber { get; }

		public Snapshot Snapshot { get; }

		public Span Span { get; }

		public abstract string Text { get; }

		public override string ToString()
		{
			return this.Text;
		}

		internal bool HasLineContinuation()
		{
			int position = this.Text.Length - 1;
			char character = this.Text[position--];

			while ((character == '\r' || character == '\n') && position >= 0)
			{
				character = this.Text[position--];
			}

			return character == '\\';
		}
	}
}