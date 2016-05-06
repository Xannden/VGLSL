using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class VariableDefinition : UserDefinition
	{
		internal VariableDefinition(SyntaxNode node, Scope scope, IdentifierSyntax identifier, string documentation, DefinitionKind kind) : base(scope, identifier, documentation, kind)
		{
			switch (node?.SyntaxType)
			{
				case SyntaxType.InitPart:
					InitPartSyntax initPart = node as InitPartSyntax;
					InitDeclaratorListSyntax initDeclaratorList = node.Parent as InitDeclaratorListSyntax;

					this.TypeQualifier = initDeclaratorList.TypeQualifier;
					this.VariableType = new TypeDefinition(initDeclaratorList.TypeSyntax);
					this.ArraySpecifiers = initPart.ArraySpecifiers;
					break;
				case SyntaxType.Condition:
					ConditionSyntax condition = node as ConditionSyntax;

					this.TypeQualifier = condition.TypeQualifier;
					this.VariableType = new TypeDefinition(condition.TypeSyntax);
					this.ArraySpecifiers = new List<ArraySpecifierSyntax>();
					break;
				case SyntaxType.StructDeclarator:
					StructDeclaratorSyntax declarator = node as StructDeclaratorSyntax;
					InterfaceBlockSyntax interfaceBlock = node.Parent as InterfaceBlockSyntax;

					this.TypeQualifier = interfaceBlock.TypeQualifier;
					this.VariableType = new TypeDefinition(interfaceBlock.Identifier);
					this.ArraySpecifiers = declarator.ArraySpecifiers;
					break;
			}
		}

		public SyntaxNode TypeQualifier { get; }

		public TypeDefinition VariableType { get; }

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers { get; }

		public override IReadOnlyList<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>();

			if (this.TypeQualifier != null)
			{
				result.AddRange(this.TypeQualifier?.GetSyntaxTokens());
			}

			if (this.VariableType != null)
			{
				result.Add(this.VariableType.TypeToken);

				if (this.VariableType.ArraySpecifiers != null)
				{
					result.AddRange(this.VariableType.ArraySpecifiers?.GetSyntaxTokens());
				}
			}

			result.Add(this.Identifier);

			if (this.ArraySpecifiers != null)
			{
				result.AddRange(this.ArraySpecifiers?.GetSyntaxTokens());
			}

			return result;
		}
	}
}
