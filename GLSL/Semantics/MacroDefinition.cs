using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class MacroDefinition : Definition
	{
		internal MacroDefinition(DefinePreprocessorSyntax define, Scope scope, IdentifierSyntax identifier) : base(scope, identifier, DefinitionKind.Macro)
		{
			this.DefinePreprocessor = define;
		}

		public DefinePreprocessorSyntax DefinePreprocessor { get; }

		public override List<SyntaxToken> GetTokens()
		{
			return this.GetSyntaxTokens(this.DefinePreprocessor);
		}
	}
}
