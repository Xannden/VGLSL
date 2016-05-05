using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class InterfaceBlockDefinition : UserDefinition
	{
		internal InterfaceBlockDefinition(InterfaceBlockSyntax interfaceBlock, Scope scope, IdentifierSyntax identifier, string documentation) : base(scope, identifier, documentation, DefinitionKind.InterfaceBlock)
		{
			this.TypeQualifier = interfaceBlock.TypeQualifier;
		}

		public TypeQualifierSyntax TypeQualifier { get; }

		public override IReadOnlyList<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>(this.TypeQualifier.SingleTypeQualifiers.Find(qualifier => qualifier.StorageQualifier != null).GetSyntaxTokens());
			result.Add(this.Identifier);

			return result;
		}
	}
}
