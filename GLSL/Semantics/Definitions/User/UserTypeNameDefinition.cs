using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics.Definitions.User
{
	public class UserTypeNameDefinition : TypeNameDefinition
	{
		public UserTypeNameDefinition(IdentifierSyntax identifier, string documentation, Scope scope)
			: base(identifier.Identifier, documentation, scope, identifier.Span)
		{
		}
	}
}
