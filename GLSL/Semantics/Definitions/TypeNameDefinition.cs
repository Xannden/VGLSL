using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class TypeNameDefinition : UserDefinition
	{
		internal TypeNameDefinition(Scope scope, IdentifierSyntax identifier, string documentation) : base(scope, identifier, documentation, DefinitionKind.TypeName)
		{
		}

		public override IReadOnlyList<SyntaxToken> GetTokens()
		{
			return new List<SyntaxToken> { this.Identifier };
		}
	}
}
