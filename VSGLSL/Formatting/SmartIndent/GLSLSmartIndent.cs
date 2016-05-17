using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Packages;

namespace Xannden.VSGLSL.Formatting.SmartIndent
{
	internal sealed class GLSLSmartIndent : ISmartIndent
	{
		private readonly GLSLPreferences preferences;
		private Source source;
		private ITextView textView;

		public GLSLSmartIndent(Source source, ITextView textView, GLSLPreferences preferences)
		{
			this.source = source;
			this.textView = textView;
			this.preferences = preferences;
		}

		public int? GetDesiredIndentation(ITextSnapshotLine line)
		{
			switch (this.preferences.IndentStyle)
			{
				case vsIndentStyle.vsIndentStyleNone:
					return null;

				case vsIndentStyle.vsIndentStyleDefault:
					return this.GetBlockIndent(line);

				case vsIndentStyle.vsIndentStyleSmart:
					return this.GetSmartIndent(line);
			}

			return null;
		}

		public void Dispose()
		{
		}

		private int? GetSmartIndent(ITextSnapshotLine line)
		{
			SyntaxTree tree = this.source.Tree;
			Snapshot snapshot = this.source.CurrentSnapshot;

			SyntaxNode node = tree?.GetNodeFromPosition(snapshot, line.Start.Position);

			if (node == null)
			{
				return null;
			}

			int indentLevel = 0;

			foreach (SyntaxNode ancestor in node.AncestorsAndSelf)
			{
				if (ancestor.Children.Contains(child => child.SyntaxType == SyntaxType.LeftBraceToken && !child.IsMissing))
				{
					indentLevel += this.preferences.InsertTabs ? this.preferences.TabSize : 1;
				}
			}

			return indentLevel;
		}

		private int? GetBlockIndent(ITextSnapshotLine line)
		{
			for (int i = line.LineNumber - 1; i >= 0; i--)
			{
				string lineText = line.Snapshot.GetLineFromLineNumber(i).GetText();

				if (!string.IsNullOrEmpty(lineText))
				{
					return this.GetLeadingWhitespace(lineText);
				}
			}

			return null;
		}

		private int GetLeadingWhitespace(string text)
		{
			int size = 0;

			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '\t')
				{
					size += this.preferences.TabSize;
				}
				else if (text[i] == ' ')
				{
					size++;
				}
				else
				{
					break;
				}
			}

			return size;
		}
	}
}
