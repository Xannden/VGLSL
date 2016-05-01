using System.Collections.Generic;
using System.IO;
using Xannden.GLSL.Properties;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Text.Utility;

namespace Xannden.GLSL.BuiltIn
{
	// Be careful when changing things in this class because it defines all of the built-in functions in GLSL and you don't want to accidentally change or remove some of them
	public sealed class BuiltInData
	{
		private readonly List<BuiltInDefinition> definitions = new List<BuiltInDefinition>();

		static BuiltInData()
		{
			GenTypes = new Dictionary<string, string[]>
			{
				["genType"] = new string[] { "float", "vec2", "vec3", "vec4" },
				["genIType"] = new string[] { "int", "ivec2", "ivec3", "ivec4" },
				["genUType"] = new string[] { "uint", "uvec2", "uvec3", "uvec4" },
				["genBType"] = new string[] { "bool", "bvec2", "bvec3", "bvec4" },
				["genDType"] = new string[] { "double", "dvec2", "dvec3", "dvec4" },
				["mat"] = new string[] { "mat2", "mat2x3", "mat2x4", "mat3x2", "mat3", "mat3x4", "mat4x2", "mat4x3", "mat4" },
				["dmat"] = new string[] { "dmat2", "dmat2x3", "dmat2x4", "dmat3x2", "dmat3", "dmat3x4", "dmat4x2", "dmat4x3", "dmat4" },
				["bvec"] = new string[] { "bvec2", "bvec3", "bvec4" },
				["ivec"] = new string[] { "ivec2", "ivec3", "ivec4" },
				["uvec"] = new string[] { "uvec2", "uvec3", "uvec4" },
				["vec"] = new string[] { "vec2", "vec3", "vec4" },
				["dvec"] = new string[] { "dvec2", "dvec3", "dvec4" },
				["gsampler1D"] = new string[] { "sampler1D", "isampler1D", "usampler1D" },
				["gsampler2D"] = new string[] { "sampler2D", "isampler2D", "usampler2D" },
				["gsampler3D"] = new string[] { "sampler3D", "isampler3D", "usampler3D" },
				["gsamplerCube"] = new string[] { "samplerCube", "isamplerCube", "usamplerCube" },
				["gsamplerCubeArray"] = new string[] { "samplerCubeArray", "isamplerCubeArray", "usamplerCubeArray" },
				["gsampler2DRect"] = new string[] { "sampler2DRect", "isampler2DRect", "usampler2DRect" },
				["gsampler1DArray"] = new string[] { "sampler1DArray", "isampler1DArray", "usampler1DArray" },
				["gsampler2DArray"] = new string[] { "sampler2DArray", "isampler2DArray", "usampler2DArray" },
				["gsampler2DMS"] = new string[] { "sampler2DMS", "isampler2DMS", "usampler2DMS" },
				["gsampler2DMSArray"] = new string[] { "sampler2DMSArray", "isampler2DMSArray", "usampler2DMSArray" },
				["gsamplerCubeArrayShadow"] = new string[] { "samplerCubeArrayShadow", "isamplerCubeArrayShadow", "usamplerCubeArrayShadow" },
				["gsamplerBuffer"] = new string[] { "samplerBuffer", "isamplerBuffer", "usamplerBuffer" },
				["gvec4"] = new string[] { "vec4", "ivec4", "uvec4" },
				["gimage1D"] = new string[] { "image1D", "iimage1D", "uimage1D" },
				["gimage2D"] = new string[] { "image2D", "iimage2D", "uimage2D" },
				["gimage3D"] = new string[] { "image3D", "iimage3D", "uimage3D" },
				["gimageCube"] = new string[] { "imageCube", "iimageCube", "uimageCube" },
				["gimageCubeArray"] = new string[] { "imageCubeArray", "iimageCubeArray", "uimageCubeArray" },
				["gimageRect"] = new string[] { "imageRect", "iimageRect", "uimageRect" },
				["gimage1DArray"] = new string[] { "image1DArray", "iimage1DArray", "uimage1DArray" },
				["gimage2DArray"] = new string[] { "image2DArray", "iimage2DArray", "uimage2DArray" },
				["gimageBuffer"] = new string[] { "imageBuffer", "iimageBuffer", "uimageBuffer" },
				["gimage2DMS"] = new string[] { "image2DMS", "iimage2DMS", "uimage2DMS" },
				["gimage2DMSArray"] = new string[] { "image2DMSArray", "iimage2DMSArray", "uimage2DMSArray" },
				["gimage2DRect"] = new string[] { "image2DRect", "iimage2DRect", "uimage2DRect" }
			};
		}

		public static BuiltInData Instance { get; } = new BuiltInData();

		public IReadOnlyList<Definition> Definitions => this.definitions;

		internal static Dictionary<string, string[]> GenTypes { get; }

		internal void LoadData()
		{
			this.LoadFunctions();
			this.LoadVariables();
		}

