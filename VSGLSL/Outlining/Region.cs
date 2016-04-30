using Xannden.GLSL.Text;

namespace Xannden.VSGLSL.Outlining
{
	internal sealed class Region
	{
		public Region(TrackingSpan span, string text)
		{
			this.Span = span;
			this.Text = text;
		}

		public TrackingSpan Span { get; }

		public string Text { get; }
	}
}
