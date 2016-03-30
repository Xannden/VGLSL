using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;

namespace Xannden.VSGLSL.Sources
{
	internal sealed class VSSourceLine : SourceLine
	{
		private ITextSnapshotLine line;
		private string text;

		public VSSourceLine(Snapshot snapshot, ITextSnapshotLine line) : base(snapshot, line.Start.Position, line.EndIncludingLineBreak.Position - 1, line.LineNumber)
		{
			this.line = line;
			this.text = this.line.GetTextIncludingLineBreak();
		}

		public override int Length => this.line.LengthIncludingLineBreak;

		public override string Text => this.text;
	}
}