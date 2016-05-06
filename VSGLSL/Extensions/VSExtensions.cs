using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Extensions
{
	internal static class VSExtensions
	{
		public static void CommentSelection(this ITextView textView, bool comment)
		{
			SnapshotPoint start;
			SnapshotPoint end;

			if (textView.Selection.IsActive && !textView.Selection.IsEmpty)
			{
				start = textView.Selection.Start.Position;

				end = textView.Selection.End.Position;

				ITextSnapshotLine endLine = end.GetContainingLine();
				if (end.Position == endLine.Start)
				{
					end = end.Snapshot.GetLineFromLineNumber(endLine.LineNumber - 1).End;
				}
			}
			else
			{
				start = end = textView.Selection.Start.Position;
			}

			if (comment)
			{
				CommentRegion(start, end);
			}
			else
			{
				UnCommentRegion(start, end);
			}
		}

		public static int GetPosition(this ITrackingPoint point, Snapshot snapshot)
		{
			VSSnapshot vs = snapshot as VSSnapshot;

			if (vs == null)
			{
				throw new ArgumentException($"{nameof(snapshot)} must be a VSSnapshot", nameof(snapshot));
			}

			return point.GetPosition(vs.TextSnapshot);
		}

		public static void NavigateTo(this IServiceProvider serviceProvider, string fileName, int startLine, int startColumn)
		{
			Guid logicalViewGuid = new Guid(LogicalViewID.TextView);

			IVsUIHierarchy hierarchy;
			uint itemId;
			IVsWindowFrame frame;
			bool isOpened = VsShellUtilities.IsDocumentOpen(serviceProvider, fileName, logicalViewGuid, out hierarchy, out itemId, out frame);

			if (!isOpened)
			{
				try
				{
					VsShellUtilities.OpenDocument(serviceProvider, fileName, logicalViewGuid, out hierarchy, out itemId, out frame);
				}
				catch (Exception)
				{
					// TODO: write error to output
					return;
				}
			}

			frame.Show();

			IVsTextLines textBuffer;

			VsShellUtilities.GetTextView(frame).GetBuffer(out textBuffer);

			IVsTextManager manager = serviceProvider.GetService<IVsTextManager, SVsTextManager>();
			manager.NavigateToLineAndColumn(textBuffer, logicalViewGuid, startLine, startColumn, startLine, startColumn);
		}

		public static string GetFileName(this ITextBuffer buffer)
		{
			return buffer.Properties.GetProperty<ITextDocument>().FilePath;
		}

		public static T GetProperty<T>(this PropertyCollection collection)
		{
			return collection.GetProperty<T>(typeof(T));
		}

		public static T GetProperty<T, TKey>(this PropertyCollection collection)
		{
			return collection.GetProperty<T>(typeof(TKey));
		}

		public static void AddProperty<T>(this PropertyCollection collection, T property)
		{
			collection.AddProperty(property.GetType(), property);
		}

		public static T GetService<T>(this IServiceProvider provider)
		{
			return (T)provider.GetService(typeof(T));
		}

		public static T GetService<T, TKey>(this IServiceProvider provider)
		{
			return (T)provider.GetService(typeof(TKey));
		}

		private static int GetIndexOfFirstNoneWhiteSpace(string text)
		{
			for (int i = 0; i < text.Length; i++)
			{
				if (!char.IsWhiteSpace(text[i]))
				{
					return i;
				}
			}

			return -1;
		}

		private static void CommentRegion(SnapshotPoint start, SnapshotPoint end)
		{
			ITextSnapshot snapshot = start.Snapshot;

			using (ITextEdit edit = snapshot.TextBuffer.CreateEdit())
			{
				int column = int.MaxValue;

				for (int i = start.GetContainingLine().LineNumber; i <= end.GetContainingLine().LineNumber; i++)
				{
					string text = snapshot.GetLineFromLineNumber(i).GetText();

					int index = GetIndexOfFirstNoneWhiteSpace(text);

					if (index >= 0 && index < column)
					{
						column = index;
					}
				}

				for (int i = start.GetContainingLine().LineNumber; i <= end.GetContainingLine().LineNumber; i++)
				{
					ITextSnapshotLine currentLine = snapshot.GetLineFromLineNumber(i);

					if (string.IsNullOrEmpty(currentLine.GetText()))
					{
						continue;
					}

					edit.Insert(currentLine.Start.Position + column, "//");
				}

				edit.Apply();
			}
		}

		private static void UnCommentRegion(SnapshotPoint start, SnapshotPoint end)
		{
			ITextSnapshot snapshot = start.Snapshot;

			using (ITextEdit edit = snapshot.TextBuffer.CreateEdit())
			{
				for (int i = start.GetContainingLine().LineNumber; i <= end.GetContainingLine().LineNumber; i++)
				{
					ITextSnapshotLine currentLine = snapshot.GetLineFromLineNumber(i);
					string lineText = currentLine.GetText();

					for (int j = 0; j < lineText.Length - 1; j++)
					{
						if (!char.IsWhiteSpace(lineText[j]))
						{
							if (lineText[j] == '/' && lineText[j + 1] == '/')
							{
								edit.Delete(currentLine.Start.Position + j, 2);
								break;
							}
						}
					}
				}

				edit.Apply();
			}
		}
	}
}