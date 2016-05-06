using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public class ParameterDefinition : UserDefinition
	{
		internal ParameterDefinition(ParameterSyntax parameter, Scope scope, IdentifierSyntax identifier, string documentation) : base(scope, identifier, documentation, DefinitionKind.Parameter)
		{
			this.TypeQualifier = parameter.TypeQualifier;
			this.ParameterType = new TypeDefinition(parameter.TypeSyntax);
			this.ArraySpecifiers = parameter.ArraySpecifiers;
		}

		public TypeQualifierSyntax TypeQualifier { get; }

		public TypeDefinition ParameterType { get; }

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers { get; }

		public override IReadOnlyList<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>();

			if (this.TypeQualifier != null)
			{
				result.AddRange(this.TypeQualifier.GetSyntaxTokens());
			}

			result.Add(this.ParameterType.TypeToken);

			if (this.ParameterType.ArraySpecifiers != null)
			{
				result.AddRange(this.ParameterType.ArraySpecifiers?.GetSyntaxTokens());
			}

			result.Add(this.Identifier);

			if (this.ArraySpecifiers != null)
			{
				result.AddRange(this.ArraySpecifiers?.GetSyntaxTokens());
			}

			return result;
		}
	}
}
