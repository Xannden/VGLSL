using System.Collections.Generic;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public class TokenSparatedList<T> where T : SyntaxNode
	{
		private List<T> nodes = new List<T>();
		private List<SyntaxToken> tokens = new List<SyntaxToken>();

		public void AddNode(T node)
		{
			this.nodes.Add(node);
		}

		public void AddToken(SyntaxToken token)
		{
			this.tokens.Add(token);
		}

		public IEnumerable<SyntaxNode> GetList()
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

		public T GetNode(int index)
		{
			return this.nodes[index];
		}

		public List<T> GetNodes()
		{
			return this.nodes;
		}

		public SyntaxToken GetToken(int index)
		{
			return this.tokens[index];
		}

		public List<SyntaxToken> GetTokens()
		{
			return this.tokens;
		}

		public T LastNode()
		{
			if (this.nodes.Count > 0)
			{
				return this.nodes[this.nodes.Count - 1];
			}

			return null;
		}

		public SyntaxToken LastToken()
		{
			if (this.tokens.Count > 0)
			{
				return this.tokens[this.tokens.Count - 1];
			}

			return null;
		}
	}
}