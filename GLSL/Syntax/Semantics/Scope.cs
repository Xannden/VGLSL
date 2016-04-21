using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Semantics
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
	}
}