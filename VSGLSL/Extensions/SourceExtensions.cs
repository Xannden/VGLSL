using System;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Extensions
{
	internal static class SourceExtensions
	{
		public static SnapshotSpan GetSnapshotSpan(this Snapshot snapshot, GLSL.Text.Span span)
		{
			VSSnapshot vs = snapshot as VSSnapshot;

			if (vs == null)
			{
				throw new ArgumentException("snapshot must be a VSSnapshot");
			}

			return new SnapshotSpan(vs.TextSnapshot, span.ToVSSpan());
		}
	}
}