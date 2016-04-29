using Xannden.GLSL.Semantics;
using Xannden.GLSL.Text.Utility;

namespace Xannden.GLSL.BuiltIn
{
	public abstract class BuiltInDefinition : Definition
	{
		protected BuiltInDefinition(string name, string documentation, DefinitionKind kind) : base(new Scope(true), name, documentation, kind)
		{
		}

		internal abstract void WriteToXml(IndentedTextWriter writer);
	}
}
