using System;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions.Text;

namespace Xannden.VSGLSL.Sources
{
	internal sealed class VSTrackingSpan : TrackingSpan
	{
		private ITrackingSpan trackingSpan;

		public VSTrackingSpan(ITrackingSpan span)
		{
			this.trackingSpan = span;
		}

		public override GLSL.Text.Span GetSpan(Snapshot snapshot)
		{
			VSSnapshot vs = snapshot as VSSnapshot;

			if (vs == null)
			{
				throw new ArgumentException("snapshot must be a VSSnapshot");
			}

			return this.trackingSpan.GetSpan(vs.TextSnapshot).ToGLSLSpan();
		}
	}
}