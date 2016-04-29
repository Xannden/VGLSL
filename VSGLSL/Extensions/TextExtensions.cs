using System;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;

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
	}
}
