using Xannden.GLSL.Text;

namespace Xannden.GLSL.Test.Text
{
	internal sealed class TrackingTextPoint : TrackingPoint
	{
		private int position;

		public TrackingTextPoint(int position)
		{
			this.position = position;
		}

		public override int GetPosition(Snapshot snapshot)
		{
			return position;
		}

		protected internal override void SetPosition(Snapshot snapshot, int position)
		{
			this.position = position;
		}

		public override string ToString()
		{
			return this.position.ToString();
		}
	}
}
