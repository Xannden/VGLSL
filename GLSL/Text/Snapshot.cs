namespace Xannden.GLSL.Text
{
	public abstract class Snapshot
	{
		protected Snapshot(Source source)
		{
			this.Source = source;
			this.FileName = this.Source.FileName;
		}

		public abstract int Length { get; }

		public abstract int LineCount { get; }

		public Source Source { get; }

		public string FileName { get; }

		public Span Span
		{
			get
			{
				if (this.Length > 0)
				{
					return Span.Create(0, this.Length - 1);
				}

				return Span.Create(0, 0);
			}
		}

		public abstract TrackingSpan CreateTrackingSpan(Span span);

		public abstract TrackingPoint CreateTrackingPoint(int position);

		public abstract SourceLine GetLineFromLineNumber(int lineNumber);

		public abstract SourceLine GetLineFromPosition(int position);

		public abstract string GetText(int start, int length);
	}
}