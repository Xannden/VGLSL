using System.Collections.Generic;
using System.Text;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree
{
	public class SyntaxToken : SyntaxNode
	{
		private readonly TrackingSpan fullSpan;

		internal SyntaxToken(SyntaxTree tree, SyntaxType type, TrackingSpan span, string text, SyntaxTrivia leadingTrivia, SyntaxTrivia trailingTrivia, Snapshot snapshot, bool isMissing = false) : base(tree, type, span)
		{
			this.Text = text;
			this.LeadingTrivia = leadingTrivia;
			this.TrailingTrivia = trailingTrivia;

			int start = this.HasLeadingTrivia ? this.LeadingTrivia.Span.GetSpan(snapshot).Start : this.Span.GetSpan(snapshot).Start;
			int end = this.HasTrailingTrivia ? this.TrailingTrivia.Span.GetSpan(snapshot).End : this.Span.GetSpan(snapshot).End;

			this.fullSpan = snapshot.CreateTrackingSpan(GLSL.Text.Span.Create(start, end));
			this.IsMissing = isMissing;
		}

		public override TrackingSpan FullSpan => this.fullSpan;

		public SyntaxTrivia LeadingTrivia { get; private set; }

		public string Text { get; private set; }

		public SyntaxTrivia TrailingTrivia { get; private set; }

		public bool HasLeadingTrivia => this.LeadingTrivia != null;

		public bool HasTrailingTrivia => this.TrailingTrivia != null;

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			if (this.HasLeadingTrivia)
			{
				builder.Append(this.LeadingTrivia.ToString());
			}

			builder.Append(this.Text);

			if (this.HasTrailingTrivia)
			{
				builder.Append(this.TrailingTrivia.ToString());
			}

			return builder.ToString();
		}

		internal override List<string> GetExtraXmlTagInfo()
		{
			List<string> elements = new List<string>();

			elements.Add($"Text=\"{Escape(this.Text)}\"");

			if (this.HasLeadingTrivia)
			{
				elements.Add($"LeadingTrivia=\"{this.LeadingTrivia.ToString()}\"");
			}

			if (this.HasTrailingTrivia)
			{
				elements.Add($"TrailingTrivia=\"{this.TrailingTrivia.ToString()}\"");
			}

			return elements;
		}

		protected override void ToColoredString(List<ColoredString> list)
		{
			ColorType type = ColorType.Identifier;

			if (this.SyntaxType.IsPreprocessor())
			{
				type = ColorType.PreprocessorKeyword;
			}
			else if (this?.IsExcludedCode() ?? false)
			{
				type = ColorType.ExcludedCode;
			}
			else if (this.SyntaxType.IsPunctuation())
			{
				type = ColorType.Punctuation;
			}
			else if (this?.IsPreprocessorText() ?? false)
			{
				type = ColorType.PreprocessorText;
			}
			else if (this.SyntaxType.IsKeyword())
			{
				type = ColorType.Keyword;
			}
			else if (this.SyntaxType.IsNumber())
			{
				type = ColorType.Number;
			}

			list.Add(ColoredString.Create(this.Text, type));

			if (this.HasTrailingTrivia)
			{
				this.TrailingTrivia.ToColoredString(list);
			}
		}

		protected override void ToSyntaxTypes(List<SyntaxType> list)
		{
			list.Add(this.SyntaxType);
		}

		protected override void ToString(StringBuilder builder)
		{
			if (this.HasLeadingTrivia)
			{
				builder.Append(this.LeadingTrivia.ToString());
			}

			builder.Append(this.Text);

			if (this.HasTrailingTrivia)
			{
				builder.Append(this.TrailingTrivia.ToString());
			}
		}

		private static string Escape(string text)
		{
			return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
		}
	}
}