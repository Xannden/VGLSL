using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class TypeNameDefinition : Definition
	{
		private List<FieldDefinition> fields = new List<FieldDefinition>();

		internal TypeNameDefinition(Scope scope, IdentifierSyntax identifier) : base(scope, identifier, DefinitionKind.TypeName)
		{
		}

		public IReadOnlyList<FieldDefinition> Fields => this.fields;

		public override List<SyntaxToken> GetTokens()
		{
			return new List<SyntaxToken> { this.Identifier };
		}

		internal void AddField(FieldDefinition field)
		{
			this.fields.Add(field);
		}
	}
}
