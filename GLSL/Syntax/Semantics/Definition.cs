using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Syntax.Semantics
{
	public sealed class Definition
	{
		internal Definition(SyntaxNode node, Scope scope, IdentifierSyntax identifier, DefinitionType type)
		{
			this.Node = node;
			this.Scope = scope;
			this.Identifier = identifier;
			this.Type = type;
		}

		public SyntaxNode Node { get; }

		public Scope Scope { get; }

		public IdentifierSyntax Identifier { get; }

		public DefinitionType Type { get; }

		public override string ToString()
		{
			return $"Identifier = {this.Identifier.ToString()}, Type = {this.Type.ToString()}, Scope = [{this.Scope.Start.ToString()},{this.Scope.End.ToString()}]";
		}
	}
}
