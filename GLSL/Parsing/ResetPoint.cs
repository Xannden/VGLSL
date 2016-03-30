using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tokens;

namespace Xannden.GLSL.Parsing
{
	internal class ResetPoint
	{
		public ResetPoint(LinkedListNode<Token> node)
		{
			this.Node = node;
		}

		internal LinkedListNode<Token> Node { get; }
	}
}