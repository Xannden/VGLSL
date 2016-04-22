using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public sealed class Definition
	{
		internal Definition(SyntaxNode node, Scope scope, IdentifierSyntax identifier, DefinitionType type)
		{
			this.Node = node;
			this.Scope = scope;
			this.Identifier = identifier;
			this.DefinitionType = type;
		}

		public SyntaxNode Node { get; }

		public Scope Scope { get; }

		public IdentifierSyntax Identifier { get; }

		public DefinitionType DefinitionType { get; }

		public override string ToString()
		{
			return $"Identifier = {this.Identifier.ToString()}, Type = {this.DefinitionType.ToString()}, Scope = [{this.Scope.Start.ToString()},{this.Scope.End.ToString()}]";
		}
	}
}
