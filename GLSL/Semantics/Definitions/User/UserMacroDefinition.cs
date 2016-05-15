using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics.Definitions.User
{
	public sealed class UserMacroDefinition : MacroDefinition
	{
		public UserMacroDefinition(DefinePreprocessorSyntax define, IdentifierSyntax identifier, string documentation, Scope scope)
			: base(identifier.Identifier, define.Arguments?.ToColoredString(), define.TokenString?.ToColoredString(), documentation, scope, identifier.Span)
		{
		}
	}
}
