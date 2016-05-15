using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class VariableDefinition : Definition
	{
		public VariableDefinition(IReadOnlyList<SyntaxType> typeQualifier, TypeDefinition type, ColoredString name, IReadOnlyList<ColoredString> arraySpecifiers, string documentation, DefinitionKind kind, Scope scope, TrackingSpan span)
			: base(name, documentation, kind, scope, span)
		{
			this.TypeQualifiers = typeQualifier ?? new List<SyntaxType>();
			this.Type = type;
			this.ArraySpecifiers = arraySpecifiers ?? new List<ColoredString>();
		}

		protected VariableDefinition(string documentation, DefinitionKind kind, Scope scope, TrackingSpan span) : base(documentation, kind, scope, span)
		{
		}

		public IReadOnlyList<SyntaxType> TypeQualifiers { get; protected set; }

		public TypeDefinition Type { get; protected set; }

		public IReadOnlyList<ColoredString> ArraySpecifiers { get; protected set; }

		public override bool Equals(Definition definition)
		{
			VariableDefinition other = definition as VariableDefinition;

			if (other == null)
			{
				return false;
			}

			if (this.TypeQualifiers != other.TypeQualifiers || !this.Type.Equals(other.Type) || this.ArraySpecifiers.Count != other.ArraySpecifiers.Count)
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

			list.AddRange(this.TypeQualifiers.ConvertList(text => text.ToColoredString(), ColoredString.Space, true));

			list.AddRange(this.Type.GetColoredText());

			list.Add(ColoredString.Space);

			list.Add(this.Name);

			list.AddRange(this.ArraySpecifiers);

			return list;
		}
	}
}
