using Xannden.GLSL.Text;

namespace Xannden.GLSL.Test.Text
{
	internal sealed class TrackingTextSpan : TrackingSpan
	{
		private int end;
		private int start;

		public TrackingTextSpan(Span span)
		{
			this.start = span.Start;
			this.end = span.End;
		}

		public override Span GetSpan(Snapshot snapshot)
		{
			return Span.Create(this.start, this.end);
		}
	}
}