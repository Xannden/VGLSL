namespace Xannden.GLSL.BuiltIn
{
	public sealed class Variable
	{
		public Variable(string type, string identifier)
		{
			this.VariableType = type;
			this.Identifier = identifier;
		}

		public string VariableType { get; }

		public string Identifier { get; }
	}
}
