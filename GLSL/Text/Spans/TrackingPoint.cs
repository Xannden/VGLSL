namespace Xannden.GLSL.Text
{
#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
	public abstract class TrackingPoint
#pragma warning restore S1694 // An abstract class should have both abstract and concrete methods
	{
		public abstract int GetPosition(Snapshot snapshot);

		protected internal abstract void SetPosition(Snapshot snapshot, int position);
	}
}
