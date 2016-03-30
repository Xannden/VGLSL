using System.Collections.Generic;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Settings
{
	internal class IfPreprocessor : Preprocessor
	{
		internal IfPreprocessor(SyntaxToken keyword, bool value) : base(keyword, value)
		{
		}

		public List<Preprocessor> ElsePreprocessors { get; } = new List<Preprocessor>();

		public void CheckForElse()
		{
			if (this.Value || this.ElsePreprocessors.Find(preproc => preproc.Value) != null)
			{
				return;
			}

			Preprocessor preprocessor = this.ElsePreprocessors.Find(preproc => preproc.Keyword.SyntaxType == SyntaxType.ElsePreprocessorKeyword);

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

			return this.ElsePreprocessors.Find(preprocessor => preprocessor.Keyword.Span.GetSpan(snapshot).Contains(position));
		}

		public void SetAllValues(bool value)
		{
			this.ElsePreprocessors.ForEach(preprocessor => preprocessor.Value = value);

			this.Value = value;
		}
	}
}