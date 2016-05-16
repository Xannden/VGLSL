using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class FieldDefinition : Definition
	{
		public FieldDefinition(IReadOnlyList<SyntaxType> typeQualifier, TypeDefinition type, string name, IReadOnlyList<ColoredString> arraySpecifiers, string documentation, Scope scope, TrackingSpan span)
			: base(ColoredString.Create(name, ColorType.Field), documentation, DefinitionKind.Field, scope, span)
		{
			this.TypeQualifiers = typeQualifier ?? new List<SyntaxType>();
			this.Type = type;
			this.ArraySpecifiers = arraySpecifiers ?? new List<ColoredString>();
		}

		internal FieldDefinition(IReadOnlyList<SyntaxType> typeQualifier, TypeDefinition type, string name, string documentation, Scope scope, TrackingSpan span)
			: base(ColoredString.Create(name, ColorType.Field), documentation, DefinitionKind.Field, scope, span)
		{
			this.TypeQualifiers = typeQualifier ?? new List<SyntaxType>();
			this.Type = type;
		}

		public IReadOnlyList<SyntaxType> TypeQualifiers { get; }

		public TypeDefinition Type { get; }

		public IReadOnlyList<ColoredString> ArraySpecifiers { get; internal set; }

		public override bool Equals(Definition definition)
		{
			FieldDefinition other = definition as FieldDefinition;

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

			list.Add(ColoredString.Create(";", ColorType.Punctuation));

			return list;
		}

		internal static FieldDefinition Create(SyntaxType type, string name)
		{
			return new FieldDefinition(null, new TypeDefinition(type), name, string.Empty, Scope.BuiltIn, null);
		}

		internal static FieldDefinition Create(SyntaxType type, string name, bool isArray)
		{
			if (isArray)
			{
				return new FieldDefinition(null, new TypeDefinition(type), name, new List<ColoredString> { ColoredString.Create("[", ColorType.Punctuation), ColoredString.Create("]", ColorType.Punctuation) }, string.Empty, Scope.BuiltIn, null);
			}
			else
			{
				return new FieldDefinition(null, new TypeDefinition(type), name, string.Empty, Scope.BuiltIn, null);
			}
		}
	}
}
