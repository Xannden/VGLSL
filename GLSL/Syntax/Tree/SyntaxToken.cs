using System.Collections.Generic;
using System.Text;
using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree
{
	public class SyntaxToken : SyntaxNode
	{
		private TrackingSpan fullSpan;

		internal SyntaxToken(SyntaxTree tree, SyntaxType type, TrackingSpan span, string text, SyntaxTrivia leadingTrivia, SyntaxTrivia trailingTrivia, Snapshot snapshot, bool isMissing = false) : base(tree, type, span)
		{
			this.Text = text;
			this.LeadingTrivia = leadingTrivia;
			this.TrailingTrivia = trailingTrivia;

			int start = this.HasLeadingTrivia() ? this.LeadingTrivia.Span.GetSpan(snapshot).Start : this.Span.GetSpan(snapshot).Start;
			int end = this.HasTrailingTrivia() ? this.TrailingTrivia.Span.GetSpan(snapshot).End : this.Span.GetSpan(snapshot).End;

			this.fullSpan = snapshot.CreateTrackingSpan(GLSL.Text.Span.Create(start, end));
			this.IsMissing = isMissing;
		}

		public override TrackingSpan FullSpan => this.fullSpan;

		public SyntaxTrivia LeadingTrivia { get; private set; }

		public string Text { get; private set; }

		public SyntaxTrivia TrailingTrivia { get; private set; }

		public bool HasLeadingTrivia() => this.LeadingTrivia != null;

		public bool HasTrailingTrivia() => this.TrailingTrivia != null;

		public string ToStringWithoutNewLines()
		{
			StringBuilder builder = new StringBuilder();

			if (this.HasLeadingTrivia())
			{
				this.LeadingTrivia.ToStringWithoutNewLines(builder, true);
			}

			builder.Append(this.Text);

			if (this.HasTrailingTrivia())
			{
				this.TrailingTrivia.ToStringWithoutNewLines(builder, false);
			}

			return builder.ToString();
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			if (this.HasLeadingTrivia())
			{
				builder.Append(this.LeadingTrivia.ToString());
			}

			builder.Append(this.Text);

			if (this.HasTrailingTrivia())
			{
				builder.Append(this.TrailingTrivia.ToString());
			}

			return builder.ToString();
		}

		internal override List<string> GetExtraXmlTagInfo()
		{
			List<string> elements = new List<string>();

			elements.Add($"Text=\"{this.Escape(this.Text)}\"");

			if (this.HasLeadingTrivia())
			{
				elements.Add($"LeadingTrivia=\"{this.LeadingTrivia.ToString()}\"");
			}

			if (this.HasTrailingTrivia())
			{
				elements.Add($"TrailingTrivia=\"{this.TrailingTrivia.ToString()}\"");
			}

			return elements;
		}

		internal override void ToString(StringBuilder builder)
		{
			if (this.HasLeadingTrivia())
			{
				builder.Append(this.LeadingTrivia.ToString());
			}

			builder.Append(this.Text);

			if (this.HasTrailingTrivia())
			{
				builder.Append(this.TrailingTrivia.ToString());
			}
		}

		private string Escape(string text)
		{
			return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
		}
	}
}