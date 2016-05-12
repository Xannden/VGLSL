using System;
using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Properties;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Semantics.Definitions.Base;

namespace Xannden.GLSL.BuiltIn
{
	// Be careful when changing things in this class because it defines all of the built-in functions in GLSL and you don't want to accidentally change or remove some of them
	public sealed class BuiltInData
	{
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

		public IReadOnlyDictionary<string, IReadOnlyList<Definition>> Definitions { get; private set; }

		internal static Dictionary<string, string[]> GenTypes { get; }

		internal void LoadData()
		{
			Dictionary<string, List<Definition>> dictionary = new Dictionary<string, List<Definition>>(StringComparer.Ordinal);

			this.LoadFunctions(dictionary);
			this.LoadVariables();

			SortedDictionary<string, IReadOnlyList<Definition>> temp = new SortedDictionary<string, IReadOnlyList<Definition>>(StringComparer.Ordinal);

			foreach (KeyValuePair<string, List<Definition>> item in dictionary)
			{
				temp.Add(item.Key, item.Value);
			}

			this.Definitions = temp;
		}

		private void LoadFunctions(Dictionary<string, List<Definition>> dictionary)
		{
			// Angle and Trigonometry
			this.AddFunction(dictionary, "genType", "radians", Resources.RadiansDoc, ParameterDefinition.Create("genType", "degrees"));
			this.AddFunction(dictionary, "genType", "degrees", Resources.DegreesDoc, ParameterDefinition.Create("genType", "radians"));
			this.AddFunction(dictionary, "genType", "sin", Resources.SinDoc, ParameterDefinition.Create("genType", "angle"));
			this.AddFunction(dictionary, "genType", "cos", Resources.CosDoc, ParameterDefinition.Create("genType", "angle"));
			this.AddFunction(dictionary, "genType", "tan", Resources.TanDoc, ParameterDefinition.Create("genType", "angle"));
			this.AddFunction(dictionary, "genType", "asin", Resources.AsinDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "acos", Resources.AcosDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "atan", Resources.AtanDoc, ParameterDefinition.Create("genType", "y"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "atan", Resources.AtanDoc, ParameterDefinition.Create("genType", "y_over_x"));
			this.AddFunction(dictionary, "genType", "sinh", Resources.SinhDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "cosh", Resources.CoshDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "tanh", Resources.TanhDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "asinh", Resources.AsinhDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "acosh", Resources.AcoshDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "atanh", Resources.AtanhDoc, ParameterDefinition.Create("genType", "x"));

			// Exponential
			this.AddFunction(dictionary, "genType", "pow", Resources.PowDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "genType", "exp", Resources.ExpDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "log", Resources.LogDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "exp2", Resources.Exp2Doc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "log2", Resources.Log2Doc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "sqrt", Resources.SqrtDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "sqrt", Resources.SqrtDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "inversesqrt", Resources.InversesqrtDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "inversesqrt", Resources.InversesqrtDoc, ParameterDefinition.Create("genDType", "x"));

			// Common
			this.AddFunction(dictionary, "genType", "abs", Resources.AbsDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genIType", "abs", Resources.AbsDoc, ParameterDefinition.Create("genIType", "x"));
			this.AddFunction(dictionary, "genDType", "abs", Resources.AbsDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "sign", Resources.SignDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genIType", "sign", Resources.SignDoc, ParameterDefinition.Create("genIType", "x"));
			this.AddFunction(dictionary, "genDType", "sign", Resources.SignDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "floor", Resources.FloorDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "floor", Resources.FloorDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "trunc", Resources.TruncDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "trunc", Resources.TruncDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "round", Resources.RoundDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "round", Resources.RoundDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "roundEven", Resources.RoundEvenDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "roundEven", Resources.RoundEvenDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "ceil", Resources.CeilDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "ceil", Resources.CeilDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "fract", Resources.FractDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "fract", Resources.FractDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "mod", Resources.ModDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("float", "y"));
			this.AddFunction(dictionary, "genType", "mod", Resources.ModDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "genDType", "mod", Resources.ModDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("double", "y"));
			this.AddFunction(dictionary, "genDType", "mod", Resources.ModDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"));
			this.AddFunction(dictionary, "genType", "modf", Resources.ModfDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("out", "genType", "i"));
			this.AddFunction(dictionary, "genDType", "modf", Resources.ModfDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("out", "genDType", "i"));
			this.AddFunction(dictionary, "genType", "min", Resources.MinDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "genType", "min", Resources.MinDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("float", "y"));
			this.AddFunction(dictionary, "genDType", "min", Resources.MinDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"));
			this.AddFunction(dictionary, "genDType", "min", Resources.MinDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("double", "y"));
			this.AddFunction(dictionary, "genIType", "min", Resources.MinDoc, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("genIType", "y"));
			this.AddFunction(dictionary, "genIType", "min", Resources.MinDoc, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("int", "y"));
			this.AddFunction(dictionary, "genUType", "min", Resources.MinDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"));
			this.AddFunction(dictionary, "genUType", "min", Resources.MinDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("uint", "y"));
			this.AddFunction(dictionary, "genType", "max", Resources.MaxDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "genType", "max", Resources.MaxDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("float", "y"));
			this.AddFunction(dictionary, "genDType", "max", Resources.MaxDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"));
			this.AddFunction(dictionary, "genDType", "max", Resources.MaxDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("double", "y"));
			this.AddFunction(dictionary, "genIType", "max", Resources.MaxDoc, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("genIType", "y"));
			this.AddFunction(dictionary, "genIType", "max", Resources.MaxDoc, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("int", "y"));
			this.AddFunction(dictionary, "genUType", "max", Resources.MaxDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"));
			this.AddFunction(dictionary, "genUType", "max", Resources.MaxDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("uint", "y"));
			this.AddFunction(dictionary, "genType", "clamp", Resources.ClampDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "minVal"), ParameterDefinition.Create("genType", "maxVal"));
			this.AddFunction(dictionary, "genType", "clamp", Resources.ClampDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("float", "minVal"), ParameterDefinition.Create("float", "maxVal"));
			this.AddFunction(dictionary, "genDType", "clamp", Resources.ClampDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "minVal"), ParameterDefinition.Create("genDType", "maxVal"));
			this.AddFunction(dictionary, "genDType", "clamp", Resources.ClampDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("double", "minVal"), ParameterDefinition.Create("double", "maxVal"));
			this.AddFunction(dictionary, "genIType", "clamp", Resources.ClampDoc, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("genIType", "minVal"), ParameterDefinition.Create("genIType", "maxVal"));
			this.AddFunction(dictionary, "genIType", "clamp", Resources.ClampDoc, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("int", "minVal"), ParameterDefinition.Create("float", "int"));
			this.AddFunction(dictionary, "genUType", "clamp", Resources.ClampDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "minVal"), ParameterDefinition.Create("genUType", "maxVal"));
			this.AddFunction(dictionary, "genUType", "clamp", Resources.ClampDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("uint", "minVal"), ParameterDefinition.Create("uint", "maxVal"));
			this.AddFunction(dictionary, "genType", "mix", Resources.MixDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"), ParameterDefinition.Create("genType", "a"));
			this.AddFunction(dictionary, "genType", "mix", Resources.MixDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"), ParameterDefinition.Create("float", "a"));
			this.AddFunction(dictionary, "genDType", "mix", Resources.MixDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"), ParameterDefinition.Create("genDType", "a"));
			this.AddFunction(dictionary, "genDType", "mix", Resources.MixDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"), ParameterDefinition.Create("double", "a"));
			this.AddFunction(dictionary, "genType", "mix", Resources.MixDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"), ParameterDefinition.Create("genBType", "a"));
			this.AddFunction(dictionary, "genDType", "mix", Resources.MixDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"), ParameterDefinition.Create("genBType", "a"));
			this.AddFunction(dictionary, "genType", "step", Resources.StepDoc, ParameterDefinition.Create("genType", "edge"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "step", Resources.StepDoc, ParameterDefinition.Create("float", "edge"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "step", Resources.StepDoc, ParameterDefinition.Create("genDType", "edge"), ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genDType", "step", Resources.StepDoc, ParameterDefinition.Create("double", "edge"), ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "smoothstep", Resources.SmoothstepDoc, ParameterDefinition.Create("genType", "edge0"), ParameterDefinition.Create("genType", "edge1"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "smoothstep", Resources.SmoothstepDoc, ParameterDefinition.Create("float", "edge0"), ParameterDefinition.Create("float", "edge1"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "smoothstep", Resources.SmoothstepDoc, ParameterDefinition.Create("genDType", "edge0"), ParameterDefinition.Create("genDType", "edge1"), ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genDType", "smoothstep", Resources.SmoothstepDoc, ParameterDefinition.Create("double", "edge0"), ParameterDefinition.Create("double", "edge1"), ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genBType", "isnan", Resources.IsnanDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genBType", "isnan", Resources.IsnanDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genBType", "isinf", Resources.IsinfDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genBType", "isinf", Resources.IsinfDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genIType", "floatBitsToInt", Resources.FloatBitsToIntDoc, ParameterDefinition.Create("genType", "value"));
			this.AddFunction(dictionary, "genUType", "floatBitsToUint", Resources.FloatBitsToUintDoc, ParameterDefinition.Create("genType", "value"));
			this.AddFunction(dictionary, "genType", "intBitsToFloat", Resources.IntBitsToFloatDoc, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genType", "uintBitsToFloat", Resources.UintBitsToFloatDoc, ParameterDefinition.Create("genUType", "value"));
			this.AddFunction(dictionary, "genType", "fma", Resources.FmaDoc, ParameterDefinition.Create("genType", "a"), ParameterDefinition.Create("genType", "b"), ParameterDefinition.Create("genType", "c"));
			this.AddFunction(dictionary, "genDType", "fma", Resources.FmaDoc, ParameterDefinition.Create("genDType", "a"), ParameterDefinition.Create("genDType", "b"), ParameterDefinition.Create("genDType", "c"));
			this.AddFunction(dictionary, "genType", "frexp", Resources.FrexpDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("out", "genIType", "exp"));
			this.AddFunction(dictionary, "genDType", "frexp", Resources.FrexpDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("out", "genIType", "exp"));
			this.AddFunction(dictionary, "genType", "ldexp", Resources.LdexpDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("in", "genIType", "exp"));
			this.AddFunction(dictionary, "genDType", "ldexp", Resources.LdexpDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("in", "genIType", "exp"));

			// Floating-Point Pack and Unpack
			this.AddFunction(dictionary, "uint", "packUnorm2x16", Resources.PackUnorm2x16Doc, ParameterDefinition.Create("vec2", "v"));
			this.AddFunction(dictionary, "uint", "packSnorm2x16", Resources.PackSnorm2x16Doc, ParameterDefinition.Create("vec2", "v"));
			this.AddFunction(dictionary, "uint", "packUnorm4x8", Resources.PackUnorm4x8Doc, ParameterDefinition.Create("vec4", "v"));
			this.AddFunction(dictionary, "uint", "packSnorm4x8", Resources.PackSnorm4x8Doc, ParameterDefinition.Create("vec4", "v"));
			this.AddFunction(dictionary, "vec2", "unpackUnorm2x16", Resources.UnpackUnorm2x16Doc, ParameterDefinition.Create("uint", "p"));
			this.AddFunction(dictionary, "vec2", "unpackSnorm2x16", Resources.UnpackSnorm2x16Doc, ParameterDefinition.Create("uint", "p"));
			this.AddFunction(dictionary, "vec4", "unpackUnorm4x8", Resources.UnpackUnorm4x8Doc, ParameterDefinition.Create("uint", "p"));
			this.AddFunction(dictionary, "vec4", "unpackSnorm4x8", Resources.UnpackSnorm4x8Doc, ParameterDefinition.Create("uint", "p"));
			this.AddFunction(dictionary, "double", "packDouble2x32", Resources.PackDouble2x32Doc, ParameterDefinition.Create("uvec2", "v"));
			this.AddFunction(dictionary, "uvec2", "unpackDouble2x32", Resources.UnpackDouble2x32Doc, ParameterDefinition.Create("double", "v"));
			this.AddFunction(dictionary, "uint", "packHalf2x16", Resources.PackHalf2x16Doc, ParameterDefinition.Create("vec2", "v"));
			this.AddFunction(dictionary, "vec2", "unpackHalf2x16", Resources.UnpackHalf2x16Doc, ParameterDefinition.Create("uint", "v"));

			// Geometric
			this.AddFunction(dictionary, "float", "length", Resources.LengthDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "double", "length", Resources.LengthDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "float", "distance", Resources.DistanceDoc, ParameterDefinition.Create("genType", "p0"), ParameterDefinition.Create("genType", "p1"));
			this.AddFunction(dictionary, "double", "distance", Resources.DistanceDoc, ParameterDefinition.Create("genDType", "p0"), ParameterDefinition.Create("genDType", "p1"));
			this.AddFunction(dictionary, "float", "dot", Resources.DotDoc, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "double", "dot", Resources.DotDoc, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"));
			this.AddFunction(dictionary, "vec3", "cross", Resources.CrossDoc, ParameterDefinition.Create("vec3", "x"), ParameterDefinition.Create("vec3", "y"));
			this.AddFunction(dictionary, "dvec3", "cross", Resources.CrossDoc, ParameterDefinition.Create("dvec3", "x"), ParameterDefinition.Create("dvec3", "y"));
			this.AddFunction(dictionary, "genType", "normalize", Resources.NormalizeDoc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "normalize", Resources.NormalizeDoc, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "vec4", "ftransform", Resources.FtransformDoc);
			this.AddFunction(dictionary, "genType", "faceforward", Resources.FaceforwardDoc, ParameterDefinition.Create("genType", "N"), ParameterDefinition.Create("genType", "I"), ParameterDefinition.Create("genType", "Nref"));
			this.AddFunction(dictionary, "genDType", "faceforward", Resources.FaceforwardDoc, ParameterDefinition.Create("genDType", "N"), ParameterDefinition.Create("genDType", "I"), ParameterDefinition.Create("genDType", "Nref"));
			this.AddFunction(dictionary, "genType", "reflect", Resources.ReflectDoc, ParameterDefinition.Create("genType", "I"), ParameterDefinition.Create("genType", "N"));
			this.AddFunction(dictionary, "genDType", "reflect", Resources.ReflectDoc, ParameterDefinition.Create("genDType", "I"), ParameterDefinition.Create("genDType", "N"));
			this.AddFunction(dictionary, "genType", "refract", Resources.RefractDoc, ParameterDefinition.Create("genType", "I"), ParameterDefinition.Create("genType", "N"), ParameterDefinition.Create("float", "eta"));
			this.AddFunction(dictionary, "genDType", "refract", Resources.RefractDoc, ParameterDefinition.Create("genDType", "I"), ParameterDefinition.Create("genDType", "N"), ParameterDefinition.Create("float", "eta"));

			// Matrix
			this.AddFunction(dictionary, "mat", "matrixCompMult", Resources.MatrixCompMultDoc, ParameterDefinition.Create("mat", "x"), ParameterDefinition.Create("mat", "y"));
			this.AddFunction(dictionary, "mat2", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec2", "c"), ParameterDefinition.Create("vec2", "r"));
			this.AddFunction(dictionary, "mat3", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec3", "c"), ParameterDefinition.Create("vec3", "r"));
			this.AddFunction(dictionary, "mat4", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec4", "c"), ParameterDefinition.Create("vec4", "r"));
			this.AddFunction(dictionary, "mat2x3", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec3", "c"), ParameterDefinition.Create("vec2", "r"));
			this.AddFunction(dictionary, "mat3x2", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec2", "c"), ParameterDefinition.Create("vec3", "r"));
			this.AddFunction(dictionary, "mat2x4", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec4", "c"), ParameterDefinition.Create("vec2", "r"));
			this.AddFunction(dictionary, "mat4x2", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec2", "c"), ParameterDefinition.Create("vec4", "r"));
			this.AddFunction(dictionary, "mat3x4", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec4", "c"), ParameterDefinition.Create("vec3", "r"));
			this.AddFunction(dictionary, "mat4x3", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("vec3", "c"), ParameterDefinition.Create("vec4", "r"));
			this.AddFunction(dictionary, "mat2", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat2", "m"));
			this.AddFunction(dictionary, "mat3", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat3", "m"));
			this.AddFunction(dictionary, "mat4", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat4", "m"));
			this.AddFunction(dictionary, "mat2x3", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat3x2", "m"));
			this.AddFunction(dictionary, "mat3x2", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat2x3", "m"));
			this.AddFunction(dictionary, "mat2x4", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat4x2", "m"));
			this.AddFunction(dictionary, "mat4x2", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat2x4", "m"));
			this.AddFunction(dictionary, "mat3x4", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat4x3", "m"));
			this.AddFunction(dictionary, "mat4x3", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("mat3x4", "m"));
			this.AddFunction(dictionary, "float", "determinant", Resources.DeterminantDoc, ParameterDefinition.Create("mat2", "m"));
			this.AddFunction(dictionary, "float", "determinant", Resources.DeterminantDoc, ParameterDefinition.Create("mat3", "m"));
			this.AddFunction(dictionary, "float", "determinant", Resources.DeterminantDoc, ParameterDefinition.Create("mat4", "m"));
			this.AddFunction(dictionary, "mat2", "inverse", Resources.InverseDoc, ParameterDefinition.Create("mat2", "m"));
			this.AddFunction(dictionary, "mat3", "inverse", Resources.InverseDoc, ParameterDefinition.Create("mat3", "m"));
			this.AddFunction(dictionary, "mat4", "inverse", Resources.InverseDoc, ParameterDefinition.Create("mat4", "m"));
			this.AddFunction(dictionary, "dmat", "matrixCompMult", Resources.MatrixCompMultDoc, ParameterDefinition.Create("dmat", "x"), ParameterDefinition.Create("dmat", "y"));
			this.AddFunction(dictionary, "dmat2", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec2", "c"), ParameterDefinition.Create("dvec2", "r"));
			this.AddFunction(dictionary, "dmat3", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec3", "c"), ParameterDefinition.Create("dvec3", "r"));
			this.AddFunction(dictionary, "dmat4", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec4", "c"), ParameterDefinition.Create("dvec4", "r"));
			this.AddFunction(dictionary, "dmat2x3", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec3", "c"), ParameterDefinition.Create("dvec2", "r"));
			this.AddFunction(dictionary, "dmat3x2", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec2", "c"), ParameterDefinition.Create("dvec3", "r"));
			this.AddFunction(dictionary, "dmat2x4", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec4", "c"), ParameterDefinition.Create("dvec2", "r"));
			this.AddFunction(dictionary, "dmat4x2", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec2", "c"), ParameterDefinition.Create("dvec4", "r"));
			this.AddFunction(dictionary, "dmat3x4", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec4", "c"), ParameterDefinition.Create("dvec3", "r"));
			this.AddFunction(dictionary, "dmat4x3", "outerProduct", Resources.OuterProductDoc, ParameterDefinition.Create("dvec3", "c"), ParameterDefinition.Create("dvec4", "r"));
			this.AddFunction(dictionary, "dmat2", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat2", "m"));
			this.AddFunction(dictionary, "dmat3", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat3", "m"));
			this.AddFunction(dictionary, "dmat4", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat4", "m"));
			this.AddFunction(dictionary, "dmat2x3", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat3x2", "m"));
			this.AddFunction(dictionary, "dmat3x2", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat2x3", "m"));
			this.AddFunction(dictionary, "dmat2x4", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat4x2", "m"));
			this.AddFunction(dictionary, "dmat4x2", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat2x4", "m"));
			this.AddFunction(dictionary, "dmat3x4", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat4x3", "m"));
			this.AddFunction(dictionary, "dmat4x3", "transpose", Resources.TransposeDoc, ParameterDefinition.Create("dmat3x4", "m"));
			this.AddFunction(dictionary, "double", "determinant", Resources.DeterminantDoc, ParameterDefinition.Create("dmat2", "m"));
			this.AddFunction(dictionary, "double", "determinant", Resources.DeterminantDoc, ParameterDefinition.Create("dmat3", "m"));
			this.AddFunction(dictionary, "double", "determinant", Resources.DeterminantDoc, ParameterDefinition.Create("dmat4", "m"));
			this.AddFunction(dictionary, "dmat2", "inverse", Resources.InverseDoc, ParameterDefinition.Create("dmat2", "m"));
			this.AddFunction(dictionary, "dmat3", "inverse", Resources.InverseDoc, ParameterDefinition.Create("dmat3", "m"));
			this.AddFunction(dictionary, "dmat4", "inverse", Resources.InverseDoc, ParameterDefinition.Create("dmat4", "m"));

			// Vector Relational
			this.AddFunction(dictionary, "bvec", "lessThan", Resources.LessThanDoc, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThan", Resources.LessThanDoc, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThan", Resources.LessThanDoc, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThan", Resources.LessThanDoc, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThan", Resources.GreaterThanDoc, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThan", Resources.GreaterThanDoc, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThan", Resources.GreaterThanDoc, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThan", Resources.GreaterThanDoc, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ParameterDefinition.Create("bvec", "x"), ParameterDefinition.Create("bvec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ParameterDefinition.Create("bvec", "x"), ParameterDefinition.Create("bvec", "y"));
			this.AddFunction(dictionary, "bool", "any", Resources.AnyDoc, ParameterDefinition.Create("bvec", "x"));
			this.AddFunction(dictionary, "bool", "all", Resources.AllDoc, ParameterDefinition.Create("bvec", "x"));
			this.AddFunction(dictionary, "bvec", "not", Resources.NotDoc, ParameterDefinition.Create("bvec", "x"));

			// Integer
			this.AddFunction(dictionary, "genUType", "uaddCarry", Resources.UaddCarryDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"), ParameterDefinition.Create("out", "genUType", "carry"));
			this.AddFunction(dictionary, "genUType", "usubBorrow", Resources.UsubBorrowDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"), ParameterDefinition.Create("out", "genUType", "borrow"));
			this.AddFunction(dictionary, "void", "umulExtended", Resources.UmulExtendedDoc, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"), ParameterDefinition.Create("out", "genUType", "msb"), ParameterDefinition.Create("out", "genUType", "lsb"));
			this.AddFunction(dictionary, "void", "imulExtended", Resources.ImulExtendedDoc, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("genIType", "y"), ParameterDefinition.Create("out", "genIType", "msb"), ParameterDefinition.Create("out", "genIType", "lsb"));
			this.AddFunction(dictionary, "genIType", "bitfieldExtract", Resources.BitfieldExtractDoc, ParameterDefinition.Create("genIType", "value"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("int", "bits"));
			this.AddFunction(dictionary, "genUType", "bitfieldExtract", Resources.BitfieldExtractDoc, ParameterDefinition.Create("genUType", "value"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("int", "bits"));
			this.AddFunction(dictionary, "genIType", "bitfieldInsert", Resources.BitfieldInsertDoc, ParameterDefinition.Create("genIType", "base"), ParameterDefinition.Create("genIType", "insert"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("int", "bits"));
			this.AddFunction(dictionary, "genUType", "bitfieldInsert", Resources.BitfieldInsertDoc, ParameterDefinition.Create("genUType", "value"), ParameterDefinition.Create("genUType", "insert"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("int", "bits"));
			this.AddFunction(dictionary, "genIType", "bitfieldReverse", Resources.BitfieldReverseDoc, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genUType", "bitfieldReverse", Resources.BitfieldReverseDoc, ParameterDefinition.Create("genUType", "value"));
			this.AddFunction(dictionary, "genIType", "bitCount", Resources.BitCountDoc, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genIType", "bitCount", Resources.BitCountDoc, ParameterDefinition.Create("genUType", "value"));
			this.AddFunction(dictionary, "genIType", "findLSB", Resources.FindLSBDoc, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genIType", "findLSB", Resources.FindLSBDoc, ParameterDefinition.Create("genUType", "value"));
			this.AddFunction(dictionary, "genIType", "findMSB", Resources.FindMSBDoc, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genIType", "findMSB", Resources.FindMSBDoc, ParameterDefinition.Create("genUType", "value"));

			// Texture Query
			this.AddFunction(dictionary, "int", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "int", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "int", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsamplerBuffer", "sampler"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsampler2DMS", "sampler"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ParameterDefinition.Create("gsampler2DMSArray", "sampler"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("float", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("float", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("float", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("gsampler1D", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("gsampler2D", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("gsampler3D", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("gsamplerCube", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("gsamplerCubeArray", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("samplerCubeShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"));

			// Texture Lookup
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ParameterDefinition.Create("gsamplerCubeArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "compare"));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec4", "P"));
			this.AddFunction(dictionary, "float", "textureProj", Resources.TextureProjDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec4", "P"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec3", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("ivec2", "P"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsamplerBuffer", "sampler"), ParameterDefinition.Create("int", "P"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsampler2DMS", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ParameterDefinition.Create("gsampler2DMSArray", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec3", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureProjLod", Resources.TextureProjLodDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureProjLod", Resources.TextureProjLodDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "float", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "float", "textureProjGrad", Resources.TextureProjGradDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));

			// Texture Gather
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offsets", 4), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offsets", 4), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offsets", 4), ParameterDefinition.Create("int", "comp", true));
			this.AddFunction(dictionary, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offsets", 4));
			this.AddFunction(dictionary, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offsets", 4));
			this.AddFunction(dictionary, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offset", 4));

			// Compatibility Profile Texture
			this.AddFunction(dictionary, "vec4", "texture1D", Resources.Texture1DDoc, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("float", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "texture1DProj", Resources.Texture1DProjDoc, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("vec2", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "texture1DProj", Resources.Texture1DProjDoc, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "texture1DLod", Resources.Texture1DLodDoc, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("float", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture1DProjLod", Resources.Texture1DProjLodDoc, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("vec2", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture1DProjLod", Resources.Texture1DProjLodDoc, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture2D", Resources.Texture2DDoc, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec2", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "texture2DProj", Resources.Texture2DProjDoc, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "texture2DProj", Resources.Texture2DProjDoc, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "texture2DLod", Resources.Texture2DLodDoc, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec2", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture2DProjLod", Resources.Texture2DProjLodDoc, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture2DProjLod", Resources.Texture2DProjLodDoc, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture3D", Resources.Texture3DDoc, ParameterDefinition.Create("sampler3D", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "texture3DProj", Resources.Texture3DProjDoc, ParameterDefinition.Create("sampler3D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "texture3DLod", Resources.Texture3DLodDoc, ParameterDefinition.Create("sampler3D", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture3DProjLod", Resources.Texture3DProjLodDoc, ParameterDefinition.Create("sampler3D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "textureCube", Resources.TextureCubeDoc, ParameterDefinition.Create("samplerCube", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "textureCubeLod", Resources.TextureCubeLodDoc, ParameterDefinition.Create("samplerCube", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "shadow1D", Resources.Shadow1DDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "shadow2D", Resources.Shadow2DDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "shadow1DProj", Resources.Shadow1DProjDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "shadow2DProj", Resources.Shadow2DProjDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true));
			this.AddFunction(dictionary, "vec4", "shadow1DLod", Resources.Shadow1DLodDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "shadow2DLod", Resources.Shadow2DLodDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "shadow1DProjLod", Resources.Shadow1DProjLodDoc, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "shadow2DProjLod", Resources.Shadow2DProjLodDoc, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));

			// Atomic-Counter
			this.AddFunction(dictionary, "uint", "atomicCounterIncrement", Resources.AtomicCounterIncrementDoc, ParameterDefinition.Create("atomic_uint", "c"));
			this.AddFunction(dictionary, "uint", "atomicCounterDecrement", Resources.AtomicCounterDecrementDoc, ParameterDefinition.Create("atomic_uint", "c"));
			this.AddFunction(dictionary, "uint", "atomicCounter", Resources.AtomicCounterDoc, ParameterDefinition.Create("atomic_uint", "c"));

			// Atomic Memory
			this.AddFunction(dictionary, "uint", "atomicAdd", Resources.AtomicAddDoc, ParameterDefinition.Create("inout", "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicAdd", Resources.AtomicAddDoc, ParameterDefinition.Create("inout", "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicMin", Resources.AtomicMinDoc, ParameterDefinition.Create("inout", "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicMin", Resources.AtomicMinDoc, ParameterDefinition.Create("inout", "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicMax", Resources.AtomicMaxDoc, ParameterDefinition.Create("inout", "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicMax", Resources.AtomicMaxDoc, ParameterDefinition.Create("inout", "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicAnd", Resources.AtomicAndDoc, ParameterDefinition.Create("inout", "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicAnd", Resources.AtomicAndDoc, ParameterDefinition.Create("inout", "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicOr", Resources.AtomicOrDoc, ParameterDefinition.Create("inout", "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicOr", Resources.AtomicOrDoc, ParameterDefinition.Create("inout", "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicXor", Resources.AtomicXorDoc, ParameterDefinition.Create("inout", "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicXor", Resources.AtomicXorDoc, ParameterDefinition.Create("inout", "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicExchange", Resources.AtomicExchangeDoc, ParameterDefinition.Create("inout", "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicExchange", Resources.AtomicExchangeDoc, ParameterDefinition.Create("inout", "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicCompSwap", Resources.AtomicCompSwapDoc, ParameterDefinition.Create("inout", "uint", "mem"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicCompSwap", Resources.AtomicCompSwapDoc, ParameterDefinition.Create("inout", "int", "mem"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));

			// Image
			this.AddFunction(dictionary, "int", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimage1D", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimage2D", "image"));
			this.AddFunction(dictionary, "ivec3", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimage3D", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimageCube", "image"));
			this.AddFunction(dictionary, "ivec3", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimageCubeArray", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimageRect", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimage1DArray", "image"));
			this.AddFunction(dictionary, "ivec3", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimage2DArray", "image"));
			this.AddFunction(dictionary, "int", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimageBuffer", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimage2DMS", "image"));
			this.AddFunction(dictionary, "ivec3", "imageSize", Resources.ImageSizeDoc, ParameterDefinition.Create("readonly writeonly", "gimage2DMSArray", "image"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimage1D", "image"), ParameterDefinition.Create("int", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimageBuffer", "image"), ParameterDefinition.Create("int", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ParameterDefinition.Create("readonly", "gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ParameterDefinition.Create("writeonly", "gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));

			// Fragment Processing
			// Derivative
			this.AddFunction(dictionary, "genType", "dFdx", Resources.DFdxDoc, ParameterDefinition.Create("genType", "p"));
			this.AddFunction(dictionary, "genType", "dFdy", Resources.DFdyDoc, ParameterDefinition.Create("genType", "p"));
			this.AddFunction(dictionary, "genType", "fwidth", Resources.FwidthDoc, ParameterDefinition.Create("genType", "p"));

			// Interpolation
			this.AddFunction(dictionary, "float", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, ParameterDefinition.Create("float", "interpolant"));
			this.AddFunction(dictionary, "vec2", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, ParameterDefinition.Create("vec2", "interpolant"));
			this.AddFunction(dictionary, "vec3", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, ParameterDefinition.Create("vec3", "interpolant"));
			this.AddFunction(dictionary, "vec4", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, ParameterDefinition.Create("vec4", "interpolant"));

			this.AddFunction(dictionary, "float", "interpolateAtSample", Resources.InterpolateAtSampleDoc, ParameterDefinition.Create("float", "interpolant"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "vec2", "interpolateAtSample", Resources.InterpolateAtSampleDoc, ParameterDefinition.Create("vec2", "interpolant"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "vec3", "interpolateAtSample", Resources.InterpolateAtSampleDoc, ParameterDefinition.Create("vec3", "interpolant"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "vec4", "interpolateAtSample", Resources.InterpolateAtSampleDoc, ParameterDefinition.Create("vec4", "interpolant"), ParameterDefinition.Create("int", "sample"));

			this.AddFunction(dictionary, "float", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, ParameterDefinition.Create("float", "interpolant"), ParameterDefinition.Create("vec2", "offset"));
			this.AddFunction(dictionary, "vec2", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, ParameterDefinition.Create("vec2", "interpolant"), ParameterDefinition.Create("vec2", "offset"));
			this.AddFunction(dictionary, "vec3", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, ParameterDefinition.Create("vec3", "interpolant"), ParameterDefinition.Create("vec2", "offset"));
			this.AddFunction(dictionary, "vec4", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, ParameterDefinition.Create("vec4", "interpolant"), ParameterDefinition.Create("vec2", "offset"));

			// Noise
			this.AddFunction(dictionary, "float", "noise1", Resources.Noise1Doc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "vec2", "noise2", Resources.Noise2Doc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "vec3", "noise3", Resources.Noise3Doc, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "vec4", "noise4", Resources.Noise4Doc, ParameterDefinition.Create("genType", "x"));

			// Geometry Shader
			this.AddFunction(dictionary, "void", "EmitStreamVertex", Resources.EmitStreamVertexDoc, ParameterDefinition.Create("int", "stream"));
			this.AddFunction(dictionary, "void", "EmitStreamPrimitive", Resources.EmitStreamPrimitiveDoc, ParameterDefinition.Create("int", "stream"));
			this.AddFunction(dictionary, "void", "EmitVertex", Resources.EmitVertexDoc);
			this.AddFunction(dictionary, "void", "EmitPrimitive", Resources.EmitPrimitiveDoc);

			// Shader Invocation Control
			this.AddFunction(dictionary, "void", "barrier", Resources.BarrierDoc);

			// Shader Memory Control
			this.AddFunction(dictionary, "void", "memoryBarrier", Resources.MemoryBarrierDoc);
			this.AddFunction(dictionary, "void", "memoryBarrierAtomicCounter", Resources.MemoryBarrierAtomicCounterDoc);
			this.AddFunction(dictionary, "void", "memoryBarrierBuffer", Resources.MemoryBarrierBufferDoc);
			this.AddFunction(dictionary, "void", "memoryBarrierShared", Resources.MemoryBarrierSharedDoc);
			this.AddFunction(dictionary, "void", "memoryBarrierImage", Resources.MemoryBarrierImageDoc);
			this.AddFunction(dictionary, "void", "groupMemoryBarrier", Resources.GroupMemoryBarrierDoc);
		}

		private void LoadVariables()
		{
		}

		private void AddFunction(Dictionary<string, List<Definition>> dictionary, string returnType, string name, string documentation, params ParameterDefinition[][] parameters)
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
				List<ParameterDefinition> paramList = new List<ParameterDefinition>(parameters.Length);

				for (int i = 0; i < parameters.Length; i++)
				{
					paramList.Add(parameters[i][0]);
				}

				this.AddToDic(dictionary, new FunctionDefinition(new TypeDefinition(returnType), name, paramList, documentation, Scope.Global, null));
			}
			else
			{
				for (int i = 0; i < overloads; i++)
				{
					List<ParameterDefinition> paramList = new List<ParameterDefinition>(parameters.Length);

					for (int j = 0; j < parameters.Length; j++)
					{
						paramList.Add(parameters[j][i % parameters[j].Length]);
					}

					this.AddToDic(dictionary, new FunctionDefinition(new TypeDefinition(returnTypes[i % returnTypes.Length]), name, paramList, documentation, Scope.Global, null));
				}
			}
		}

		private void AddToDic(Dictionary<string, List<Definition>> dictionary, Definition definition)
		{
			if (dictionary.ContainsKey(definition.Name.Text))
			{
				if (!dictionary[definition.Name.Text].Contains(def => def == definition))
				{
					dictionary[definition.Name.Text].Add(definition);
				}
			}
			else
			{
				dictionary.Add(definition.Name.Text, new List<Definition> { definition });
			}

			definition.Overloads = dictionary[definition.Name.Text];
		}
	}
}