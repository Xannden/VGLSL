using Xannden.GLSL.Semantics;

namespace Xannden.GLSL.BuiltIn
{
	public abstract class BuiltInDefinition : Definition
	{
		protected BuiltInDefinition(string name, string documentation, DefinitionKind kind) : base(new Scope(true), name, documentation, kind)
		{
		}
	}
}
