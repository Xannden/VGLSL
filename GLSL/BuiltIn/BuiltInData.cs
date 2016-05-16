using System;
using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Properties;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Semantics.Definitions.Base;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Text;

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
				["gsamplerBuffer"] = new string[] { "samplerBuffer", "isamplerBuffer", "usamplerBuffer" },
				["gvec4"] = new string[] { "vec4", "ivec4", "uvec4" },
				["gimage1D"] = new string[] { "image1D", "iimage1D", "uimage1D" },
				["gimage2D"] = new string[] { "image2D", "iimage2D", "uimage2D" },
				["gimage3D"] = new string[] { "image3D", "iimage3D", "uimage3D" },
				["gimageCube"] = new string[] { "imageCube", "iimageCube", "uimageCube" },
				["gimageCubeArray"] = new string[] { "imageCubeArray", "iimageCubeArray", "uimageCubeArray" },
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
			this.LoadVariables(dictionary);

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
			this.AddFunction(dictionary, "genType", "radians", Resources.RadiansDoc, ShaderType.All, ParameterDefinition.Create("genType", "degrees"));
			this.AddFunction(dictionary, "genType", "degrees", Resources.DegreesDoc, ShaderType.All, ParameterDefinition.Create("genType", "radians"));
			this.AddFunction(dictionary, "genType", "sin", Resources.SinDoc, ShaderType.All, ParameterDefinition.Create("genType", "angle"));
			this.AddFunction(dictionary, "genType", "cos", Resources.CosDoc, ShaderType.All, ParameterDefinition.Create("genType", "angle"));
			this.AddFunction(dictionary, "genType", "tan", Resources.TanDoc, ShaderType.All, ParameterDefinition.Create("genType", "angle"));
			this.AddFunction(dictionary, "genType", "asin", Resources.AsinDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "acos", Resources.AcosDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "atan", Resources.AtanDoc, ShaderType.All, ParameterDefinition.Create("genType", "y"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "atan", Resources.AtanDoc, ShaderType.All, ParameterDefinition.Create("genType", "y_over_x"));
			this.AddFunction(dictionary, "genType", "sinh", Resources.SinhDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "cosh", Resources.CoshDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "tanh", Resources.TanhDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "asinh", Resources.AsinhDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "acosh", Resources.AcoshDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "atanh", Resources.AtanhDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));

			// Exponential
			this.AddFunction(dictionary, "genType", "pow", Resources.PowDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "genType", "exp", Resources.ExpDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "log", Resources.LogDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "exp2", Resources.Exp2Doc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "log2", Resources.Log2Doc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "sqrt", Resources.SqrtDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "sqrt", Resources.SqrtDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "inversesqrt", Resources.InversesqrtDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "inversesqrt", Resources.InversesqrtDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));

			// Common
			this.AddFunction(dictionary, "genType", "abs", Resources.AbsDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genIType", "abs", Resources.AbsDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"));
			this.AddFunction(dictionary, "genDType", "abs", Resources.AbsDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "sign", Resources.SignDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genIType", "sign", Resources.SignDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"));
			this.AddFunction(dictionary, "genDType", "sign", Resources.SignDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "floor", Resources.FloorDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "floor", Resources.FloorDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "trunc", Resources.TruncDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "trunc", Resources.TruncDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "round", Resources.RoundDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "round", Resources.RoundDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "roundEven", Resources.RoundEvenDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "roundEven", Resources.RoundEvenDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "ceil", Resources.CeilDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "ceil", Resources.CeilDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "fract", Resources.FractDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "fract", Resources.FractDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "mod", Resources.ModDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("float", "y"));
			this.AddFunction(dictionary, "genType", "mod", Resources.ModDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "genDType", "mod", Resources.ModDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("double", "y"));
			this.AddFunction(dictionary, "genDType", "mod", Resources.ModDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"));
			this.AddFunction(dictionary, "genType", "modf", Resources.ModfDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genType", "i"));
			this.AddFunction(dictionary, "genDType", "modf", Resources.ModfDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genDType", "i"));
			this.AddFunction(dictionary, "genType", "min", Resources.MinDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "genType", "min", Resources.MinDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("float", "y"));
			this.AddFunction(dictionary, "genDType", "min", Resources.MinDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"));
			this.AddFunction(dictionary, "genDType", "min", Resources.MinDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("double", "y"));
			this.AddFunction(dictionary, "genIType", "min", Resources.MinDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("genIType", "y"));
			this.AddFunction(dictionary, "genIType", "min", Resources.MinDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("int", "y"));
			this.AddFunction(dictionary, "genUType", "min", Resources.MinDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"));
			this.AddFunction(dictionary, "genUType", "min", Resources.MinDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("uint", "y"));
			this.AddFunction(dictionary, "genType", "max", Resources.MaxDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "genType", "max", Resources.MaxDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("float", "y"));
			this.AddFunction(dictionary, "genDType", "max", Resources.MaxDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"));
			this.AddFunction(dictionary, "genDType", "max", Resources.MaxDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("double", "y"));
			this.AddFunction(dictionary, "genIType", "max", Resources.MaxDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("genIType", "y"));
			this.AddFunction(dictionary, "genIType", "max", Resources.MaxDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("int", "y"));
			this.AddFunction(dictionary, "genUType", "max", Resources.MaxDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"));
			this.AddFunction(dictionary, "genUType", "max", Resources.MaxDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("uint", "y"));
			this.AddFunction(dictionary, "genType", "clamp", Resources.ClampDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "minVal"), ParameterDefinition.Create("genType", "maxVal"));
			this.AddFunction(dictionary, "genType", "clamp", Resources.ClampDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("float", "minVal"), ParameterDefinition.Create("float", "maxVal"));
			this.AddFunction(dictionary, "genDType", "clamp", Resources.ClampDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "minVal"), ParameterDefinition.Create("genDType", "maxVal"));
			this.AddFunction(dictionary, "genDType", "clamp", Resources.ClampDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("double", "minVal"), ParameterDefinition.Create("double", "maxVal"));
			this.AddFunction(dictionary, "genIType", "clamp", Resources.ClampDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("genIType", "minVal"), ParameterDefinition.Create("genIType", "maxVal"));
			this.AddFunction(dictionary, "genIType", "clamp", Resources.ClampDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("int", "minVal"), ParameterDefinition.Create("float", "int"));
			this.AddFunction(dictionary, "genUType", "clamp", Resources.ClampDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "minVal"), ParameterDefinition.Create("genUType", "maxVal"));
			this.AddFunction(dictionary, "genUType", "clamp", Resources.ClampDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("uint", "minVal"), ParameterDefinition.Create("uint", "maxVal"));
			this.AddFunction(dictionary, "genType", "mix", Resources.MixDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"), ParameterDefinition.Create("genType", "a"));
			this.AddFunction(dictionary, "genType", "mix", Resources.MixDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"), ParameterDefinition.Create("float", "a"));
			this.AddFunction(dictionary, "genDType", "mix", Resources.MixDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"), ParameterDefinition.Create("genDType", "a"));
			this.AddFunction(dictionary, "genDType", "mix", Resources.MixDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"), ParameterDefinition.Create("double", "a"));
			this.AddFunction(dictionary, "genType", "mix", Resources.MixDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"), ParameterDefinition.Create("genBType", "a"));
			this.AddFunction(dictionary, "genDType", "mix", Resources.MixDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"), ParameterDefinition.Create("genBType", "a"));
			this.AddFunction(dictionary, "genType", "step", Resources.StepDoc, ShaderType.All, ParameterDefinition.Create("genType", "edge"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "step", Resources.StepDoc, ShaderType.All, ParameterDefinition.Create("float", "edge"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "step", Resources.StepDoc, ShaderType.All, ParameterDefinition.Create("genDType", "edge"), ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genDType", "step", Resources.StepDoc, ShaderType.All, ParameterDefinition.Create("double", "edge"), ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genType", "smoothstep", Resources.SmoothstepDoc, ShaderType.All, ParameterDefinition.Create("genType", "edge0"), ParameterDefinition.Create("genType", "edge1"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genType", "smoothstep", Resources.SmoothstepDoc, ShaderType.All, ParameterDefinition.Create("float", "edge0"), ParameterDefinition.Create("float", "edge1"), ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "smoothstep", Resources.SmoothstepDoc, ShaderType.All, ParameterDefinition.Create("genDType", "edge0"), ParameterDefinition.Create("genDType", "edge1"), ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genDType", "smoothstep", Resources.SmoothstepDoc, ShaderType.All, ParameterDefinition.Create("double", "edge0"), ParameterDefinition.Create("double", "edge1"), ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genBType", "isnan", Resources.IsnanDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genBType", "isnan", Resources.IsnanDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genBType", "isinf", Resources.IsinfDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genBType", "isinf", Resources.IsinfDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "genIType", "floatBitsToInt", Resources.FloatBitsToIntDoc, ShaderType.All, ParameterDefinition.Create("genType", "value"));
			this.AddFunction(dictionary, "genUType", "floatBitsToUint", Resources.FloatBitsToUintDoc, ShaderType.All, ParameterDefinition.Create("genType", "value"));
			this.AddFunction(dictionary, "genType", "intBitsToFloat", Resources.IntBitsToFloatDoc, ShaderType.All, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genType", "uintBitsToFloat", Resources.UintBitsToFloatDoc, ShaderType.All, ParameterDefinition.Create("genUType", "value"));
			this.AddFunction(dictionary, "genType", "fma", Resources.FmaDoc, ShaderType.All, ParameterDefinition.Create("genType", "a"), ParameterDefinition.Create("genType", "b"), ParameterDefinition.Create("genType", "c"));
			this.AddFunction(dictionary, "genDType", "fma", Resources.FmaDoc, ShaderType.All, ParameterDefinition.Create("genDType", "a"), ParameterDefinition.Create("genDType", "b"), ParameterDefinition.Create("genDType", "c"));
			this.AddFunction(dictionary, "genType", "frexp", Resources.FrexpDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genIType", "exp"));
			this.AddFunction(dictionary, "genDType", "frexp", Resources.FrexpDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genIType", "exp"));
			this.AddFunction(dictionary, "genType", "ldexp", Resources.LdexpDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create(SyntaxType.InKeyword, "genIType", "exp"));
			this.AddFunction(dictionary, "genDType", "ldexp", Resources.LdexpDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create(SyntaxType.InKeyword, "genIType", "exp"));

			// Floating-Point Pack and Unpack
			this.AddFunction(dictionary, "uint", "packUnorm2x16", Resources.PackUnorm2x16Doc, ShaderType.All, ParameterDefinition.Create("vec2", "v"));
			this.AddFunction(dictionary, "uint", "packSnorm2x16", Resources.PackSnorm2x16Doc, ShaderType.All, ParameterDefinition.Create("vec2", "v"));
			this.AddFunction(dictionary, "uint", "packUnorm4x8", Resources.PackUnorm4x8Doc, ShaderType.All, ParameterDefinition.Create("vec4", "v"));
			this.AddFunction(dictionary, "uint", "packSnorm4x8", Resources.PackSnorm4x8Doc, ShaderType.All, ParameterDefinition.Create("vec4", "v"));
			this.AddFunction(dictionary, "vec2", "unpackUnorm2x16", Resources.UnpackUnorm2x16Doc, ShaderType.All, ParameterDefinition.Create("uint", "p"));
			this.AddFunction(dictionary, "vec2", "unpackSnorm2x16", Resources.UnpackSnorm2x16Doc, ShaderType.All, ParameterDefinition.Create("uint", "p"));
			this.AddFunction(dictionary, "vec4", "unpackUnorm4x8", Resources.UnpackUnorm4x8Doc, ShaderType.All, ParameterDefinition.Create("uint", "p"));
			this.AddFunction(dictionary, "vec4", "unpackSnorm4x8", Resources.UnpackSnorm4x8Doc, ShaderType.All, ParameterDefinition.Create("uint", "p"));
			this.AddFunction(dictionary, "double", "packDouble2x32", Resources.PackDouble2x32Doc, ShaderType.All, ParameterDefinition.Create("uvec2", "v"));
			this.AddFunction(dictionary, "uvec2", "unpackDouble2x32", Resources.UnpackDouble2x32Doc, ShaderType.All, ParameterDefinition.Create("double", "v"));
			this.AddFunction(dictionary, "uint", "packHalf2x16", Resources.PackHalf2x16Doc, ShaderType.All, ParameterDefinition.Create("vec2", "v"));
			this.AddFunction(dictionary, "vec2", "unpackHalf2x16", Resources.UnpackHalf2x16Doc, ShaderType.All, ParameterDefinition.Create("uint", "v"));

			// Geometric
			this.AddFunction(dictionary, "float", "length", Resources.LengthDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "double", "length", Resources.LengthDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "float", "distance", Resources.DistanceDoc, ShaderType.All, ParameterDefinition.Create("genType", "p0"), ParameterDefinition.Create("genType", "p1"));
			this.AddFunction(dictionary, "double", "distance", Resources.DistanceDoc, ShaderType.All, ParameterDefinition.Create("genDType", "p0"), ParameterDefinition.Create("genDType", "p1"));
			this.AddFunction(dictionary, "float", "dot", Resources.DotDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"), ParameterDefinition.Create("genType", "y"));
			this.AddFunction(dictionary, "double", "dot", Resources.DotDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"), ParameterDefinition.Create("genDType", "y"));
			this.AddFunction(dictionary, "vec3", "cross", Resources.CrossDoc, ShaderType.All, ParameterDefinition.Create("vec3", "x"), ParameterDefinition.Create("vec3", "y"));
			this.AddFunction(dictionary, "dvec3", "cross", Resources.CrossDoc, ShaderType.All, ParameterDefinition.Create("dvec3", "x"), ParameterDefinition.Create("dvec3", "y"));
			this.AddFunction(dictionary, "genType", "normalize", Resources.NormalizeDoc, ShaderType.All, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "genDType", "normalize", Resources.NormalizeDoc, ShaderType.All, ParameterDefinition.Create("genDType", "x"));
			this.AddFunction(dictionary, "vec4", "ftransform", Resources.FtransformDoc, ShaderType.All);
			this.AddFunction(dictionary, "genType", "faceforward", Resources.FaceforwardDoc, ShaderType.All, ParameterDefinition.Create("genType", "N"), ParameterDefinition.Create("genType", "I"), ParameterDefinition.Create("genType", "Nref"));
			this.AddFunction(dictionary, "genDType", "faceforward", Resources.FaceforwardDoc, ShaderType.All, ParameterDefinition.Create("genDType", "N"), ParameterDefinition.Create("genDType", "I"), ParameterDefinition.Create("genDType", "Nref"));
			this.AddFunction(dictionary, "genType", "reflect", Resources.ReflectDoc, ShaderType.All, ParameterDefinition.Create("genType", "I"), ParameterDefinition.Create("genType", "N"));
			this.AddFunction(dictionary, "genDType", "reflect", Resources.ReflectDoc, ShaderType.All, ParameterDefinition.Create("genDType", "I"), ParameterDefinition.Create("genDType", "N"));
			this.AddFunction(dictionary, "genType", "refract", Resources.RefractDoc, ShaderType.All, ParameterDefinition.Create("genType", "I"), ParameterDefinition.Create("genType", "N"), ParameterDefinition.Create("float", "eta"));
			this.AddFunction(dictionary, "genDType", "refract", Resources.RefractDoc, ShaderType.All, ParameterDefinition.Create("genDType", "I"), ParameterDefinition.Create("genDType", "N"), ParameterDefinition.Create("float", "eta"));

			// Matrix
			this.AddFunction(dictionary, "mat", "matrixCompMult", Resources.MatrixCompMultDoc, ShaderType.All, ParameterDefinition.Create("mat", "x"), ParameterDefinition.Create("mat", "y"));
			this.AddFunction(dictionary, "mat2", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec2", "c"), ParameterDefinition.Create("vec2", "r"));
			this.AddFunction(dictionary, "mat3", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec3", "c"), ParameterDefinition.Create("vec3", "r"));
			this.AddFunction(dictionary, "mat4", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec4", "c"), ParameterDefinition.Create("vec4", "r"));
			this.AddFunction(dictionary, "mat2x3", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec3", "c"), ParameterDefinition.Create("vec2", "r"));
			this.AddFunction(dictionary, "mat3x2", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec2", "c"), ParameterDefinition.Create("vec3", "r"));
			this.AddFunction(dictionary, "mat2x4", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec4", "c"), ParameterDefinition.Create("vec2", "r"));
			this.AddFunction(dictionary, "mat4x2", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec2", "c"), ParameterDefinition.Create("vec4", "r"));
			this.AddFunction(dictionary, "mat3x4", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec4", "c"), ParameterDefinition.Create("vec3", "r"));
			this.AddFunction(dictionary, "mat4x3", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("vec3", "c"), ParameterDefinition.Create("vec4", "r"));
			this.AddFunction(dictionary, "mat2", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat2", "m"));
			this.AddFunction(dictionary, "mat3", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat3", "m"));
			this.AddFunction(dictionary, "mat4", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat4", "m"));
			this.AddFunction(dictionary, "mat2x3", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat3x2", "m"));
			this.AddFunction(dictionary, "mat3x2", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat2x3", "m"));
			this.AddFunction(dictionary, "mat2x4", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat4x2", "m"));
			this.AddFunction(dictionary, "mat4x2", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat2x4", "m"));
			this.AddFunction(dictionary, "mat3x4", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat4x3", "m"));
			this.AddFunction(dictionary, "mat4x3", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("mat3x4", "m"));
			this.AddFunction(dictionary, "float", "determinant", Resources.DeterminantDoc, ShaderType.All, ParameterDefinition.Create("mat2", "m"));
			this.AddFunction(dictionary, "float", "determinant", Resources.DeterminantDoc, ShaderType.All, ParameterDefinition.Create("mat3", "m"));
			this.AddFunction(dictionary, "float", "determinant", Resources.DeterminantDoc, ShaderType.All, ParameterDefinition.Create("mat4", "m"));
			this.AddFunction(dictionary, "mat2", "inverse", Resources.InverseDoc, ShaderType.All, ParameterDefinition.Create("mat2", "m"));
			this.AddFunction(dictionary, "mat3", "inverse", Resources.InverseDoc, ShaderType.All, ParameterDefinition.Create("mat3", "m"));
			this.AddFunction(dictionary, "mat4", "inverse", Resources.InverseDoc, ShaderType.All, ParameterDefinition.Create("mat4", "m"));
			this.AddFunction(dictionary, "dmat", "matrixCompMult", Resources.MatrixCompMultDoc, ShaderType.All, ParameterDefinition.Create("dmat", "x"), ParameterDefinition.Create("dmat", "y"));
			this.AddFunction(dictionary, "dmat2", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec2", "c"), ParameterDefinition.Create("dvec2", "r"));
			this.AddFunction(dictionary, "dmat3", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec3", "c"), ParameterDefinition.Create("dvec3", "r"));
			this.AddFunction(dictionary, "dmat4", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec4", "c"), ParameterDefinition.Create("dvec4", "r"));
			this.AddFunction(dictionary, "dmat2x3", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec3", "c"), ParameterDefinition.Create("dvec2", "r"));
			this.AddFunction(dictionary, "dmat3x2", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec2", "c"), ParameterDefinition.Create("dvec3", "r"));
			this.AddFunction(dictionary, "dmat2x4", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec4", "c"), ParameterDefinition.Create("dvec2", "r"));
			this.AddFunction(dictionary, "dmat4x2", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec2", "c"), ParameterDefinition.Create("dvec4", "r"));
			this.AddFunction(dictionary, "dmat3x4", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec4", "c"), ParameterDefinition.Create("dvec3", "r"));
			this.AddFunction(dictionary, "dmat4x3", "outerProduct", Resources.OuterProductDoc, ShaderType.All, ParameterDefinition.Create("dvec3", "c"), ParameterDefinition.Create("dvec4", "r"));
			this.AddFunction(dictionary, "dmat2", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat2", "m"));
			this.AddFunction(dictionary, "dmat3", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat3", "m"));
			this.AddFunction(dictionary, "dmat4", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat4", "m"));
			this.AddFunction(dictionary, "dmat2x3", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat3x2", "m"));
			this.AddFunction(dictionary, "dmat3x2", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat2x3", "m"));
			this.AddFunction(dictionary, "dmat2x4", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat4x2", "m"));
			this.AddFunction(dictionary, "dmat4x2", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat2x4", "m"));
			this.AddFunction(dictionary, "dmat3x4", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat4x3", "m"));
			this.AddFunction(dictionary, "dmat4x3", "transpose", Resources.TransposeDoc, ShaderType.All, ParameterDefinition.Create("dmat3x4", "m"));
			this.AddFunction(dictionary, "double", "determinant", Resources.DeterminantDoc, ShaderType.All, ParameterDefinition.Create("dmat2", "m"));
			this.AddFunction(dictionary, "double", "determinant", Resources.DeterminantDoc, ShaderType.All, ParameterDefinition.Create("dmat3", "m"));
			this.AddFunction(dictionary, "double", "determinant", Resources.DeterminantDoc, ShaderType.All, ParameterDefinition.Create("dmat4", "m"));
			this.AddFunction(dictionary, "dmat2", "inverse", Resources.InverseDoc, ShaderType.All, ParameterDefinition.Create("dmat2", "m"));
			this.AddFunction(dictionary, "dmat3", "inverse", Resources.InverseDoc, ShaderType.All, ParameterDefinition.Create("dmat3", "m"));
			this.AddFunction(dictionary, "dmat4", "inverse", Resources.InverseDoc, ShaderType.All, ParameterDefinition.Create("dmat4", "m"));

			// Vector Relational
			this.AddFunction(dictionary, "bvec", "lessThan", Resources.LessThanDoc, ShaderType.All, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThan", Resources.LessThanDoc, ShaderType.All, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThan", Resources.LessThanDoc, ShaderType.All, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThan", Resources.LessThanDoc, ShaderType.All, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, ShaderType.All, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, ShaderType.All, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, ShaderType.All, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "lessThanEqual", Resources.LessThanEqualDoc, ShaderType.All, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThan", Resources.GreaterThanDoc, ShaderType.All, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThan", Resources.GreaterThanDoc, ShaderType.All, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThan", Resources.GreaterThanDoc, ShaderType.All, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThan", Resources.GreaterThanDoc, ShaderType.All, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, ShaderType.All, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, ShaderType.All, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, ShaderType.All, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "greaterThanEqual", Resources.GreaterThanEqualDoc, ShaderType.All, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ShaderType.All, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ShaderType.All, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ShaderType.All, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ShaderType.All, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "equal", Resources.EqualDoc, ShaderType.All, ParameterDefinition.Create("bvec", "x"), ParameterDefinition.Create("bvec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ShaderType.All, ParameterDefinition.Create("vec", "x"), ParameterDefinition.Create("vec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ShaderType.All, ParameterDefinition.Create("dvec", "x"), ParameterDefinition.Create("dvec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ShaderType.All, ParameterDefinition.Create("ivec", "x"), ParameterDefinition.Create("ivec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ShaderType.All, ParameterDefinition.Create("uvec", "x"), ParameterDefinition.Create("uvec", "y"));
			this.AddFunction(dictionary, "bvec", "notEqual", Resources.NotEqualDoc, ShaderType.All, ParameterDefinition.Create("bvec", "x"), ParameterDefinition.Create("bvec", "y"));
			this.AddFunction(dictionary, "bool", "any", Resources.AnyDoc, ShaderType.All, ParameterDefinition.Create("bvec", "x"));
			this.AddFunction(dictionary, "bool", "all", Resources.AllDoc, ShaderType.All, ParameterDefinition.Create("bvec", "x"));
			this.AddFunction(dictionary, "bvec", "not", Resources.NotDoc, ShaderType.All, ParameterDefinition.Create("bvec", "x"));

			// Integer
			this.AddFunction(dictionary, "genUType", "uaddCarry", Resources.UaddCarryDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genUType", "carry"));
			this.AddFunction(dictionary, "genUType", "usubBorrow", Resources.UsubBorrowDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genUType", "borrow"));
			this.AddFunction(dictionary, "void", "umulExtended", Resources.UmulExtendedDoc, ShaderType.All, ParameterDefinition.Create("genUType", "x"), ParameterDefinition.Create("genUType", "y"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genUType", "msb"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genUType", "lsb"));
			this.AddFunction(dictionary, "void", "imulExtended", Resources.ImulExtendedDoc, ShaderType.All, ParameterDefinition.Create("genIType", "x"), ParameterDefinition.Create("genIType", "y"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genIType", "msb"), ParameterDefinition.Create(SyntaxType.OutKeyword, "genIType", "lsb"));
			this.AddFunction(dictionary, "genIType", "bitfieldExtract", Resources.BitfieldExtractDoc, ShaderType.All, ParameterDefinition.Create("genIType", "value"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("int", "bits"));
			this.AddFunction(dictionary, "genUType", "bitfieldExtract", Resources.BitfieldExtractDoc, ShaderType.All, ParameterDefinition.Create("genUType", "value"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("int", "bits"));
			this.AddFunction(dictionary, "genIType", "bitfieldInsert", Resources.BitfieldInsertDoc, ShaderType.All, ParameterDefinition.Create("genIType", "base"), ParameterDefinition.Create("genIType", "insert"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("int", "bits"));
			this.AddFunction(dictionary, "genUType", "bitfieldInsert", Resources.BitfieldInsertDoc, ShaderType.All, ParameterDefinition.Create("genUType", "value"), ParameterDefinition.Create("genUType", "insert"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("int", "bits"));
			this.AddFunction(dictionary, "genIType", "bitfieldReverse", Resources.BitfieldReverseDoc, ShaderType.All, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genUType", "bitfieldReverse", Resources.BitfieldReverseDoc, ShaderType.All, ParameterDefinition.Create("genUType", "value"));
			this.AddFunction(dictionary, "genIType", "bitCount", Resources.BitCountDoc, ShaderType.All, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genIType", "bitCount", Resources.BitCountDoc, ShaderType.All, ParameterDefinition.Create("genUType", "value"));
			this.AddFunction(dictionary, "genIType", "findLSB", Resources.FindLSBDoc, ShaderType.All, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genIType", "findLSB", Resources.FindLSBDoc, ShaderType.All, ParameterDefinition.Create("genUType", "value"));
			this.AddFunction(dictionary, "genIType", "findMSB", Resources.FindMSBDoc, ShaderType.All, ParameterDefinition.Create("genIType", "value"));
			this.AddFunction(dictionary, "genIType", "findMSB", Resources.FindMSBDoc, ShaderType.All, ParameterDefinition.Create("genUType", "value"));

			// Texture Query
			this.AddFunction(dictionary, "int", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "int", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "int", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsamplerBuffer", "sampler"));
			this.AddFunction(dictionary, "ivec2", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DMS", "sampler"));
			this.AddFunction(dictionary, "ivec3", "textureSize", Resources.TextureSizeDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DMSArray", "sampler"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("float", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("float", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("float", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "vec2", "textureQueryLod", Resources.TextureQueryLodDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCube", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCubeArray", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"));
			this.AddFunction(dictionary, "int", "textureQueryLevels", Resources.TextureQueryLevelsDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"));

			// Texture Lookup
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"));
			this.AddFunction(dictionary, "gvec4", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "float", "texture", Resources.TextureDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "compare"));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec3", "P"));
			this.AddFunction(dictionary, "gvec4", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec4", "P"));
			this.AddFunction(dictionary, "float", "textureProj", Resources.TextureProjDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec4", "P"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureLod", Resources.TextureLodDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec3", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "textureOffset", Resources.TextureOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("ivec2", "P"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "lod"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsamplerBuffer", "sampler"), ParameterDefinition.Create("int", "P"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DMS", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "gvec4", "texelFetch", Resources.TexelFetchDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DMSArray", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "texelFetchOffset", Resources.TexelFetchOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec3", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("int", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "float", "textureProjOffset", Resources.TextureProjOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureLodOffset", Resources.TextureLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "float", "textureLodOffset", Resources.TextureLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLod", Resources.TextureProjLodDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureProjLod", Resources.TextureProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "float", "textureProjLod", Resources.TextureProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "float", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureProjLodOffset", Resources.TextureProjLodOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "lod"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "float", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGrad", Resources.TextureGradDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("float", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1DArray", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureGradOffset", Resources.TextureGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "float", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"));
			this.AddFunction(dictionary, "float", "textureProjGrad", Resources.TextureProjGradDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler1D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler3D", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec3", "dPdx"), ParameterDefinition.Create("vec3", "dPdy"), ParameterDefinition.Create("ivec3", "offset"));
			this.AddFunction(dictionary, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "dPdx"), ParameterDefinition.Create("float", "dPdy"), ParameterDefinition.Create("int", "offset"));
			this.AddFunction(dictionary, "float", "textureProjGradOffset", Resources.TextureProjGradOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("vec2", "dPdx"), ParameterDefinition.Create("vec2", "dPdy"), ParameterDefinition.Create("ivec2", "offset"));

			// Texture Gather
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCube", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("gsamplerCubeArray", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "gvec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("samplerCubeArrayShadow", "sampler"), ParameterDefinition.Create("vec4", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "vec4", "textureGather", Resources.TextureGatherDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offset"), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "vec4", "textureGatherOffset", Resources.TextureGatherOffsetDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offset"));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ShaderType.All, ParameterDefinition.Create("gsampler2D", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offsets", 4), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DArray", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("ivec2", "offsets", 4), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "gvec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ShaderType.All, ParameterDefinition.Create("gsampler2DRect", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("ivec2", "offsets", 4), ParameterDefinition.Create("int", "comp", true, ShaderType.All));
			this.AddFunction(dictionary, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offsets", 4));
			this.AddFunction(dictionary, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ShaderType.All, ParameterDefinition.Create("sampler2DArrayShadow", "sampler"), ParameterDefinition.Create("vec3", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offsets", 4));
			this.AddFunction(dictionary, "vec4", "textureGatherOffsets", Resources.TextureGatherOffsetsDoc, ShaderType.All, ParameterDefinition.Create("sampler2DRectShadow", "sampler"), ParameterDefinition.Create("vec2", "P"), ParameterDefinition.Create("float", "refZ"), ParameterDefinition.Create("ivec2", "offset", 4));

			// Compatibility Profile Texture
			this.AddFunction(dictionary, "vec4", "texture1D", Resources.Texture1DDoc, ShaderType.All, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("float", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "texture1DProj", Resources.Texture1DProjDoc, ShaderType.All, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("vec2", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "texture1DProj", Resources.Texture1DProjDoc, ShaderType.All, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "texture1DLod", Resources.Texture1DLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("float", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture1DProjLod", Resources.Texture1DProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("vec2", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture1DProjLod", Resources.Texture1DProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture2D", Resources.Texture2DDoc, ShaderType.All, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec2", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "texture2DProj", Resources.Texture2DProjDoc, ShaderType.All, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "texture2DProj", Resources.Texture2DProjDoc, ShaderType.All, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "texture2DLod", Resources.Texture2DLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec2", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture2DProjLod", Resources.Texture2DProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture2DProjLod", Resources.Texture2DProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture3D", Resources.Texture3DDoc, ShaderType.All, ParameterDefinition.Create("sampler3D", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "texture3DProj", Resources.Texture3DProjDoc, ShaderType.All, ParameterDefinition.Create("sampler3D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "texture3DLod", Resources.Texture3DLodDoc, ShaderType.All, ParameterDefinition.Create("sampler3D", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "texture3DProjLod", Resources.Texture3DProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler3D", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "textureCube", Resources.TextureCubeDoc, ShaderType.All, ParameterDefinition.Create("samplerCube", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "textureCubeLod", Resources.TextureCubeLodDoc, ShaderType.All, ParameterDefinition.Create("samplerCube", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "shadow1D", Resources.Shadow1DDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "shadow2D", Resources.Shadow2DDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "shadow1DProj", Resources.Shadow1DProjDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "shadow2DProj", Resources.Shadow2DProjDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "bias", true, ShaderType.Fragment));
			this.AddFunction(dictionary, "vec4", "shadow1DLod", Resources.Shadow1DLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "shadow2DLod", Resources.Shadow2DLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec3", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "shadow1DProjLod", Resources.Shadow1DProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler1DShadow", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));
			this.AddFunction(dictionary, "vec4", "shadow2DProjLod", Resources.Shadow2DProjLodDoc, ShaderType.All, ParameterDefinition.Create("sampler2DShadow", "sampler"), ParameterDefinition.Create("vec4", "coord"), ParameterDefinition.Create("float", "lod"));

			// Atomic-Counter
			this.AddFunction(dictionary, "uint", "atomicCounterIncrement", Resources.AtomicCounterIncrementDoc, ShaderType.All, ParameterDefinition.Create("atomic_uint", "c"));
			this.AddFunction(dictionary, "uint", "atomicCounterDecrement", Resources.AtomicCounterDecrementDoc, ShaderType.All, ParameterDefinition.Create("atomic_uint", "c"));
			this.AddFunction(dictionary, "uint", "atomicCounter", Resources.AtomicCounterDoc, ShaderType.All, ParameterDefinition.Create("atomic_uint", "c"));

			// Atomic Memory
			this.AddFunction(dictionary, "uint", "atomicAdd", Resources.AtomicAddDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicAdd", Resources.AtomicAddDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicMin", Resources.AtomicMinDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicMin", Resources.AtomicMinDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicMax", Resources.AtomicMaxDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicMax", Resources.AtomicMaxDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicAnd", Resources.AtomicAndDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicAnd", Resources.AtomicAndDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicOr", Resources.AtomicOrDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicOr", Resources.AtomicOrDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicXor", Resources.AtomicXorDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicXor", Resources.AtomicXorDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicExchange", Resources.AtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "uint", "mem"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicExchange", Resources.AtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "int", "mem"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "atomicCompSwap", Resources.AtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "uint", "mem"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "atomicCompSwap", Resources.AtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.InOutKeyword, "int", "mem"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));

			// Image
			this.AddFunction(dictionary, "int", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimage1D", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimage2D", "image"));
			this.AddFunction(dictionary, "ivec3", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimage3D", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimageCube", "image"));
			this.AddFunction(dictionary, "ivec3", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimageCubeArray", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimage2DRect", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimage1DArray", "image"));
			this.AddFunction(dictionary, "ivec3", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimage2DArray", "image"));
			this.AddFunction(dictionary, "int", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimageBuffer", "image"));
			this.AddFunction(dictionary, "ivec2", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimage2DMS", "image"));
			this.AddFunction(dictionary, "ivec3", "imageSize", Resources.ImageSizeDoc, ShaderType.All, ParameterDefinition.Create(new SyntaxType[] { SyntaxType.ReadOnlyKeyword, SyntaxType.WriteOnlyKeyword }, "gimage2DMSArray", "image"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimage1D", "image"), ParameterDefinition.Create("int", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimageBuffer", "image"), ParameterDefinition.Create("int", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "gvec4", "imageLoad", Resources.ImageLoadDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.ReadOnlyKeyword, "gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "gvec4", "imageStore", Resources.ImageStoreDoc, ShaderType.All, ParameterDefinition.Create(SyntaxType.WriteOnlyKeyword, "gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("gvec4", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAdd", Resources.ImageAtomicAddDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMin", Resources.ImageAtomicMinDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicMax", Resources.ImageAtomicMaxDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicAnd", Resources.ImageAtomicAndDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicOr", Resources.ImageAtomicOrDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicXor", Resources.ImageAtomicXorDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicExchange", Resources.ImageAtomicExchangeDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "uint", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("uint", "compare"), ParameterDefinition.Create("uint", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage1D", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2D", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage3D", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2DRect", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimageCube", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimageBuffer", "image"), ParameterDefinition.Create("int", "P"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage1DArray", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2DArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimageCubeArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMS", "image"), ParameterDefinition.Create("ivec2", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));
			this.AddFunction(dictionary, "int", "imageAtomicCompSwap", Resources.ImageAtomicCompSwapDoc, ShaderType.All, ParameterDefinition.Create("gimage2DMSArray", "image"), ParameterDefinition.Create("ivec3", "P"), ParameterDefinition.Create("int", "sample"), ParameterDefinition.Create("int", "compare"), ParameterDefinition.Create("int", "data"));

			// Fragment Processing
			// Derivative
			this.AddFunction(dictionary, "genType", "dFdx", Resources.DFdxDoc, ShaderType.Fragment, ParameterDefinition.Create("genType", "p"));
			this.AddFunction(dictionary, "genType", "dFdy", Resources.DFdyDoc, ShaderType.Fragment, ParameterDefinition.Create("genType", "p"));
			this.AddFunction(dictionary, "genType", "fwidth", Resources.FwidthDoc, ShaderType.Fragment, ParameterDefinition.Create("genType", "p"));

			// Interpolation
			this.AddFunction(dictionary, "float", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, ShaderType.Fragment, ParameterDefinition.Create("float", "interpolant"));
			this.AddFunction(dictionary, "vec2", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, ShaderType.Fragment, ParameterDefinition.Create("vec2", "interpolant"));
			this.AddFunction(dictionary, "vec3", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, ShaderType.Fragment, ParameterDefinition.Create("vec3", "interpolant"));
			this.AddFunction(dictionary, "vec4", "interpolateAtCentroid", Resources.InterpolateAtCentroidDoc, ShaderType.Fragment, ParameterDefinition.Create("vec4", "interpolant"));

			this.AddFunction(dictionary, "float", "interpolateAtSample", Resources.InterpolateAtSampleDoc, ShaderType.Fragment, ParameterDefinition.Create("float", "interpolant"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "vec2", "interpolateAtSample", Resources.InterpolateAtSampleDoc, ShaderType.Fragment, ParameterDefinition.Create("vec2", "interpolant"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "vec3", "interpolateAtSample", Resources.InterpolateAtSampleDoc, ShaderType.Fragment, ParameterDefinition.Create("vec3", "interpolant"), ParameterDefinition.Create("int", "sample"));
			this.AddFunction(dictionary, "vec4", "interpolateAtSample", Resources.InterpolateAtSampleDoc, ShaderType.Fragment, ParameterDefinition.Create("vec4", "interpolant"), ParameterDefinition.Create("int", "sample"));

			this.AddFunction(dictionary, "float", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, ShaderType.Fragment, ParameterDefinition.Create("float", "interpolant"), ParameterDefinition.Create("vec2", "offset"));
			this.AddFunction(dictionary, "vec2", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, ShaderType.Fragment, ParameterDefinition.Create("vec2", "interpolant"), ParameterDefinition.Create("vec2", "offset"));
			this.AddFunction(dictionary, "vec3", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, ShaderType.Fragment, ParameterDefinition.Create("vec3", "interpolant"), ParameterDefinition.Create("vec2", "offset"));
			this.AddFunction(dictionary, "vec4", "interpolateAtOffset", Resources.InterpolateAtOffsetDoc, ShaderType.Fragment, ParameterDefinition.Create("vec4", "interpolant"), ParameterDefinition.Create("vec2", "offset"));

			// Noise
			this.AddFunction(dictionary, "float", "noise1", Resources.Noise1Doc, ShaderType.Fragment, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "vec2", "noise2", Resources.Noise2Doc, ShaderType.Fragment, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "vec3", "noise3", Resources.Noise3Doc, ShaderType.Fragment, ParameterDefinition.Create("genType", "x"));
			this.AddFunction(dictionary, "vec4", "noise4", Resources.Noise4Doc, ShaderType.Fragment, ParameterDefinition.Create("genType", "x"));

			// Geometry Shader
			this.AddFunction(dictionary, "void", "EmitStreamVertex", Resources.EmitStreamVertexDoc, ShaderType.Geometry, ParameterDefinition.Create("int", "stream"));
			this.AddFunction(dictionary, "void", "EmitStreamPrimitive", Resources.EmitStreamPrimitiveDoc, ShaderType.Geometry, ParameterDefinition.Create("int", "stream"));
			this.AddFunction(dictionary, "void", "EmitVertex", Resources.EmitVertexDoc, ShaderType.Geometry);
			this.AddFunction(dictionary, "void", "EmitPrimitive", Resources.EmitPrimitiveDoc, ShaderType.Geometry);

			// Shader Invocation Control
			this.AddFunction(dictionary, "void", "barrier", Resources.BarrierDoc, ShaderType.TessellationControl);

			// Shader Memory Control
			this.AddFunction(dictionary, "void", "memoryBarrier", Resources.MemoryBarrierDoc, ShaderType.All);
			this.AddFunction(dictionary, "void", "memoryBarrierAtomicCounter", Resources.MemoryBarrierAtomicCounterDoc, ShaderType.All);
			this.AddFunction(dictionary, "void", "memoryBarrierBuffer", Resources.MemoryBarrierBufferDoc, ShaderType.All);
			this.AddFunction(dictionary, "void", "memoryBarrierShared", Resources.MemoryBarrierSharedDoc, ShaderType.All);
			this.AddFunction(dictionary, "void", "memoryBarrierImage", Resources.MemoryBarrierImageDoc, ShaderType.All);
			this.AddFunction(dictionary, "void", "groupMemoryBarrier", Resources.GroupMemoryBarrierDoc, ShaderType.All);
		}

		private void LoadVariables(Dictionary<string, List<Definition>> dictionary)
		{
			// Compute
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.UVec3Keyword, "gl_NumWorkGroups", false, ShaderType.Compute);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.UVec3Keyword, "gl_WorkGroupSize", false, ShaderType.Compute);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.UVec3Keyword, "gl_WorkGroupID", false, ShaderType.Compute);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.UVec3Keyword, "gl_LocalInvocationID", false, ShaderType.Compute);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.UVec3Keyword, "gl_GlobalInvocationID", false, ShaderType.Compute);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.UIntKeyword, "gl_LocalInvocationIndex", false, ShaderType.Compute);

			// Vertex
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_VertexID", false, ShaderType.Vertex);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_InstanceID", false, ShaderType.Vertex);
			this.AddInterfaceBlock(dictionary, SyntaxType.OutKeyword, "gl_PerVertex", string.Empty, false, ShaderType.Vertex, FieldDefinition.Create(SyntaxType.Vec4Keyword, "gl_Position"), FieldDefinition.Create(SyntaxType.FloatConstToken, "gl_PointSize"), FieldDefinition.Create(SyntaxType.Vec4Keyword, "gl_ClipDistance", true));

			// Geometry
			this.AddInterfaceBlock(dictionary, SyntaxType.InKeyword, "gl_PerVertex", "gl_in", true, ShaderType.Geometry, FieldDefinition.Create(SyntaxType.Vec4Keyword, "gl_Position"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_PointSize"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_ClipDistance", true));
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_PrimitiveIDIn", false, ShaderType.Geometry);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_InvocationID", false, ShaderType.Geometry);
			this.AddInterfaceBlock(dictionary, SyntaxType.OutKeyword, "gl_PerVertex", string.Empty, false, ShaderType.Geometry, FieldDefinition.Create(SyntaxType.Vec4Keyword, "gl_Position"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_PointSize"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_ClipDistance", true));
			this.AddVariable(dictionary, SyntaxType.OutKeyword, SyntaxType.IntKeyword, "gl_PrimitiveID", false, ShaderType.Geometry);
			this.AddVariable(dictionary, SyntaxType.OutKeyword, SyntaxType.IntKeyword, "gl_Layer", false, ShaderType.Geometry);
			this.AddVariable(dictionary, SyntaxType.OutKeyword, SyntaxType.IntKeyword, "gl_ViewportIndex", false, ShaderType.Geometry);

			// Tessellation Control
			this.AddInterfaceBlock(dictionary, SyntaxType.InKeyword, "gl_PerVertex", "gl_in", true, ShaderType.TessellationControl, FieldDefinition.Create(SyntaxType.Vec4Keyword, "gl_Position"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_PointSize"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_ClipDistance", true));
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_PatchVerticesIn", false, ShaderType.TessellationControl);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_PrimitiveID", false, ShaderType.TessellationControl);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_InvocationID", false, ShaderType.TessellationControl);
			this.AddInterfaceBlock(dictionary, SyntaxType.OutKeyword, "gl_PerVertex", "gl_out", true, ShaderType.TessellationControl, FieldDefinition.Create(SyntaxType.Vec4Keyword, "gl_Position"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_PointSize"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_ClipDistance", true));
			this.AddVariable(dictionary, new List<SyntaxType> { SyntaxType.PatchKeyword, SyntaxType.OutKeyword }, SyntaxType.FloatKeyword, "gl_TessLevelOuter", true, ShaderType.TessellationControl);
			this.AddVariable(dictionary, new List<SyntaxType> { SyntaxType.PatchKeyword, SyntaxType.OutKeyword }, SyntaxType.FloatKeyword, "gl_TessLevelInner", true, ShaderType.TessellationControl);

			// Tessellation Evaluation
			this.AddInterfaceBlock(dictionary, SyntaxType.InKeyword, "gl_PerVertex", "gl_in", true, ShaderType.TessellationEvaluation, FieldDefinition.Create(SyntaxType.Vec4Keyword, "gl_Position"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_PointSize"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_ClipDistance", true));
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_PatchVerticesIn", false, ShaderType.TessellationEvaluation);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_PrimitiveID", false, ShaderType.TessellationEvaluation);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.Vec3Keyword, "gl_TessCoord", false, ShaderType.TessellationEvaluation);
			this.AddVariable(dictionary, new List<SyntaxType> { SyntaxType.PatchKeyword, SyntaxType.InKeyword }, SyntaxType.FloatKeyword, "gl_TessLevelOuter", true, ShaderType.TessellationEvaluation);
			this.AddVariable(dictionary, new List<SyntaxType> { SyntaxType.PatchKeyword, SyntaxType.InKeyword }, SyntaxType.FloatKeyword, "gl_TessLevelInner", true, ShaderType.TessellationEvaluation);
			this.AddInterfaceBlock(dictionary, SyntaxType.OutKeyword, "gl_PerVertex", string.Empty, false, ShaderType.TessellationEvaluation, FieldDefinition.Create(SyntaxType.Vec4Keyword, "gl_Position"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_PointSize"), FieldDefinition.Create(SyntaxType.FloatKeyword, "gl_ClipDistance", true));

			// Fragment
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.Vec4Keyword, "gl_FragCoord", false, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.BoolKeyword, "gl_FribtFacing", false, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.FloatKeyword, "gl_ClipDistance", true, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.Vec2Keyword, "gl_PointCoord", false, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_PrimitiveID", false, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_SampleID", false, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.Vec2Keyword, "gl_SamplePosition", false, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_SampleMaskIn", true, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_Layer", false, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.InKeyword, SyntaxType.IntKeyword, "gl_ViewportIndex", false, ShaderType.Fragment);

			this.AddVariable(dictionary, SyntaxType.OutKeyword, SyntaxType.FloatKeyword, "gl_FragDepth", false, ShaderType.Fragment);
			this.AddVariable(dictionary, SyntaxType.OutKeyword, SyntaxType.IntKeyword, "gl_SampleMask", true, ShaderType.Fragment);

			// Compatibility built-in variables
			// TODO: add this

			// Constants
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IVec3Keyword, "gl_MaxComputeWorkGroupCount", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IVec3Keyword, "gl_MaxComputeWorkGroupSize", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxComputeUniformComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxComputeTextureImageUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxComputeImageUniforms", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxComputeAtomicCounters", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxComputeAtomicCounterBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVertexAttribs", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVertexUniformComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVaryingComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVertexOutputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryInputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryOutputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxFragmentInputComponetns", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVertexTextureImageUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxCombinedTextureImageUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTextureImageUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxImageUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxCombinedImageUnitsAndFragmentOutputs", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxCombinedShaderOutputResources", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxImageSamples", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVertexImageUniforms", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessControlImageUniforms", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessEvaluationImageUniforms", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryImageUniforms", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxFragmentImageUniforms", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxCombinedImageUniforms", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxFragmentUniformComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxDrawBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxClipDistances", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryTextureImageUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryOutputVertices", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryTotalOutputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryUniformComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryVaryingComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessControlInputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessControlOutputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessControlTextureImageUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessControlUniformComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessControlTotalOutputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessEvaluationInputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessEvaluationOutputComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessEvaluationTextureImageUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessEvaluationUniformComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessPatchComponents", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxPatchVertices", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessGenLevel", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxViewports", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVertexUniformVectors", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxFragmentUniformVectors", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVaryingVectors", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVertexAtomicCounters", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessControlAtomicCounters", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessEvaluationAtomicCounters", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryAtomicCounters", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxFragmentAtomicCounters", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxCombinedAtomicCounters", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxAtomicCounterBindings", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVertexAtomicCounterBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessControlAtomicCounterBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTessEvaluationAtomicCounterBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxGeometryAtomicCounterBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxFragmentAtomicCounterBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxCombinedAtomicCOunterBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxAtomicCounterBufferSize", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MinProgramTexelOffset", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxProgramTexelOffset", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTransformFeedbackBuffers", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTransformFeedbackInterleavedComponents", false, ShaderType.All);

			// Compatibility Constants
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTextureUnits", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxTextureCoords", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxClipPlanes", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.ConstKeyword, SyntaxType.IntKeyword, "gl_MaxVaryingFloats", false, ShaderType.All);

			// Uniform state
			Definition definition = this.AddStruct(dictionary, "gl_DepthRangeParameters", ShaderType.All, FieldDefinition.Create(SyntaxType.FloatKeyword, "near"), FieldDefinition.Create(SyntaxType.FloatKeyword, "far"), FieldDefinition.Create(SyntaxType.FloatKeyword, "diff"));
			this.AddVariable(dictionary, SyntaxType.UniformKeyword, new TypeDefinition(definition), "gl_DepthRange", false, ShaderType.All);
			this.AddVariable(dictionary, SyntaxType.UniformKeyword, SyntaxType.IntKeyword, "gl_NumSamples", false, ShaderType.All);

			// Compatibility Profile State
			// TODO: add this
		}

		private void AddFunction(Dictionary<string, List<Definition>> dictionary, string returnType, string name, string documentation, ShaderType type, params ParameterDefinition[][] parameters)
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

			List<ParameterDefinition> paramList = new List<ParameterDefinition>(parameters.Length);

			Definition definition;

			if (overloads == 0)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					paramList.Add(parameters[i][0]);
				}

				definition = this.AddToDic(dictionary, new FunctionDefinition(new TypeDefinition(returnType.GetSyntaxType()), name, paramList, documentation, Scope.BuiltIn, null), type);

				definition.ShaderType = type;
			}
			else
			{
				for (int i = 0; i < overloads; i++)
				{
					for (int j = 0; j < parameters.Length; j++)
					{
						paramList.Add(parameters[j][i % parameters[j].Length]);
					}

					definition = this.AddToDic(dictionary, new FunctionDefinition(new TypeDefinition(returnTypes[i % returnTypes.Length].GetSyntaxType()), name, paramList, documentation, Scope.BuiltIn, null), type);

					definition.ShaderType = type;
				}
			}
		}

		private Definition AddToDic(Dictionary<string, List<Definition>> dictionary, Definition definition, ShaderType type)
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

			definition.ShaderType = type;

			return definition;
		}

		private void AddInterfaceBlock(Dictionary<string, List<Definition>> dictionary, SyntaxType typeQualifier, string typeName, string name, bool isArray, ShaderType shaderType, params FieldDefinition[] fields)
		{
			Definition block = this.AddToDic(dictionary, new InterfaceBlockDefinition(new List<SyntaxType> { typeQualifier }, typeName, string.Empty, new List<FieldDefinition>(fields), Scope.BuiltIn, null), shaderType);

			if (!string.IsNullOrEmpty(name))
			{
				this.AddVariable(dictionary, null, new TypeDefinition(block), name, isArray, shaderType);
			}
			else
			{
				for (int i = 0; i < fields.Length; i++)
				{
					if (fields[i].ShaderType == ShaderType.All)
					{
						this.AddToDic(dictionary, fields[i], shaderType);
					}
					else
					{
						this.AddToDic(dictionary, fields[i], fields[i].ShaderType);
					}
				}
			}
		}

		private Definition AddVariable(Dictionary<string, List<Definition>> dictionary, IReadOnlyList<SyntaxType> typeQualifiers, TypeDefinition type, string identifier, bool isArray, ShaderType shaderType)
		{
			List<ColoredString> arraySpecifier = new List<ColoredString>();

			if (isArray)
			{
				arraySpecifier.Add(ColoredString.Create("[]", ColorType.Punctuation));
			}

			return this.AddToDic(dictionary, new VariableDefinition(typeQualifiers, type, ColoredString.Create(identifier, ColorType.GlobalVariable), arraySpecifier, string.Empty, DefinitionKind.GlobalVariable, Scope.BuiltIn, null), shaderType);
		}

		private Definition AddVariable(Dictionary<string, List<Definition>> dictionary, SyntaxType typeQualifier, TypeDefinition type, string identifier, bool isArray, ShaderType shaderType)
		{
			return this.AddVariable(dictionary, new List<SyntaxType> { typeQualifier }, type, identifier, isArray, shaderType);
		}

		private Definition AddVariable(Dictionary<string, List<Definition>> dictionary, IReadOnlyList<SyntaxType> typeQualifiers, SyntaxType type, string identifier, bool isArray, ShaderType shaderType)
		{
			return this.AddVariable(dictionary, typeQualifiers, new TypeDefinition(type), identifier, isArray, shaderType);
		}

		private Definition AddVariable(Dictionary<string, List<Definition>> dictionary, SyntaxType typeQualifiers, SyntaxType type, string identifier, bool isArray, ShaderType shaderType)
		{
			return this.AddVariable(dictionary, new List<SyntaxType> { typeQualifiers }, type, identifier, isArray, shaderType);
		}

		private Definition AddStruct(Dictionary<string, List<Definition>> dictionary, string name, ShaderType type, params FieldDefinition[] fields)
		{
			return this.AddToDic(dictionary, new TypeNameDefinition(name, string.Empty, new List<FieldDefinition>(fields), Scope.BuiltIn, null), type);
		}
	}
}
