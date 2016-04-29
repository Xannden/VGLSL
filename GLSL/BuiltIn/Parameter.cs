namespace Xannden.GLSL.BuiltIn
{
	public sealed class Parameter
	{
		public Parameter(string type, string identifier, bool isOptional)
		{
			this.VariableType = type;
			this.Identifier = identifier;
			this.IsOptional = isOptional;
		}

		public Parameter(string type, string identifier) : this(type, identifier, string.Empty)
		{
		}

		public Parameter(string type, string identifier, string typeQualifier)
		{
			this.VariableType = type;
			this.Identifier = identifier;
			this.TypeQualifier = typeQualifier;
		}

		public string VariableType { get; }

		public string Identifier { get; }

		public string TypeQualifier { get; }

		public bool IsOptional { get; }

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.TypeQualifier))
			{
				return $"{this.TypeQualifier} {this.VariableType} {this.Identifier}";
			}
			else
			{
				return $"{this.VariableType} {this.Identifier}";
			}
		}
	}
}
