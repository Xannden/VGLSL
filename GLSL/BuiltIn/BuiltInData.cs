using System;
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
		private readonly List<BuiltInFunction> functions = new List<BuiltInFunction>();
		private readonly Dictionary<string, string[]> genTypes;

		private BuiltInData()
		{
			this.genTypes = new Dictionary<string, string[]>
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

		public IReadOnlyList<Definition> Definitions => this.functions;

		internal void LoadData()
		{
			this.LoadFunctions();
		}

		private void LoadFunctions()
		{
			List<BuiltInFunction> functionsList = new List<BuiltInFunction>();

			// Angle and Trigonometry
			this.AddFunction(functionsList, "genType", "radians", Resources.RadiansDoc, Tuple.Create("genType", "degrees"));
			this.AddFunction(functionsList, "genType", "degrees", Resources.RadiansDoc, Tuple.Create("genType", "radians"));
			this.AddFunction(functionsList, "genType", "sin", Resources.SinDoc, Tuple.Create("genType", "angle"));
			this.AddFunction(functionsList, "genType", "cos", string.Empty, Tuple.Create("genType", "angle"));
			this.AddFunction(functionsList, "genType", "tan", string.Empty, Tuple.Create("genType", "angle"));
			this.AddFunction(functionsList, "genType", "asin", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "acos", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "atan", string.Empty, Tuple.Create("genType", "y"), Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "atan", string.Empty, Tuple.Create("genType", "y_over_x"));
			this.AddFunction(functionsList, "genType", "sinh", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "cosh", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "tanh", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "asinh", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "acosh", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "atanh", string.Empty, Tuple.Create("genType", "x"));

			// Exponential
			this.AddFunction(functionsList, "genType", "pow", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "y"));
			this.AddFunction(functionsList, "genType", "exp", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "log", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "exp2", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "log2", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "sqrt", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "sqrt", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "inversesqrt", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "inversesqrt", string.Empty, Tuple.Create("genDType", "x"));

			// Common
			this.AddFunction(functionsList, "genType", "abs", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genIType", "abs", string.Empty, Tuple.Create("genIType", "x"));
			this.AddFunction(functionsList, "genDType", "abs", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "sign", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genIType", "sign", string.Empty, Tuple.Create("genIType", "x"));
			this.AddFunction(functionsList, "genDType", "sign", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "floor", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "floor", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "trunc", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "trunc", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "round", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "round", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "roundEven", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "roundEven", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "ceil", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "ceil", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "fract", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "fract", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "mod", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("float", "y"));
			this.AddFunction(functionsList, "genType", "mod", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "y"));
			this.AddFunction(functionsList, "genDType", "mod", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("double", "y"));
			this.AddFunction(functionsList, "genDType", "mod", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("genDType", "y"));
			this.AddFunction(functionsList, "genType", "modf", string.Empty, Tuple.Create(string.Empty, "genType", "x"), Tuple.Create("out", "genType", "i"));
			this.AddFunction(functionsList, "genDType", "modf", string.Empty, Tuple.Create(string.Empty, "genDType", "x"), Tuple.Create("out", "genDType", "i"));
			this.AddFunction(functionsList, "genType", "min", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "y"));
			this.AddFunction(functionsList, "genType", "min", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("float", "y"));
			this.AddFunction(functionsList, "genDType", "min", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("genDType", "y"));
			this.AddFunction(functionsList, "genDType", "min", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("double", "y"));
			this.AddFunction(functionsList, "genIType", "min", string.Empty, Tuple.Create("genIType", "x"), Tuple.Create("genIType", "y"));
			this.AddFunction(functionsList, "genIType", "min", string.Empty, Tuple.Create("genIType", "x"), Tuple.Create("int", "y"));
			this.AddFunction(functionsList, "genUType", "min", string.Empty, Tuple.Create("genUType", "x"), Tuple.Create("genUType", "y"));
			this.AddFunction(functionsList, "genUType", "min", string.Empty, Tuple.Create("genUType", "x"), Tuple.Create("uint", "y"));
			this.AddFunction(functionsList, "genType", "max", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "y"));
			this.AddFunction(functionsList, "genType", "max", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("float", "y"));
			this.AddFunction(functionsList, "genDType", "max", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("genDType", "y"));
			this.AddFunction(functionsList, "genDType", "max", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("double", "y"));
			this.AddFunction(functionsList, "genIType", "max", string.Empty, Tuple.Create("genIType", "x"), Tuple.Create("genIType", "y"));
			this.AddFunction(functionsList, "genIType", "max", string.Empty, Tuple.Create("genIType", "x"), Tuple.Create("int", "y"));
			this.AddFunction(functionsList, "genUType", "max", string.Empty, Tuple.Create("genUType", "x"), Tuple.Create("genUType", "y"));
			this.AddFunction(functionsList, "genUType", "max", string.Empty, Tuple.Create("genUType", "x"), Tuple.Create("uint", "y"));
			this.AddFunction(functionsList, "genType", "clamp", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "minVal"), Tuple.Create("genType", "maxVal"));
			this.AddFunction(functionsList, "genType", "clamp", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("float", "minVal"), Tuple.Create("float", "maxVal"));
			this.AddFunction(functionsList, "genDType", "clamp", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("genDType", "minVal"), Tuple.Create("genDType", "maxVal"));
			this.AddFunction(functionsList, "genDType", "clamp", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("double", "minVal"), Tuple.Create("double", "maxVal"));
			this.AddFunction(functionsList, "genIType", "clamp", string.Empty, Tuple.Create("genIType", "x"), Tuple.Create("genIType", "minVal"), Tuple.Create("genIType", "maxVal"));
			this.AddFunction(functionsList, "genIType", "clamp", string.Empty, Tuple.Create("genIType", "x"), Tuple.Create("int", "minVal"), Tuple.Create("float", "int"));
			this.AddFunction(functionsList, "genUType", "clamp", string.Empty, Tuple.Create("genUType", "x"), Tuple.Create("genUType", "minVal"), Tuple.Create("genUType", "maxVal"));
			this.AddFunction(functionsList, "genUType", "clamp", string.Empty, Tuple.Create("genUType", "x"), Tuple.Create("uint", "minVal"), Tuple.Create("uint", "maxVal"));
			this.AddFunction(functionsList, "genType", "mix", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "y"), Tuple.Create("genType", "a"));
			this.AddFunction(functionsList, "genType", "mix", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "y"), Tuple.Create("float", "a"));
			this.AddFunction(functionsList, "genDType", "mix", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("genDType", "y"), Tuple.Create("genDType", "a"));
			this.AddFunction(functionsList, "genDType", "mix", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("genDType", "y"), Tuple.Create("double", "a"));
			this.AddFunction(functionsList, "genType", "mix", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "y"), Tuple.Create("genBType", "a"));
			this.AddFunction(functionsList, "genDType", "mix", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("genDType", "y"), Tuple.Create("genBType", "a"));
			this.AddFunction(functionsList, "genType", "step", string.Empty, Tuple.Create("genType", "edge"), Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "step", string.Empty, Tuple.Create("float", "edge"), Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "step", string.Empty, Tuple.Create("genDType", "edge"), Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genDType", "step", string.Empty, Tuple.Create("double", "edge"), Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genType", "smoothstep", string.Empty, Tuple.Create("genType", "edge0"), Tuple.Create("genType", "edge1"), Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genType", "smoothstep", string.Empty, Tuple.Create("float", "edge0"), Tuple.Create("float", "edge1"), Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "smoothstep", string.Empty, Tuple.Create("genDType", "edge0"), Tuple.Create("genDType", "edge1"), Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genDType", "smoothstep", string.Empty, Tuple.Create("double", "edge0"), Tuple.Create("double", "edge1"), Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genBType", "isnan", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genBType", "isnan", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genBType", "isinf", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genBType", "isinf", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "genIType", "floatBitsToInt", string.Empty, Tuple.Create("genType", "value"));
			this.AddFunction(functionsList, "genUType", "floatBitsToUnt", string.Empty, Tuple.Create("genType", "value"));
			this.AddFunction(functionsList, "genType", "intBitsToFloat", string.Empty, Tuple.Create("genIType", "value"));
			this.AddFunction(functionsList, "genType", "uintBitsToFloat", string.Empty, Tuple.Create("genUType", "value"));
			this.AddFunction(functionsList, "genType", "fma", string.Empty, Tuple.Create("genType", "a"), Tuple.Create("genType", "b"), Tuple.Create("genType", "c"));
			this.AddFunction(functionsList, "genDType", "fma", string.Empty, Tuple.Create("genDType", "a"), Tuple.Create("genDType", "b"), Tuple.Create("genDType", "c"));
			this.AddFunction(functionsList, "genType", "frexp", string.Empty, Tuple.Create(string.Empty, "genType", "x"), Tuple.Create("out", "genIType", "exp"));
			this.AddFunction(functionsList, "genDType", "frexp", string.Empty, Tuple.Create(string.Empty, "genDType", "x"), Tuple.Create("out", "genIType", "exp"));
			this.AddFunction(functionsList, "genType", "ldexp", string.Empty, Tuple.Create(string.Empty, "genType", "x"), Tuple.Create("in", "genIType", "exp"));
			this.AddFunction(functionsList, "genDType", "ldexp", string.Empty, Tuple.Create(string.Empty, "genDType", "x"), Tuple.Create("in", "genIType", "exp"));

			// Floating-Point Pack and Unpack
			this.AddFunction(functionsList, "uint", "packUnorm2x16", string.Empty, Tuple.Create("vec2", "v"));
			this.AddFunction(functionsList, "uint", "packSnorm2x16", string.Empty, Tuple.Create("vec2", "v"));
			this.AddFunction(functionsList, "uint", "packUnorm4x8", string.Empty, Tuple.Create("vec4", "v"));
			this.AddFunction(functionsList, "uint", "packSnorm4x8", string.Empty, Tuple.Create("vec4", "v"));
			this.AddFunction(functionsList, "vec2", "unpackUnorm2x16", string.Empty, Tuple.Create("uint", "p"));
			this.AddFunction(functionsList, "vec2", "unpackSnorm2x16", string.Empty, Tuple.Create("uint", "p"));
			this.AddFunction(functionsList, "vec4", "unpackUnorm4x8", string.Empty, Tuple.Create("uint", "p"));
			this.AddFunction(functionsList, "vec4", "unpackSnorm4x8", string.Empty, Tuple.Create("uint", "p"));
			this.AddFunction(functionsList, "double", "packDouble2x32", string.Empty, Tuple.Create("uvec2", "v"));
			this.AddFunction(functionsList, "uvec2", "unpackDouble2x32", string.Empty, Tuple.Create("double", "v"));
			this.AddFunction(functionsList, "uint", "packHalf2x16", string.Empty, Tuple.Create("vec2", "v"));
			this.AddFunction(functionsList, "vec2", "unpackHalf2x16", string.Empty, Tuple.Create("uint", "v"));

			// Geometric
			this.AddFunction(functionsList, "float", "length", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "double", "length", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "float", "distance", string.Empty, Tuple.Create("genType", "p0"), Tuple.Create("genType", "p1"));
			this.AddFunction(functionsList, "double", "distance", string.Empty, Tuple.Create("genDType", "p0"), Tuple.Create("genDType", "p1"));
			this.AddFunction(functionsList, "float", "dot", string.Empty, Tuple.Create("genType", "x"), Tuple.Create("genType", "y"));
			this.AddFunction(functionsList, "double", "dot", string.Empty, Tuple.Create("genDType", "x"), Tuple.Create("genDType", "y"));
			this.AddFunction(functionsList, "vec3", "cross", string.Empty, Tuple.Create("vec3", "x"), Tuple.Create("vec3", "y"));
			this.AddFunction(functionsList, "dvec3", "cross", string.Empty, Tuple.Create("dvec3", "x"), Tuple.Create("dvec3", "y"));
			this.AddFunction(functionsList, "genType", "normalize", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "genDType", "normalize", string.Empty, Tuple.Create("genDType", "x"));
			this.AddFunction(functionsList, "vec4", "ftransform", string.Empty);
			this.AddFunction(functionsList, "genType", "faceforward", string.Empty, Tuple.Create("genType", "N"), Tuple.Create("genType", "I"), Tuple.Create("genType", "Nref"));
			this.AddFunction(functionsList, "genDType", "faceforward", string.Empty, Tuple.Create("genDType", "N"), Tuple.Create("genDType", "I"), Tuple.Create("genDType", "Nref"));
			this.AddFunction(functionsList, "genType", "reflect", string.Empty, Tuple.Create("genType", "I"), Tuple.Create("genType", "N"));
			this.AddFunction(functionsList, "genDType", "reflect", string.Empty, Tuple.Create("genDType", "I"), Tuple.Create("genDType", "N"));
			this.AddFunction(functionsList, "genType", "refract", string.Empty, Tuple.Create("genType", "I"), Tuple.Create("genType", "N"), Tuple.Create("float", "eta"));
			this.AddFunction(functionsList, "genDType", "refract", string.Empty, Tuple.Create("genDType", "I"), Tuple.Create("genDType", "N"), Tuple.Create("float", "eta"));

			// Matrix
			this.AddFunction(functionsList, "mat", "matrixCompMult", string.Empty, Tuple.Create("mat", "x"), Tuple.Create("mat", "y"));
			this.AddFunction(functionsList, "mat2", "outerProduct", string.Empty, Tuple.Create("vec2", "c"), Tuple.Create("vec2", "r"));
			this.AddFunction(functionsList, "mat3", "outerProduct", string.Empty, Tuple.Create("vec3", "c"), Tuple.Create("vec3", "r"));
			this.AddFunction(functionsList, "mat4", "outerProduct", string.Empty, Tuple.Create("vec4", "c"), Tuple.Create("vec4", "r"));
			this.AddFunction(functionsList, "mat2x3", "outerProduct", string.Empty, Tuple.Create("vec3", "c"), Tuple.Create("vec2", "r"));
			this.AddFunction(functionsList, "mat3x2", "outerProduct", string.Empty, Tuple.Create("vec2", "c"), Tuple.Create("vec3", "r"));
			this.AddFunction(functionsList, "mat2x4", "outerProduct", string.Empty, Tuple.Create("vec4", "c"), Tuple.Create("vec2", "r"));
			this.AddFunction(functionsList, "mat4x2", "outerProduct", string.Empty, Tuple.Create("vec2", "c"), Tuple.Create("vec4", "r"));
			this.AddFunction(functionsList, "mat3x4", "outerProduct", string.Empty, Tuple.Create("vec4", "c"), Tuple.Create("vec3", "r"));
			this.AddFunction(functionsList, "mat4x3", "outerProduct", string.Empty, Tuple.Create("vec3", "c"), Tuple.Create("vec4", "r"));
			this.AddFunction(functionsList, "mat2", "transpose", string.Empty, Tuple.Create("mat2", "m"));
			this.AddFunction(functionsList, "mat3", "transpose", string.Empty, Tuple.Create("mat3", "m"));
			this.AddFunction(functionsList, "mat4", "transpose", string.Empty, Tuple.Create("mat4", "m"));
			this.AddFunction(functionsList, "mat2x3", "transpose", string.Empty, Tuple.Create("mat3x2", "m"));
			this.AddFunction(functionsList, "mat3x2", "transpose", string.Empty, Tuple.Create("mat2x3", "m"));
			this.AddFunction(functionsList, "mat2x4", "transpose", string.Empty, Tuple.Create("mat4x2", "m"));
			this.AddFunction(functionsList, "mat4x2", "transpose", string.Empty, Tuple.Create("mat2x4", "m"));
			this.AddFunction(functionsList, "mat3x4", "transpose", string.Empty, Tuple.Create("mat4x3", "m"));
			this.AddFunction(functionsList, "mat4x3", "transpose", string.Empty, Tuple.Create("mat3x4", "m"));
			this.AddFunction(functionsList, "float", "determinant", string.Empty, Tuple.Create("mat2", "m"));
			this.AddFunction(functionsList, "float", "determinant", string.Empty, Tuple.Create("mat3", "m"));
			this.AddFunction(functionsList, "float", "determinant", string.Empty, Tuple.Create("mat4", "m"));
			this.AddFunction(functionsList, "mat2", "inverse", string.Empty, Tuple.Create("mat2", "m"));
			this.AddFunction(functionsList, "mat3", "inverse", string.Empty, Tuple.Create("mat3", "m"));
			this.AddFunction(functionsList, "mat4", "inverse", string.Empty, Tuple.Create("mat4", "m"));
			this.AddFunction(functionsList, "dmat", "matrixCompMult", string.Empty, Tuple.Create("dmat", "x"), Tuple.Create("dmat", "y"));
			this.AddFunction(functionsList, "dmat2", "outerProduct", string.Empty, Tuple.Create("dvec2", "c"), Tuple.Create("dvec2", "r"));
			this.AddFunction(functionsList, "dmat3", "outerProduct", string.Empty, Tuple.Create("dvec3", "c"), Tuple.Create("dvec3", "r"));
			this.AddFunction(functionsList, "dmat4", "outerProduct", string.Empty, Tuple.Create("dvec4", "c"), Tuple.Create("dvec4", "r"));
			this.AddFunction(functionsList, "dmat2x3", "outerProduct", string.Empty, Tuple.Create("dvec3", "c"), Tuple.Create("dvec2", "r"));
			this.AddFunction(functionsList, "dmat3x2", "outerProduct", string.Empty, Tuple.Create("dvec2", "c"), Tuple.Create("dvec3", "r"));
			this.AddFunction(functionsList, "dmat2x4", "outerProduct", string.Empty, Tuple.Create("dvec4", "c"), Tuple.Create("dvec2", "r"));
			this.AddFunction(functionsList, "dmat4x2", "outerProduct", string.Empty, Tuple.Create("dvec2", "c"), Tuple.Create("dvec4", "r"));
			this.AddFunction(functionsList, "dmat3x4", "outerProduct", string.Empty, Tuple.Create("dvec4", "c"), Tuple.Create("dvec3", "r"));
			this.AddFunction(functionsList, "dmat4x3", "outerProduct", string.Empty, Tuple.Create("dvec3", "c"), Tuple.Create("dvec4", "r"));
			this.AddFunction(functionsList, "dmat2", "transpose", string.Empty, Tuple.Create("dmat2", "m"));
			this.AddFunction(functionsList, "dmat3", "transpose", string.Empty, Tuple.Create("dmat3", "m"));
			this.AddFunction(functionsList, "dmat4", "transpose", string.Empty, Tuple.Create("dmat4", "m"));
			this.AddFunction(functionsList, "dmat2x3", "transpose", string.Empty, Tuple.Create("dmat3x2", "m"));
			this.AddFunction(functionsList, "dmat3x2", "transpose", string.Empty, Tuple.Create("dmat2x3", "m"));
			this.AddFunction(functionsList, "dmat2x4", "transpose", string.Empty, Tuple.Create("dmat4x2", "m"));
			this.AddFunction(functionsList, "dmat4x2", "transpose", string.Empty, Tuple.Create("dmat2x4", "m"));
			this.AddFunction(functionsList, "dmat3x4", "transpose", string.Empty, Tuple.Create("dmat4x3", "m"));
			this.AddFunction(functionsList, "dmat4x3", "transpose", string.Empty, Tuple.Create("dmat3x4", "m"));
			this.AddFunction(functionsList, "double", "determinant", string.Empty, Tuple.Create("dmat2", "m"));
			this.AddFunction(functionsList, "double", "determinant", string.Empty, Tuple.Create("dmat3", "m"));
			this.AddFunction(functionsList, "double", "determinant", string.Empty, Tuple.Create("dmat4", "m"));
			this.AddFunction(functionsList, "dmat2", "inverse", string.Empty, Tuple.Create("dmat2", "m"));
			this.AddFunction(functionsList, "dmat3", "inverse", string.Empty, Tuple.Create("dmat3", "m"));
			this.AddFunction(functionsList, "dmat4", "inverse", string.Empty, Tuple.Create("dmat4", "m"));

			// Vector Relational
			this.AddFunction(functionsList, "bvec", "lessThan", string.Empty, Tuple.Create("vec", "x"), Tuple.Create("vec", "y"));
			this.AddFunction(functionsList, "bvec", "lessThan", string.Empty, Tuple.Create("dvec", "x"), Tuple.Create("dvec", "y"));
			this.AddFunction(functionsList, "bvec", "lessThan", string.Empty, Tuple.Create("ivec", "x"), Tuple.Create("ivec", "y"));
			this.AddFunction(functionsList, "bvec", "lessThan", string.Empty, Tuple.Create("uvec", "x"), Tuple.Create("uvec", "y"));
			this.AddFunction(functionsList, "bvec", "lessThanEqual", string.Empty, Tuple.Create("vec", "x"), Tuple.Create("vec", "y"));
			this.AddFunction(functionsList, "bvec", "lessThanEqual", string.Empty, Tuple.Create("dvec", "x"), Tuple.Create("dvec", "y"));
			this.AddFunction(functionsList, "bvec", "lessThanEqual", string.Empty, Tuple.Create("ivec", "x"), Tuple.Create("ivec", "y"));
			this.AddFunction(functionsList, "bvec", "lessThanEqual", string.Empty, Tuple.Create("uvec", "x"), Tuple.Create("uvec", "y"));
			this.AddFunction(functionsList, "bvec", "greaterThan", string.Empty, Tuple.Create("vec", "x"), Tuple.Create("vec", "y"));
			this.AddFunction(functionsList, "bvec", "greaterThan", string.Empty, Tuple.Create("dvec", "x"), Tuple.Create("dvec", "y"));
			this.AddFunction(functionsList, "bvec", "greaterThan", string.Empty, Tuple.Create("ivec", "x"), Tuple.Create("ivec", "y"));
			this.AddFunction(functionsList, "bvec", "greaterThan", string.Empty, Tuple.Create("uvec", "x"), Tuple.Create("uvec", "y"));
			this.AddFunction(functionsList, "bvec", "greaterThanEqual", string.Empty, Tuple.Create("vec", "x"), Tuple.Create("vec", "y"));
			this.AddFunction(functionsList, "bvec", "greaterThanEqual", string.Empty, Tuple.Create("dvec", "x"), Tuple.Create("dvec", "y"));
			this.AddFunction(functionsList, "bvec", "greaterThanEqual", string.Empty, Tuple.Create("ivec", "x"), Tuple.Create("ivec", "y"));
			this.AddFunction(functionsList, "bvec", "greaterThanEqual", string.Empty, Tuple.Create("uvec", "x"), Tuple.Create("uvec", "y"));
			this.AddFunction(functionsList, "bvec", "equal", string.Empty, Tuple.Create("vec", "x"), Tuple.Create("vec", "y"));
			this.AddFunction(functionsList, "bvec", "equal", string.Empty, Tuple.Create("dvec", "x"), Tuple.Create("dvec", "y"));
			this.AddFunction(functionsList, "bvec", "equal", string.Empty, Tuple.Create("ivec", "x"), Tuple.Create("ivec", "y"));
			this.AddFunction(functionsList, "bvec", "equal", string.Empty, Tuple.Create("uvec", "x"), Tuple.Create("uvec", "y"));
			this.AddFunction(functionsList, "bvec", "equal", string.Empty, Tuple.Create("bvec", "x"), Tuple.Create("bvec", "y"));
			this.AddFunction(functionsList, "bvec", "notEqual", string.Empty, Tuple.Create("vec", "x"), Tuple.Create("vec", "y"));
			this.AddFunction(functionsList, "bvec", "notEqual", string.Empty, Tuple.Create("dvec", "x"), Tuple.Create("dvec", "y"));
			this.AddFunction(functionsList, "bvec", "notEqual", string.Empty, Tuple.Create("ivec", "x"), Tuple.Create("ivec", "y"));
			this.AddFunction(functionsList, "bvec", "notEqual", string.Empty, Tuple.Create("uvec", "x"), Tuple.Create("uvec", "y"));
			this.AddFunction(functionsList, "bvec", "notEqual", string.Empty, Tuple.Create("bvec", "x"), Tuple.Create("bvec", "y"));
			this.AddFunction(functionsList, "bool", "any", string.Empty, Tuple.Create("bvec", "x"));
			this.AddFunction(functionsList, "bool", "all", string.Empty, Tuple.Create("bvec", "x"));
			this.AddFunction(functionsList, "bvec", "not", string.Empty, Tuple.Create("bvec", "x"));

			// Integer
			this.AddFunction(functionsList, "genUType", "uaddCarry", string.Empty, Tuple.Create(string.Empty, "genUType", "x"), Tuple.Create(string.Empty, "genUType", "y"), Tuple.Create("out", "genUType", "carry"));
			this.AddFunction(functionsList, "genUType", "usubBorrow", string.Empty, Tuple.Create(string.Empty, "genUType", "x"), Tuple.Create(string.Empty, "genUType", "y"), Tuple.Create("out", "genUType", "borrow"));
			this.AddFunction(functionsList, "void", "umulExtended", string.Empty, Tuple.Create(string.Empty, "genUType", "x"), Tuple.Create(string.Empty, "genUType", "y"), Tuple.Create("out", "genUType", "msb"), Tuple.Create("out", "genUType", "lsb"));
			this.AddFunction(functionsList, "void", "imulExtended", string.Empty, Tuple.Create(string.Empty, "genIType", "x"), Tuple.Create(string.Empty, "genIType", "y"), Tuple.Create("out", "genIType", "msb"), Tuple.Create("out", "genIType", "lsb"));
			this.AddFunction(functionsList, "genIType", "bitfieldExtract", string.Empty, Tuple.Create("genIType", "value"), Tuple.Create("int", "offset"), Tuple.Create("int", "bits"));
			this.AddFunction(functionsList, "genUType", "bitfieldExtract", string.Empty, Tuple.Create("genUType", "value"), Tuple.Create("int", "offset"), Tuple.Create("int", "bits"));
			this.AddFunction(functionsList, "genIType", "bitfieldInsert", string.Empty, Tuple.Create("genIType", "base"), Tuple.Create("genIType", "insert"), Tuple.Create("int", "offset"), Tuple.Create("int", "bits"));
			this.AddFunction(functionsList, "genUType", "bitfieldInsert", string.Empty, Tuple.Create("genUType", "value"), Tuple.Create("genUType", "insert"), Tuple.Create("int", "offset"), Tuple.Create("int", "bits"));
			this.AddFunction(functionsList, "genIType", "bitfieldReverse", string.Empty, Tuple.Create("genIType", "value"));
			this.AddFunction(functionsList, "genUType", "bitfieldReverse", string.Empty, Tuple.Create("genUType", "value"));
			this.AddFunction(functionsList, "genIType", "bitCount", string.Empty, Tuple.Create("genIType", "value"));
			this.AddFunction(functionsList, "genIType", "bitCount", string.Empty, Tuple.Create("genUType", "value"));
			this.AddFunction(functionsList, "genIType", "findLSB", string.Empty, Tuple.Create("genIType", "value"));
			this.AddFunction(functionsList, "genIType", "findLSB", string.Empty, Tuple.Create("genUType", "value"));
			this.AddFunction(functionsList, "genIType", "findMSB", string.Empty, Tuple.Create("genIType", "value"));
			this.AddFunction(functionsList, "genIType", "findMSB", string.Empty, Tuple.Create("genUType", "value"));

			// Texture Query
			this.AddFunction(functionsList, "int", "textureSize", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec3", "textureSize", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("gsamplerCube", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "int", "textureSize", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("samplerCubeShadow", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec3", "textureSize", string.Empty, Tuple.Create("gsamplerCubeArray", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec3", "textureSize", string.Empty, Tuple.Create("samplerCubeArrayShadow", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("gsampler2DRect", "sampler"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("gsampler1DArray", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec3", "textureSize", string.Empty, Tuple.Create("gsampler2DArray", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "ivec3", "textureSize", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "int", "textureSize", string.Empty, Tuple.Create("gsamplerBuffer", "sampler"));
			this.AddFunction(functionsList, "ivec2", "textureSize", string.Empty, Tuple.Create("gsampler2DMS", "sampler"));
			this.AddFunction(functionsList, "ivec3", "textureSize", string.Empty, Tuple.Create("gsampler2DMSArray", "sampler"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("float", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec2", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec3", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("gsamplerCube", "sampler"), Tuple.Create("vec3", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("gsampler1DArray", "sampler"), Tuple.Create("float", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("gsampler2DArray", "sampler"), Tuple.Create("vec2", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("gsamplerCubeArray", "sampler"), Tuple.Create("vec3", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("float", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec2", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("samplerCubeShadow", "sampler"), Tuple.Create("vec3", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler"), Tuple.Create("float", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"), Tuple.Create("vec2", "P"));
			this.AddFunction(functionsList, "vec2", "textureQueryLod", string.Empty, Tuple.Create("samplerCubeArrayShadow", "sampler"), Tuple.Create("vec3", "P"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("gsampler1D", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("gsampler2D", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("gsampler3D", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("gsamplerCube", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("gsampler1DArray", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("gsampler2DArray", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("gsamplerCubeArray", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("sampler1DShadow", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("sampler2DShadow", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("samplerCubeShadow", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"));
			this.AddFunction(functionsList, "int", "textureQueryLevels", string.Empty, Tuple.Create("samplerCubeArrayShadow", "sampler"));

			// Texture Lookup
			this.AddFunction(functionsList, "gvec4", "texture", string.Empty, Tuple.Create("gsampler1D", "sampler", false), Tuple.Create("float", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "texture", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "texture", string.Empty, Tuple.Create("gsampler3D", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "texture", string.Empty, Tuple.Create("gsamplerCube", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "texture", string.Empty, Tuple.Create("sampler1DShadow", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "texture", string.Empty, Tuple.Create("sampler2DShadow", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "texture", string.Empty, Tuple.Create("samplerCubeShadow", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "texture", string.Empty, Tuple.Create("gsampler1DArray", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "texture", string.Empty, Tuple.Create("gsampler2DArray", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "texture", string.Empty, Tuple.Create("gsamplerCubeArray", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "texture", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "texture", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"), Tuple.Create("vec4", "P"));
			this.AddFunction(functionsList, "gvec4", "texture", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec2", "P"));
			this.AddFunction(functionsList, "float", "texture", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec3", "P"));
			this.AddFunction(functionsList, "float", "texture", string.Empty, Tuple.Create("gsamplerCubeArrayShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "compare"));
			this.AddFunction(functionsList, "gvec4", "textureProj", string.Empty, Tuple.Create("gsampler1D", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProj", string.Empty, Tuple.Create("gsampler1D", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProj", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProj", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProj", string.Empty, Tuple.Create("gsampler3D", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "textureProj", string.Empty, Tuple.Create("sampler1DShadow", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "textureProj", string.Empty, Tuple.Create("sampler2DShadow", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProj", string.Empty, Tuple.Create("gsampler2DRect", "sampler", false), Tuple.Create("vec3", "P", false));
			this.AddFunction(functionsList, "gvec4", "textureProj", string.Empty, Tuple.Create("gsampler2DRect", "sampler", false), Tuple.Create("vec4", "P", false));
			this.AddFunction(functionsList, "float", "textureProj", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler", false), Tuple.Create("vec4", "P", false));
			this.AddFunction(functionsList, "gvec4", "textureLod", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("float", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureLod", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureLod", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureLod", string.Empty, Tuple.Create("gsamplerCube", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "float", "textureLod", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "float", "textureLod", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureLod", string.Empty, Tuple.Create("gsampler1DArray", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureLod", string.Empty, Tuple.Create("gsampler2DArray", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "float", "textureLod", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureLod", string.Empty, Tuple.Create("gsamplerCubeArray", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureOffset", string.Empty, Tuple.Create("gsampler1D", "sampler", false), Tuple.Create("float", "P", false), Tuple.Create("int", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureOffset", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureOffset", string.Empty, Tuple.Create("gsampler3D", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("ivec3", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureOffset", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureOffset", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureOffset", string.Empty, Tuple.Create("sampler1DShadow", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("int", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "textureOffset", string.Empty, Tuple.Create("sampler2DShadow", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureOffset", string.Empty, Tuple.Create("gsampler1DArray", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("int", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureOffset", string.Empty, Tuple.Create("gsampler2DArray", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "textureOffset", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("int", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "textureOffset", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("int", "P"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("ivec2", "P"));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsampler1DArray", "sampler"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsampler2DArray", "sampler"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "lod"));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsamplerBuffer", "sampler"), Tuple.Create("int", "P"));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsampler2DMS", "sampler"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"));
			this.AddFunction(functionsList, "gvec4", "texelFetch", string.Empty, Tuple.Create("gsampler2DMSArray", "sampler"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"));
			this.AddFunction(functionsList, "gvec4", "texelFetchOffset", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("int", "P"), Tuple.Create("int", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "texelFetchOffset", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "lod"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "texelFetchOffset", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "lod"), Tuple.Create("ivec3", "offset"));
			this.AddFunction(functionsList, "gvec4", "texelFetchOffset", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("ivec2", "P"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "texelFetchOffset", string.Empty, Tuple.Create("gsampler1DArray", "sampler"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "texelFetchOffset", string.Empty, Tuple.Create("gsampler2DArray", "sampler"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "lod"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjOffset", string.Empty, Tuple.Create("gsampler1D", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("int", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProjOffset", string.Empty, Tuple.Create("gsampler1D", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("int", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProjOffset", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProjOffset", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProjOffset", string.Empty, Tuple.Create("gsampler3D", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("ivec3", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureProjOffset", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjOffset", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureProjOffset", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureProjOffset", string.Empty, Tuple.Create("sampler1DShadow", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("int", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "float", "textureProjOffset", string.Empty, Tuple.Create("sampler2DShadow", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "gvec4", "textureLodOffset", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("float", "P"), Tuple.Create("float", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureLodOffset", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "lod"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureLodOffset", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureLodOffset", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "float", "textureLodOffset", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureLodOffset", string.Empty, Tuple.Create("gsampler1DArray", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureLodOffset", string.Empty, Tuple.Create("gsampler2DArray", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"), Tuple.Create("ivec3", "offset"));
			this.AddFunction(functionsList, "float", "textureLodOffset", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjLod", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureProjLod", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureProjLod", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureProjLod", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureProjLod", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "float", "textureProjLod", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "float", "textureProjLod", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "gvec4", "textureProjLodOffset", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjLodOffset", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjLodOffset", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "lod"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjLodOffset", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjLodOffset", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"), Tuple.Create("ivec3", "offset"));
			this.AddFunction(functionsList, "float", "textureProjLodOffset", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "float", "textureProjLodOffset", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "lod"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureGrad", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("float", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureGrad", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureGrad", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec3", "dPdx"), Tuple.Create("vec3", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureGrad", string.Empty, Tuple.Create("gsamplerCube", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec3", "dPdx"), Tuple.Create("vec3", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureGrad", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "float", "textureGrad", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "float", "textureGrad", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"));
			this.AddFunction(functionsList, "float", "textureGrad", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "float", "textureGrad", string.Empty, Tuple.Create("samplerCubeShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec3", "dPdx"), Tuple.Create("vec3", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureGrad", string.Empty, Tuple.Create("gsampler1DArray", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureGrad", string.Empty, Tuple.Create("gsampler2DArray", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "float", "textureGrad", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"));
			this.AddFunction(functionsList, "float", "textureGrad", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureGrad", string.Empty, Tuple.Create("gsamplerCubeArray", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec3", "dPdx"), Tuple.Create("vec3", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureGradOffset", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("float", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureGradOffset", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureGradOffset", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec3", "dPdx"), Tuple.Create("vec3", "dPdy"), Tuple.Create("ivec3", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureGradOffset", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureGradOffset", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureGradOffset", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "float", "textureGradOffset", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureGradOffset", string.Empty, Tuple.Create("gsampler1DArray", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureGradOffset", string.Empty, Tuple.Create("gsampler2DArray", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureGradOffset", string.Empty, Tuple.Create("sampler1DArrayShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "float", "textureGradOffset", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjGrad", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureProjGrad", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureProjGrad", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureProjGrad", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureProjGrad", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec3", "dPdx"), Tuple.Create("vec3", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureProjGrad", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureProjGrad", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "float", "textureProjGrad", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "float", "textureProjGrad", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"));
			this.AddFunction(functionsList, "float", "textureProjGrad", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"));
			this.AddFunction(functionsList, "gvec4", "textureProjGradOffset", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjGradOffset", string.Empty, Tuple.Create("gsampler1D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjGradOffset", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjGradOffset", string.Empty, Tuple.Create("gsampler2D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjGradOffset", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjGradOffset", string.Empty, Tuple.Create("gsampler2DRect", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "float", "textureProjGradOffset", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureProjGradOffset", string.Empty, Tuple.Create("gsampler3D", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec3", "dPdx"), Tuple.Create("vec3", "dPdy"), Tuple.Create("ivec3", "offset"));
			this.AddFunction(functionsList, "float", "textureProjGradOffset", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "dPdx"), Tuple.Create("float", "dPdy"), Tuple.Create("int", "offset"));
			this.AddFunction(functionsList, "float", "textureProjGradOffset", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("vec2", "dPdx"), Tuple.Create("vec2", "dPdy"), Tuple.Create("ivec2", "offset"));

			// Texture Gather
			this.AddFunction(functionsList, "gvec4", "textureGather", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "gvec4", "textureGather", string.Empty, Tuple.Create("gsampler2DArray", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "gvec4", "textureGather", string.Empty, Tuple.Create("gsamplerCube", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "gvec4", "textureGather", string.Empty, Tuple.Create("gsamplerCubeArray", "sampler", false), Tuple.Create("vec4", "P", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "gvec4", "textureGather", string.Empty, Tuple.Create("gsampler2DRect", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "vec4", "textureGather", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "refZ"));
			this.AddFunction(functionsList, "vec4", "textureGather", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "refZ"));
			this.AddFunction(functionsList, "vec4", "textureGather", string.Empty, Tuple.Create("samplerCubeShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "refZ"));
			this.AddFunction(functionsList, "vec4", "textureGather", string.Empty, Tuple.Create("samplerCubeArrayShadow", "sampler"), Tuple.Create("vec4", "P"), Tuple.Create("float", "refZ"));
			this.AddFunction(functionsList, "vec4", "textureGather", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "refZ"));
			this.AddFunction(functionsList, "gvec4", "textureGatherOffset", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "gvec4", "textureGatherOffset", string.Empty, Tuple.Create("gsampler2DArray", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "gvec4", "textureGatherOffset", string.Empty, Tuple.Create("gsampler2DRect", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("ivec2", "offset", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "vec4", "textureGatherOffset", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "refZ"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "vec4", "textureGatherOffset", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "refZ"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "vec4", "textureGatherOffset", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "refZ"), Tuple.Create("ivec2", "offset"));
			this.AddFunction(functionsList, "gvec4", "textureGatherOffsets", string.Empty, Tuple.Create("gsampler2D", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("ivec2", "offsets[4]", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "gvec4", "textureGatherOffsets", string.Empty, Tuple.Create("gsampler2DArray", "sampler", false), Tuple.Create("vec3", "P", false), Tuple.Create("ivec2", "offsets[4]", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "gvec4", "textureGatherOffsets", string.Empty, Tuple.Create("gsampler2DRect", "sampler", false), Tuple.Create("vec2", "P", false), Tuple.Create("ivec2", "offsets[4]", false), Tuple.Create("int", "comp", true));
			this.AddFunction(functionsList, "vec4", "textureGatherOffsets", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "refZ"), Tuple.Create("ivec2", "offsets[4]"));
			this.AddFunction(functionsList, "vec4", "textureGatherOffsets", string.Empty, Tuple.Create("sampler2DArrayShadow", "sampler"), Tuple.Create("vec3", "P"), Tuple.Create("float", "refZ"), Tuple.Create("ivec2", "offsets[4]"));
			this.AddFunction(functionsList, "vec4", "textureGatherOffsets", string.Empty, Tuple.Create("sampler2DRectShadow", "sampler"), Tuple.Create("vec2", "P"), Tuple.Create("float", "refZ"), Tuple.Create("ivec2", "offset[4]"));

			// Compatibility Profile Texture
			this.AddFunction(functionsList, "vec4", "texture1D", string.Empty, Tuple.Create("sampler1D", "sampler", false), Tuple.Create("float", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "texture1DProj", string.Empty, Tuple.Create("sampler1D", "sampler", false), Tuple.Create("vec2", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "texture1DProj", string.Empty, Tuple.Create("sampler1D", "sampler", false), Tuple.Create("vec4", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "texture1DLod", string.Empty, Tuple.Create("sampler1D", "sampler"), Tuple.Create("float", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "texture1DProjLod", string.Empty, Tuple.Create("sampler1D", "sampler"), Tuple.Create("vec2", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "texture1DProjLod", string.Empty, Tuple.Create("sampler1D", "sampler"), Tuple.Create("vec4", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "texture2D", string.Empty, Tuple.Create("sampler2D", "sampler", false), Tuple.Create("vec2", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "texture2DProj", string.Empty, Tuple.Create("sampler2D", "sampler", false), Tuple.Create("vec3", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "texture2DProj", string.Empty, Tuple.Create("sampler2D", "sampler", false), Tuple.Create("vec4", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "texture2DLod", string.Empty, Tuple.Create("sampler2D", "sampler"), Tuple.Create("vec2", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "texture2DProjLod", string.Empty, Tuple.Create("sampler2D", "sampler"), Tuple.Create("vec3", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "texture2DProjLod", string.Empty, Tuple.Create("sampler2D", "sampler"), Tuple.Create("vec4", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "texture3D", string.Empty, Tuple.Create("sampler3D", "sampler", false), Tuple.Create("vec3", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "texture3DProj", string.Empty, Tuple.Create("sampler3D", "sampler", false), Tuple.Create("vec4", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "texture3DLod", string.Empty, Tuple.Create("sampler3D", "sampler"), Tuple.Create("vec3", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "texture3DProjLod", string.Empty, Tuple.Create("sampler3D", "sampler"), Tuple.Create("vec4", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "textureCube", string.Empty, Tuple.Create("samplerCube", "sampler", false), Tuple.Create("vec3", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "textureCubeLod", string.Empty, Tuple.Create("samplerCube", "sampler"), Tuple.Create("vec3", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "shadow1D", string.Empty, Tuple.Create("sampler1DShadow", "sampler", false), Tuple.Create("vec3", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "shadow2D", string.Empty, Tuple.Create("sampler2DShadow", "sampler", false), Tuple.Create("vec3", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "shadow1DProj", string.Empty, Tuple.Create("sampler1DShadow", "sampler", false), Tuple.Create("vec4", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "shadow2DProj", string.Empty, Tuple.Create("sampler2DShadow", "sampler", false), Tuple.Create("vec4", "coord", false), Tuple.Create("float", "bias", true));
			this.AddFunction(functionsList, "vec4", "shadow1DLod", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec3", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "shadow2DLod", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec3", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "shadow1DProjLod", string.Empty, Tuple.Create("sampler1DShadow", "sampler"), Tuple.Create("vec4", "coord"), Tuple.Create("float", "lod"));
			this.AddFunction(functionsList, "vec4", "shadow2DProjLod", string.Empty, Tuple.Create("sampler2DShadow", "sampler"), Tuple.Create("vec4", "coord"), Tuple.Create("float", "lod"));

			// Atomic-Counter
			this.AddFunction(functionsList, "uint", "atomicCounterIncrement", string.Empty, Tuple.Create("atomic_uint", "c"));
			this.AddFunction(functionsList, "uint", "atomicCounterDecrement", string.Empty, Tuple.Create("atomic_uint", "c"));
			this.AddFunction(functionsList, "uint", "atomicCounter", string.Empty, Tuple.Create("atomic_uint", "c"));

			// Atomic Memory
			this.AddFunction(functionsList, "uint", "atomicAdd", string.Empty, Tuple.Create("inout", "uint", "mem"), Tuple.Create(string.Empty, "uint", "data"));
			this.AddFunction(functionsList, "int", "atomicAdd", string.Empty, Tuple.Create("inout", "int", "mem"), Tuple.Create(string.Empty, "int", "data"));
			this.AddFunction(functionsList, "uint", "atomicMin", string.Empty, Tuple.Create("inout", "uint", "mem"), Tuple.Create(string.Empty, "uint", "data"));
			this.AddFunction(functionsList, "int", "atomicMin", string.Empty, Tuple.Create("inout", "int", "mem"), Tuple.Create(string.Empty, "int", "data"));
			this.AddFunction(functionsList, "uint", "atomicMax", string.Empty, Tuple.Create("inout", "uint", "mem"), Tuple.Create(string.Empty, "uint", "data"));
			this.AddFunction(functionsList, "int", "atomicMax", string.Empty, Tuple.Create("inout", "int", "mem"), Tuple.Create(string.Empty, "int", "data"));
			this.AddFunction(functionsList, "uint", "atomicAnd", string.Empty, Tuple.Create("inout", "uint", "mem"), Tuple.Create(string.Empty, "uint", "data"));
			this.AddFunction(functionsList, "int", "atomicAnd", string.Empty, Tuple.Create("inout", "int", "mem"), Tuple.Create(string.Empty, "int", "data"));
			this.AddFunction(functionsList, "uint", "atomicOr", string.Empty, Tuple.Create("inout", "uint", "mem"), Tuple.Create(string.Empty, "uint", "data"));
			this.AddFunction(functionsList, "int", "atomicOr", string.Empty, Tuple.Create("inout", "int", "mem"), Tuple.Create(string.Empty, "int", "data"));
			this.AddFunction(functionsList, "uint", "atomicXor", string.Empty, Tuple.Create("inout", "uint", "mem"), Tuple.Create(string.Empty, "uint", "data"));
			this.AddFunction(functionsList, "int", "atomicXor", string.Empty, Tuple.Create("inout", "int", "mem"), Tuple.Create(string.Empty, "int", "data"));
			this.AddFunction(functionsList, "uint", "atomicExchange", string.Empty, Tuple.Create("inout", "uint", "mem"), Tuple.Create(string.Empty, "uint", "data"));
			this.AddFunction(functionsList, "int", "atomicExchange", string.Empty, Tuple.Create("inout", "int", "mem"), Tuple.Create(string.Empty, "int", "data"));
			this.AddFunction(functionsList, "uint", "atomicCompSwap", string.Empty, Tuple.Create("inout", "uint", "mem"), Tuple.Create(string.Empty, "uint", "compare"), Tuple.Create(string.Empty, "uint", "data"));
			this.AddFunction(functionsList, "int", "atomicCompSwap", string.Empty, Tuple.Create("inout", "int", "mem"), Tuple.Create(string.Empty, "int", "compare"), Tuple.Create(string.Empty, "int", "data"));

			// Image
			this.AddFunction(functionsList, "int", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimage1D", "image"));
			this.AddFunction(functionsList, "ivec2", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimage2D", "image"));
			this.AddFunction(functionsList, "ivec3", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimage3D", "image"));
			this.AddFunction(functionsList, "ivec2", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimageCube", "image"));
			this.AddFunction(functionsList, "ivec3", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimageCubeArray", "image"));
			this.AddFunction(functionsList, "ivec2", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimageRect", "image"));
			this.AddFunction(functionsList, "ivec2", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimage1DArray", "image"));
			this.AddFunction(functionsList, "ivec3", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimage2DArray", "image"));
			this.AddFunction(functionsList, "int", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimageBuffer", "image"));
			this.AddFunction(functionsList, "ivec2", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimage2DMS", "image"));
			this.AddFunction(functionsList, "ivec3", "imageSize", string.Empty, Tuple.Create("readonly writeonly", "gimage2DMSArray", "image"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimage1D", "image"), Tuple.Create(string.Empty, "int", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimage2D", "image"), Tuple.Create(string.Empty, "ivec2", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimage3D", "image"), Tuple.Create(string.Empty, "ivec3", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimage2DRect", "image"), Tuple.Create(string.Empty, "ivec2", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimageCube", "image"), Tuple.Create(string.Empty, "ivec3", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimageBuffer", "image"), Tuple.Create(string.Empty, "int", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimage1DArray", "image"), Tuple.Create(string.Empty, "ivec2", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimage2DArray", "image"), Tuple.Create(string.Empty, "ivec3", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimageCubeArray", "image"), Tuple.Create(string.Empty, "ivec3", "P"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimage2DMS", "image"), Tuple.Create(string.Empty, "ivec2", "P"), Tuple.Create(string.Empty, "int", "sample"));
			this.AddFunction(functionsList, "gvec4", "imageLoad", string.Empty, Tuple.Create("readonly", "gimage2DMSArray", "image"), Tuple.Create(string.Empty, "ivec3", "P"), Tuple.Create(string.Empty, "int", "sample"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimage1D", "image"), Tuple.Create(string.Empty, "int", "P"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimage2D", "image"), Tuple.Create(string.Empty, "ivec2", "P"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimage3D", "image"), Tuple.Create(string.Empty, "ivec3", "P"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimage2DRect", "image"), Tuple.Create(string.Empty, "ivec2", "P"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimageCube", "image"), Tuple.Create(string.Empty, "ivec3", "P"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimageBuffer", "image"), Tuple.Create(string.Empty, "int", "P"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimage1DArray", "image"), Tuple.Create(string.Empty, "ivec2", "P"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimage2DArray", "image"), Tuple.Create(string.Empty, "ivec3", "P"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimageCubeArray", "image"), Tuple.Create(string.Empty, "ivec3", "P"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimage2DMS", "image"), Tuple.Create(string.Empty, "ivec2", "P"), Tuple.Create(string.Empty, "int", "sample"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "gvec4", "imageStore", string.Empty, Tuple.Create("writeonly", "gimage2DMSArray", "image"), Tuple.Create(string.Empty, "ivec3", "P"), Tuple.Create(string.Empty, "int", "sample"), Tuple.Create(string.Empty, "gvec4", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAdd", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMin", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMin", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicMax", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicMax", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicAnd", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicOr", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicOr", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicXor", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicXor", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicExchange", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "uint", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("uint", "compare"), Tuple.Create("uint", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage1D", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2D", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage3D", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2DRect", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimageCube", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimageBuffer", "image"), Tuple.Create("int", "P"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage1DArray", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2DArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimageCubeArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2DMS", "image"), Tuple.Create("ivec2", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));
			this.AddFunction(functionsList, "int", "imageAtomicCompSwap", string.Empty, Tuple.Create("gimage2DMSArray", "image"), Tuple.Create("ivec3", "P"), Tuple.Create("int", "sample"), Tuple.Create("int", "compare"), Tuple.Create("int", "data"));

			// Fragment Processing
			// Derivative
			this.AddFunction(functionsList, "genType", "dFdx", string.Empty, Tuple.Create("genType", "p"));
			this.AddFunction(functionsList, "genType", "dFdy", string.Empty, Tuple.Create("genType", "p"));
			this.AddFunction(functionsList, "genType", "fwidth", string.Empty, Tuple.Create("genType", "p"));

			// Interpolation
			this.AddFunction(functionsList, "float", "interpolateAtCentroid", string.Empty, Tuple.Create("float", "interpolant"));
			this.AddFunction(functionsList, "vec2", "interpolateAtCentroid", string.Empty, Tuple.Create("vec2", "interpolant"));
			this.AddFunction(functionsList, "vec3", "interpolateAtCentroid", string.Empty, Tuple.Create("vec3", "interpolant"));
			this.AddFunction(functionsList, "vec4", "interpolateAtCentroid", string.Empty, Tuple.Create("vec4", "interpolant"));

			this.AddFunction(functionsList, "float", "interpolateAtSample", string.Empty, Tuple.Create("float", "interpolant"), Tuple.Create("int", "sample"));
			this.AddFunction(functionsList, "vec2", "interpolateAtSample", string.Empty, Tuple.Create("vec2", "interpolant"), Tuple.Create("int", "sample"));
			this.AddFunction(functionsList, "vec3", "interpolateAtSample", string.Empty, Tuple.Create("vec3", "interpolant"), Tuple.Create("int", "sample"));
			this.AddFunction(functionsList, "vec4", "interpolateAtSample", string.Empty, Tuple.Create("vec4", "interpolant"), Tuple.Create("int", "sample"));

			this.AddFunction(functionsList, "float", "interpolateAtOffset", string.Empty, Tuple.Create("float", "interpolant"), Tuple.Create("vec2", "offset"));
			this.AddFunction(functionsList, "vec2", "interpolateAtOffset", string.Empty, Tuple.Create("vec2", "interpolant"), Tuple.Create("vec2", "offset"));
			this.AddFunction(functionsList, "vec3", "interpolateAtOffset", string.Empty, Tuple.Create("vec3", "interpolant"), Tuple.Create("vec2", "offset"));
			this.AddFunction(functionsList, "vec4", "interpolateAtOffset", string.Empty, Tuple.Create("vec4", "interpolant"), Tuple.Create("vec2", "offset"));

			// Noise
			this.AddFunction(functionsList, "float", "noise1", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "vec2", "noise2", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "vec3", "noise3", string.Empty, Tuple.Create("genType", "x"));
			this.AddFunction(functionsList, "vec4", "noise4", string.Empty, Tuple.Create("genType", "x"));

			// Geometry Shader
			this.AddFunction(functionsList, "void", "EmitStreamVertex", string.Empty, Tuple.Create("int", "stream"));
			this.AddFunction(functionsList, "void", "EmitStreamPrimitive", string.Empty, Tuple.Create("int", "stream"));
			this.AddFunction(functionsList, "void", "EmitVertex", string.Empty);
			this.AddFunction(functionsList, "void", "EmitPrimitive", string.Empty);

			// Shader Invocation Control
			this.AddFunction(functionsList, "void", "barrier", string.Empty);

			// Shader Memory Control
			this.AddFunction(functionsList, "void", "memoryBarrier", string.Empty);
			this.AddFunction(functionsList, "void", "memoryBarrierAtomicCounter", string.Empty);
			this.AddFunction(functionsList, "void", "memoryBarrierBuffer", string.Empty);
			this.AddFunction(functionsList, "void", "memoryBarrierShared", string.Empty);
			this.AddFunction(functionsList, "void", "memoryBarrierImage", string.Empty);
			this.AddFunction(functionsList, "void", "groupMemoryBarrier", string.Empty);

			this.functions.AddRange(functionsList);
		}

		private void AddFunction(List<BuiltInFunction> list, string returnType, string name, string documentation, params Tuple<string, string>[] parameters)
		{
			int genTypeCount = 0;

			for (int i = 0; i < parameters.Length; i++)
			{
				if (this.genTypes.ContainsKey(parameters[i].Item1))
				{
					genTypeCount = this.genTypes[parameters[i].Item1].Length;
				}
			}

			if (this.genTypes.ContainsKey(returnType))
			{
				for (int i = 0; i < this.genTypes[returnType].Length; i++)
				{
					list.Add(new BuiltInFunction(this.genTypes[returnType][i], name, documentation, this.CreateParameters(parameters, i)));
				}
			}
			else if (genTypeCount > 0)
			{
				for (int i = 0; i < genTypeCount; i++)
				{
					list.Add(new BuiltInFunction(returnType, name, documentation, this.CreateParameters(parameters, i)));
				}
			}
			else
			{
				list.Add(new BuiltInFunction(returnType, name, documentation, this.CreateParameters(parameters, 0)));
			}
		}

		private void AddFunction(List<BuiltInFunction> list, string returnType, string name, string documentation, params Tuple<string, string, bool>[] parameters)
		{
			int genTypeCount = 0;

			for (int i = 0; i < parameters.Length; i++)
			{
				if (this.genTypes.ContainsKey(parameters[i].Item1))
				{
					genTypeCount = this.genTypes[parameters[i].Item1].Length;
				}
			}

			if (this.genTypes.ContainsKey(returnType))
			{
				for (int i = 0; i < this.genTypes[returnType].Length; i++)
				{
					list.Add(new BuiltInFunction(this.genTypes[returnType][i], name, documentation, this.CreateParameters(parameters, i)));
				}
			}
			else if (genTypeCount > 0)
			{
				for (int i = 0; i < genTypeCount; i++)
				{
					list.Add(new BuiltInFunction(returnType, name, documentation, this.CreateParameters(parameters, i)));
				}
			}
			else
			{
				list.Add(new BuiltInFunction(returnType, name, documentation, this.CreateParameters(parameters, 0)));
			}
		}

		private void AddFunction(List<BuiltInFunction> list, string returnType, string name, string documentation, params Tuple<string, string, string>[] parameters)
		{
			int genTypeCount = 0;

			for (int i = 0; i < parameters.Length; i++)
			{
				if (this.genTypes.ContainsKey(parameters[i].Item2))
				{
					genTypeCount = this.genTypes[parameters[i].Item2].Length;
					break;
				}
			}

			if (this.genTypes.ContainsKey(returnType))
			{
				for (int i = 0; i < this.genTypes[returnType].Length; i++)
				{
					list.Add(new BuiltInFunction(this.genTypes[returnType][i], name, documentation, this.CreateParameters(parameters, i)));
				}
			}
			else if (genTypeCount > 0)
			{
				for (int i = 0; i < genTypeCount; i++)
				{
					list.Add(new BuiltInFunction(returnType, name, documentation, this.CreateParameters(parameters, i)));
				}
			}
			else
			{
				list.Add(new BuiltInFunction(returnType, name, documentation, this.CreateParameters(parameters, 0)));
			}
		}

		private void AddFunction(List<BuiltInFunction> list, string returnType, string name, string documentation)
		{
			if (this.genTypes.ContainsKey(returnType))
			{
				list.Add(new BuiltInFunction(this.genTypes[returnType][0], name, documentation, null));
				list.Add(new BuiltInFunction(this.genTypes[returnType][1], name, documentation, null));
				list.Add(new BuiltInFunction(this.genTypes[returnType][2], name, documentation, null));
				list.Add(new BuiltInFunction(this.genTypes[returnType][3], name, documentation, null));
			}
			else
			{
				list.Add(new BuiltInFunction(returnType, name, documentation, null));
			}
		}

		private Parameter[] CreateParameters(Tuple<string, string>[] parameters, int genIndex)
		{
			Parameter[] result = new Parameter[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				if (this.genTypes.ContainsKey(parameters[i].Item1))
				{
					result[i] = new Parameter(this.genTypes[parameters[i].Item1][genIndex], parameters[i].Item2);
				}
				else
				{
					result[i] = new Parameter(parameters[i].Item1, parameters[i].Item2);
				}
			}

			return result;
		}

		private Parameter[] CreateParameters(Tuple<string, string, bool>[] parameters, int genIndex)
		{
			Parameter[] result = new Parameter[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				if (this.genTypes.ContainsKey(parameters[i].Item1))
				{
					result[i] = new Parameter(this.genTypes[parameters[i].Item1][genIndex], parameters[i].Item2, parameters[i].Item3);
				}
				else
				{
					result[i] = new Parameter(parameters[i].Item1, parameters[i].Item2, parameters[i].Item3);
				}
			}

			return result;
		}

		private Parameter[] CreateParameters(Tuple<string, string, string>[] parameters, int genIndex)
		{
			Parameter[] result = new Parameter[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				if (this.genTypes.ContainsKey(parameters[i].Item2))
				{
					result[i] = new Parameter(this.genTypes[parameters[i].Item2][genIndex], parameters[i].Item3, parameters[i].Item1);
				}
				else
				{
					result[i] = new Parameter(parameters[i].Item2, parameters[i].Item3, parameters[i].Item1);
				}
			}

			return result;
		}

		private void WriteToXml()
		{
			using (IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(File.Create("builtIn.xml")), "\t"))
			{
				writer.WriteLine("<Data>");
				writer.IndentLevel++;

				foreach (BuiltInDefinition definition in this.functions)
				{
					definition.WriteToXml(writer);
				}

				writer.IndentLevel--;
				writer.WriteLine("</Data>");
			}
		}
	}
}