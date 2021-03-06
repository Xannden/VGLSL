﻿using System;
using System.Collections.Generic;
using System.Text;
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

		internal SyntaxNode(SyntaxTree tree, SyntaxType type, TrackingSpan span)
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

		public IEnumerable<SyntaxNode> DescendantsAndSelf
		{
			get
			{
				yield return this;

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

		public virtual TrackingSpan FullSpan { get; private set; }

		public bool IsMissing { get; internal set; } = false;

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

		public SyntaxType SyntaxType { get; }

		public SyntaxTree Tree { get; private set; }

		internal List<SyntaxNode> InternalChildren { get; } = new List<SyntaxNode>();

		internal int TempStart { get; private set; }

		internal int TempFullStart { get; private set; }

		public IReadOnlyList<SyntaxToken> GetSyntaxTokens()
		{
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			this.GetSyntaxTokensRecursive(this, tokens);

			return tokens;
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

		public IReadOnlyList<ColoredString> ToColoredString()
		{
			List<ColoredString> list = new List<ColoredString>();

			for (int i = 0; i < this.InternalChildren.Count; i++)
			{
				this.InternalChildren[i].ToColoredString(list);
			}

			return list;
		}

		public IReadOnlyList<SyntaxType> ToSyntaxTypes()
		{
			List<SyntaxType> list = new List<SyntaxType>();

			for (int i = 0; i < this.InternalChildren.Count; i++)
			{
				this.InternalChildren[i].ToSyntaxTypes(list);
			}

			return list;
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

		internal virtual List<string> GetExtraXmlTagInfo()
		{
			return null;
		}

		internal void SetEnd(Snapshot snapshot, int end, int fullEnd)
		{
			this.Span = snapshot.CreateTrackingSpan(Text.Span.Create(this.TempStart, end));
			this.FullSpan = snapshot.CreateTrackingSpan(Text.Span.Create(this.TempFullStart, fullEnd));
		}

		internal virtual void WriteToXml(IndentedTextWriter writer, Snapshot snapshot)
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
				this.InternalChildren[i].WriteToXml(writer, snapshot);
			}

			writer.IndentLevel--;

			if (this.InternalChildren.Count > 0)
			{
				writer.WriteLine($"</{Enum.GetName(typeof(SyntaxType), this.SyntaxType)}>");
			}
		}

		protected virtual void ToString(StringBuilder builder)
		{
			for (int i = 0; i < this.InternalChildren.Count; i++)
			{
				this.InternalChildren[i].ToString(builder);
			}
		}

		protected virtual void ToColoredString(List<ColoredString> list)
		{
			for (int i = 0; i < this.InternalChildren.Count; i++)
			{
				this.InternalChildren[i].ToColoredString(list);
			}
		}

		protected virtual void ToSyntaxTypes(List<SyntaxType> list)
		{
			for (int i = 0; i < this.InternalChildren.Count; i++)
			{
				this.InternalChildren[i].ToSyntaxTypes(list);
			}
		}

		protected virtual void NewChild(SyntaxNode node)
		{
		}

		private void GetSyntaxTokensRecursive(SyntaxNode node, List<SyntaxToken> tokens)
		{
			SyntaxToken token = node as SyntaxToken;

			if (token != null)
			{
				tokens.Add(token);
			}
			else
			{
				for (int i = 0; i < node?.Children.Count; i++)
				{
					if (node.SyntaxType != SyntaxType.Preprocessor)
					{
						this.GetSyntaxTokensRecursive(node.Children[i], tokens);
					}
				}
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