using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class FieldDefinition : UserDefinition
	{
		internal FieldDefinition(StructDeclaratorSyntax field, Scope scope, IdentifierSyntax identifier, string documentation) : base(scope, identifier, documentation, DefinitionKind.Field)
		{
			StructDeclarationSyntax declaration = field.Parent as StructDeclarationSyntax;

			this.TypeQualifier = declaration.TypeQualifier;
			this.FieldType = new TypeDefinition(declaration.TypeSyntax);
			this.ArraySpecifiers = field.ArraySpecifiers;
		}

		public TypeQualifierSyntax TypeQualifier { get; }

		public TypeDefinition FieldType { get; }

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers { get; }

		public override IReadOnlyList<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>(this.GetSyntaxTokens(this.TypeQualifier));

			result.Add(this.FieldType.TypeToken);

			result.AddRange(this.GetSyntaxTokens(this.FieldType.ArraySpecifiers));

			result.Add(this.Identifier);

			result.AddRange(this.GetSyntaxTokens(this.ArraySpecifiers));

			return result;
		}
	}
}