using System;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions.Text;

namespace Xannden.VSGLSL.Sources
{
	internal sealed class VSTrackingSpan : TrackingSpan
	{
		public VSTrackingSpan(ITrackingSpan span)
		{
			this.TrackingSpan = span;
		}

		internal ITrackingSpan TrackingSpan { get; }

		public override GLSL.Text.Span GetSpan(Snapshot snapshot)
		{
			VSSnapshot vs = snapshot as VSSnapshot;

			if (vs == null)
			{
				throw new ArgumentException("snapshot must be a VSSnapshot");
			}

			return this.TrackingSpan.GetSpan(vs.TextSnapshot).ToGLSLSpan();
		}
	}
}