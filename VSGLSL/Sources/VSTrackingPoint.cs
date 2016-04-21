using System;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;

namespace Xannden.VSGLSL.Sources
{
	internal sealed class VSTrackingPoint : TrackingPoint
	{
		private ITrackingPoint point;

		public VSTrackingPoint(ITrackingPoint point)
		{
			this.point = point;
		}

		public override int GetPosition(Snapshot snapshot)
		{
			VSSnapshot vs = snapshot as VSSnapshot;

			if (vs == null)
			{
				throw new ArgumentException("snapshot must be a VSSnapshot");
			}

			return this.point.GetPosition(vs.TextSnapshot);
		}

		protected override void SetPosition(Snapshot snapshot, int position)
		{
			VSSnapshot vs = snapshot as VSSnapshot;

			if (vs == null)
			{
				throw new ArgumentException("snapshot must be a VSSnapshot");
			}

			this.point = vs.TextSnapshot.CreateTrackingPoint(position, PointTrackingMode.Positive);
		}
	}
}
