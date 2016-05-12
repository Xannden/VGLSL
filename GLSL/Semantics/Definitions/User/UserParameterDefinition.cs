using System.Collections.Generic;
using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.User
{
	public sealed class UserParameterDefinition : ParameterDefinition
	{
		public UserParameterDefinition(ParameterSyntax parameter, IdentifierSyntax identifier, string documentation, Scope scope) : base(parameter.TypeQualifier?.ToColoredString(), new TypeDefinition(parameter.TypeSyntax), identifier.Identifier, documentation, scope, identifier.Span)
		{
			List<ColoredString> arraySpecifiers = new List<ColoredString>();

			for (int i = 0; i < parameter.ArraySpecifiers.Count; i++)
			{
				arraySpecifiers.AddRange(parameter.ArraySpecifiers[i].ToColoredString());
			}

			this.ArraySpecifiers = arraySpecifiers;
		}
	}
}
