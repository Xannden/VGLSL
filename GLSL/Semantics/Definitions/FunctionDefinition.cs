using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class FunctionDefinition : UserDefinition
	{
		private List<ParameterDefinition> parameters = new List<ParameterDefinition>();
		private FunctionHeaderSyntax header;

		internal FunctionDefinition(FunctionHeaderSyntax header, Scope scope, IdentifierSyntax identifier, string documentation) : base(scope, identifier, documentation, DefinitionKind.Function)
		{
			this.header = header;
			this.TypeQualifier = header.TypeQualifier;
			this.ReturnType = new TypeDefinition(header.ReturnType);
		}

		public TypeQualifierSyntax TypeQualifier { get; }

		public TypeDefinition ReturnType { get; }

		public IReadOnlyList<ParameterDefinition> Parameters => this.parameters;

		public override List<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>(this.GetSyntaxTokens(this.TypeQualifier));

			result.Add(this.ReturnType.Type);

			result.AddRange(this.GetSyntaxTokens(this.ReturnType.ArraySpecifiers));

			result.Add(this.Identifier);

			result.Add(this.header.LeftParentheses);

			for (int i = 0; i < this.parameters.Count; i++)
			{
				result.AddRange(this.parameters[i].GetTokens());

				if (i != this.parameters.Count - 1)
				{
					result.Add(this.header.Parameters.Tokens[i]);
				}
			}

			result.Add(this.header.RightParentheses);

			return result;
		}

		internal void AddParameter(ParameterDefinition parameter)
		{
			this.parameters.Add(parameter);
		}
	}
}