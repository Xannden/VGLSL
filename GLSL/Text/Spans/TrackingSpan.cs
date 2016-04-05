namespace Xannden.GLSL.Text
{
	public abstract class TrackingSpan
	{
		public abstract Span GetSpan(Snapshot snapshot);
	}
}