using System.Collections.Generic;
using Xannden.GLSL.BuiltIn;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class ParameterDefinition : Definition
	{
		public ParameterDefinition(IReadOnlyList<SyntaxType> typeQualifier, TypeDefinition type, string identifier, string documentation, IReadOnlyList<ColoredString> arraySpecifiers, Scope scope, TrackingSpan span)
			: base(ColoredString.Create(identifier, ColorType.Parameter), documentation, DefinitionKind.Parameter, scope, span)
		{
			this.TypeQualifiers = typeQualifier ?? new List<SyntaxType>();
			this.Type = type;
			this.ArraySpecifiers = arraySpecifiers ?? new List<ColoredString>();
		}

		protected ParameterDefinition(IReadOnlyList<SyntaxType> typeQualifier, TypeDefinition type, string identifier, string documentation, Scope scope, TrackingSpan span)
			: this(typeQualifier, type, identifier, documentation, new List<ColoredString>(), scope, span)
		{
		}

		public IReadOnlyList<SyntaxType> TypeQualifiers { get; }

		public TypeDefinition Type { get; }

		public IReadOnlyList<ColoredString> ArraySpecifiers { get; protected set; }

		public bool IsOptional { get; private set; }

		public override bool Equals(Definition definition)
		{
			ParameterDefinition other = definition as ParameterDefinition;

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

			list.AddRange(this.TypeQualifiers.ConvertList(text => text.ToColoredString(), ColoredString.Space));

			list.AddRange(this.Type.GetColoredText());

			list.Add(ColoredString.Space);

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
					parameters[i] = new ParameterDefinition(null, new TypeDefinition(types[i].GetSyntaxType()), name, string.Empty, Scope.Global, null);
				}

				return parameters;
			}
			else
			{
				return new ParameterDefinition[] { new ParameterDefinition(null, new TypeDefinition(type.GetSyntaxType()), name, string.Empty, Scope.Global, null) };
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
					parameters[i] = new ParameterDefinition(new List<SyntaxType> { typeQualifier.GetSyntaxType() }, new TypeDefinition(types[i].GetSyntaxType()), name, string.Empty, Scope.Global, null);
				}

				return parameters;
			}
			else
			{
				return new ParameterDefinition[] { new ParameterDefinition(new List<SyntaxType> { typeQualifier.GetSyntaxType() }, new TypeDefinition(type.GetSyntaxType()), name, string.Empty, Scope.Global, null) };
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
					parameters[i] = new ParameterDefinition(null, new TypeDefinition(types[i].GetSyntaxType()), name, string.Empty, Scope.Global, null);
					parameters[i].IsOptional = isOptional;
				}

				return parameters;
			}
			else
			{
				ParameterDefinition param = new ParameterDefinition(null, new TypeDefinition(type.GetSyntaxType()), name, string.Empty, Scope.Global, null);
				param.IsOptional = isOptional;

				return new ParameterDefinition[] { param };
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
					parameters[i] = new ParameterDefinition(null, new TypeDefinition(types[i].GetSyntaxType()), name, string.Empty, new List<ColoredString> { ColoredString.Create("[", ColorType.Punctuation), ColoredString.Create(arraySize.ToString(), ColorType.Number), ColoredString.Create("]", ColorType.Punctuation) }, Scope.Global, null);
				}

				return parameters;
			}
			else
			{
				return new ParameterDefinition[] { new ParameterDefinition(null, new TypeDefinition(type.GetSyntaxType()), name, string.Empty, new List<ColoredString> { ColoredString.Create("[", ColorType.Punctuation), ColoredString.Create(arraySize.ToString(), ColorType.Number), ColoredString.Create("]", ColorType.Punctuation) }, Scope.Global, null) };
			}
		}
	}
}
