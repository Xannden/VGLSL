namespace Xannden.GLSL.Text
{
	internal abstract class TrackingSpan
	{
		internal TrackingSpan()
		{
		}

		public abstract Span GetSpan(Snapshot snapshot);
	}
}