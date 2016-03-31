using Xannden.GLSL.Errors;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Test.Text
{
	internal sealed class TextSource : Source
	{
		public TextSource(string text, ErrorHandler reporter) : base(reporter)
		{
			this.CurrentSnapshot = new TextSnapshot(this, text);
		}

		public override Snapshot CurrentSnapshot { get; }
	}
}