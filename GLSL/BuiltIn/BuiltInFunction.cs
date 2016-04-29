using System.Collections.Generic;
using System.Text;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Text.Utility;

namespace Xannden.GLSL.BuiltIn
{
	public sealed class BuiltInFunction : BuiltInDefinition
	{
		internal BuiltInFunction(string returnType, string identifier, string documentation, params Parameter[] parameters) : base(identifier, documentation, DefinitionKind.Function)
		{
			this.ReturnType = returnType;

			if (parameters == null)
			{
				this.Parameters = new List<Parameter>();
			}
			else
			{
				this.Parameters = parameters;
			}
		}

		public string ReturnType { get; }

		public IReadOnlyList<Parameter> Parameters { get; }

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < this.Parameters.Count; i++)
			{
				if (i != 0)
				{
					if (this.Parameters[i].IsOptional)
					{
						builder.Append(" [, ");
					}
					else
					{
						builder.Append(", ");
					}
				}

				if (this.Parameters[i].IsOptional)
				{
					builder.Append($"{this.Parameters[i].ToString()}]");
				}
				else
				{
					builder.Append($"{this.Parameters[i].ToString()}");
				}
			}

			return $"{this.ReturnType} {this.Name}({builder.ToString()})";
		}

		internal override void WriteToXml(IndentedTextWriter writer)
		{
			writer.WriteLine("<Function>");
			writer.IndentLevel++;

			writer.WriteLine($"<ReturnType>{this.ReturnType}</ReturnType>");

			writer.WriteLine($"<Name>{this.Name}</Name>");

			writer.WriteLine("<Parameters>");
			writer.IndentLevel++;

			foreach (Parameter parameter in this.Parameters)
			{
				writer.WriteLine($"<Parameter IsOptional=\"{parameter.IsOptional}\">");
				writer.IndentLevel++;

				if (!string.IsNullOrEmpty(parameter.TypeQualifier))
				{
					writer.WriteLine($"<TypeQualifier>{parameter.TypeQualifier}</TypeQualifier>");
				}

				writer.WriteLine($"<Type>{parameter.VariableType}</Type>");

				writer.WriteLine($"<Name>{parameter.Identifier}</Name>");

				writer.IndentLevel--;
				writer.WriteLine("</Parameter>");
			}

			writer.IndentLevel--;
			writer.WriteLine("</Parameters>");

			writer.IndentLevel--;
			writer.WriteLine("</Function>");
		}
	}
}
