using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class FieldDefinition : Definition
	{
		public FieldDefinition(IReadOnlyList<ColoredString> typeQualifier, TypeDefinition type, string name, IReadOnlyList<ColoredString> arraySpecifiers, string documentation, Scope scope, TrackingSpan span) : base(ColoredString.Create(name, ColorType.Field), documentation, DefinitionKind.Field, scope, span)
		{
			this.TypeQualifier = typeQualifier ?? new List<ColoredString>();
			this.Type = type;
			this.ArraySpecifiers = arraySpecifiers ?? new List<ColoredString>();
		}

		internal FieldDefinition(IReadOnlyList<ColoredString> typeQualifier, TypeDefinition type, string name, string documentation, Scope scope, TrackingSpan span) : base(ColoredString.Create(name, ColorType.Field), documentation, DefinitionKind.Field, scope, span)
		{
			this.TypeQualifier = typeQualifier ?? new List<ColoredString>();
			this.Type = type;
		}

		public IReadOnlyList<ColoredString> TypeQualifier { get; }

		public TypeDefinition Type { get; }

		public IReadOnlyList<ColoredString> ArraySpecifiers { get; internal set; }

		public override bool Equals(Definition definition)
		{
			FieldDefinition other = definition as FieldDefinition;

			if (other == null)
			{
				return false;
			}

			if (this.TypeQualifier != other.TypeQualifier || !this.Type.Equals(other.Type) || this.ArraySpecifiers.Count != other.ArraySpecifiers.Count)
			{
				return false;
			}

			for (int i = 0; i < this.ArraySpecifiers.Count; i++)
			{
				if (this.ArraySpecifiers[i] != other.ArraySpecifiers[i])
				{
					return false;
				}
			}

			return true;
		}

		public override IReadOnlyList<ColoredString> GetColoredText()
		{
			List<ColoredString> list = new List<ColoredString>();

			list.AddRange(this.TypeQualifier);

			list.AddRange(this.Type.GetColoredText());

			list.Add(this.Name);

			list.AddRange(this.ArraySpecifiers);

			list.Add(ColoredString.Create(";", ColorType.Punctuation));

			return list;
		}
	}
}
