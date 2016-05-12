using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class FunctionDefinition : Definition
	{
		public FunctionDefinition(TypeDefinition returnType, string identifier, string documentation, Scope scope, TrackingSpan span) : this(returnType, identifier, new List<ParameterDefinition>(), documentation, scope, span)
		{
		}

		public FunctionDefinition(TypeDefinition returnType, string identifier, List<ParameterDefinition> parameters, string documentation, Scope scope, TrackingSpan span) : base(ColoredString.Create(identifier, ColorType.Function), documentation, DefinitionKind.Function, scope, span)
		{
			this.ReturnType = returnType;
			this.InternalParameters = parameters;
		}

		public TypeDefinition ReturnType { get; }

		public IReadOnlyList<ParameterDefinition> Parameters => this.InternalParameters;

		internal List<ParameterDefinition> InternalParameters { get; }

		public override bool Equals(Definition definition)
		{
			FunctionDefinition other = definition as FunctionDefinition;

			if (other == null)
			{
				return false;
			}

			if (!this.ReturnType.Equals(other.ReturnType) || this.Parameters.Count != other.Parameters.Count)
			{
				return false;
			}

			for (int i = 0; i < this.Parameters.Count; i++)
			{
				if (this.Parameters[i] != other.Parameters[i])
				{
					return false;
				}
			}

			return true;
		}

		public override IReadOnlyList<ColoredString> GetColoredText()
		{
			List<ColoredString> list = new List<ColoredString>();

			list.AddRange(this.ReturnType.GetColoredText());

			list.Add(this.Name);

			list.Add(ColoredString.Create("(", ColorType.Punctuation));

			for (int i = 0; i < this.Parameters.Count; i++)
			{
				if (i != 0)
				{
					list.Add(ColoredString.Create(",", ColorType.Punctuation));
					list.Add(ColoredString.Create(" ", ColorType.WhiteSpace));
				}

				list.AddRange(this.Parameters[i].GetColoredText());
			}

			list.Add(ColoredString.Create(")", ColorType.Punctuation));

			return list;
		}
	}
}