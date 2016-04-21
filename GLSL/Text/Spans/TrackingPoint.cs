namespace Xannden.GLSL.Text
{
	public abstract class TrackingPoint
	{
		public abstract int GetPosition(Snapshot snapshot);

		protected internal abstract void SetPosition(Snapshot snapshot, int position);
	}
}
