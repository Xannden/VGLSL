using System.Text;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Trivia
{
	public class SyntaxTrivia
	{
		private string text = string.Empty;

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

		internal virtual void ToStringWithoutNewLines(StringBuilder builder)
		{
			if (this.SyntaxType != SyntaxType.NewLineTrivia && this.SyntaxType != SyntaxType.LineCommentTrivia && this.SyntaxType != SyntaxType.BlockCommentTrivia)
			{
				builder.Append(this.text.Trim('\t'));
			}
		}
	}
}