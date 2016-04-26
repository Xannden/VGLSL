namespace Xannden.GLSL.BuiltIn
{
	public sealed class Variable
	{
		public Variable(string type, string identifier)
		{
			this.Type = type;
			this.Identifier = identifier;
		}

		public string Type { get; }

		public string Identifier { get; }
	}
}
