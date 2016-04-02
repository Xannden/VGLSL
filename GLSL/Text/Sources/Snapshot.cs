namespace Xannden.GLSL.Text
{
	public abstract class Snapshot
	{
		protected Snapshot(Source source)
		{
			this.Source = source;
		}

		public abstract int Length { get; }

		public abstract int LineCount { get; }

		public Source Source { get; }

		public Span Span => Span.Create(0, this.Length - 1);

		public abstract TrackingSpan CreateTrackingSpan(Span span);

		public abstract SourceLine GetLineFromLineNumber(int lineNumber);

		public abstract SourceLine GetLineFromPosition(int position);

		public abstract string GetText(int start, int length);
	}
}