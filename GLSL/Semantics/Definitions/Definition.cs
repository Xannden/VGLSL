using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics
{
	public class Definition
	{
		protected Definition(Scope scope, string name, string documentation, DefinitionKind kind, TrackingSpan span)
		{
			this.Scope = scope;
			this.Name = name;
			this.Kind = kind;
			this.Documentation = documentation;
			this.Span = span;
		}

		public Scope Scope { get; }

		public string Name { get; }

		public string Documentation { get; }

		public DefinitionKind Kind { get; }

		public TrackingSpan Span { get; }

		public override string ToString()
		{
			return $"Identifier = {this.Name}, Type = {this.Kind.ToString()}, Scope = [{this.Scope.Start.ToString()},{this.Scope.End.ToString()}]";
		}
	}
}