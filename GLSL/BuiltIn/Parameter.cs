using System.Text;

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

		public Parameter(string type, string identifier)
		{
			this.TypeQualifier = string.Empty;
			this.VariableType = type;
			this.Identifier = identifier;
		}

		public Parameter(string typeQualifier, string type, string identifier)
		{
			this.TypeQualifier = typeQualifier;
			this.VariableType = type;
			this.Identifier = identifier;
		}

		public Parameter(string type, string identifier, int arraySize)
		{
			this.TypeQualifier = type;
			this.Identifier = identifier;
			this.ArraySize = arraySize;
		}

		public string VariableType { get; }

		public string Identifier { get; }

		public string TypeQualifier { get; }

		public bool IsOptional { get; }

		public int ArraySize { get; }

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			if (!string.IsNullOrEmpty(this.TypeQualifier))
			{
				builder.Append(this.TypeQualifier);
				builder.Append(" ");
			}

			builder.Append(this.VariableType);
			builder.Append(" ");
			builder.Append(this.Identifier);

			if (this.ArraySize > 0)
			{
				builder.Append($"[{this.ArraySize}]");
			}

			return builder.ToString();
		}

		internal static Parameter[] Create(string type, string name)
		{
			string[] types;

			if (BuiltInData.GenTypes.TryGetValue(type, out types))
			{
				Parameter[] parameters = new Parameter[types.Length];

				for (int i = 0; i < types.Length; i++)
				{
					parameters[i] = new Parameter(types[i], name);
				}

				return parameters;
			}
			else
			{
				return new Parameter[] { new Parameter(type, name) };
			}
		}

		internal static Parameter[] Create(string typeQualifier, string type, string name)
		{
			string[] types;

			if (BuiltInData.GenTypes.TryGetValue(type, out types))
			{
				Parameter[] parameters = new Parameter[types.Length];

				for (int i = 0; i < types.Length; i++)
				{
					parameters[i] = new Parameter(typeQualifier, types[i], name);
				}

				return parameters;
			}
			else
			{
				return new Parameter[] { new Parameter(typeQualifier, type, name) };
			}
		}

		internal static Parameter[] Create(string type, string name, bool isOptional)
		{
			string[] types;

			if (BuiltInData.GenTypes.TryGetValue(type, out types))
			{
				Parameter[] parameters = new Parameter[types.Length];

				for (int i = 0; i < types.Length; i++)
				{
					parameters[i] = new Parameter(types[i], name, isOptional);
				}

				return parameters;
			}
			else
			{
				return new Parameter[] { new Parameter(type, name, isOptional) };
			}
		}

		internal static Parameter[] Create(string type, string name, int arraySize)
		{
			string[] types;

			if (BuiltInData.GenTypes.TryGetValue(type, out types))
			{
				Parameter[] parameters = new Parameter[types.Length];

				for (int i = 0; i < types.Length; i++)
				{
					parameters[i] = new Parameter(types[i], name, arraySize);
				}

				return parameters;
			}
			else
			{
				return new Parameter[] { new Parameter(type, name, arraySize) };
			}
		}
	}
}
