using System.Collections.Generic;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public abstract class Definition
	{
		internal Definition(Scope scope, IdentifierSyntax identifier, DefinitionKind kind)
		{
			this.Scope = scope;
			this.Identifier = identifier;
			this.Kind = kind;
		}

		public Scope Scope { get; }

		public IdentifierSyntax Identifier { get; }

		public DefinitionKind Kind { get; }

		public override string ToString()
		{
			return $"Identifier = {this.Identifier.ToString()}, Type = {this.Kind.ToString()}, Scope = [{this.Scope.Start.ToString()},{this.Scope.End.ToString()}]";
		}

		public abstract List<SyntaxToken> GetTokens();

		protected List<SyntaxToken> GetSyntaxTokens(SyntaxNode node)
		{
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			this.RecusiveGetSyntaxTokens(node, tokens);

			return tokens;
		}

		protected List<SyntaxToken> GetSyntaxTokens(IReadOnlyList<SyntaxNode> list)
		{
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			for (int i = 0; i < list.Count; i++)
			{
				tokens.AddRange(this.GetSyntaxTokens(list[i]));
			}

			return tokens;
		}

		private void RecusiveGetSyntaxTokens(SyntaxNode node, List<SyntaxToken> tokens)
		{
			if (node == null)
			{
				return;
			}

			if (node is SyntaxToken)
			{
				tokens.Add(node as SyntaxToken);
			}
			else
			{
				for (int i = 0; i < node.Children.Count; i++)
				{
					if (node.Children[i].SyntaxType != SyntaxType.Preprocessor)
					{
						this.RecusiveGetSyntaxTokens(node.Children[i], tokens);
					}
				}
			}
		}
	}
}