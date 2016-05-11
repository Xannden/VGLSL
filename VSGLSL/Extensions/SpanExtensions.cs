using System;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;
using GLSLSpan = Xannden.GLSL.Text.Span;
using VSSpan = Microsoft.VisualStudio.Text.Span;

namespace Xannden.VSGLSL.Extensions
{
	internal static class SpanExtensions
	{
		public static bool Overlaps(this GLSLSpan glslSpan, SnapshotSpan span)
		{
			if ((span.Start.Position >= glslSpan.Start && span.Start.Position <= glslSpan.End) || (span.End.Position - 1 <= glslSpan.End && span.End.Position - 1 >= glslSpan.Start))
			{
				return true;
			}

			return false;
		}

		public static GLSLSpan ToGLSLSpan(this VSSpan span)
		{
			return GLSLSpan.Create(span.Start, span.End - 1);
		}

		public static GLSLSpan ToGLSLSpan(this SnapshotSpan span)
		{
			if (span.Start.Position == span.End.Position)
			{
				return GLSLSpan.Create(span.Start.Position, span.End.Position);
			}

			return GLSLSpan.Create(span.Start.Position, span.End.Position - 1);
		}

		public static VSSpan ToVSSpan(this GLSLSpan span)
		{
			return VSSpan.FromBounds(span.Start, span.End + 1);

			// return new VSSpan(span.Start, span.Length);
		}

		public static ITrackingSpan ToITrackingSpan(this TrackingSpan span)
		{
			VSTrackingSpan trackingSpan = span as VSTrackingSpan;

			if (trackingSpan == null)
			{
				throw new ArgumentException($"{nameof(span)} must be a VSTrackingSpan and not null");
			}

			return trackingSpan.TrackingSpan;
		}
	}
}