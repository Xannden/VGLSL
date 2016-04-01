using System;
using System.Collections.Generic;
using System.Text;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Text;
using Xannden.GLSL.Text.Utility;

namespace Xannden.GLSL.Syntax.Tree
{
	internal class SyntaxNode
	{
		internal SyntaxNode(SyntaxTree tree, SyntaxType type, int start)
		{
			this.TempStart = start;
			this.SyntaxType = type;
			this.Tree = tree;
			this.Children = new List<SyntaxNode>();
		}

		protected SyntaxNode(SyntaxTree tree, SyntaxType type, TrackingSpan span)
		{
			this.SyntaxType = type;
			this.Span = span;
			this.Children = new List<SyntaxNode>();
			this.Tree = tree;
		}

		protected SyntaxNode(SyntaxType type)
		{
			this.SyntaxType = type;
			this.Children = new List<SyntaxNode>();
		}

		public IEnumerable<SyntaxNode> Ancestors
		{
			get
			{
				SyntaxNode node = this.Parent;

				while (node != null)
				{
					yield return node;

					node = node.Parent;
				}
			}
		}

		public IEnumerable<SyntaxNode> AncestorsAndSelf
		{
			get
			{
				SyntaxNode node = this;

				while (node != null)
				{
					yield return node;

					node = node.Parent;
				}
			}
		}

		public List<SyntaxNode> Children { get; }

		public IEnumerable<SyntaxNode> Descendants
		{
			get
			{
				for (int i = 0; i < this.Children.Count; i++)
				{
					yield return this.Children[i];

					foreach (SyntaxNode child in this.Children[i].Descendants)
					{
						yield return child;
					}
				}
			}
		}

		public virtual TrackingSpan FullSpan => this.Span;

		public bool IsMissing { get; protected set; } = false;

		public SyntaxNode Parent { get; private set; }

		public List<PreprocessorSyntax> Prepocessors { get; } = new List<PreprocessorSyntax>();

		public IEnumerable<SyntaxNode> Siblings
		{
			get
			{
				for (int i = 0; i < this.Parent?.Children.Count; i++)
				{
					if (this.Parent.Children[i] != this)
					{
						yield return this.Parent.Children[i];
					}
				}
			}
		}

		public IEnumerable<SyntaxNode> SiblingsAndSelf
		{
			get
			{
				for (int i = 0; i < this.Parent?.Children.Count; i++)
				{
					yield return this.Parent.Children[i];
				}
			}
		}

		public TrackingSpan Span { get; set; }

		public SyntaxType SyntaxType { get; }

		internal int TempStart { get; private set; }

		protected SyntaxTree Tree { get; private set; }

		public static SyntaxNode Create<T>(SyntaxTree tree, int start) where T : SyntaxNode, new()
		{
			T node = new T();

			node.Initilize(tree, start);

			return node;
		}

		public SyntaxTrivia GetTrailingTrivia()
		{
			SyntaxToken token = (SyntaxToken)this.Children.FindLast(node => node is SyntaxToken);

			return token?.TrailingTrivia;
		}

		public void AddChild(SyntaxNode child)
		{
			child.Parent = this;
			this.Children.Add(child);

			this.NewChild(child);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].ToString(builder);
			}

			return builder.ToString();
		}

		internal void SetEnd(Snapshot snapshot, int end)
		{
			this.Span = snapshot.CreateTrackingSpan(Text.Span.Create(this.TempStart, end));
		}

		internal virtual void WriteToXML(IndentedTextWriter writer, Snapshot snapshot)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append($"<{Enum.GetName(typeof(SyntaxType), this.SyntaxType)} Span=\"{this.Span?.GetSpan(snapshot).ToString()}\"");

			if (this.IsMissing)
			{
				builder.Append($" Missing=\"{this.IsMissing}\"");
			}

			this.GetFormatedTagInfo(builder);

			if (this.Children.Count <= 0)
			{
				builder.Append("/>");
			}
			else
			{
				builder.Append(">");
			}

			writer.WriteLine(builder.ToString());

			writer.IndentLevel++;

			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].WriteToXML(writer, snapshot);
			}

			writer.IndentLevel--;

			if (this.Children.Count > 0)
			{
				writer.WriteLine($"</{Enum.GetName(typeof(SyntaxType), this.SyntaxType)}>");
			}
		}

		protected virtual List<string> GetExtraXmlTagInfo()
		{
			return null;
		}

		protected void Initilize(SyntaxTree tree, int start)
		{
			this.Tree = tree;
			this.TempStart = start;
		}

		protected void Initilize(SyntaxTree tree, TrackingSpan span)
		{
			this.Tree = tree;
			this.Span = span;
		}

		protected virtual void NewChild(SyntaxNode node)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.Preprocessor:
					this.Prepocessors.Add(node as PreprocessorSyntax);
					break;
			}
		}

		protected virtual void ToString(StringBuilder builder)
		{
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].ToString(builder);
			}
		}

		private void GetFormatedTagInfo(StringBuilder builder)
		{
			List<string> list = this.GetExtraXmlTagInfo();

			for (int i = 0; i < list?.Count; i++)
			{
				if (i != list.Count)
				{
					builder.Append(" ");
				}

				builder.Append(list[i]);
			}
		}
	}
}