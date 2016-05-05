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
			this.AddFunction(functionList, "genType", "cos", Resources.CosDoc, Parameter.Create("genType", "angle"));
			this.AddFunction(functionList, "genType", "tan", Resources.TanDoc, Parameter.Create("genType", "angle"));
			this.AddFunction(functionList, "genType", "asin", Resources.AsinDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "acos", Resources.AcosDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "atan", Resources.AtanDoc, Parameter.Create("genType", "y"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "atan", Resources.AtanDoc, Parameter.Create("genType", "y_over_x"));
			this.AddFunction(functionList, "genType", "sinh", Resources.SinhDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "cosh", Resources.CoshDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "tanh", Resources.TanhDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "asinh", Resources.AsinhDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "acosh", Resources.AcoshDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "atanh", Resources.AtanhDoc, Parameter.Create("genType", "x"));

			// Exponential
			this.AddFunction(functionList, "genType", "pow", Resources.PowDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "genType", "exp", Resources.ExpDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "log", Resources.LogDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "exp2", Resources.Exp2Doc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "log2", Resources.Log2Doc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "sqrt", Resources.SqrtDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "sqrt", Resources.SqrtDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "inversesqrt", Resources.InversesqrtDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "inversesqrt", Resources.InversesqrtDoc, Parameter.Create("genDType", "x"));

			// Common
			this.AddFunction(functionList, "genType", "abs", Resources.AbsDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genIType", "abs", Resources.AbsDoc, Parameter.Create("genIType", "x"));
			this.AddFunction(functionList, "genDType", "abs", Resources.AbsDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "sign", Resources.SignDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genIType", "sign", Resources.SignDoc, Parameter.Create("genIType", "x"));
			this.AddFunction(functionList, "genDType", "sign", Resources.SignDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "floor", Resources.FloorDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "floor", Resources.FloorDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "trunc", Resources.TruncDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "trunc", Resources.TruncDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "round", Resources.RoundDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "round", Resources.RoundDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "roundEven", Resources.RoundEvenDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "roundEven", Resources.RoundEvenDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "ceil", Resources.CeilDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "ceil", Resources.CeilDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "fract", Resources.FractDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "fract", Resources.FractDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "mod", Resources.ModDoc, Parameter.Create("genType", "x"), Parameter.Create("float", "y"));
			this.AddFunction(functionList, "genType", "mod", Resources.ModDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "genDType", "mod", Resources.ModDoc, Parameter.Create("genDType", "x"), Parameter.Create("double", "y"));
			this.AddFunction(functionList, "genDType", "mod", Resources.ModDoc, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"));
			this.AddFunction(functionList, "genType", "modf", Resources.ModfDoc, Parameter.Create("genType", "x"), Parameter.Create("out", "genType", "i"));
			this.AddFunction(functionList, "genDType", "modf", Resources.ModfDoc, Parameter.Create("genDType", "x"), Parameter.Create("out", "genDType", "i"));
			this.AddFunction(functionList, "genType", "min", Resources.MinDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "genType", "min", Resources.MinDoc, Parameter.Create("genType", "x"), Parameter.Create("float", "y"));
			this.AddFunction(functionList, "genDType", "min", Resources.MinDoc, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"));
			this.AddFunction(functionList, "genDType", "min", Resources.MinDoc, Parameter.Create("genDType", "x"), Parameter.Create("double", "y"));
			this.AddFunction(functionList, "genIType", "min", Resources.MinDoc, Parameter.Create("genIType", "x"), Parameter.Create("genIType", "y"));
			this.AddFunction(functionList, "genIType", "min", Resources.MinDoc, Parameter.Create("genIType", "x"), Parameter.Create("int", "y"));
			this.AddFunction(functionList, "genUType", "min", Resources.MinDoc, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"));
			this.AddFunction(functionList, "genUType", "min", Resources.MinDoc, Parameter.Create("genUType", "x"), Parameter.Create("uint", "y"));
			this.AddFunction(functionList, "genType", "max", Resources.MaxDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "genType", "max", Resources.MaxDoc, Parameter.Create("genType", "x"), Parameter.Create("float", "y"));
			this.AddFunction(functionList, "genDType", "max", Resources.MaxDoc, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"));
			this.AddFunction(functionList, "genDType", "max", Resources.MaxDoc, Parameter.Create("genDType", "x"), Parameter.Create("double", "y"));
			this.AddFunction(functionList, "genIType", "max", Resources.MaxDoc, Parameter.Create("genIType", "x"), Parameter.Create("genIType", "y"));
			this.AddFunction(functionList, "genIType", "max", Resources.MaxDoc, Parameter.Create("genIType", "x"), Parameter.Create("int", "y"));
			this.AddFunction(functionList, "genUType", "max", Resources.MaxDoc, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"));
			this.AddFunction(functionList, "genUType", "max", Resources.MaxDoc, Parameter.Create("genUType", "x"), Parameter.Create("uint", "y"));
			this.AddFunction(functionList, "genType", "clamp", Resources.ClampDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "minVal"), Parameter.Create("genType", "maxVal"));
			this.AddFunction(functionList, "genType", "clamp", Resources.ClampDoc, Parameter.Create("genType", "x"), Parameter.Create("float", "minVal"), Parameter.Create("float", "maxVal"));
			this.AddFunction(functionList, "genDType", "clamp", Resources.ClampDoc, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "minVal"), Parameter.Create("genDType", "maxVal"));
			this.AddFunction(functionList, "genDType", "clamp", Resources.ClampDoc, Parameter.Create("genDType", "x"), Parameter.Create("double", "minVal"), Parameter.Create("double", "maxVal"));
			this.AddFunction(functionList, "genIType", "clamp", Resources.ClampDoc, Parameter.Create("genIType", "x"), Parameter.Create("genIType", "minVal"), Parameter.Create("genIType", "maxVal"));
			this.AddFunction(functionList, "genIType", "clamp", Resources.ClampDoc, Parameter.Create("genIType", "x"), Parameter.Create("int", "minVal"), Parameter.Create("float", "int"));
			this.AddFunction(functionList, "genUType", "clamp", Resources.ClampDoc, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "minVal"), Parameter.Create("genUType", "maxVal"));
			this.AddFunction(functionList, "genUType", "clamp", Resources.ClampDoc, Parameter.Create("genUType", "x"), Parameter.Create("uint", "minVal"), Parameter.Create("uint", "maxVal"));
			this.AddFunction(functionList, "genType", "mix", Resources.MixDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"), Parameter.Create("genType", "a"));
			this.AddFunction(functionList, "genType", "mix", Resources.MixDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"), Parameter.Create("float", "a"));
			this.AddFunction(functionList, "genDType", "mix", Resources.MixDoc, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"), Parameter.Create("genDType", "a"));
			this.AddFunction(functionList, "genDType", "mix", Resources.MixDoc, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"), Parameter.Create("double", "a"));
			this.AddFunction(functionList, "genType", "mix", Resources.MixDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"), Parameter.Create("genBType", "a"));
			this.AddFunction(functionList, "genDType", "mix", Resources.MixDoc, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"), Parameter.Create("genBType", "a"));
			this.AddFunction(functionList, "genType", "step", Resources.StepDoc, Parameter.Create("genType", "edge"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "step", Resources.StepDoc, Parameter.Create("float", "edge"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "step", Resources.StepDoc, Parameter.Create("genDType", "edge"), Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genDType", "step", Resources.StepDoc, Parameter.Create("double", "edge"), Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genType", "smoothstep", Resources.SmoothstepDoc, Parameter.Create("genType", "edge0"), Parameter.Create("genType", "edge1"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genType", "smoothstep", Resources.SmoothstepDoc, Parameter.Create("float", "edge0"), Parameter.Create("float", "edge1"), Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "smoothstep", Resources.SmoothstepDoc, Parameter.Create("genDType", "edge0"), Parameter.Create("genDType", "edge1"), Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genDType", "smoothstep", Resources.SmoothstepDoc, Parameter.Create("double", "edge0"), Parameter.Create("double", "edge1"), Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genBType", "isnan", Resources.IsnanDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genBType", "isnan", Resources.IsnanDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genBType", "isinf", Resources.IsinfDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genBType", "isinf", Resources.IsinfDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "genIType", "floatBitsToInt", Resources.FloatBitsToIntDoc, Parameter.Create("genType", "value"));
			this.AddFunction(functionList, "genUType", "floatBitsToUint", Resources.FloatBitsToUintDoc, Parameter.Create("genType", "value"));
			this.AddFunction(functionList, "genType", "intBitsToFloat", Resources.IntBitsToFloatDoc, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genType", "uintBitsToFloat", Resources.UintBitsToFloatDoc, Parameter.Create("genUType", "value"));
			this.AddFunction(functionList, "genType", "fma", Resources.FmaDoc, Parameter.Create("genType", "a"), Parameter.Create("genType", "b"), Parameter.Create("genType", "c"));
			this.AddFunction(functionList, "genDType", "fma", Resources.FmaDoc, Parameter.Create("genDType", "a"), Parameter.Create("genDType", "b"), Parameter.Create("genDType", "c"));
			this.AddFunction(functionList, "genType", "frexp", Resources.FrexpDoc, Parameter.Create("genType", "x"), Parameter.Create("out", "genIType", "exp"));
			this.AddFunction(functionList, "genDType", "frexp", Resources.FrexpDoc, Parameter.Create("genDType", "x"), Parameter.Create("out", "genIType", "exp"));
			this.AddFunction(functionList, "genType", "ldexp", Resources.LdexpDoc, Parameter.Create("genType", "x"), Parameter.Create("in", "genIType", "exp"));
			this.AddFunction(functionList, "genDType", "ldexp", Resources.LdexpDoc, Parameter.Create("genDType", "x"), Parameter.Create("in", "genIType", "exp"));

			// Floating-Point Pack and Unpack
			this.AddFunction(functionList, "uint", "packUnorm2x16", Resources.PackUnorm2x16Doc, Parameter.Create("vec2", "v"));
			this.AddFunction(functionList, "uint", "packSnorm2x16", Resources.PackSnorm2x16Doc, Parameter.Create("vec2", "v"));
			this.AddFunction(functionList, "uint", "packUnorm4x8", Resources.PackUnorm4x8Doc, Parameter.Create("vec4", "v"));
			this.AddFunction(functionList, "uint", "packSnorm4x8", Resources.PackSnorm4x8Doc, Parameter.Create("vec4", "v"));
			this.AddFunction(functionList, "vec2", "unpackUnorm2x16", Resources.UnpackUnorm2x16Doc, Parameter.Create("uint", "p"));
			this.AddFunction(functionList, "vec2", "unpackSnorm2x16", Resources.UnpackSnorm2x16Doc, Parameter.Create("uint", "p"));
			this.AddFunction(functionList, "vec4", "unpackUnorm4x8", Resources.UnpackUnorm4x8Doc, Parameter.Create("uint", "p"));
			this.AddFunction(functionList, "vec4", "unpackSnorm4x8", Resources.UnpackSnorm4x8Doc, Parameter.Create("uint", "p"));
			this.AddFunction(functionList, "double", "packDouble2x32", Resources.PackDouble2x32Doc, Parameter.Create("uvec2", "v"));
			this.AddFunction(functionList, "uvec2", "unpackDouble2x32", Resources.UnpackDouble2x32Doc, Parameter.Create("double", "v"));
			this.AddFunction(functionList, "uint", "packHalf2x16", Resources.PackHalf2x16Doc, Parameter.Create("vec2", "v"));
			this.AddFunction(functionList, "vec2", "unpackHalf2x16", Resources.UnpackHalf2x16Doc, Parameter.Create("uint", "v"));

			// Geometric
			this.AddFunction(functionList, "float", "length", Resources.LengthDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "double", "length", Resources.LengthDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "float", "distance", Resources.DistanceDoc, Parameter.Create("genType", "p0"), Parameter.Create("genType", "p1"));
			this.AddFunction(functionList, "double", "distance", Resources.DistanceDoc, Parameter.Create("genDType", "p0"), Parameter.Create("genDType", "p1"));
			this.AddFunction(functionList, "float", "dot", Resources.DotDoc, Parameter.Create("genType", "x"), Parameter.Create("genType", "y"));
			this.AddFunction(functionList, "double", "dot", Resources.DotDoc, Parameter.Create("genDType", "x"), Parameter.Create("genDType", "y"));
			this.AddFunction(functionList, "vec3", "cross", Resources.CrossDoc, Parameter.Create("vec3", "x"), Parameter.Create("vec3", "y"));
			this.AddFunction(functionList, "dvec3", "cross", Resources.CrossDoc, Parameter.Create("dvec3", "x"), Parameter.Create("dvec3", "y"));
			this.AddFunction(functionList, "genType", "normalize", Resources.NormalizeDoc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "genDType", "normalize", Resources.NormalizeDoc, Parameter.Create("genDType", "x"));
			this.AddFunction(functionList, "vec4", "ftransform", Resources.FtransformDoc);
			this.AddFunction(functionList, "genType", "faceforward", Resources.FaceforwardDoc, Parameter.Create("genType", "N"), Parameter.Create("genType", "I"), Parameter.Create("genType", "Nref"));
			this.AddFunction(functionList, "genDType", "faceforward", Resources.FaceforwardDoc, Parameter.Create("genDType", "N"), Parameter.Create("genDType", "I"), Parameter.Create("genDType", "Nref"));
			this.AddFunction(functionList, "genType", "reflect", Resources.ReflectDoc, Parameter.Create("genType", "I"), Parameter.Create("genType", "N"));
			this.AddFunction(functionList, "genDType", "reflect", Resources.ReflectDoc, Parameter.Create("genDType", "I"), Parameter.Create("genDType", "N"));
			this.AddFunction(functionList, "genType", "refract", Resources.RefractDoc, Parameter.Create("genType", "I"), Parameter.Create("genType", "N"), Parameter.Create("float", "eta"));
			this.AddFunction(functionList, "genDType", "refract", Resources.RefractDoc, Parameter.Create("genDType", "I"), Parameter.Create("genDType", "N"), Parameter.Create("float", "eta"));

			// Matrix
			this.AddFunction(functionList, "mat", "matrixCompMult", Resources.MatrixCompMultDoc, Parameter.Create("mat", "x"), Parameter.Create("mat", "y"));
			this.AddFunction(functionList, "mat2", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec2", "c"), Parameter.Create("vec2", "r"));
			this.AddFunction(functionList, "mat3", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec3", "c"), Parameter.Create("vec3", "r"));
			this.AddFunction(functionList, "mat4", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec4", "c"), Parameter.Create("vec4", "r"));
			this.AddFunction(functionList, "mat2x3", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec3", "c"), Parameter.Create("vec2", "r"));
			this.AddFunction(functionList, "mat3x2", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec2", "c"), Parameter.Create("vec3", "r"));
			this.AddFunction(functionList, "mat2x4", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec4", "c"), Parameter.Create("vec2", "r"));
			this.AddFunction(functionList, "mat4x2", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec2", "c"), Parameter.Create("vec4", "r"));
			this.AddFunction(functionList, "mat3x4", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec4", "c"), Parameter.Create("vec3", "r"));
			this.AddFunction(functionList, "mat4x3", "outerProduct", Resources.OuterProductDoc, Parameter.Create("vec3", "c"), Parameter.Create("vec4", "r"));
			this.AddFunction(functionList, "mat2", "transpose", Resources.TransposeDoc, Parameter.Create("mat2", "m"));
			this.AddFunction(functionList, "mat3", "transpose", Resources.TransposeDoc, Parameter.Create("mat3", "m"));
			this.AddFunction(functionList, "mat4", "transpose", Resources.TransposeDoc, Parameter.Create("mat4", "m"));
			this.AddFunction(functionList, "mat2x3", "transpose", Resources.TransposeDoc, Parameter.Create("mat3x2", "m"));
			this.AddFunction(functionList, "mat3x2", "transpose", Resources.TransposeDoc, Parameter.Create("mat2x3", "m"));
			this.AddFunction(functionList, "mat2x4", "transpose", Resources.TransposeDoc, Parameter.Create("mat4x2", "m"));
			this.AddFunction(functionList, "mat4x2", "transpose", Resources.TransposeDoc, Parameter.Create("mat2x4", "m"));
			this.AddFunction(functionList, "mat3x4", "transpose", Resources.TransposeDoc, Parameter.Create("mat4x3", "m"));
			this.AddFunction(functionList, "mat4x3", "transpose", Resources.TransposeDoc, Parameter.Create("mat3x4", "m"));
			this.AddFunction(functionList, "float", "determinant", Resources.DeterminantDoc, Parameter.Create("mat2", "m"));
			this.AddFunction(functionList, "float", "determinant", Resources.DeterminantDoc, Parameter.Create("mat3", "m"));
			this.AddFunction(functionList, "float", "determinant", Resources.DeterminantDoc, Parameter.Create("mat4", "m"));
			this.AddFunction(functionList, "mat2", "inverse", Resources.InverseDoc, Parameter.Create("mat2", "m"));
			this.AddFunction(functionList, "mat3", "inverse", Resources.InverseDoc, Parameter.Create("mat3", "m"));
			this.AddFunction(functionList, "mat4", "inverse", Resources.InverseDoc, Parameter.Create("mat4", "m"));
			this.AddFunction(functionList, "dmat", "matrixCompMult", Resources.MatrixCompMultDoc, Parameter.Create("dmat", "x"), Parameter.Create("dmat", "y"));
			this.AddFunction(functionList, "dmat2", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec2", "c"), Parameter.Create("dvec2", "r"));
			this.AddFunction(functionList, "dmat3", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec3", "c"), Parameter.Create("dvec3", "r"));
			this.AddFunction(functionList, "dmat4", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec4", "c"), Parameter.Create("dvec4", "r"));
			this.AddFunction(functionList, "dmat2x3", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec3", "c"), Parameter.Create("dvec2", "r"));
			this.AddFunction(functionList, "dmat3x2", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec2", "c"), Parameter.Create("dvec3", "r"));
			this.AddFunction(functionList, "dmat2x4", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec4", "c"), Parameter.Create("dvec2", "r"));
			this.AddFunction(functionList, "dmat4x2", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec2", "c"), Parameter.Create("dvec4", "r"));
			this.AddFunction(functionList, "dmat3x4", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec4", "c"), Parameter.Create("dvec3", "r"));
			this.AddFunction(functionList, "dmat4x3", "outerProduct", Resources.OuterProductDoc, Parameter.Create("dvec3", "c"), Parameter.Create("dvec4", "r"));
			this.AddFunction(functionList, "dmat2", "transpose", Resources.TransposeDoc, Parameter.Create("dmat2", "m"));
			this.AddFunction(functionList, "dmat3", "transpose", Resources.TransposeDoc, Parameter.Create("dmat3", "m"));
			this.AddFunction(functionList, "dmat4", "transpose", Resources.TransposeDoc, Parameter.Create("dmat4", "m"));
			this.AddFunction(functionList, "dmat2x3", "transpose", Resources.TransposeDoc, Parameter.Create("dmat3x2", "m"));
			this.AddFunction(functionList, "dmat3x2", "transpose", Resources.TransposeDoc, Parameter.Create("dmat2x3", "m"));
			this.AddFunction(functionList, "dmat2x4", "transpose", Resources.TransposeDoc, Parameter.Create("dmat4x2", "m"));
			this.AddFunction(functionList, "dmat4x2", "transpose", Resources.TransposeDoc, Parameter.Create("dmat2x4", "m"));
			this.AddFunction(functionList, "dmat3x4", "transpose", Resources.TransposeDoc, Parameter.Create("dmat4x3", "m"));
			this.AddFunction(functionList, "dmat4x3", "transpose", Resources.TransposeDoc, Parameter.Create("dmat3x4", "m"));
			this.AddFunction(functionList, "double", "determinant", Resources.DeterminantDoc, Parameter.Create("dmat2", "m"));
			this.AddFunction(functionList, "double", "determinant", Resources.DeterminantDoc, Parameter.Create("dmat3", "m"));
			this.AddFunction(functionList, "double", "determinant", Resources.DeterminantDoc, Parameter.Create("dmat4", "m"));
			this.AddFunction(functionList, "dmat2", "inverse", Resources.InverseDoc, Parameter.Create("dmat2", "m"));
			this.AddFunction(functionList, "dmat3", "inverse", Resources.InverseDoc, Parameter.Create("dmat3", "m"));
			this.AddFunction(functionList, "dmat4", "inverse", Resources.InverseDoc, Parameter.Create("dmat4", "m"));

			// Vector Relational
			this.AddFunction(functionList, "bvec", "lessThan", Resources.LessThanDoc, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "lessThan", Resources.LessThanDoc, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "lessThan", Resources.LessThanDoc, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "lessThan", Resources.LessThanDoc, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThan", Resources.GreaterThanDoc, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThan", Resources.GreaterThanDoc, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThan", Resources.GreaterThanDoc, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThan", Resources.GreaterThanDoc, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "equal", Resources.EqualDoc, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "equal", Resources.EqualDoc, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "equal", Resources.EqualDoc, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "equal", Resources.EqualDoc, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "equal", Resources.EqualDoc, Parameter.Create("bvec", "x"), Parameter.Create("bvec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", Resources.NotEqualDoc, Parameter.Create("vec", "x"), Parameter.Create("vec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", Resources.NotEqualDoc, Parameter.Create("dvec", "x"), Parameter.Create("dvec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", Resources.NotEqualDoc, Parameter.Create("ivec", "x"), Parameter.Create("ivec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", Resources.NotEqualDoc, Parameter.Create("uvec", "x"), Parameter.Create("uvec", "y"));
			this.AddFunction(functionList, "bvec", "notEqual", Resources.NotEqualDoc, Parameter.Create("bvec", "x"), Parameter.Create("bvec", "y"));
			this.AddFunction(functionList, "bool", "any", Resources.AnyDoc, Parameter.Create("bvec", "x"));
			this.AddFunction(functionList, "bool", "all", Resources.AllDoc, Parameter.Create("bvec", "x"));
			this.AddFunction(functionList, "bvec", "not", Resources.NotDoc, Parameter.Create("bvec", "x"));

			// Integer
			this.AddFunction(functionList, "genUType", "uaddCarry", Resources.UaddCarryDoc, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"), Parameter.Create("out", "genUType", "carry"));
			this.AddFunction(functionList, "genUType", "usubBorrow", Resources.UsubBorrowDoc, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"), Parameter.Create("out", "genUType", "borrow"));
			this.AddFunction(functionList, "void", "umulExtended", Resources.UmulExtendedDoc, Parameter.Create("genUType", "x"), Parameter.Create("genUType", "y"), Parameter.Create("out", "genUType", "msb"), Parameter.Create("out", "genUType", "lsb"));
			this.AddFunction(functionList, "void", "imulExtended", Resources.ImulExtendedDoc, Parameter.Create("genIType", "x"), Parameter.Create("genIType", "y"), Parameter.Create("out", "genIType", "msb"), Parameter.Create("out", "genIType", "lsb"));
			this.AddFunction(functionList, "genIType", "bitfieldExtract", Resources.BitfieldExtractDoc, Parameter.Create("genIType", "value"), Parameter.Create("int", "offset"), Parameter.Create("int", "bits"));
			this.AddFunction(functionList, "genUType", "bitfieldExtract", Resources.BitfieldExtractDoc, Parameter.Create("genUType", "value"), Parameter.Create("int", "offset"), Parameter.Create("int", "bits"));
			this.AddFunction(functionList, "genIType", "bitfieldInsert", Resources.BitfieldInsertDoc, Parameter.Create("genIType", "base"), Parameter.Create("genIType", "insert"), Parameter.Create("int", "offset"), Parameter.Create("int", "bits"));
			this.AddFunction(functionList, "genUType", "bitfieldInsert", Resources.BitfieldInsertDoc, Parameter.Create("genUType", "value"), Parameter.Create("genUType", "insert"), Parameter.Create("int", "offset"), Parameter.Create("int", "bits"));
			this.AddFunction(functionList, "genIType", "bitfieldReverse", Resources.BitfieldReverseDoc, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genUType", "bitfieldReverse", Resources.BitfieldReverseDoc, Parameter.Create("genUType", "value"));
			this.AddFunction(functionList, "genIType", "bitCount", Resources.BitCountDoc, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genIType", "bitCount", Resources.BitCountDoc, Parameter.Create("genUType", "value"));
			this.AddFunction(functionList, "genIType", "findLSB", Resources.FindLSBDoc, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genIType", "findLSB", Resources.FindLSBDoc, Parameter.Create("genUType", "value"));
			this.AddFunction(functionList, "genIType", "findMSB", Resources.FindMSBDoc, Parameter.Create("genIType", "value"));
			this.AddFunction(functionList, "genIType", "findMSB", Resources.FindMSBDoc, Parameter.Create("genUType", "value"));

			// Texture Query
			this.AddFunction(functionList, "int", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "int", "textureSize", Resources.TextureSizeDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", Resources.TextureSizeDoc, Parameter.Create("samplerCubeArrayShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsampler2DRect", "sampler"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("sampler2DRectShadow", "sampler"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "ivec3", "textureSize", Resources.TextureSizeDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "int", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsamplerBuffer", "sampler"));
			this.AddFunction(functionList, "ivec2", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsampler2DMS", "sampler"));
			this.AddFunction(functionList, "ivec3", "textureSize", Resources.TextureSizeDoc, Parameter.Create("gsampler2DMSArray", "sampler"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("float", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("float", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("float", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, Parameter.Create("samplerCubeArrayShadow", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("gsampler1D", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("gsampler2D", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("gsampler3D", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("gsamplerCube", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("gsampler1DArray", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("gsampler2DArray", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("gsamplerCubeArray", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("sampler1DShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("sampler2DShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("samplerCubeShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("sampler1DArrayShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("sampler2DArrayShadow", "sampler"));
			this.AddFunction(functionList, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, Parameter.Create("samplerCubeArrayShadow", "sampler"));

			// Texture Lookup
			this.AddFunction(functionList, "gvec4", "texture", Resources.TextureDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", Resources.TextureDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", Resources.TextureDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", Resources.TextureDoc, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", Resources.TextureDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", Resources.TextureDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", Resources.TextureDoc, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", Resources.TextureDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", Resources.TextureDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texture", Resources.TextureDoc, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", Resources.TextureDoc, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "texture", Resources.TextureDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec4", "P"));
			this.AddFunction(functionList, "gvec4", "texture", Resources.TextureDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"));
			this.AddFunction(functionList, "float", "texture", Resources.TextureDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "float", "texture", Resources.TextureDoc, Parameter.Create("gsamplerCubeArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "compare"));
			this.AddFunction(functionList, "gvec4", "textureProj", Resources.TextureProjDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", Resources.TextureProjDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", Resources.TextureProjDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", Resources.TextureProjDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", Resources.TextureProjDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureProj", Resources.TextureProjDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureProj", Resources.TextureProjDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProj", Resources.TextureProjDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec3", "P"));
			this.AddFunction(functionList, "gvec4", "textureProj", Resources.TextureProjDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec4", "P"));
			this.AddFunction(functionList, "float", "textureProj", Resources.TextureProjDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec4", "P"));
			this.AddFunction(functionList, "gvec4", "textureLod", Resources.TextureLodDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", Resources.TextureLodDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", Resources.TextureLodDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", Resources.TextureLodDoc, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureLod", Resources.TextureLodDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureLod", Resources.TextureLodDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", Resources.TextureLodDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", Resources.TextureLodDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureLod", Resources.TextureLodDoc, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureLod", Resources.TextureLodDoc, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec3", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureOffset", Resources.TextureOffsetDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("int", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("ivec2", "P"));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "lod"));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsamplerBuffer", "sampler"), Parameter.Create("int", "P"));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsampler2DMS", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "gvec4", "texelFetch", Resources.TexelFetchDoc, Parameter.Create("gsampler2DMSArray", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("int", "P"), Parameter.Create("int", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "lod"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec3", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("int", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureProjLod", Resources.TextureProjLodDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "float", "textureProjLod", Resources.TextureProjLodDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "float", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "lod"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGrad", Resources.TextureGradDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", Resources.TextureGradDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", Resources.TextureGradDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", Resources.TextureGradDoc, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", Resources.TextureGradDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", Resources.TextureGradDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", Resources.TextureGradDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", Resources.TextureGradDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", Resources.TextureGradDoc, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", Resources.TextureGradDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", Resources.TextureGradDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", Resources.TextureGradDoc, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "float", "textureGrad", Resources.TextureGradDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGrad", Resources.TextureGradDoc, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("float", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("gsampler1DArray", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("sampler1DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "float", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"));
			this.AddFunction(functionList, "float", "textureProjGrad", Resources.TextureProjGradDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("gsampler1D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("gsampler3D", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec3", "dPdx"), Parameter.Create("vec3", "dPdy"), Parameter.Create("ivec3", "offset"));
			this.AddFunction(functionList, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "dPdx"), Parameter.Create("float", "dPdy"), Parameter.Create("int", "offset"));
			this.AddFunction(functionList, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("vec2", "dPdx"), Parameter.Create("vec2", "dPdy"), Parameter.Create("ivec2", "offset"));

			// Texture Gather
			this.AddFunction(functionList, "gvec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("gsamplerCube", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("gsamplerCubeArray", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "vec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "vec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "vec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("samplerCubeShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "vec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("samplerCubeArrayShadow", "sampler"), Parameter.Create("vec4", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "vec4", "textureGather", Resources.TextureGatherDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"));
			this.AddFunction(functionList, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offset"), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offset"));
			this.AddFunction(functionList, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, Parameter.Create("gsampler2D", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offsets", 4), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, Parameter.Create("gsampler2DArray", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("ivec2", "offsets", 4), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, Parameter.Create("gsampler2DRect", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("ivec2", "offsets", 4), Parameter.Create("int", "comp", true));
			this.AddFunction(functionList, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offsets", 4));
			this.AddFunction(functionList, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, Parameter.Create("sampler2DArrayShadow", "sampler"), Parameter.Create("vec3", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offsets", 4));
			this.AddFunction(functionList, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, Parameter.Create("sampler2DRectShadow", "sampler"), Parameter.Create("vec2", "P"), Parameter.Create("float", "refZ"), Parameter.Create("ivec2", "offset", 4));

			// Compatibility Profile Texture
			this.AddFunction(functionList, "vec4", "texture1D", Resources.Texture1DDoc, Parameter.Create("sampler1D", "sampler"), Parameter.Create("float", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture1DProj", Resources.Texture1DProjDoc, Parameter.Create("sampler1D", "sampler"), Parameter.Create("vec2", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture1DProj", Resources.Texture1DProjDoc, Parameter.Create("sampler1D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture1DLod", Resources.Texture1DLodDoc, Parameter.Create("sampler1D", "sampler"), Parameter.Create("float", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture1DProjLod", Resources.Texture1DProjLodDoc, Parameter.Create("sampler1D", "sampler"), Parameter.Create("vec2", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture1DProjLod", Resources.Texture1DProjLodDoc, Parameter.Create("sampler1D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture2D", Resources.Texture2DDoc, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec2", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture2DProj", Resources.Texture2DProjDoc, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture2DProj", Resources.Texture2DProjDoc, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture2DLod", Resources.Texture2DLodDoc, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec2", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture2DProjLod", Resources.Texture2DProjLodDoc, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture2DProjLod", Resources.Texture2DProjLodDoc, Parameter.Create("sampler2D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture3D", Resources.Texture3DDoc, Parameter.Create("sampler3D", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture3DProj", Resources.Texture3DProjDoc, Parameter.Create("sampler3D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "texture3DLod", Resources.Texture3DLodDoc, Parameter.Create("sampler3D", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "texture3DProjLod", Resources.Texture3DProjLodDoc, Parameter.Create("sampler3D", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "textureCube", Resources.TextureCubeDoc, Parameter.Create("samplerCube", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "textureCubeLod", Resources.TextureCubeLodDoc, Parameter.Create("samplerCube", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "shadow1D", Resources.Shadow1DDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "shadow2D", Resources.Shadow2DDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "shadow1DProj", Resources.Shadow1DProjDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "shadow2DProj", Resources.Shadow2DProjDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "bias", true));
			this.AddFunction(functionList, "vec4", "shadow1DLod", Resources.Shadow1DLodDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "shadow2DLod", Resources.Shadow2DLodDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec3", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "shadow1DProjLod", Resources.Shadow1DProjLodDoc, Parameter.Create("sampler1DShadow", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));
			this.AddFunction(functionList, "vec4", "shadow2DProjLod", Resources.Shadow2DProjLodDoc, Parameter.Create("sampler2DShadow", "sampler"), Parameter.Create("vec4", "coord"), Parameter.Create("float", "lod"));

			// Atomic-Counter
			this.AddFunction(functionList, "uint", "atomicCounterIncrement", Resources.AtomicCounterIncrementDoc, Parameter.Create("atomic_uint", "c"));
			this.AddFunction(functionList, "uint", "atomicCounterDecrement", Resources.AtomicCounterDecrementDoc, Parameter.Create("atomic_uint", "c"));
			this.AddFunction(functionList, "uint", "atomicCounter", Resources.AtomicCounterDoc, Parameter.Create("atomic_uint", "c"));

			// Atomic Memory
			this.AddFunction(functionList, "uint", "atomicAdd", Resources.AtomicAddDoc, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicAdd", Resources.AtomicAddDoc, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicMin", Resources.AtomicMinDoc, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicMin", Resources.AtomicMinDoc, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicMax", Resources.AtomicMaxDoc, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicMax", Resources.AtomicMaxDoc, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicAnd", Resources.AtomicAndDoc, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicAnd", Resources.AtomicAndDoc, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicOr", Resources.AtomicOrDoc, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicOr", Resources.AtomicOrDoc, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicXor", Resources.AtomicXorDoc, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicXor", Resources.AtomicXorDoc, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicExchange", Resources.AtomicExchangeDoc, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicExchange", Resources.AtomicExchangeDoc, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "atomicCompSwap", Resources.AtomicCompSwapDoc, Parameter.Create("inout", "uint", "mem"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "atomicCompSwap", Resources.AtomicCompSwapDoc, Parameter.Create("inout", "int", "mem"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));

			// Image
			this.AddFunction(functionList, "int", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimage1D", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimage2D", "image"));
			this.AddFunction(functionList, "ivec3", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimage3D", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimageCube", "image"));
			this.AddFunction(functionList, "ivec3", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimageCubeArray", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimageRect", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimage1DArray", "image"));
			this.AddFunction(functionList, "ivec3", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimage2DArray", "image"));
			this.AddFunction(functionList, "int", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimageBuffer", "image"));
			this.AddFunction(functionList, "ivec2", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimage2DMS", "image"));
			this.AddFunction(functionList, "ivec3", "imageSize", Resources.ImageSizeDoc, Parameter.Create("readonly writeonly", "gimage2DMSArray", "image"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimage1D", "image"), Parameter.Create("int", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimage2D", "image"), Parameter.Create("ivec2", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimage3D", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimage2DRect", "image"), Parameter.Create("ivec2", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimageCube", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimageBuffer", "image"), Parameter.Create("int", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimage1DArray", "image"), Parameter.Create("ivec2", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimage2DArray", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimageCubeArray", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "gvec4", "imageLoad", Resources.ImageLoadDoc, Parameter.Create("readonly", "gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimage3D", "image"), Parameter.Create("ivec3", "P"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "gvec4", "imageStore", Resources.ImageStoreDoc, Parameter.Create("writeonly", "gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("gvec4", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("uint", "compare"), Parameter.Create("uint", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage1D", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2D", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage3D", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2DRect", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimageCube", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimageBuffer", "image"), Parameter.Create("int", "P"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage1DArray", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2DArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimageCubeArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2DMS", "image"), Parameter.Create("ivec2", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));
			this.AddFunction(functionList, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, Parameter.Create("gimage2DMSArray", "image"), Parameter.Create("ivec3", "P"), Parameter.Create("int", "sample"), Parameter.Create("int", "compare"), Parameter.Create("int", "data"));

			// Fragment Processing
			// Derivative
			this.AddFunction(functionList, "genType", "dFdx", Resources.DFdxDoc, Parameter.Create("genType", "p"));
			this.AddFunction(functionList, "genType", "dFdy", Resources.DFdyDoc, Parameter.Create("genType", "p"));
			this.AddFunction(functionList, "genType", "fwidth", Resources.FwidthDoc, Parameter.Create("genType", "p"));

			// Interpolation
			this.AddFunction(functionList, "float", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, Parameter.Create("float", "interpolant"));
			this.AddFunction(functionList, "vec2", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, Parameter.Create("vec2", "interpolant"));
			this.AddFunction(functionList, "vec3", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, Parameter.Create("vec3", "interpolant"));
			this.AddFunction(functionList, "vec4", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, Parameter.Create("vec4", "interpolant"));

			this.AddFunction(functionList, "float", "interpolateAtSample", Resources.InterpolateAtSampleDoc, Parameter.Create("float", "interpolant"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "vec2", "interpolateAtSample", Resources.InterpolateAtSampleDoc, Parameter.Create("vec2", "interpolant"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "vec3", "interpolateAtSample", Resources.InterpolateAtSampleDoc, Parameter.Create("vec3", "interpolant"), Parameter.Create("int", "sample"));
			this.AddFunction(functionList, "vec4", "interpolateAtSample", Resources.InterpolateAtSampleDoc, Parameter.Create("vec4", "interpolant"), Parameter.Create("int", "sample"));

			this.AddFunction(functionList, "float", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, Parameter.Create("float", "interpolant"), Parameter.Create("vec2", "offset"));
			this.AddFunction(functionList, "vec2", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, Parameter.Create("vec2", "interpolant"), Parameter.Create("vec2", "offset"));
			this.AddFunction(functionList, "vec3", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, Parameter.Create("vec3", "interpolant"), Parameter.Create("vec2", "offset"));
			this.AddFunction(functionList, "vec4", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, Parameter.Create("vec4", "interpolant"), Parameter.Create("vec2", "offset"));

			// Noise
			this.AddFunction(functionList, "float", "noise1", Resources.Noise1Doc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "vec2", "noise2", Resources.Noise2Doc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "vec3", "noise3", Resources.Noise3Doc, Parameter.Create("genType", "x"));
			this.AddFunction(functionList, "vec4", "noise4", Resources.Noise4Doc, Parameter.Create("genType", "x"));

			// Geometry Shader
			this.AddFunction(functionList, "void", "EmitStreamVertex", Resources.EmitStreamVertexDoc, Parameter.Create("int", "stream"));
			this.AddFunction(functionList, "void", "EmitStreamPrimitive", Resources.EmitStreamPrimitiveDoc, Parameter.Create("int", "stream"));
			this.AddFunction(functionList, "void", "EmitVertex", Resources.EmitVertexDoc);
			this.AddFunction(functionList, "void", "EmitPrimitive", Resources.EmitPrimitiveDoc);

			// Shader Invocation Control
			this.AddFunction(functionList, "void", "barrier", Resources.BarrierDoc);

			// Shader Memory Control
			this.AddFunction(functionList, "void", "memoryBarrier", Resources.MemoryBarrierDoc);
			this.AddFunction(functionList, "void", "memoryBarrierAtomicCounter", Resources.MemoryBarrierAtomicCounterDoc);
			this.AddFunction(functionList, "void", "memoryBarrierBuffer", Resources.MemoryBarrierBufferDoc);
			this.AddFunction(functionList, "void", "memoryBarrierShared", Resources.MemoryBarrierSharedDoc);
			this.AddFunction(functionList, "void", "memoryBarrierImage", Resources.MemoryBarrierImageDoc);
			this.AddFunction(functionList, "void", "groupMemoryBarrier", Resources.GroupMemoryBarrierDoc);

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