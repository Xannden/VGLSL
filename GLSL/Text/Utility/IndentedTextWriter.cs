using System;
using System.IO;

namespace Xannden.GLSL.Text.Utility
{
	internal class IndentedTextWriter : IDisposable
	{
		private readonly TextWriter writer;

		internal IndentedTextWriter(TextWriter textWriter, string indentString)
		{
			this.writer = textWriter;
			this.IndentString = indentString;
		}

		public int IndentLevel { get; set; } = 0;

		public string IndentString { get; }

		public void WriteLine(string line)
		{
			for (int i = 0; i < this.IndentLevel; i++)
			{
				this.writer.Write(this.IndentString);
			}

			this.writer.WriteLine(line);
		}

		#region IDisposable Support

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.writer.Dispose();
			}
		}

		#endregion
	}
}