		private void LoadFunctions()
		{
			List<BuiltInFunction> functionList = new List<BuiltInFunction>();

			// Angle and Trigonometry
			this.AddFunction(functionList, "genType", "radians", Resources.RadiansDoc, Parameter.Create("genType", "degrees"));
			this.AddFunction(functionList, "genType", "degrees", Resources.DegreesDoc, Parameter.Create("genType", "radians"));
			this.AddFunction(functionList, "genType", "sin", Resources.SinDoc, Parameter.Create("genType", "angle"));
			this.AddFunction(functionList, "genType", "cos", string.Empty, Parameter.Create("genType", "angle"));
			this.AddFunction(functionList, "genType", "tan", string.Empty, Parameter.Create("genType", "angle"));
			this.AddFunction(functionList, "genType", "asin", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "acos", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "atan", string.Empty, Parameter.Create("genType", "y"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "atan", string.Empty, Parameter.Create("genType", "y_over_x"));
			this.AddFunction(functionList, "genType", "sinh", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "cosh", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "tanh", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "asinh", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "acosh", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "atanh", string.Empty, Parameter.Create("genType", "x"));

			// Exponential
			this.AddFunction(functionList, "genType", "pow", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "genType", "exp", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "log", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "exp2", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "log2", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "sqrt", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "sqrt", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "inversesqrt", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "inversesqrt", string.Empty, Parameter.Create("genDType", "x"));

			// Common
			this.AddFunction(functionList, "genType", "abs", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genIType", "abs", string.Empty, Parameter.Create("genIType", "x"));
			this.AddFunction(functionList, "genDType", "abs", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "sign", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genIType", "sign", string.Empty, Parameter.Create("genIType", "x"));
			this.AddFunction(functionList, "genDType", "sign", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "floor", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "floor", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "trunc", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "trunc", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "round", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "round", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "roundEven", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "roundEven", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "ceil", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "ceil", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "fract", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "fract", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "mod", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("float", "y"));
			this.AddFunction(functionList, "genType", "mod", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "genDType", "mod", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("double", "y"));
			this.AddFunction(functionList, "genDType", "mod", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"));
			this.AddFunction(functionList, "genType", "modf", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("out", "genType", "i"));
			this.AddFunction(functionList, "genDType", "modf", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("out", "genDType", "i"));
			this.AddFunction(functionList, "genType", "min", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "genType", "min", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("float", "y"));
			this.AddFunction(functionList, "genDType", "min", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"));
			this.AddFunction(functionList, "genDType", "min", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("double", "y"));
			this.AddFunction(functionList, "genIType", "min", string.Empty, Parameter.Create("genIType", "x"), Parameter.Create("genIType", "y"));
			this.AddFunction(functionList, "genIType", "min", string.Empty, Parameter.Create("genIType", "x"), Parameter.Create("int", "y"));
			this.AddFunction(functionList, "genUType", "min", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"));
			this.AddFunction(functionList, "genUType", "min", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("uint", "y"));
			this.AddFunction(functionList, "genType", "max", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "genType", "max", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("float", "y"));
			this.AddFunction(functionList, "genDType", "max", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"));
			this.AddFunction(functionList, "genDType", "max", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("double", "y"));
			this.AddFunction(functionList, "genIType", "max", string.Empty, Parameter.Create("genIType", "x"), Parameter.Create("genIType", "y"));
			this.AddFunction(functionList, "genIType", "max", string.Empty, Parameter.Create("genIType", "x"), Parameter.Create("int", "y"));
			this.AddFunction(functionList, "genUType", "max", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"));
			this.AddFunction(functionList, "genUType", "max", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("uint", "y"));
			this.AddFunction(functionList, "genType", "clamp", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "minVal"), Parameter.Create("genType", "maxVal"));
			this.AddFunction(functionList, "genType", "clamp", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("float", "minVal"), Parameter.Create("float", "maxVal"));
			this.AddFunction(functionList, "genDType", "clamp", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "minVal"), Parameter.Create("genDType", "maxVal"));
			this.AddFunction(functionList, "genDType", "clamp", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("double", "minVal"), Parameter.Create("double", "maxVal"));
			this.AddFunction(functionList, "genIType", "clamp", string.Empty, Parameter.Create("genIType", "x"), Parameter.Create("genIType", "minVal"), Parameter.Create("genIType", "maxVal"));
			this.AddFunction(functionList, "genIType", "clamp", string.Empty, Parameter.Create("genIType", "x"), Parameter.Create("int", "minVal"), Parameter.Create("float", "int"));
			this.AddFunction(functionList, "genUType", "clamp", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "minVal"), Parameter.Create("genUType", "maxVal"));
			this.AddFunction(functionList, "genUType", "clamp", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("uint", "minVal"), Parameter.Create("uint", "maxVal"));
			this.AddFunction(functionList, "genType", "mix", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"), Parameter.Create("genType", "a"));
			this.AddFunction(functionList, "genType", "mix", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"), Parameter.Create("float", "a"));
			this.AddFunction(functionList, "genDType", "mix", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"), Parameter.Create("genDType", "a"));
			this.AddFunction(functionList, "genDType", "mix", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"), Parameter.Create("double", "a"));
			this.AddFunction(functionList, "genType", "mix", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"), Parameter.Create("genBType", "a"));
			this.AddFunction(functionList, "genDType", "mix", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"), Parameter.Create("genBType", "a"));
			this.AddFunction(functionList, "genType", "step", string.Empty, Parameter.Create("genType", "edge"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "step", string.Empty, Parameter.Create("float", "edge"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "step", string.Empty, Parameter.Create("genDType", "edge"), Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genDType", "step", string.Empty, Parameter.Create("double", "edge"), Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "smoothstep", string.Empty, Parameter.Create("genType", "edge0"), Parameter.Create("genType", "edge1"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "smoothstep", string.Empty, Parameter.Create("float", "edge0"), Parameter.Create("float", "edge1"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "smoothstep", string.Empty, Parameter.Create("genDType", "edge0"), Parameter.Create("genDType", "edge1"), Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genDType", "smoothstep", string.Empty, Parameter.Create("double", "edge0"), Parameter.Create("double", "edge1"), Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genBType", "isnan", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genBType", "isnan", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genBType", "isinf", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genBType", "isinf", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genIType", "floatBitsToInt", string.Empty, Parameter.Create("genType", "value"));
			this.AddFunction(functionList, "genUType", "floatBitsToUnt", string.Empty, Parameter.Create("genType", "value"));
			this.AddFunction(functionList, "genType", "intBitsToFloat", string.Empty, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genType", "uintBitsToFloat", string.Empty, Parameter.Create("genUType", "value"));
			this.AddFunction(functionList, "genType", "fma", string.Empty, Parameter.Create("genType", "a"), Parameter.Create("genType", "b"), Parameter.Create("genType", "c"));
			this.AddFunction(functionList, "genDType", "fma", string.Empty, Parameter.Create("genDType", "a"), Parameter.Create("genDType", "b"), Parameter.Create("genDType", "c"));
			this.AddFunction(functionList, "genType", "frexp", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("out", "genIType", "exp"));
			this.AddFunction(functionList, "genDType", "frexp", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("out", "genIType", "exp"));
			this.AddFunction(functionList, "genType", "ldexp", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("in", "genIType", "exp"));
			this.AddFunction(functionList, "genDType", "ldexp", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("in", "genIType", "exp"));

			// Floating-Point Pack and Unpack
			this.AddFunction(functionList, "uint", "packUnorm2x16", string.Empty, Parameter.Create("vec2", "v"));
			this.AddFunction(functionList, "uint", "packSnorm2x16", string.Empty, Parameter.Create("vec2", "v"));
			this.AddFunction(functionList, "uint", "packUnorm4x8", string.Empty, Parameter.Create("vec4", "v"));
			this.AddFunction(functionList, "uint", "packSnorm4x8", string.Empty, Parameter.Create("vec4", "v"));
			this.AddFunction(functionList, "vec2", "unpackUnorm2x16", string.Empty, Parameter.Create("uint", "p"));
			this.AddFunction(functionList, "vec2", "unpackSnorm2x16", string.Empty, Parameter.Create("uint", "p"));
			this.AddFunction(functionList, "vec4", "unpackUnorm4x8", string.Empty, Parameter.Create("uint", "p"));
			this.AddFunction(functionList, "vec4", "unpackSnorm4x8", string.Empty, Parameter.Create("uint", "p"));
			this.AddFunction(functionList, "double", "packDouble2x32", string.Empty, Parameter.Create("uvec2", "v"));
			this.AddFunction(functionList, "uvec2", "unpackDouble2x32", string.Empty, Parameter.Create("double", "v"));
			this.AddFunction(functionList, "uint", "packHalf2x16", string.Empty, Parameter.Create("vec2", "v"));
			this.AddFunction(functionList, "vec2", "unpackHalf2x16", string.Empty, Parameter.Create("uint", "v"));

			// Geometric
			this.AddFunction(functionList, "float", "length", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "double", "length", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "float", "distance", string.Empty, Parameter.Create("genType", "p0"), Parameter.Create("genType", "p1"));
			this.AddFunction(functionList, "double", "distance", string.Empty, Parameter.Create("genDType", "p0"), Parameter.Create("genDType", "p1"));
			this.AddFunction(functionList, "float", "dot", string.Empty, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "double", "dot", string.Empty, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"));
			this.AddFunction(functionList, "vec3", "cross", string.Empty, Parameter.Create("vec3", "x"), Parameter.Create("vec3", "y"));
			this.AddFunction(functionList, "dvec3", "cross", string.Empty, Parameter.Create("dvec3", "x"), Parameter.Create("dvec3", "y"));
			this.AddFunction(functionList, "genType", "normalize", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "normalize", string.Empty, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "vec4", "ftransform", string.Empty);
			this.AddFunction(functionList, "genType", "faceforward", string.Empty, Parameter.Create("genType", "N"), Parameter.Create("genType", "I"), Parameter.Create("genType", "Nref"));
			this.AddFunction(functionList, "genDType", "faceforward", string.Empty, Parameter.Create("genDType", "N"), Parameter.Create("genDType", "I"), Parameter.Create("genDType", "Nref"));
			this.AddFunction(functionList, "genType", "reflect", string.Empty, Parameter.Create("genType", "I"), Parameter.Create("genType", "N"));
			this.AddFunction(functionList, "genDType", "reflect", string.Empty, Parameter.Create("genDType", "I"), Parameter.Create("genDType", "N"));
			this.AddFunction(functionList, "genType", "refract", string.Empty, Parameter.Create("genType", "I"), Parameter.Create("genType", "N"), Parameter.Create("float", "eta"));
			this.AddFunction(functionList, "genDType", "refract", string.Empty, Parameter.Create("genDType", "I"), Parameter.Create("genDType", "N"), Parameter.Create("float", "eta"));

			// Matrix
			this.AddFunction(functionList, "mat", "matrixCompMult", string.Empty, Parameter.Create("mat", "x"), Parameter.Create("mat", "y"));
			this.AddFunction(functionList, "mat2", "outerProduct", string.Empty, Parameter.Create("vec2", "c"), Parameter.Create("vec2", "r"));
			this.AddFunction(functionList, "mat3", "outerProduct", string.Empty, Parameter.Create("vec3", "c"), Parameter.Create("vec3", "r"));
			this.AddFunction(functionList, "mat4", "outerProduct", string.Empty, Parameter.Create("vec4", "c"), Parameter.Create("vec4", "r"));
			this.AddFunction(functionList, "mat2x3", "outerProduct", string.Empty, Parameter.Create("vec3", "c"), Parameter.Create("vec2", "r"));
			this.AddFunction(functionList, "mat3x2", "outerProduct", string.Empty, Parameter.Create("vec2", "c"), Parameter.Create("vec3", "r"));
			this.AddFunction(functionList, "mat2x4", "outerProduct", string.Empty, Parameter.Create("vec4", "c"), Parameter.Create("vec2", "r"));
			this.AddFunction(functionList, "mat4x2", "outerProduct", string.Empty, Parameter.Create("vec2", "c"), Parameter.Create("vec4", "r"));
			this.AddFunction(functionList, "mat3x4", "outerProduct", string.Empty, Parameter.Create("vec4", "c"), Parameter.Create("vec3", "r"));
			this.AddFunction(functionList, "mat4x3", "outerProduct", string.Empty, Parameter.Create("vec3", "c"), Parameter.Create("vec4", "r"));
			this.AddFunction(functionList, "mat2", "transpose", string.Empty, Parameter.Create("mat2", "m"));
			this.AddFunction(functionList, "mat3", "transpose", string.Empty, Parameter.Create("mat3", "m"));
			this.AddFunction(functionList, "mat4", "transpose", string.Empty, Parameter.Create("mat4", "m"));
			this.AddFunction(functionList, "mat2x3", "transpose", string.Empty, Parameter.Create("mat3x2", "m"));
			this.AddFunction(functionList, "mat3x2", "transpose", string.Empty, Parameter.Create("mat2x3", "m"));
			this.AddFunction(functionList, "mat2x4", "transpose", string.Empty, Parameter.Create("mat4x2", "m"));
			this.AddFunction(functionList, "mat4x2", "transpose", string.Empty, Parameter.Create("mat2x4", "m"));
			this.AddFunction(functionList, "mat3x4", "transpose", string.Empty, Parameter.Create("mat4x3", "m"));
			this.AddFunction(functionList, "mat4x3", "transpose", string.Empty, Parameter.Create("mat3x4", "m"));
			this.AddFunction(functionList, "float", "determinant", string.Empty, Parameter.Create("mat2", "m"));
			this.AddFunction(functionList, "float", "determinant", string.Empty, Parameter.Create("mat3", "m"));
			this.AddFunction(functionList, "float", "determinant", string.Empty, Parameter.Create("mat4", "m"));
			this.AddFunction(functionList, "mat2", "inverse", string.Empty, Parameter.Create("mat2", "m"));
			this.AddFunction(functionList, "mat3", "inverse", string.Empty, Parameter.Create("mat3", "m"));
			this.AddFunction(functionList, "mat4", "inverse", string.Empty, Parameter.Create("mat4", "m"));
			this.AddFunction(functionList, "dmat", "matrixCompMult", string.Empty, Parameter.Create("dmat", "x"), Parameter.Create("dmat", "y"));
			this.AddFunction(functionList, "dmat2", "outerProduct", string.Empty, Parameter.Create("dvec2", "c"), Parameter.Create("dvec2", "r"));
			this.AddFunction(functionList, "dmat3", "outerProduct", string.Empty, Parameter.Create("dvec3", "c"), Parameter.Create("dvec3", "r"));
			this.AddFunction(functionList, "dmat4", "outerProduct", string.Empty, Parameter.Create("dvec4", "c"), Parameter.Create("dvec4", "r"));
			this.AddFunction(functionList, "dmat2x3", "outerProduct", string.Empty, Parameter.Create("dvec3", "c"), Parameter.Create("dvec2", "r"));
			this.AddFunction(functionList, "dmat3x2", "outerProduct", string.Empty, Parameter.Create("dvec2", "c"), Parameter.Create("dvec3", "r"));
			this.AddFunction(functionList, "dmat2x4", "outerProduct", string.Empty, Parameter.Create("dvec4", "c"), Parameter.Create("dvec2", "r"));
			this.AddFunction(functionList, "dmat4x2", "outerProduct", string.Empty, Parameter.Create("dvec2", "c"), Parameter.Create("dvec4", "r"));
			this.AddFunction(functionList, "dmat3x4", "outerProduct", string.Empty, Parameter.Create("dvec4", "c"), Parameter.Create("dvec3", "r"));
			this.AddFunction(functionList, "dmat4x3", "outerProduct", string.Empty, Parameter.Create("dvec3", "c"), Parameter.Create("dvec4", "r"));
			this.AddFunction(functionList, "dmat2", "transpose", string.Empty, Parameter.Create("dmat2", "m"));
			this.AddFunction(functionList, "dmat3", "transpose", string.Empty, Parameter.Create("dmat3", "m"));
			this.AddFunction(functionList, "dmat4", "transpose", string.Empty, Parameter.Create("dmat4", "m"));
			this.AddFunction(functionList, "dmat2x3", "transpose", string.Empty, Parameter.Create("dmat3x2", "m"));
			this.AddFunction(functionList, "dmat3x2", "transpose", string.Empty, Parameter.Create("dmat2x3", "m"));
			this.AddFunction(functionList, "dmat2x4", "transpose", string.Empty, Parameter.Create("dmat4x2", "m"));
			this.AddFunction(functionList, "dmat4x2", "transpose", string.Empty, Parameter.Create("dmat2x4", "m"));
			this.AddFunction(functionList, "dmat3x4", "transpose", string.Empty, Parameter.Create("dmat4x3", "m"));
			this.AddFunction(functionList, "dmat4x3", "transpose", string.Empty, Parameter.Create("dmat3x4", "m"));
			this.AddFunction(functionList, "double", "determinant", string.Empty, Parameter.Create("dmat2", "m"));
			this.AddFunction(functionList, "double", "determinant", string.Empty, Parameter.Create("dmat3", "m"));
			this.AddFunction(functionList, "double", "determinant", string.Empty, Parameter.Create("dmat4", "m"));
			this.AddFunction(functionList, "dmat2", "inverse", string.Empty, Parameter.Create("dmat2", "m"));
			this.AddFunction(functionList, "dmat3", "inverse", string.Empty, Parameter.Create("dmat3", "m"));
			this.AddFunction(functionList, "dmat4", "inverse", string.Empty, Parameter.Create("dmat4", "m"));

			// Vector Relational
			this.AddFunction(functionList, "bvec", "lessThan", string.Empty, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "lessThan", string.Empty, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "lessThan", string.Empty, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "lessThan", string.Empty, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "lessThanEqual", string.Empty, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "lessThanEqual", string.Empty, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "lessThanEqual", string.Empty, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "lessThanEqual", string.Empty, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThan", string.Empty, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThan", string.Empty, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThan", string.Empty, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThan", string.Empty, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThanEqual", string.Empty, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThanEqual", string.Empty, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThanEqual", string.Empty, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThanEqual", string.Empty, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "equal", string.Empty, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "equal", string.Empty, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "equal", string.Empty, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "equal", string.Empty, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "equal", string.Empty, Parameter.Create("bvec", "x"), Parameter.Create("bvec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", string.Empty, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", string.Empty, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", string.Empty, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", string.Empty, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", string.Empty, Parameter.Create("bvec", "x"), Parameter.Create("bvec", "y"));
			this.AddFunction(functionList, "bool", "any", string.Empty, Parameter.Create("bvec", "x"));
			this.AddFunction(functionList, "bool", "all", string.Empty, Parameter.Create("bvec", "x"));
			this.AddFunction(functionList, "bvec", "not", string.Empty, Parameter.Create("bvec", "x"));

			// Integer
			this.AddFunction(functionList, "genUType", "uaddCarry", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"), Parameter.Create("out", "genUType", "carry"));
			this.AddFunction(functionList, "genUType", "usubBorrow", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"), Parameter.Create("out", "genUType", "borrow"));
			this.AddFunction(functionList, "void", "umulExtended", string.Empty, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"), Parameter.Create("out", "genUType", "msb"), Parameter.Create("out", "genUType", "lsb"));
			this.AddFunction(functionList, "void", "imulExtended", string.Empty, Parameter.Create("genIType", "x"), Parameter.Create("genIType", "y"), Parameter.Create("out", "genIType", "msb"), Parameter.Create("out", "genIType", "lsb"));
			this.AddFunction(functionList, "genIType", "bitfieldExtract", string.Empty, Parameter.Create("genIType", "value"), Parameter.Create("int", "offset"), Parameter.Create("int", "bits"));
			this.AddFunction(functionList, "genUType", "bitfieldExtract", string.Empty, Parameter.Create("genUType", "value"), Parameter.Create("int", "offset"), Parameter.Create("int", "bits"));
			this.AddFunction(functionList, "genIType", "bitfieldInsert", string.Empty, Parameter.Create("genIType", "base"), Parameter.Create("genIType", "insert"), Parameter.Create("int", "offset"), Parameter.Create("int", "bits"));
			this.AddFunction(functionList, "genUType", "bitfieldInsert", string.Empty, Parameter.Create("genUType", "value"), Parameter.Create("genUType", "insert"), Parameter.Create("int", "offset"), Parameter.Create("int", "bits"));
			this.AddFunction(functionList, "genIType", "bitfieldReverse", string.Empty, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genUType", "bitfieldReverse", string.Empty, Parameter.Create("genUType", "value"));
			this.AddFunction(functionList, "genIType", "bitCount", string.Empty, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genIType", "bitCount", string.Empty, Parameter.Create("genUType", "value"));
			this.AddFunction(functionList, "genIType", "findLSB", string.Empty, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genIType", "findLSB", string.Empty, Parameter.Create("genUType", "value"));
			this.AddFunction(functionList, "genIType", "findMSB", string.Empty, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genIType", "findMSB", string.Empty, Parameter.Create("genUType", "value"));

			// Texture Query
			this.AddFunction(functionList, "int", "textureSize", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "int", "textureSize", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", string.Empty, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", string.Empty, Parameter.Create("samplerCubeArrayShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("gsampler2DRect", "sampler"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "int", "textureSize", string.Empty, Parameter.Create("gsamplerBuffer", "sampler"));
			this.AddFunction(functionList, "ivec2", "textureSize", string.Empty, Parameter.Create("gsampler2DMS", "sampler"));
			this.AddFunction(functionList, "ivec3", "textureSize", string.Empty, Parameter.Create("gsampler2DMSArray", "sampler"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("float", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("float", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("float", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", string.Empty, Parameter.Create("samplerCubeArrayShadow", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("gsampler1D", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("gsampler2D", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("gsampler3D", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("gsamplerCube", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("gsampler1DArray", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("gsampler2DArray", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("gsamplerCubeArray", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("sampler1DShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("sampler2DShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("samplerCubeShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", string.Empty, Parameter.Create("samplerCubeArrayShadow", "sampler"));

			// Texture Lookup
			this.AddFunction(functionList, "gvec4", "texture", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", string.Empty, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", string.Empty, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", string.Empty, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec4", "P"));
			this.AddFunction(functionList, "gvec4", "texture", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "float", "texture", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "float", "texture", string.Empty, Parameter.Create("gsamplerCubeArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "compare"));
			this.AddFunction(functionList, "gvec4", "textureProj", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureProj", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureProj", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "gvec4", "textureProj", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec4", "P"));
			this.AddFunction(functionList, "float", "textureProj", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec4", "P"));
			this.AddFunction(functionList, "gvec4", "textureLod", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", string.Empty, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureLod", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureLod", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureLod", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", string.Empty, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec3", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureOffset", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureOffset", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureOffset", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureOffset", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureOffset", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("int", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("ivec2", "P"));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsamplerBuffer", "sampler"), Parameter.Create("int", "P"));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsampler2DMS", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "gvec4", "texelFetch", string.Empty, Parameter.Create("gsampler2DMSArray", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("int", "P"), Parameter.Create("int", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "lod"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec3", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureProjOffset", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureProjOffset", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureProjOffset", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureLodOffset", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureLodOffset", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "float", "textureLodOffset", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureProjLod", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureProjLod", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "float", "textureProjLodOffset", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureProjLodOffset", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGrad", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", string.Empty, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", string.Empty, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", string.Empty, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", string.Empty, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", string.Empty, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureProjGrad", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureProjGrad", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "float", "textureProjGrad", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", string.Empty, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureProjGradOffset", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", string.Empty, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "float", "textureProjGradOffset", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureProjGradOffset", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));

			// Texture Gather
			this.AddFunction(functionList, "gvec4", "textureGather", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGather", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGather", string.Empty, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGather", string.Empty, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGather", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "vec4", "textureGather", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "vec4", "textureGather", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "vec4", "textureGather", string.Empty, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "vec4", "textureGather", string.Empty, Parameter.Create("samplerCubeArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "vec4", "textureGather", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "gvec4", "textureGatherOffset", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGatherOffset", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGatherOffset", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "vec4", "textureGatherOffset", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "vec4", "textureGatherOffset", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "vec4", "textureGatherOffset", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGatherOffsets", string.Empty, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offsets", 4), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGatherOffsets", string.Empty, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offsets", 4), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGatherOffsets", string.Empty, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offsets", 4), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "vec4", "textureGatherOffsets", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offsets", 4));
			this.AddFunction(functionList, "vec4", "textureGatherOffsets", string.Empty, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offsets", 4));
			this.AddFunction(functionList, "vec4", "textureGatherOffsets", string.Empty, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offset", 4));

			// Compatibility Profile Texture
			this.AddFunction(functionList, "vec4", "texture1D", string.Empty, Parameter.Create("sampler1D", "sampler"), Parameter.Create("float", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture1DProj", string.Empty, Parameter.Create("sampler1D", "sampler"), Parameter.Create("vec2", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture1DProj", string.Empty, Parameter.Create("sampler1D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture1DLod", string.Empty, Parameter.Create("sampler1D", "sampler"), Parameter.Create("float", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture1DProjLod", string.Empty, Parameter.Create("sampler1D", "sampler"), Parameter.Create("vec2", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture1DProjLod", string.Empty, Parameter.Create("sampler1D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture2D", string.Empty, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec2", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture2DProj", string.Empty, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture2DProj", string.Empty, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture2DLod", string.Empty, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec2", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture2DProjLod", string.Empty, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture2DProjLod", string.Empty, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture3D", string.Empty, Parameter.Create("sampler3D", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture3DProj", string.Empty, Parameter.Create("sampler3D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture3DLod", string.Empty, Parameter.Create("sampler3D", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture3DProjLod", string.Empty, Parameter.Create("sampler3D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "textureCube", string.Empty, Parameter.Create("samplerCube", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "textureCubeLod", string.Empty, Parameter.Create("samplerCube", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "shadow1D", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "shadow2D", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "shadow1DProj", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "shadow2DProj", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "shadow1DLod", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "shadow2DLod", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "shadow1DProjLod", string.Empty, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "shadow2DProjLod", string.Empty, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));

			// Atomic-Counter
			this.AddFunction(functionList, "uint", "atomicCounterIncrement", string.Empty, Parameter.Create("atomic_uint", "c"));
			this.AddFunction(functionList, "uint", "atomicCounterDecrement", string.Empty, Parameter.Create("atomic_uint", "c"));
			this.AddFunction(functionList, "uint", "atomicCounter", string.Empty, Parameter.Create("atomic_uint", "c"));

			// Atomic Memory
			this.AddFunction(functionList, "uint", "atomicAdd", string.Empty, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicAdd", string.Empty, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicMin", string.Empty, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicMin", string.Empty, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicMax", string.Empty, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicMax", string.Empty, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicAnd", string.Empty, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicAnd", string.Empty, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicOr", string.Empty, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicOr", string.Empty, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicXor", string.Empty, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicXor", string.Empty, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicExchange", string.Empty, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicExchange", string.Empty, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicCompSwap", string.Empty, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicCompSwap", string.Empty, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));

			// Image
			this.AddFunction(functionList, "int", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimage1D", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimage2D", "image"));
			this.AddFunction(functionList, "ivec3", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimage3D", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimageCube", "image"));
			this.AddFunction(functionList, "ivec3", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimageCubeArray", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimageRect", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimage1DArray", "image"));
			this.AddFunction(functionList, "ivec3", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimage2DArray", "image"));
			this.AddFunction(functionList, "int", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimageBuffer", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimage2DMS", "image"));
			this.AddFunction(functionList, "ivec3", "imageSize", string.Empty, Parameter.Create("readonly writeonly", "gimage2DMSArray", "image"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimage1D", "image"), Parameter.Create("int", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimage2D", "image"), Parameter.Create("ivec2", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimage3D", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimage2DRect", "image"), Parameter.Create("ivec2", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimageCube", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimageBuffer", "image"), Parameter.Create("int", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimage1DArray", "image"), Parameter.Create("ivec2", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimage2DArray", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimageCubeArray", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "gvec4", "imageLoad", string.Empty, Parameter.Create("readonly", "gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimage3D", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", string.Empty, Parameter.Create("writeonly", "gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", string.Empty, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));

			// Fragment Processing
			// Derivative
			this.AddFunction(functionList, "genType", "dFdx", string.Empty, Parameter.Create("genType", "p"));
			this.AddFunction(functionList, "genType", "dFdy", string.Empty, Parameter.Create("genType", "p"));
			this.AddFunction(functionList, "genType", "fwidth", string.Empty, Parameter.Create("genType", "p"));

			// Interpolation
			this.AddFunction(functionList, "float", "interpolateAtCentroid", string.Empty, Parameter.Create("float", "interpolant"));
			this.AddFunction(functionList, "vec2", "interpolateAtCentroid", string.Empty, Parameter.Create("vec2", "interpolant"));
			this.AddFunction(functionList, "vec3", "interpolateAtCentroid", string.Empty, Parameter.Create("vec3", "interpolant"));
			this.AddFunction(functionList, "vec4", "interpolateAtCentroid", string.Empty, Parameter.Create("vec4", "interpolant"));

			this.AddFunction(functionList, "float", "interpolateAtSample", string.Empty, Parameter.Create("float", "interpolant"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "vec2", "interpolateAtSample", string.Empty, Parameter.Create("vec2", "interpolant"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "vec3", "interpolateAtSample", string.Empty, Parameter.Create("vec3", "interpolant"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "vec4", "interpolateAtSample", string.Empty, Parameter.Create("vec4", "interpolant"), Parameter.Create("int", "sample"));

			this.AddFunction(functionList, "float", "interpolateAtOffset", string.Empty, Parameter.Create("float", "interpolant"), Parameter.Create("vec2", "offset"));
			this.AddFunction(functionList, "vec2", "interpolateAtOffset", string.Empty, Parameter.Create("vec2", "interpolant"), Parameter.Create("vec2", "offset"));
			this.AddFunction(functionList, "vec3", "interpolateAtOffset", string.Empty, Parameter.Create("vec3", "interpolant"), Parameter.Create("vec2", "offset"));
			this.AddFunction(functionList, "vec4", "interpolateAtOffset", string.Empty, Parameter.Create("vec4", "interpolant"), Parameter.Create("vec2", "offset"));

			// Noise
			this.AddFunction(functionList, "float", "noise1", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "vec2", "noise2", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "vec3", "noise3", string.Empty, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "vec4", "noise4", string.Empty, Parameter.Create("genType", "x"));

			// Geometry Shader
			this.AddFunction(functionList, "void", "EmitStreamVertex", string.Empty, Parameter.Create("int", "stream"));
			this.AddFunction(functionList, "void", "EmitStreamPrimitive", string.Empty, Parameter.Create("int", "stream"));
			this.AddFunction(functionList, "void", "EmitVertex", string.Empty);
			this.AddFunction(functionList, "void", "EmitPrimitive", string.Empty);

			// Shader Invocation Control
			this.AddFunction(functionList, "void", "barrier", string.Empty);

			// Shader Memory Control
			this.AddFunction(functionList, "void", "memoryBarrier", string.Empty);
			this.AddFunction(functionList, "void", "memoryBarrierAtomicCounter", string.Empty);
			this.AddFunction(functionList, "void", "memoryBarrierBuffer", string.Empty);
			this.AddFunction(functionList, "void", "memoryBarrierShared", string.Empty);
			this.AddFunction(functionList, "void", "memoryBarrierImage", string.Empty);
			this.AddFunction(functionList, "void", "groupMemoryBarrier", string.Empty);

			this.definitions.AddRange(functionList);
		}

		private void LoadVariables()
		{
		}

		private void WriteToXml()
		{
			using (IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(File.Create("builtIn2.xml")), "\t"))
			{
				writer.WriteLine("<Data>");
				writer.IndentLevel++;

				foreach (BuiltInDefinition definition in this.definitions)
				{
					definition.WriteToXml(writer);
				}

				writer.IndentLevel--;
				writer.WriteLine("</Data>");
			}
		}

		private void AddFunction(List<BuiltInFunction> list, string returnType, string name, string documentation, params Parameter[][] parameters)
		{
			int overloads = 0;
			string[] returnTypes;

			if (GenTypes.ContainsKey(returnType))
			{
				overloads = GenTypes[returnType].Length;
				returnTypes = GenTypes[returnType];
			}
			else
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					if (overloads < parameters[i].Length)
					{
						overloads = parameters[i].Length;
					}
				}

				returnTypes = new string[] { returnType };
			}

			if (overloads == 0)
			{
				Parameter[] paramArray = new Parameter[parameters.Length];

				for (int i = 0; i < parameters.Length; i++)
				{
					paramArray[i] = parameters[i][0];
				}

				list.Add(new BuiltInFunction(returnType, name, documentation, paramArray));
			}
			else
			{
				for (int i = 0; i < overloads; i++)
				{
					Parameter[] paramArray = new Parameter[parameters.Length];

					for (int j = 0; j < parameters.Length; j++)
					{
						paramArray[j] = parameters[j][i % parameters[j].Length];
					}

					list.Add(new BuiltInFunction(returnTypes[i % returnTypes.Length], name, documentation, paramArray));
				}
			}
		}
	}
}