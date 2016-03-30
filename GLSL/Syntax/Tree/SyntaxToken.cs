using System.Collections.Generic;
using System.Security;
using System.Text;
using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree
{
	internal class SyntaxToken : SyntaxNode
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

		protected SyntaxToken(SyntaxType type) : base(type)
		{
		}

		public override TrackingSpan FullSpan => this.fullSpan;

		public SyntaxTrivia LeadingTrivia { get; private set; }

		public string Text { get; private set; }

		public SyntaxTrivia TrailingTrivia { get; private set; }

		public static SyntaxToken Create<T>(SyntaxTree tree, TrackingSpan span, string text, SyntaxTrivia leadingTrivia, SyntaxTrivia trailingTrivia, Snapshot snapshot, bool isMissing = false) where T : SyntaxToken, new()
		{
			T node = new T();

			node.Initilize(tree, span, text, leadingTrivia, trailingTrivia, snapshot);
			node.IsMissing = isMissing;

			return node;
		}

		public bool HasLeadingTrivia() => this.LeadingTrivia != null;

		public bool HasTrailingTrivia() => this.TrailingTrivia != null;

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

		protected override List<string> GetExtraXmlTagInfo()
		{
			List<string> elements = new List<string>();

			elements.Add($"Text=\"{SecurityElement.Escape(this.Text)}\"");

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

		protected void Initilize(SyntaxTree tree, TrackingSpan span, string text, SyntaxTrivia leadingTrivia, SyntaxTrivia trailingTrivia, Snapshot snapshot)
		{
			this.Initilize(tree, span);

			this.Text = text;
			this.LeadingTrivia = leadingTrivia;
			this.TrailingTrivia = trailingTrivia;

			int start = this.HasLeadingTrivia() ? this.LeadingTrivia.Span.GetSpan(snapshot).Start : this.Span.GetSpan(snapshot).Start;
			int end = this.HasTrailingTrivia() ? this.TrailingTrivia.Span.GetSpan(snapshot).End : this.Span.GetSpan(snapshot).End;

			this.fullSpan = snapshot.CreateTrackingSpan(GLSL.Text.Span.Create(start, end));
		}

		protected override void ToString(StringBuilder builder)
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
	}
}