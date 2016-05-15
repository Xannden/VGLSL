using System.Collections.Generic;
using System.Text;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics
{
	public abstract class Definition
	{
		internal Definition(string documentation, DefinitionKind kind, Scope scope, TrackingSpan span)
		{
			this.Kind = kind;
			this.Documentation = documentation;
			this.Scope = scope;
			this.DefinitionSpan = span;
		}

		protected Definition(ColoredString name, string documentation, DefinitionKind kind, Scope scope, TrackingSpan span)
		{
			this.Name = name;
			this.Kind = kind;
			this.Documentation = documentation;
			this.Scope = scope;
			this.DefinitionSpan = span;
		}

		public Scope Scope { get; }

		public ColoredString Name { get; internal set; }

		public string Documentation { get; }

		public DefinitionKind Kind { get; }

		public ShaderType ShaderType { get; internal set; } = ShaderType.All;

		public TrackingSpan DefinitionSpan { get; }

		public List<Definition> Overloads { get; internal set; }

		public static bool operator ==(Definition left, Definition right)
		{
			if (ReferenceEquals(left, right))
			{
				return true;
			}

			if (Equals(left, null) || Equals(right, null))
			{
				return false;
			}

			if (left.ShaderType != right.ShaderType || left.Name != right.Name)
			{
				return false;
			}

			return left?.Equals(right) ?? false;
		}

		public static bool operator !=(Definition left, Definition right)
		{
			return !(left == right);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			IReadOnlyList<ColoredString> list = this.GetColoredText();

			for (int i = 0; i < list.Count; i++)
			{
				builder.Append(list[i].Text);
			}

			return builder.ToString();
		}

		public override bool Equals(object obj)
		{
			Definition other = obj as Definition;

			if (other == null)
			{
				return false;
			}

			return this == other;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public abstract bool Equals(Definition definition);

		public abstract IReadOnlyList<ColoredString> GetColoredText();
	}
}