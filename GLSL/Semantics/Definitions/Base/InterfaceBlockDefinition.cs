using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class InterfaceBlockDefinition : Definition
	{
		public InterfaceBlockDefinition(IReadOnlyList<ColoredString> typeQualifier, string name, string documentation, Scope scope, TrackingSpan span) : base(ColoredString.Create(name, ColorType.TypeName), documentation, DefinitionKind.InterfaceBlock, scope, span)
		{
			this.TypeQualifier = typeQualifier ?? new List<ColoredString>();
		}

		public IReadOnlyList<ColoredString> TypeQualifier { get; }

		public override bool Equals(Definition definition)
		{
			InterfaceBlockDefinition other = definition as InterfaceBlockDefinition;

			if (other == null)
			{
				return false;
			}

			if (this.TypeQualifier != other.TypeQualifier || this.Name != other.Name)
			{
				return false;
			}

			return true;
		}

		public override IReadOnlyList<ColoredString> GetColoredText()
		{
			List<ColoredString> list = new List<ColoredString>();

			list.AddRange(this.TypeQualifier);

			list.Add(this.Name);

			return list;
		}
	}
}
