using System.IO;
using Xannden.GLSL.Text;
using Xannden.GLSL.Text.Utility;

namespace Xannden.GLSL.Syntax.Tree
{
	internal class SyntaxTree
	{
		public SyntaxTree()
		{
		}

		public SyntaxTree(SyntaxNode root)
		{
			this.Root = root;
		}

		public SyntaxNode Root { get; private set; }

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

			SyntaxNode child = node.Children.Find(n => n.Span.GetSpan(snapshot).Contains(position));
			while (node.Children.Count > 0 && child != null)
			{
				node = child;

				child = node.Children.Find(n => n.Span.GetSpan(snapshot).Contains(position));
			}

			return node;
		}

		public void WriteToXML(string file, Snapshot snapshot)
		{
			using (IndentedTextWriter indentedWriter = new IndentedTextWriter(new StreamWriter(File.Create(file)), "\t"))
			{
				this.Root.WriteToXML(indentedWriter, snapshot);
			}
		}
	}
}