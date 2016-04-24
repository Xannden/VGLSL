using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public class ParameterDefinition : Definition
	{
		internal ParameterDefinition(ParameterSyntax parameter, Scope scope, IdentifierSyntax identifier) : base(scope, identifier, DefinitionKind.Parameter)
		{
			this.TypeQualfier = parameter.TypeQualifier;
			this.Type = new TypeDefinition(parameter.TypeSyntax);
			this.ArraySpecifiers = parameter.ArraySpecifiers;
		}

		public TypeQualifierSyntax TypeQualfier { get; }

		public TypeDefinition Type { get; }

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers { get; }

		public override List<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>(this.GetSyntaxTokens(this.TypeQualfier));

			result.Add(this.Type.Type);

			result.AddRange(this.GetSyntaxTokens(this.Type.ArraySpecifiers));

			result.Add(this.Identifier);

			result.AddRange(this.GetSyntaxTokens(this.ArraySpecifiers));

			return result;
		}
	}
}
