using System.Collections.Generic;
using Xannden.GLSL.Semantics;

namespace Xannden.GLSL.BuiltIn
{
	public sealed class BuiltInFunction : BuiltInDefinition
	{
		internal BuiltInFunction(string returnType, string identifier, string documentation, params Parameter[] parameters) : base(identifier, documentation, DefinitionKind.Function)
		{
			this.ReturnType = returnType;
			this.Parameters = parameters;
		}

		public string ReturnType { get; }

		public IReadOnlyList<Parameter> Parameters { get; }
	}
}
