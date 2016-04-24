using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class InterfaceBlockDefinition : Definition
	{
		internal InterfaceBlockDefinition(InterfaceBlockSyntax interfaceBlock, Scope scope, IdentifierSyntax identifier) : base(scope, identifier, DefinitionKind.InterfaceBlock)
		{
			this.TypeQualifier = interfaceBlock.TypeQualifier;
		}

		public TypeQualifierSyntax TypeQualifier { get; }

		public override List<SyntaxToken> GetTokens()
		{
			List<SyntaxToken> result = new List<SyntaxToken>(this.GetSyntaxTokens(this.TypeQualifier.SingleTypeQualifiers.Find(qualifier => qualifier.StorageQualifier != null)));
			result.Add(this.Identifier);

			return result;
		}
	}
}
