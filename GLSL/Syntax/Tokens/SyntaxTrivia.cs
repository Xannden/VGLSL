﻿using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tokens
{
	public class SyntaxTrivia
	{
		private readonly string text = string.Empty;

		public SyntaxTrivia(SyntaxType type, TrackingSpan span, string text)
		{
			this.SyntaxType = type;
			this.Span = span;
			this.text = text;
		}

		protected SyntaxTrivia(SyntaxType type, TrackingSpan span)
		{
			this.SyntaxType = type;
			this.Span = span;
		}

		protected SyntaxTrivia()
		{
		}

		public TrackingSpan Span { get; }

		public virtual string Text => this.text;

		public SyntaxType SyntaxType { get; }

		public override string ToString()
		{
			return this.Text.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
		}

		public virtual string GetTextAndReplaceNewLines(string replaceValue)
		{
			if (this.SyntaxType != SyntaxType.NewLineTrivia && this.SyntaxType != SyntaxType.LineCommentTrivia && this.SyntaxType != SyntaxType.BlockCommentTrivia)
			{
				return this.text.Trim('\t');
			}

			if (this.SyntaxType == SyntaxType.NewLineTrivia)
			{
				return replaceValue;
			}

			return string.Empty;
		}

		internal void ToColoredString(List<ColoredString> list)
		{
			this.GetColoredString(list);
		}

		protected virtual void GetColoredString(List<ColoredString> list)
		{
			if (this.SyntaxType == SyntaxType.WhiteSpaceTrivia)
			{
				list.Add(ColoredString.Create(this.text, ColorType.WhiteSpace));
			}
		}
	}
}