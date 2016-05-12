using System.Collections.Generic;
using Xannden.GLSL.BuiltIn;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class ParameterDefinition : Definition
	{
		public ParameterDefinition(IReadOnlyList<ColoredString> typeQualifier, TypeDefinition type, string identifier, string documentation, IReadOnlyList<ColoredString> arraySpecifiers, Scope scope, TrackingSpan span) : base(ColoredString.Create(identifier, ColorType.Parameter), documentation, DefinitionKind.Parameter, scope, span)
		{
			this.TypeQualifier = typeQualifier ?? new List<ColoredString>();
			this.Type = type;
			this.ArraySpecifiers = arraySpecifiers ?? new List<ColoredString>();
		}

		protected ParameterDefinition(IReadOnlyList<ColoredString> typeQualifier, TypeDefinition type, string identifier, string documentation, Scope scope, TrackingSpan span) : this(typeQualifier, type, identifier, documentation, new List<ColoredString>(), scope, span)
		{
		}

		private ParameterDefinition(IReadOnlyList<ColoredString> typeQualifier, TypeDefinition type, string identifier, string documentation, bool isOptional) : this(typeQualifier, type, identifier, documentation, new List<ColoredString>(), null, null)
		{
			this.IsOptional = isOptional;
		}

		private ParameterDefinition(IReadOnlyList<ColoredString> typeQualifier, TypeDefinition type, string identifier, string documentation, int arraySize) : this(typeQualifier, type, identifier, documentation, new List<ColoredString> { ColoredString.Create("[", ColorType.Punctuation), ColoredString.Create(arraySize.ToString(), ColorType.Number), ColoredString.Create("]", ColorType.Punctuation) }, null, null)
		{
		}

		private ParameterDefinition(IReadOnlyList<ColoredString> typeQualifier, TypeDefinition type, string identifier, string documentation) : this(typeQualifier, type, identifier, documentation, new List<ColoredString>(), null, null)
		{
		}

		public IReadOnlyList<ColoredString> TypeQualifier { get; }

		public TypeDefinition Type { get; }

		public IReadOnlyList<ColoredString> ArraySpecifiers { get; protected set; }

		public bool IsOptional { get; }

		public override bool Equals(Definition definition)
		{
			ParameterDefinition other = definition as ParameterDefinition;

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

			return list;
		}

		internal static ParameterDefinition[] Create(string type, string name)
		{
			string[] types;

			if (BuiltInData.GenTypes.TryGetValue(type, out types))
			{
				ParameterDefinition[] parameters = new ParameterDefinition[types.Length];

				for (int i = 0; i < types.Length; i++)
				{
					parameters[i] = new ParameterDefinition(null, new TypeDefinition(ColoredString.Create(types[i], ColorType.Keyword)), name, string.Empty);
				}

				return parameters;
			}
			else
			{
				return new ParameterDefinition[] { new ParameterDefinition(null, new TypeDefinition(ColoredString.Create(type, ColorType.Keyword)), name, string.Empty) };
			}
		}

		internal static ParameterDefinition[] Create(string typeQualifier, string type, string name)
		{
			string[] types;

			if (BuiltInData.GenTypes.TryGetValue(type, out types))
			{
				ParameterDefinition[] parameters = new ParameterDefinition[types.Length];

				for (int i = 0; i < types.Length; i++)
				{
					parameters[i] = new ParameterDefinition(new List<ColoredString> { ColoredString.Create(typeQualifier, ColorType.Keyword), ColoredString.Create(" ", ColorType.WhiteSpace) }, new TypeDefinition(ColoredString.Create(types[i], ColorType.Keyword)), name, string.Empty);
				}

				return parameters;
			}
			else
			{
				return new ParameterDefinition[] { new ParameterDefinition(new List<ColoredString> { ColoredString.Create(typeQualifier, ColorType.Keyword), ColoredString.Create(" ", ColorType.WhiteSpace) }, new TypeDefinition(ColoredString.Create(type, ColorType.Keyword)), name, string.Empty) };
			}
		}

		internal static ParameterDefinition[] Create(string type, string name, bool isOptional)
		{
			string[] types;

			if (BuiltInData.GenTypes.TryGetValue(type, out types))
			{
				ParameterDefinition[] parameters = new ParameterDefinition[types.Length];

				for (int i = 0; i < types.Length; i++)
				{
					parameters[i] = new ParameterDefinition(null, new TypeDefinition(ColoredString.Create(types[i], ColorType.Keyword)), name, string.Empty, isOptional);
				}

				return parameters;
			}
			else
			{
				return new ParameterDefinition[] { new ParameterDefinition(null, new TypeDefinition(ColoredString.Create(type, ColorType.Keyword)), name, string.Empty, isOptional) };
			}
		}

		internal static ParameterDefinition[] Create(string type, string name, int arraySize)
		{
			string[] types;

			if (BuiltInData.GenTypes.TryGetValue(type, out types))
			{
				ParameterDefinition[] parameters = new ParameterDefinition[types.Length];

				for (int i = 0; i < types.Length; i++)
				{
					parameters[i] = new ParameterDefinition(null, new TypeDefinition(ColoredString.Create(types[i], ColorType.Keyword)), name, string.Empty, arraySize);
				}

				return parameters;
			}
			else
			{
				return new ParameterDefinition[] { new ParameterDefinition(null, new TypeDefinition(ColoredString.Create(type, ColorType.Keyword)), name, string.Empty, arraySize) };
			}
		}
	}
}