using Microsoft.VisualStudio.Text;
using GLSLSpan = Xannden.GLSL.Text.Span;

namespace Xannden.VSGLSL.Extensions.Text
{
	internal static class SpanExtensions
	{
		public static bool Overlaps(this GLSLSpan glslSpan, SnapshotSpan span)
		{
			if (span.Start.Position >= glslSpan.Start && span.Start.Position <= glslSpan.End)
			{
				return true;
			}
			else if (span.End.Position - 1 <= glslSpan.End && span.End.Position - 1 >= glslSpan.Start)
			{
				return true;
			}

			return false;
		}

		public static bool Overlaps(this GLSLSpan glslSpan, Span span)
		{
			if (span.Start >= glslSpan.Start && span.Start <= glslSpan.End)
			{
				return true;
			}
			else if (span.End - 1 <= glslSpan.End && span.End - 1 >= glslSpan.Start)
			{
				return true;
			}

			return false;
		}

		public static GLSLSpan ToGLSLSpan(this Span span)
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

		public static Span ToVSSpan(this GLSLSpan span)
		{
			return new Span(span.Start, span.Length);
		}
	}
}