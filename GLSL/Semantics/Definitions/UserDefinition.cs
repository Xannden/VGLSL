using System.Collections.Generic;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public abstract class UserDefinition : Definition
	{
		protected UserDefinition(Scope scope, IdentifierSyntax identifier, string documentation, DefinitionKind kind) : base(scope, identifier?.Identifier, documentation, kind)
		{
			this.Identifier = identifier;
		}

		public IdentifierSyntax Identifier { get; }

		public abstract IReadOnlyList<SyntaxToken> GetTokens();

		protected IReadOnlyList<SyntaxToken> GetSyntaxTokens(SyntaxNode node)
		{
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			this.GetSyntaxTokensRecursive(node, tokens);

			return tokens;
		}

		protected IReadOnlyList<SyntaxToken> GetSyntaxTokens<T>(IReadOnlyList<T> list) where T : SyntaxNode
		{
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			for (int i = 0; i < list?.Count; i++)
			{
				this.GetSyntaxTokensRecursive(list?[i], tokens);
			}

			return tokens;
		}

		private void GetSyntaxTokensRecursive(SyntaxNode node, List<SyntaxToken> tokens)
		{
			SyntaxToken token = node as SyntaxToken;

			if (token != null)
			{
				tokens.Add(token);
			}
			else
			{
				for (int i = 0; i < node?.Children.Count; i++)
				{
					if (node.SyntaxType != SyntaxType.Preprocessor)
					{
						this.GetSyntaxTokensRecursive(node.Children[i], tokens);
					}
				}
			}
		}
	}
}
