using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace Xannden.VSGLSL.Tagging.BraceMatching
{
	internal sealed class GLSLBraceMatchingTagger : ITagger<ITextMarkerTag>
	{
		private readonly ITextView textView;
		private readonly Dictionary<char, char> bracePairs;
		private readonly TextMarkerTag tag = new TextMarkerTag("bracehighlight");

		public GLSLBraceMatchingTagger(ITextView textView)
		{
			this.bracePairs = new Dictionary<char, char>
			{
				['{'] = '}',
				['['] = ']',
				['('] = ')'
			};

			this.textView = textView;

			this.textView.Caret.PositionChanged += this.OnCaretPositionChanged;
		}

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

		public IEnumerable<ITagSpan<ITextMarkerTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			if (spans[0].Snapshot != this.textView.TextBuffer.CurrentSnapshot)
			{
				yield break;
			}

			SnapshotPoint? caretPoint = this.textView.Caret.Position.Point.GetPoint(this.textView.TextBuffer, this.textView.Caret.Position.Affinity);

			if (!caretPoint.HasValue || caretPoint.Value.Snapshot.Length == 0)
			{
				yield break;
			}

			SnapshotPoint currPoint;

			if (caretPoint.Value.Position == caretPoint.Value.Snapshot.Length && caretPoint.Value.Position != 0)
			{
				currPoint = caretPoint.Value - 1;
			}
			else
			{
				currPoint = caretPoint.Value;
			}

			SnapshotPoint prevPoint = caretPoint.Value.Position != 0 ? caretPoint.Value - 1 : caretPoint.Value;

			char currentCharacter = currPoint.GetChar();
			char lastCharacter = prevPoint.GetChar();
			SnapshotPoint matchedPoint;

			if (this.bracePairs.ContainsKey(currentCharacter))
			{
				if (this.FindCloseChar(currPoint, currentCharacter, this.bracePairs[currentCharacter], out matchedPoint))
				{
					yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(currPoint, 1), this.tag);
					yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(matchedPoint, 1), this.tag);
				}
			}
			else if (this.bracePairs.ContainsValue(lastCharacter))
			{
				char openCharacter = (char)0;

				foreach (KeyValuePair<char, char> pair in this.bracePairs)
				{
					if (pair.Value == lastCharacter)
					{
						openCharacter = pair.Key;
					}
				}

				if (this.FindOpenChar(prevPoint, openCharacter, lastCharacter, out matchedPoint))
				{
					yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(matchedPoint, 1), this.tag);
					yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(prevPoint, 1), this.tag);
				}
			}
		}

		private void OnCaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
		{
			this.TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(this.textView.TextBuffer.CurrentSnapshot, 0, this.textView.TextBuffer.CurrentSnapshot.Length)));
		}

		private bool FindCloseChar(SnapshotPoint start, char openCharacter, char closeCharacter, out SnapshotPoint end)
		{
			ITextSnapshotLine line = start.GetContainingLine();
			string text = line.GetText();

			int level = 0;
			int lineNumber = line.LineNumber;
			int offset = start.Position - line.Start.Position + 1;

			while (true)
			{
				for (int i = offset; i < text.Length; i++)
				{
					if (text[i] == openCharacter)
					{
						level++;
					}
					else if (text[i] == closeCharacter)
					{
						if (level > 0)
						{
							level--;
						}
						else
						{
							end = new SnapshotPoint(line.Snapshot, line.Start.Position + i);
							return true;
						}
					}
				}

				if (lineNumber + 1 >= this.textView.TextBuffer.CurrentSnapshot.LineCount)
				{
					break;
				}

				line = line.Snapshot.GetLineFromLineNumber(++lineNumber);
				text = line.GetText();
				offset = 0;
			}

			end = start;
			return false;
		}

		private bool FindOpenChar(SnapshotPoint end, char openCharacter, char closeCharacter, out SnapshotPoint start)
		{
			ITextSnapshotLine line = end.GetContainingLine();
			string text = line.GetText();

			int level = 0;
			int lineNumber = line.LineNumber;
			int offset = end.Position - line.Start.Position - 1;

			while (true)
			{
				for (int i = offset; i >= 0; i--)
				{
					if (text[i] == closeCharacter)
					{
						level++;
					}
					else if (text[i] == openCharacter)
					{
						if (level > 0)
						{
							level--;
						}
						else
						{
							start = new SnapshotPoint(line.Snapshot, line.Start.Position + i);
							return true;
						}
					}
				}

				if (lineNumber-- <= 0)
				{
					break;
				}

				line = line.Snapshot.GetLineFromLineNumber(lineNumber);
				text = line.GetText();
				offset = text.Length - 1;
			}

			start = end;
			return false;
		}
	}
}