using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class MacroDefinition : UserDefinition
	{
		internal MacroDefinition(DefinePreprocessorSyntax define, Scope scope, IdentifierSyntax identifier, string documentation) : base(scope, identifier, documentation, DefinitionKind.Macro)
		{
			this.DefinePreprocessor = define;
		}

		public DefinePreprocessorSyntax DefinePreprocessor { get; }

		public override IReadOnlyList<SyntaxToken> GetTokens()
		{
			return this.GetSyntaxTokens(this.DefinePreprocessor);
		}
	}
}
