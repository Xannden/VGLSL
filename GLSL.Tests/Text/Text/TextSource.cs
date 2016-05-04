using Xannden.GLSL.Text;

namespace Xannden.GLSL.Test.Text
{
	internal sealed class TextSource : Source
	{
		public TextSource(string text) : base(string.Empty)
		{
			this.CurrentSnapshot = new TextSnapshot(this, text);
		}

		public override Snapshot CurrentSnapshot { get; }
	}
}