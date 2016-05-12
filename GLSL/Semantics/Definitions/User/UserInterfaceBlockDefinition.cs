using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics.Definitions.User
{
	public sealed class UserInterfaceBlockDefinition : InterfaceBlockDefinition
	{
		public UserInterfaceBlockDefinition(InterfaceBlockSyntax interfaceBlock, IdentifierSyntax identifier, string documentation, Scope scope) : base(interfaceBlock.TypeQualifier?.ToColoredString(), identifier.Identifier, documentation, scope, identifier.Span)
		{
		}
	}
}
