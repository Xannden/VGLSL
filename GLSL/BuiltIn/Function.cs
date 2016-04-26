using System.Collections.Generic;

namespace Xannden.GLSL.BuiltIn
{
	public sealed class BuiltInFunctionDefinition : BuiltInDefinition
	{
		public BuiltInFunctionDefinition(string returnType, string identifier, string documentation, params Parameter[] parameters) : base(identifier)
		{
			this.ReturnType = returnType;
			this.Documentation = documentation;
			this.Parameters = parameters;
		}

		public string ReturnType { get; }

		public string Documentation { get; }

		public IReadOnlyList<Parameter> Parameters { get; }
	}
}
