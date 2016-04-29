namespace Xannden.GLSL.Text
{
#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
	public abstract class TrackingSpan
#pragma warning restore S1694 // An abstract class should have both abstract and concrete methods
	{
		public abstract Span GetSpan(Snapshot snapshot);
	}
}