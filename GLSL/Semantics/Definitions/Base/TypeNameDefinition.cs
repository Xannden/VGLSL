using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class TypeNameDefinition : Definition
	{
		internal TypeNameDefinition(string name, string documentation, Scope scope, TrackingSpan span)
			: base(ColoredString.Create(name, ColorType.TypeName), documentation, DefinitionKind.TypeName, scope, span)
		{
		}

		internal TypeNameDefinition(string name, string documentation, List<FieldDefinition> fields, Scope scope, TrackingSpan span)
			: base(ColoredString.Create(name, ColorType.TypeName), documentation, DefinitionKind.TypeName, scope, span)
		{
			this.InternalFields = fields;
		}

		public IReadOnlyList<FieldDefinition> Fields => this.InternalFields;

		internal List<FieldDefinition> InternalFields { get; } = new List<FieldDefinition>();

		public override bool Equals(Definition definition)
		{
			TypeNameDefinition other = definition as TypeNameDefinition;

			if (other == null)
			{
				return false;
			}

			if (this.Name != other.Name)
			{
				return false;
			}

			return true;
		}

		public override IReadOnlyList<ColoredString> GetColoredText()
		{
			List<ColoredString> list = new List<ColoredString>();

			list.Add(this.Name);

			return list;
		}
	}
}
