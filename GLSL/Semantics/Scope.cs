using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics
{
	public sealed class Scope
	{
		private readonly bool isBuiltIn;

		internal Scope(TrackingPoint start, TrackingPoint end)
		{
			this.Start = start;
			this.End = end;
		}

		private Scope()
		{
			this.isBuiltIn = true;
		}

		internal static Scope BuiltIn { get; } = new Scope();

		internal TrackingPoint End { get; set; }

		internal TrackingPoint Start { get; set; }

		public bool Contains(Snapshot snapshot, int position)
		{
			if (this.isBuiltIn)
			{
				return true;
			}

			return position >= this.Start.GetPosition(snapshot) && position <= this.End.GetPosition(snapshot);
		}

		public bool Contains(Snapshot snapshot, Span span)
		{
			if (this.isBuiltIn)
			{
				return true;
			}

			return span?.Start >= this.Start.GetPosition(snapshot) && span?.End <= this.End.GetPosition(snapshot);
		}

		public bool Contains(Snapshot snapshot, TrackingSpan span)
		{
			if (this.isBuiltIn)
			{
				return true;
			}

			return this.Contains(snapshot, span?.GetSpan(snapshot));
		}
	}
}