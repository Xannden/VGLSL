using System.Text;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Trivia
{
	public class SyntaxTrivia
	{
		private string text = string.Empty;

		public SyntaxTrivia(SyntaxType type, TrackingSpan span, string text)
		{
			this.Type = type;
			this.Span = span;
			this.text = text;
		}

		protected SyntaxTrivia(SyntaxType type, TrackingSpan span)
		{
			this.Type = type;
			this.Span = span;
		}

		protected SyntaxTrivia()
		{
		}

		public TrackingSpan Span { get; }

		public virtual string Text => this.text;

		public SyntaxType Type { get; }

		public virtual void ToStringWithoutNewLines(StringBuilder builder)
		{
			if (this.Type != SyntaxType.NewLineTrivia && this.Type != SyntaxType.LineCommentTrivia && this.Type != SyntaxType.BlockCommentTrivia)
			{
				builder.Append(this.text.Trim('\t'));
			}
		}

		public override string ToString()
		{
			return this.Text.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
		}
	}
}