using System.Collections.Generic;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Settings
{
	public sealed class IfPreprocessor : Preprocessor
	{
		internal IfPreprocessor(SyntaxToken keyword, bool value) : base(keyword, value)
		{
		}

		public IReadOnlyList<Preprocessor> ElsePreprocessors => this.InternalElsePreprocessors;

		public EndIfPreprocessorSyntax EndIf { get; internal set; }

		internal List<Preprocessor> InternalElsePreprocessors { get; } = new List<Preprocessor>();

		public void CheckForElse()
		{
			if (this.Value || this.InternalElsePreprocessors.Find(preproc => preproc.Value) != null)
			{
				return;
			}

			Preprocessor preprocessor = this.InternalElsePreprocessors.Find(preproc => preproc.Keyword.SyntaxType == SyntaxType.ElsePreprocessorKeyword);

			if (preprocessor != null)
			{
				preprocessor.Value = true;
			}
		}

		public bool Contains(Snapshot snapshot, int position)
		{
			if (this.Keyword.Span.GetSpan(snapshot).Contains(position))
			{
				return true;
			}

			for (int i = 0; i < this.ElsePreprocessors.Count; i++)
			{
				if (this.ElsePreprocessors[i].Keyword.Span.GetSpan(snapshot).Contains(position))
				{
					return true;
				}
			}

			return false;
		}

		public Preprocessor GetPreprocessor(Snapshot snapshot, int position)
		{
			if (this.Keyword.Span.GetSpan(snapshot).Contains(position))
			{
				return this;
			}

			return this.InternalElsePreprocessors.Find(preprocessor => preprocessor.Keyword.Span.GetSpan(snapshot).Contains(position));
		}

		public void SetAllValues(bool value)
		{
			this.InternalElsePreprocessors.ForEach(preprocessor => preprocessor.Value = value);

			this.Value = value;
		}
	}
}