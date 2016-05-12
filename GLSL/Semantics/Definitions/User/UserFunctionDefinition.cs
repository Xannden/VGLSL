using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics.Definitions.User
{
	public sealed class UserFunctionDefinition : FunctionDefinition
	{
		public UserFunctionDefinition(FunctionHeaderSyntax header, IdentifierSyntax identifier, string documentation, Scope scope) : base(new TypeDefinition(header.ReturnType), identifier.Identifier, documentation, scope, identifier.Span)
		{
		}
	}
}
