using System.Collections.Generic;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class VariableDefinition : Definition
	{
		internal VariableDefinition(SyntaxNode node, Scope scope, IdentifierSyntax identifier, DefinitionKind kind) : base(scope, identifier, kind)
		{
			switch (node.SyntaxType)
			{
				case SyntaxType.InitPart:
					InitPartSyntax initPart = node as InitPartSyntax;
					InitDeclaratorListSyntax initDeclaratorList = node.Parent as InitDeclaratorListSyntax;

					this.TypeQualifier = initDeclaratorList.TypeQualifier;
					this.Type = new TypeDefinition(initDeclaratorList.TypeSyntax);
					this.ArraySpecifiers = initPart.ArraySpecifiers;
					break;
				case SyntaxType.Condition:
					ConditionSyntax condition = node as ConditionSyntax;

					this.TypeQualifier = condition.TypeQualifier;
					this.Type = new TypeDefinition(condition.TypeSyntax);
					this.ArraySpecifiers = new List<ArraySpecifierSyntax>();
					break;
				case SyntaxType.StructDeclarator:
					StructDeclaratorSyntax declarator = node as StructDeclaratorSyntax;
					InterfaceBlockSyntax interfaceBlock = node.Parent as InterfaceBlockSyntax;

					this.TypeQualifier = interfaceBlock.TypeQualifier;
					this.Type = new TypeDefinition(interfaceBlock.Identifier);
					this.ArraySpecifiers = declarator.ArraySpecifiers;
					break;
			}
		}

		public SyntaxNode TypeQualifier { get; }

		public TypeDefinition Type { get; }

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers { get; }

		public override List<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>(this.GetSyntaxTokens(this.TypeQualifier));

			if (this.Type != null)
			{
				result.Add(this.Type.Type);

				result.AddRange(this.GetSyntaxTokens(this.Type.ArraySpecifiers));
			}

			result.Add(this.Identifier);

			result.AddRange(this.GetSyntaxTokens(this.ArraySpecifiers));

			return result;
		}
	}
}
