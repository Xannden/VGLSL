using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class InterfaceBlockDefinition : Definition
	{
		public InterfaceBlockDefinition(IReadOnlyList<SyntaxType> typeQualifier, string typeName, string documentation, Scope scope, TrackingSpan span)
			: this(typeQualifier, typeName, documentation, null, scope, span)
		{
		}

		public InterfaceBlockDefinition(IReadOnlyList<SyntaxType> typeQualifier, string typeName, string documentation, List<FieldDefinition> fields, Scope scope, TrackingSpan span)
			: base(ColoredString.Create(typeName, ColorType.TypeName), documentation, DefinitionKind.InterfaceBlock, scope, span)
		{
			this.TypeQualifiers = typeQualifier ?? new List<SyntaxType>();
			this.InternalFields = fields ?? new List<FieldDefinition>();
		}

		public IReadOnlyList<SyntaxType> TypeQualifiers { get; }

		public IReadOnlyList<FieldDefinition> Fields => this.InternalFields;

		public ColoredString VariableName { get; }

		public IReadOnlyList<ColoredString> ArraySpecifiers { get; }

		internal List<FieldDefinition> InternalFields { get; } = new List<FieldDefinition>();

		public override bool Equals(Definition definition)
		{
			InterfaceBlockDefinition other = definition as InterfaceBlockDefinition;

			if (other == null)
			{
				return false;
			}

			if (this.TypeQualifiers.Count != other.TypeQualifiers.Count || this.Name != other.Name || this.Fields.Count != other.Fields.Count)
			{
				return false;
			}

			return true;
		}

		public override IReadOnlyList<ColoredString> GetColoredText()
		{
			List<ColoredString> list = new List<ColoredString>();

			list.AddRange(this.TypeQualifiers.ConvertList(text => text.ToColoredString(), ColoredString.Space, true));

			list.Add(this.Name);

			return list;
		}
	}
}