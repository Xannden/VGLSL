using System.Collections.Generic;
using Xannden.GLSL.Properties;
using Xannden.GLSL.Semantics;

namespace Xannden.GLSL.BuiltIn
{
	public sealed class BuiltInData
	{
		private List<BuiltInDefinition> definitions = new List<BuiltInDefinition>();

		private BuiltInData()
		{
			this.LoadData();
		}

		public static BuiltInData Instance { get; } = new BuiltInData();

		public IReadOnlyList<Definition> Definitions => this.definitions;

		private void LoadData()
		{
			this.LoadFunctions();
		}

		private void LoadFunctions()
		{
			// genType = float, vec2, vec3, vec4
			// genIType = int, ivec2, ivec3, ivec4
			// genUType = uint, uvec2, uvec3, uvec4
			// genBType = bool, bvec2, bvec3, bvec4
			// genDType = double, dvec2, dvec3, dvec4
			List<BuiltInDefinition> functions = new List<BuiltInDefinition>();

			functions.Add(new BuiltInFunction("float", "radians", Resources.RadiansDoc, new Parameter("float", "degrees")));
			functions.Add(new BuiltInFunction("vec2", "radians", Resources.RadiansDoc, new Parameter("vec2", "degrees")));
			functions.Add(new BuiltInFunction("vec3", "radians", Resources.RadiansDoc, new Parameter("vec3", "degrees")));
			functions.Add(new BuiltInFunction("vec4", "radians", Resources.RadiansDoc, new Parameter("vec4", "degrees")));

			functions.Add(new BuiltInFunction("float", "degrees", Resources.DegreesDoc, new Parameter("float", "radians")));
			functions.Add(new BuiltInFunction("vec2", "degrees", Resources.DegreesDoc, new Parameter("vec2", "radians")));
			functions.Add(new BuiltInFunction("vec3", "degrees", Resources.DegreesDoc, new Parameter("vec3", "radians")));
			functions.Add(new BuiltInFunction("vec4", "degrees", Resources.DegreesDoc, new Parameter("vec4", "radians")));

			functions.Add(new BuiltInFunction("float", "sin", Resources.SinDoc, new Parameter("float", "angle")));
			functions.Add(new BuiltInFunction("vec2", "sin", Resources.SinDoc, new Parameter("vec2", "angle")));
			functions.Add(new BuiltInFunction("vec3", "sin", Resources.SinDoc, new Parameter("vec3", "angle")));
			functions.Add(new BuiltInFunction("vec4", "sin", Resources.SinDoc, new Parameter("vec4", "angle")));

			this.definitions.AddRange(functions);
		}
	}
}
