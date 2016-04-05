using System;
using System.Collections.Generic;
using System.Text;
using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Text;
using Xannden.GLSL.Text.Utility;

namespace Xannden.GLSL.Syntax.Tree
{
	public class SyntaxNode
	{
		internal SyntaxNode(SyntaxTree tree, SyntaxType type, int start)
		{
			this.TempStart = start;
			this.SyntaxType = type;
			this.Tree = tree;
		}

		protected SyntaxNode(SyntaxTree tree, SyntaxType type, TrackingSpan span)
		{
			this.SyntaxType = type;
			this.Span = span;
			this.Tree = tree;
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

		public IReadOnlyList<SyntaxNode> Children => this.InternalChildren;

		public IEnumerable<SyntaxNode> Descendants
		{
			get
			{
				for (int i = 0; i < this.InternalChildren.Count; i++)
				{
					yield return this.InternalChildren[i];

					foreach (SyntaxNode child in this.InternalChildren[i].Descendants)
					{
						yield return child;
					}
				}
			}
		}

		public virtual TrackingSpan FullSpan => this.Span;

		public bool IsMissing { get; protected set; } = false;

		public SyntaxNode Parent { get; private set; }

		public IEnumerable<SyntaxNode> Siblings
		{
			get
			{
				for (int i = 0; i < this.Parent?.InternalChildren.Count; i++)
				{
					if (this.Parent.InternalChildren[i] != this)
					{
						yield return this.Parent.InternalChildren[i];
					}
				}
			}
		}

		public IEnumerable<SyntaxNode> SiblingsAndSelf
		{
			get
			{
				for (int i = 0; i < this.Parent?.InternalChildren.Count; i++)
				{
					yield return this.Parent.InternalChildren[i];
				}
			}
		}

		public TrackingSpan Span { get; internal set; }

		public IEnumerable<SyntaxToken> SyntaxTokens
		{
			get
			{
				if (this is SyntaxToken)
				{
					yield return this as SyntaxToken;
				}
				else
				{
					foreach (SyntaxNode decendent in this.Descendants)
					{
						if (decendent is SyntaxToken)
						{
							yield return decendent as SyntaxToken;
						}
					}
				}
			}
		}

		public SyntaxType SyntaxType { get; }

		public SyntaxTree Tree { get; private set; }

		internal List<SyntaxNode> InternalChildren { get; } = new List<SyntaxNode>();

		internal int TempStart { get; private set; }

		public SyntaxTrivia GetTrailingTrivia()
		{
			SyntaxToken token = (SyntaxToken)this.InternalChildren.FindLast(node => node is SyntaxToken);

			return token?.TrailingTrivia;
		}

		public bool IsExcludedCode()
		{
			return this.Parent?.SyntaxType == SyntaxType.ExcludedCode;
		}

		public bool IsPreprocessorText()
		{
			foreach (SyntaxNode ancestor in this.AncestorsAndSelf)
			{
				if (ancestor.SyntaxType == SyntaxType.Preprocessor)
				{
					return true;
				}
			}

			return false;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < this.InternalChildren.Count; i++)
			{
				this.InternalChildren[i].ToString(builder);
			}

			return builder.ToString();
		}

		internal void AddChild(SyntaxNode child)
		{
			child.Parent = this;
			this.InternalChildren.Add(child);

			this.NewChild(child);
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

			if (this.InternalChildren.Count <= 0)
			{
				builder.Append("/>");
			}
			else
			{
				builder.Append(">");
			}

			writer.WriteLine(builder.ToString());

			writer.IndentLevel++;

			for (int i = 0; i < this.InternalChildren.Count; i++)
			{
				this.InternalChildren[i].WriteToXML(writer, snapshot);
			}

			writer.IndentLevel--;

			if (this.InternalChildren.Count > 0)
			{
				writer.WriteLine($"</{Enum.GetName(typeof(SyntaxType), this.SyntaxType)}>");
			}
		}

		protected virtual List<string> GetExtraXmlTagInfo()
		{
			return null;
		}

		protected virtual void NewChild(SyntaxNode node)
		{
		}

		protected virtual void ToString(StringBuilder builder)
		{
			for (int i = 0; i < this.InternalChildren.Count; i++)
			{
				this.InternalChildren[i].ToString(builder);
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