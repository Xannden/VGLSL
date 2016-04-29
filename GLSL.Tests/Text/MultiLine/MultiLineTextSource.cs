using System;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Test.Text
{
	internal class MultiLineTextSource : Source
	{
		private readonly MultiLineSnapshot snapshot;

		public MultiLineTextSource(ErrorHandler reporter) : base(reporter)
		{
			this.snapshot = new MultiLineSnapshot(this);
		}

		public override Snapshot CurrentSnapshot => this.snapshot;

		public static MultiLineTextSource FromString(string text, ErrorHandler reporter)
		{
			string[] lines = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			MultiLineTextSource source = new MultiLineTextSource(reporter);

			for (int i = 0; i < lines.Length; i++)
			{
				source.AddLine(lines[i] + Environment.NewLine);
			}

			return source;
		}

		public static MultiLineTextSource FromString(string[] lines, ErrorHandler reporter, bool addNewLine = false)
		{
			MultiLineTextSource source = new MultiLineTextSource(reporter);

			for (int i = 0; i < lines.Length; i++)
			{
				if (addNewLine)
				{
					source.AddLine(lines[i] + Environment.NewLine);
				}
				else
				{
					source.AddLine(lines[i]);
				}
			}

			return source;
		}

		private void AddLine(string text)
		{
			if (this.snapshot.Lines.Count != 0)
			{
				TextLine prev = this.snapshot.Lines[this.snapshot.Lines.Count - 1];

				this.snapshot.Lines.Add(new TextLine(this.snapshot, prev.Span.End + 1, prev.Span.End + text.Length, this.snapshot.Lines.Count, text));
			}
			else
			{
				this.snapshot.Lines.Add(new TextLine(this.snapshot, 0, text.Length - 1, 0, text));
			}
		}
	}
}