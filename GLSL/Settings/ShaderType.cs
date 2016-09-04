using System;

namespace Xannden.GLSL.Settings
{
	[Flags]
	public enum ShaderType : uint
	{
		None = 0,
		Compute = 1,
		Vertex = 2,
		Geometry = 4,
		TessellationControl = 8,
		TessellationEvaluation = 16,
		Fragment = 32,
		All = Compute | Vertex | Geometry | TessellationControl | TessellationEvaluation | Fragment,
	}
}