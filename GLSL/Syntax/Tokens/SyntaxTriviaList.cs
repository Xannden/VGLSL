using System;
using System.Collections.Generic;
using System.Text;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tokens
{
	internal sealed class SyntaxTriviaList : SyntaxTrivia
	{
		private SyntaxTriviaList(SyntaxType type, TrackingSpan span) : base(type, span)
		{
		}

		public List<SyntaxTrivia> List { get; private set; }

		public override string Text
		{
			get
			{
				StringBuilder builder = new StringBuilder();

				for (int i = 0; i < this.List.Count; i++)
				{
					builder.Append(this.List[i].ToString());
				}

				return builder.ToString();
			}
		}

		public static SyntaxTriviaList Create(List<SyntaxTrivia> trivia, Snapshot snapshot)
		{
			if (trivia == null)
			{
				throw new ArgumentNullException(nameof(trivia));
			}

			if (trivia.Count <= 0)
			{
				throw new ArgumentException($"{nameof(trivia)} must contain at least one sub-trivia");
			}

			Span span = GLSL.Text.Span.Create(trivia[0].Span.GetSpan(snapshot).Start, trivia[trivia.Count - 1].Span.GetSpan(snapshot).End);

			SyntaxTriviaList syntaxTriviaList = new SyntaxTriviaList(SyntaxType.TriviaList, snapshot.CreateTrackingSpan(span));

			syntaxTriviaList.List = new List<SyntaxTrivia>(trivia);

			return syntaxTriviaList;
		}

		public override string GetTextAndReplaceNewLines(string replaceValue)
		{
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < this.List.Count; i++)
			{
				builder.Append(this.List[i].GetTextAndReplaceNewLines(replaceValue));
			}

			return builder.ToString();
		}

		protected override void GetColoredString(List<ColoredString> list)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				this.List[i].ToColoredString(list);
			}
		}
	}
}