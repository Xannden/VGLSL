using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class TypeNameDefinition : Definition
	{
		internal TypeNameDefinition(string name, string documentation, Scope scope, TrackingSpan span) : base(ColoredString.Create(name, ColorType.TypeName), documentation, DefinitionKind.TypeName, scope, span)
		{
		}

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
