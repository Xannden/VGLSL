using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class TokenSeparatedList<T> where T : SyntaxNode
	{
		private readonly List<T> nodes = new List<T>();
		private readonly List<SyntaxToken> tokens = new List<SyntaxToken>();

		public IEnumerable<SyntaxNode> List
		{
			get
			{
				int nodeIndex = 0;
				int tokenIndex = 0;

				while (nodeIndex < this.nodes.Count)
				{
					yield return this.nodes[nodeIndex++];

					if (tokenIndex < this.tokens.Count)
					{
						yield return this.tokens[tokenIndex++];
					}
				}
			}
		}

		public IReadOnlyList<T> Nodes => this.nodes;

		public IReadOnlyList<SyntaxToken> Tokens => this.tokens;

		internal void AddNode(T node)
		{
			this.nodes.Add(node);
		}

		internal void AddToken(SyntaxToken token)
		{
			this.tokens.Add(token);
		}
	}
}