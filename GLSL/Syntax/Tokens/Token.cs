using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tokens
{
	public class Token
	{
		internal Token(SyntaxType type, Span span, SourceLine line, string text, SyntaxTrivia leadingTrivia)
		{
			this.SyntaxType = type;
			this.Span = span;
			this.Line = line;
			this.Text = text;
			this.LeadingTrivia = leadingTrivia;
		}

		public bool HasLeadingTrivia => this.LeadingTrivia != null;

		public bool HasTrailingTrivia => this.TrailingTrivia != null;

		public SyntaxTrivia LeadingTrivia { get; }

		public int Length => this.Span.End - this.Span.Start;

		public SourceLine Line { get; }

		public Span Span { get; }

		public string Text { get; }

		public SyntaxTrivia TrailingTrivia { get; internal set; }

		public SyntaxType SyntaxType { get; }

		public Span FullSpan(Snapshot snapshot)
		{
			int start = this.HasLeadingTrivia ? this.LeadingTrivia.Span.GetSpan(snapshot).Start : this.Span.Start;
			int end = this.HasTrailingTrivia ? this.TrailingTrivia.Span.GetSpan(snapshot).End : this.Span.End;

			return Span.Create(start, end);
		}

		public override string ToString()
		{
			return this.Text;
		}
	}
}