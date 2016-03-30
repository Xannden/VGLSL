using Xannden.GLSL.Text;

namespace Xannden.GLSL.Lexing
{
	internal class TextNavigator
	{
		public const char EndCharacter = char.MaxValue;

		private SourceLine currentLine;
		private int lineIndex;
		private int offset;
		private Snapshot snapshot;

		public TextNavigator(Snapshot snapshot, Span span = null)
		{
			this.snapshot = snapshot;

			if (span != null)
			{
				this.Position = span.Start;
				this.EndPosition = span.End;
				this.currentLine = this.snapshot.GetLineFromPosition(this.Position);
				this.lineIndex = this.currentLine.LineNumber;
				this.offset = this.Position - this.currentLine.Span.Start;
			}
			else
			{
				this.Position = 0;
				this.EndPosition = this.snapshot.Length;
				this.currentLine = this.snapshot.GetLineFromLineNumber(this.lineIndex);
				this.lineIndex = 0;
				this.offset = 0;
			}
		}

		public int EndPosition { get; private set; } = 0;

		public SourceLine Line => this.currentLine;

		public int Position { get; private set; }

		public void Advance()
		{
			this.Advance(1);
		}

		public void Advance(int amount)
		{
			this.Position += amount;
			this.offset += amount;

			if (this.offset >= this.currentLine.Length)
			{
				if (this.Position < this.snapshot.Length)
				{
					this.currentLine = this.snapshot.GetLineFromPosition(this.Position);
					this.offset = this.Position - this.currentLine.Span.Start;
					this.lineIndex = this.currentLine.LineNumber;
				}
				else
				{
					this.lineIndex = this.snapshot.LineCount;
				}
			}
		}

		public bool CurrentLineContains(string token)
		{
			return this.currentLine.Text.Contains(token);
		}

		public int GetEndOfLine()
		{
			return this.currentLine.Span.End;
		}

		public int GetStartOfLine()
		{
			return this.currentLine.Span.Start;
		}

		public string GetText(int start, int length)
		{
			if (start < this.snapshot.Length && start + length <= this.snapshot.Length)
			{
				return this.snapshot.GetText(start, length);
			}

			return string.Empty;
		}

		public bool IsLineContinuation()
		{
			if (this.currentLine.Length - 1 <= this.offset)
			{
				return false;
			}

			if (this.currentLine.Text[this.offset] == '\\' && this.currentLine.Length > this.offset + 1)
			{
				if (this.currentLine.Text[this.offset + 1] == '\n' || this.currentLine.Text[this.offset + 1] == '\r')
				{
					return true;
				}
			}

			return false;
		}

		public bool IsLineEnd()
		{
			return this.currentLine.Length < this.offset || (this.currentLine.Text[this.offset] == '\n' || this.currentLine.Text[this.offset] == '\r');
		}

		public void MoveToEndOfLine()
		{
			this.Position = this.currentLine.Span.End;
			this.offset = this.currentLine.Length - 1;

			while (this.offset > 0 && (this.IsLineEnd() || this.IsLineContinuation()))
			{
				this.offset--;
				this.Position--;
			}

			this.Advance();
		}

		public void MoveToNextLine()
		{
			if (this.lineIndex + 1 >= this.snapshot.LineCount)
			{
				this.Position = this.currentLine.Span.End;
				this.offset = this.currentLine.Length;
				return;
			}

			this.Position += this.currentLine.Length - this.offset;
			this.currentLine = this.snapshot.GetLineFromLineNumber(++this.lineIndex);
			this.offset = 0;
		}

		public char PeekChar()
		{
			return this.PeekChar(0);
		}

		public char PeekChar(int lookAhead)
		{
			if (this.Position + lookAhead > this.EndPosition)
			{
				return EndCharacter;
			}

			if (this.offset + lookAhead >= this.currentLine.Length)
			{
				if (this.lineIndex >= this.snapshot.LineCount - 1)
				{
					return EndCharacter;
				}

				SourceLine line = this.snapshot.GetLineFromPosition(this.Position + lookAhead);

				return line.Text[(this.Position + lookAhead) - line.Span.Start];
			}

			return this.currentLine.Text[this.offset + lookAhead];
		}
	}
}