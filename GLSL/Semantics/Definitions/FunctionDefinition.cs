using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics
{
	public sealed class FunctionDefinition : UserDefinition
	{
		private readonly List<ParameterDefinition> parameters = new List<ParameterDefinition>();
		private readonly FunctionHeaderSyntax header;

		internal FunctionDefinition(FunctionHeaderSyntax header, Scope scope, IdentifierSyntax identifier, string documentation) : base(scope, identifier, documentation, DefinitionKind.Function)
		{
			this.header = header;
			this.TypeQualifier = header.TypeQualifier;
			this.ReturnType = new TypeDefinition(header.ReturnType);
		}

		public TypeQualifierSyntax TypeQualifier { get; }

		public TypeDefinition ReturnType { get; }

		public IReadOnlyList<ParameterDefinition> Parameters => this.parameters;

		public override IReadOnlyList<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>();

			if (this.TypeQualifier != null)
			{
				result.AddRange(this.TypeQualifier.GetSyntaxTokens());
			}

			result.Add(this.ReturnType.TypeToken);

			if (this.ReturnType.ArraySpecifiers != null)
			{
				result.AddRange(this.ReturnType.ArraySpecifiers?.GetSyntaxTokens());
			}

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

		public Span GetRelativeParameterSpan(int paramIndex, Snapshot snapshot)
		{
			int start = this.header.Span.GetSpan(snapshot).Start;

			Span span = this.header.Parameters.Nodes[paramIndex].Span.GetSpan(snapshot);

			return Text.Span.Create(span.Start - start, span.End - start);
		}

		internal void AddParameter(ParameterDefinition parameter)
		{
			this.parameters.Add(parameter);
		}
	}
}