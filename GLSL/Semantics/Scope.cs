using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics
{
	public sealed class Scope
	{
		internal Scope(TrackingPoint start, TrackingPoint end)
		{
			this.Start = start;
			this.End = end;
		}

		public TrackingPoint End { get; internal set; }

		public TrackingPoint Start { get; internal set; }

		public Span GetSpan(Snapshot snapshot)
		{
			return Span.Create(this.Start.GetPosition(snapshot), this.End.GetPosition(snapshot));
		}

		public bool Contains(Snapshot snapshot, int position)
		{
			return position >= this.Start.GetPosition(snapshot) && position <= this.End.GetPosition(snapshot);
		}

		public bool Contains(Snapshot snapshot, Span span)
		{
			return span.Start >= this.Start.GetPosition(snapshot) && span.End <= this.End.GetPosition(snapshot);
		}

		public bool Contains(Snapshot snapshot, TrackingSpan span)
		{
			return this.Contains(snapshot, span.GetSpan(snapshot));
		}
	}
}