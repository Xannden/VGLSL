using System;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;
using VSSpan = Microsoft.VisualStudio.Text.Span;

namespace Xannden.VSGLSL.Extensions
{
	internal static class TextExtensions
	{
		public static int GetPosition(this ITrackingPoint point, Snapshot snapshot)
		{
			VSSnapshot vs = snapshot as VSSnapshot;

			if (vs == null)
			{
				throw new ArgumentException($"{nameof(snapshot)} must be a VSSnapshot", nameof(snapshot));
			}

			return point.GetPosition(vs.TextSnapshot);
		}

		public static VSSpan GetSpan(this ITrackingSpan span, Snapshot snapshot)
		{
			VSSnapshot vs = snapshot as VSSnapshot;

			if (vs == null)
			{
				throw new ArgumentException($"{nameof(snapshot)} must be a VSSnapshot", nameof(snapshot));
			}

			return span.GetSpan(vs.TextSnapshot);
		}
	}
}
