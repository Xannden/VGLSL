using System.Collections.Generic;
using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.User
{
	public sealed class UserVariableDefinition : VariableDefinition
	{
		public UserVariableDefinition(InitPartSyntax initPart, IdentifierSyntax identifier, string documentation, DefinitionKind kind, Scope scope) : base(documentation, kind, scope, identifier.Span)
		{
			if (kind == DefinitionKind.LocalVariable)
			{
				this.Name = ColoredString.Create(identifier.Identifier, ColorType.LocalVariable);
			}
			else
			{
				this.Name = ColoredString.Create(identifier.Identifier, ColorType.GlobalVariable);
			}

			InitDeclaratorListSyntax initDeclaratorList = initPart.Parent as InitDeclaratorListSyntax;

			this.TypeQualifier = initDeclaratorList.TypeQualifier?.ToColoredString() ?? new List<ColoredString>();
			this.Type = new TypeDefinition(initDeclaratorList.TypeSyntax);

			List<ColoredString> arraySpecifiers = new List<ColoredString>();

			for (int i = 0; i < initPart.ArraySpecifiers.Count; i++)
			{
				arraySpecifiers.AddRange(initPart.ArraySpecifiers[i].ToColoredString());
			}

			this.ArraySpecifiers = arraySpecifiers;
		}

		public UserVariableDefinition(ConditionSyntax condition, IdentifierSyntax identifier, string documentation, DefinitionKind kind, Scope scope) : base(documentation, kind, scope, identifier.Span)
		{
			if (kind == DefinitionKind.LocalVariable)
			{
				this.Name = ColoredString.Create(identifier.Identifier, ColorType.LocalVariable);
			}
			else
			{
				this.Name = ColoredString.Create(identifier.Identifier, ColorType.GlobalVariable);
			}

			this.TypeQualifier = condition.TypeQualifier?.ToColoredString() ?? new List<ColoredString>();
			this.Type = new TypeDefinition(condition.TypeSyntax);
			this.ArraySpecifiers = new List<ColoredString>();
		}

		public UserVariableDefinition(StructDeclaratorSyntax declarator, IdentifierSyntax identifier, string documentation, DefinitionKind kind, Scope scope) : base(documentation, kind, scope, identifier.Span)
		{
			if (kind == DefinitionKind.LocalVariable)
			{
				this.Name = ColoredString.Create(identifier.Identifier, ColorType.LocalVariable);
			}
			else
			{
				this.Name = ColoredString.Create(identifier.Identifier, ColorType.GlobalVariable);
			}

			InterfaceBlockSyntax block = declarator.Parent as InterfaceBlockSyntax;

			this.TypeQualifier = block.TypeQualifier?.ToColoredString() ?? new List<ColoredString>();
			this.Type = new TypeDefinition(block.Identifier.Identifier);

			List<ColoredString> arraySpecifiers = new List<ColoredString>();

			for (int i = 0; i < declarator.ArraySpecifiers.Count; i++)
			{
				arraySpecifiers.AddRange(declarator.ArraySpecifiers[i].ToColoredString());
			}

			this.ArraySpecifiers = arraySpecifiers;
		}
	}
}
