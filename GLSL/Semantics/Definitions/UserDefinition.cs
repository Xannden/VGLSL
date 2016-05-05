using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public abstract class UserDefinition : Definition
	{
		protected UserDefinition(Scope scope, IdentifierSyntax identifier, string documentation, DefinitionKind kind) : base(scope, identifier?.Identifier, documentation, kind, identifier.Span)
		{
			this.Identifier = identifier;
		}

		public IdentifierSyntax Identifier { get; }

		public abstract IReadOnlyList<SyntaxToken> GetTokens();
	}
}
