using Xannden.GLSL.Text;

namespace Xannden.GLSL.Test.Text
{
	internal sealed class TextLine : SourceLine
	{
		public TextLine(Snapshot snapshot, int start, int end, int lineNumber, string line) : base(snapshot, start, end, lineNumber)
		{
			this.Text = line;
		}

		public override int Length => this.Span.Length;

		public override string Text { get; }
	}
}