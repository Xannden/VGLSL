using System.Collections.Generic;
using System.IO;
using Xannden.GLSL.Syntax.Semantics;
using Xannden.GLSL.Text;
using Xannden.GLSL.Text.Utility;

namespace Xannden.GLSL.Syntax.Tree
{
	public sealed class SyntaxTree
	{
		internal SyntaxTree()
		{
		}

		public IReadOnlyList<Definition> Definitions { get; internal set; }

		public SyntaxNode Root { get; internal set; }

		public SyntaxNode GetNodeContainingSpan(Snapshot snapshot, Span span)
		{
			SyntaxNode node = this.Root;

			while (node.FullSpan.GetSpan(snapshot).Contains(span) && node.Children.Count > 0)
			{
				for (int i = 0; i < node.Children.Count; i++)
				{
					if (node.Children[i].FullSpan.GetSpan(snapshot).Contains(span))
					{
						node = node.Children[i];
						break;
					}
				}
			}

			return node;
		}

		public SyntaxNode GetNodeFromPosition(Snapshot snapshot, int position)
		{
			SyntaxNode node = this.Root;

			SyntaxNode child = node.InternalChildren.Find(n => n.Span.GetSpan(snapshot).Contains(position));
			while (node.Children.Count > 0 && child != null)
			{
				node = child;

				SyntaxNode result = node.InternalChildren.Find(n => n.Span?.GetSpan(snapshot).Contains(position) ?? false);

				if (result != null)
				{
					child = result;
				}
				else
				{
					break;
				}
			}

			return node;
		}

		internal void WriteToXML(string file, Snapshot snapshot)
		{
			using (IndentedTextWriter indentedWriter = new IndentedTextWriter(new StreamWriter(File.Create(file)), "\t"))
			{
				this.Root.WriteToXML(indentedWriter, snapshot);
			}
		}
	}
}