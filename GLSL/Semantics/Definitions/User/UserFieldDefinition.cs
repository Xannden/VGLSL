using System.Collections.Generic;
using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.User
{
	public sealed class UserFieldDefinition : FieldDefinition
	{
		public UserFieldDefinition(StructDeclaratorSyntax field, IdentifierSyntax identifier, string documentation, Scope scope) : base((field.Parent as StructDeclarationSyntax).TypeQualifier?.ToColoredString(), new TypeDefinition((field.Parent as StructDeclarationSyntax).TypeSyntax), identifier.Identifier, documentation, scope, identifier.Span)
		{
			List<ColoredString> arraySpecifiers = new List<ColoredString>();

			for (int i = 0; i < field.ArraySpecifiers.Count; i++)
			{
				arraySpecifiers.AddRange(field.ArraySpecifiers[i].ToColoredString());
			}

			this.ArraySpecifiers = arraySpecifiers;
		}
	}
}
