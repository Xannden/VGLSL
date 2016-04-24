using Xannden.GLSL.Utility;

namespace Xannden.GLSL.Syntax
{
	public enum SyntaxType
	{
		None,
		EOF,
		InvalidToken,
		Any,

		// punctuation
		[Text("(")]
		LeftParenToken,

		[Text(")")]
		RightParenToken,

		[Text("[")]
		LeftBracketToken,

		[Text("]")]
		RightBracketToken,

		[Text("{")]
		LeftBraceToken,

		[Text("}")]
		RightBraceToken,

		[Text(".")]
		DotToken,

		[Text(",")]
		CommaToken,

		[Text(":")]
		ColonToken,

		[Text("=")]
		EqualToken,

		[Text(";")]
		SemicolonToken,

		[Text("!")]
		ExclamationToken,

		[Text("-")]
		MinusToken,

		[Text("~")]
		TildeToken,

		[Text("+")]
		PlusToken,

		[Text("*")]
		StarToken,

		[Text("/")]
		SlashToken,

		[Text("%")]
		PercentToken,

		[Text("<")]
		LessThenToken,

		[Text(">")]
		GreaterThenToken,

		[Text("|")]
		VerticalBarToken,

		[Text("^")]
		CaretToken,

		[Text("&")]
		AmpersandToken,

		[Text("?")]
		QuestionToken,

		// compound punctuation
		[Text("<<")]
		LessThenLessThenToken,

		[Text(">>")]
		GreaterThenGreaterThenToken,

		[Text("++")]
		PlusPlusToken,

		[Text("--")]
		MinusMinusToken,

		[Text("<=")]
		LessThenEqualToken,

		[Text(">=")]
		GreaterThenEqualToken,

		[Text("==")]
		EqualEqualToken,

		[Text("!=")]
		ExclamationEqualToken,

		[Text("&&")]
		AmpersandAmpersandToken,

		[Text("||")]
		BarBarToken,

		[Text("^^")]
		CaretCaretToken,

		[Text("*=")]
		StarEqualToken,

		[Text("/=")]
		SlashEqualToken,

		[Text("+=")]
		PlusEqualToken,

		[Text("%=")]
		PercentEqualToken,

		[Text("<<=")]
		LessThenLessThenEqualToken,

		[Text(">>=")]
		GreaterThenGreaterThenEqualToken,

		[Text("&=")]
		AmpersandEqualToken,

		[Text("^=")]
		CaretEqualToken,

		[Text("|=")]
		BarEqualToken,

		[Text("-=")]
		MinusEqualToken,

		// keywords
		[Text("attribute")]
		AttributeKeyword,

		[Text("const")]
		ConstKeyword,

		[Text("uniform")]
		UniformKeyword,

		[Text("buffer")]
		BufferKeyword,

		[Text("shared")]
		SharedKeyword,

		[Text("coherent")]
		CoherentKeyword,

		[Text("volatile")]
		VolatileKeyword,

		[Text("restrict")]
		RestrictKeyword,

		[Text("readonly")]
		ReadOnlyKeyword,

		[Text("writeonly")]
		WriteOnlyKeyword,

		[Text("atomic_uint")]
		AtomicUIntKeyword,

		[Text("precise")]
		PreciseKeyword,

		[Text("break")]
		BreakKeyword,

		[Text("continue")]
		ContinueKeyword,

		[Text("do")]
		DoKeyword,

		[Text("else")]
		ElseKeyword,

		[Text("for")]
		ForKeyword,

		[Text("if")]
		IfKeyword,

		[Text("discard")]
		DiscardKeyword,

		[Text("return")]
		ReturnKeyword,

		[Text("switch")]
		SwitchKeyword,

		[Text("case")]
		CaseKeyword,

		[Text("default")]
		DefaultKeyword,

		[Text("subroutine")]
		SubroutineKeyword,

		[Text("centroid")]
		CentroidKeyword,

		[Text("in")]
		InKeyword,

		[Text("out")]
		OutKeyword,

		[Text("inout")]
		InOutKeyword,

		[Text("varying")]
		VaryingKeyword,

		[Text("patch")]
		PatchKeyword,

		[Text("sample")]
		SampleKeyword,

		// Types
		[Text("bool")]
		BoolKeyword,

		// Scalers
		[Text("float")]
		FloatKeyword,
		[Text("double")]
		DoubleKeyword,
		[Text("int")]
		IntKeyword,
		[Text("uint")]
		UIntKeyword,

		// Vectors
		[Text("vec2")]
		Vec2Keyword,
		[Text("vec3")]
		Vec3Keyword,
		[Text("vec4")]
		Vec4Keyword,
		[Text("uvec2")]
		UVec2Keyword,
		[Text("uvec3")]
		UVec3Keyword,
		[Text("uvec4")]
		UVec4Keyword,
		[Text("ivec2")]
		IVec2Keyword,
		[Text("ivec3")]
		IVec3Keyword,
		[Text("ivec4")]
		IVec4Keyword,
		[Text("dvec2")]
		DVec2Keyword,
		[Text("dvec3")]
		DVec3Keyword,
		[Text("dvec4")]
		DVec4Keyword,
		[Text("bvec2")]
		BVec2Keyword,
		[Text("bvec3")]
		BVec3Keyword,
		[Text("bvec4")]
		BVec4Keyword,

		// Matrices
		[Text("mat2")]
		Mat2Keyword,
		[Text("mat3")]
		Mat3Keyword,
		[Text("mat4")]
		Mat4Keyword,
		[Text("mat2x2")]
		Mat2X2Keyword,
		[Text("mat2x3")]
		Mat2X3Keyword,
		[Text("mat2x4")]
		Mat2X4Keyword,
		[Text("mat3x2")]
		Mat3X2Keyword,
		[Text("mat3x3")]
		Mat3X3Keyword,
		[Text("mat3x4")]
		Mat3X4Keyword,
		[Text("mat4x2")]
		Mat4X2Keyword,
		[Text("mat4x3")]
		Mat4X3Keyword,
		[Text("mat4x4")]
		Mat4X4Keyword,
		[Text("dmat2")]
		DMat2Keyword,
		[Text("dmat3")]
		DMat3Keyword,
		[Text("dmat4")]
		DMat4Keyword,
		[Text("dmat2x2")]
		DMat2X2Keyword,
		[Text("dmat2x3")]
		DMat2X3Keyword,
		[Text("dmat2x4")]
		DMat2X4Keyword,
		[Text("dmat3x2")]
		DMat3X2Keyword,
		[Text("dmat3x3")]
		DMat3X3Keyword,
		[Text("dmat3x4")]
		DMat3X4Keyword,
		[Text("dmat4x2")]
		DMat4X2Keyword,
		[Text("dmat4x3")]
		DMat4X3Keyword,
		[Text("dmat4x4")]
		DMat4X4Keyword,

		// Samplers
		[Text("sampler1D")]
		Sampler1DKeyword,
		[Text("sampler2D")]
		Sampler2DKeyword,
		[Text("sampler3D")]
		Sampler3DKeyword,
		[Text("samplerCube")]
		SamplerCubeKeyword,
		[Text("sampler1DShadow")]
		Sampler1DShadowKeyword,
		[Text("sampler2DShadow")]
		Sampler2DShadowKeyword,
		[Text("samplerCubeShadow")]
		SamplerCubeShadowKeyword,
		[Text("sampler1DArray")]
		Sampler1DArrayKeyword,
		[Text("sampler2DArray")]
		Sampler2DArrayKeyword,
		[Text("sampler1DArrayShadow")]
		Sampler1DArrayShadowKeyword,
		[Text("sampler2DArrayShadow")]
		Sampler2DArrayShadowKeyword,
		[Text("isampler1D")]
		ISampler1DKeyword,
		[Text("isampler2D")]
		ISampler2DKeyword,
		[Text("isampler3D")]
		ISampler3DKeyword,
		[Text("isamplerCube")]
		ISamplerCubeKeyword,
		[Text("isampler1DArray")]
		ISampler1DArrayKeyword,
		[Text("isampler2DArray")]
		ISampler2DArrayKeyword,
		[Text("usampler1D")]
		USampler1DKeyword,
		[Text("usampler2D")]
		USampler2DKeyword,
		[Text("usampler3D")]
		USampler3DKeyword,
		[Text("usamplerCube")]
		USamplerCubeKeyword,
		[Text("usampler1DArray")]
		USampler1DArrayKeyword,
		[Text("usampler2DArray")]
		USampler2DArrayKeyword,
		[Text("sampler2DRect")]
		Sampler2DRectKeyword,
		[Text("sampler2DRectShadow")]
		Sampler2DRectShadowKeyword,
		[Text("isampler2DRect")]
		ISampler2DRectKeyword,
		[Text("usampler2DRect")]
		USampler2DRectKeyword,
		[Text("samplerBuffer")]
		SamplerBufferKeyword,
		[Text("isamplerBuffer")]
		ISamplerBufferKeyword,
		[Text("usamplerBuffer")]
		USamplerBufferKeyword,
		[Text("samplerCubeArray")]
		SamplerCubeArrayKeyword,
		[Text("samplerCubeArrayShadow")]
		SamplerCubeArrayShadowKeyword,
		[Text("isamplerCubeArray")]
		ISamplerCubeArrayKeyword,
		[Text("usamplerCubeArray")]
		USamplerCubeArrayKeyword,
		[Text("sampler2DMS")]
		Sampler2DMSKeyword,
		[Text("isampler2DMS")]
		ISampler2DMSKeyword,
		[Text("usampler2DMS")]
		USampler2DMSKeyword,
		[Text("sampler2DMSArray")]
		Sampler2DMSArrayKeyword,
		[Text("isampler2DMSArray")]
		ISampler2DMSArrayKeyword,
		[Text("usampler2DMSArray")]
		USampler2DMSArrayKeyword,

		// Images
		[Text("image1D")]
		Image1DKeyword,
		[Text("image2D")]
		Image2DKeyword,
		[Text("image3D")]
		Image3DKeyword,
		[Text("iimage1D")]
		IImage1DKeyword,
		[Text("iimage2D")]
		IImage2DKeyword,
		[Text("iimage3D")]
		IImage3DKeyword,
		[Text("uimage1D")]
		UImage1DKeyword,
		[Text("uimage2D")]
		UImage2DKeyword,
		[Text("uimage3D")]
		UImage3DKeyword,
		[Text("image2DRect")]
		Image2DRectKeyword,
		[Text("iimage2DRect")]
		IImage2DRectKeyword,
		[Text("uimage2DRect")]
		UImage2DRectKeyword,
		[Text("imageCube")]
		ImageCubeKeyword,
		[Text("iimageCube")]
		IImageCubeKeyword,
		[Text("uimageCube")]
		UImageCubeKeyword,
		[Text("imageBuffer")]
		ImageBufferKeyword,
		[Text("iimageBuffer")]
		IImageBufferKeyword,
		[Text("uimageBuffer")]
		UImageBufferKeyword,
		[Text("image1DArray")]
		Image1DArrayKeyword,
		[Text("iimage1DArray")]
		IImage1DArrayKeyword,
		[Text("uimage1DArray")]
		UImage1DArrayKeyword,
		[Text("image2DArray")]
		Image2DArrayKeyword,
		[Text("iimage2DArray")]
		IImage2DArrayKeyword,
		[Text("uimage2DArray")]
		UImage2DArrayKeyword,
		[Text("imageCubeArray")]
		ImageCubeArrayKeyword,
		[Text("iimageCubeArray")]
		IImageCubeArrayKeyword,
		[Text("uimageCubeArray")]
		UImageCubeArrayKeyword,
		[Text("image2DMS")]
		Image2DMSKeyword,
		[Text("iimage2DMS")]
		IImage2DMSKeyword,
		[Text("uimage2DMS")]
		UImage2DMSKeyword,
		[Text("image2DMSArray")]
		Image2DMSArrayKeyword,
		[Text("iimage2DMSArray")]
		IImage2DMSArrayKeyword,
		[Text("uimage2DMSArray")]
		UImage2DMSArrayKeyword,
		[Text("void")]
		VoidKeyword,
		[Text("noperspective")]
		NoPerspectiveKeyword,
		[Text("flat")]
		FlatKeyword,
		[Text("smooth")]
		SmoothKeyword,
		[Text("layout")]
		LayoutKeyword,
		[Text("struct")]
		StructKeyword,
		[Text("while")]
		WhileKeyword,
		[Text("invariant")]
		InvariantKeyword,
		[Text("highp")]
		HighPrecisionKeyword,
		[Text("mediump")]
		MediumPrecisionKeyword,
		[Text("lowp")]
		LowPrecisionKeyword,
		[Text("precision")]
		PrecisionKeyword,
		[Text("true")]
		TrueKeyword,
		[Text("false")]
		FalseKeyword,

		// preprocessor keywords
		[Text("#define")]
		DefinePreprocessorKeyword,
		[Text("#")]
		PoundToken,
		[Text("#undefine")]
		UndefinePreprocessorKeyword,
		[Text("#if")]
		IfPreprocessorKeyword,
		[Text("#ifdef")]
		IfDefinedPreprocessorKeyword,
		[Text("#ifndef")]
		IfNotDefinedPreprocessorKeyword,
		[Text("#else")]
		ElsePreprocessorKeyword,
		[Text("#elseif")]
		ElseIfPreprocessorKeyword,
		[Text("#endif")]
		EndIfPreprocessorKeyword,
		[Text("#error")]
		ErrorPreprocessorKeyword,
		[Text("#pragma")]
		PragmaPreprocessorKeyword,
		[Text("#extension")]
		ExtensionPreprocessorKeyword,
		[Text("#version")]
		VersionPreprocessorKeyword,
		[Text("#line")]
		LinePreprocessorKeyword,

		// Reserved Keywords
		[Text("common")]
		CommonKeyword,
		[Text("partition")]
		PartitionKeyword,
		[Text("active")]
		ActiveKeyword,
		[Text("asm")]
		ASMKeyword,
		[Text("class")]
		ClassKeyword,
		[Text("union")]
		UnionKeyword,
		[Text("enum")]
		EnumKeyword,
		[Text("typedef")]
		TypedefKeyword,
		[Text("template")]
		TemplateKeyword,
		[Text("this")]
		ThisKeyword,
		[Text("resource")]
		ResourceKeyword,
		[Text("goto")]
		GotoKeyword,
		[Text("inline")]
		InlineKeyword,
		[Text("noinline")]
		NoInlineKeyword,
		[Text("public")]
		PublicKeyword,
		[Text("static")]
		StaticKeyword,
		[Text("extern")]
		ExternKeyword,
		[Text("external")]
		ExternalKeyword,
		[Text("interface")]
		InterfaceKeyword,
		[Text("long")]
		LongKeyword,
		[Text("short")]
		ShortKeyword,
		[Text("half")]
		HalfKeyword,
		[Text("fixed")]
		FixedKeyword,
		[Text("unsigned")]
		UnsignedKeyword,
		[Text("superp")]
		SuperPKeyword,
		[Text("input")]
		InputKeyword,
		[Text("output")]
		OutputKeyword,
		[Text("hvec2")]
		HVec2Keyword,
		[Text("hvec3")]
		HVec3Keyword,
		[Text("hvec4")]
		HVec4Keyword,
		[Text("fvec2")]
		FVec2Keyword,
		[Text("fvec3")]
		FVec3Keyword,
		[Text("fvec4")]
		FVec4Keyword,
		[Text("sampler3DRect")]
		Sampler3DRectKeyword,
		[Text("filter")]
		FilterKeyword,
		[Text("sizeof")]
		SizeofKeyword,
		[Text("cast")]
		CastKeyword,
		[Text("namespace")]
		NamespaceKeyword,
		[Text("using")]
		UsingKeyword,

		// tokens with Text
		IdentifierToken,

		BoolConstToken,
		FloatConstToken,
		DoubleConstToken,
		IntConstToken,
		UIntConstToken,
		PreprocessorToken,

		// Rules
		Program,

		Declaration,
		PrecisionDeclaration,
		ArraySpecifier,
		Type,
		TypeNonArray,
		TypeName,
		TypeQualifier,
		SingleTypeQualifier,
		StorageQualifier,
		LayoutQualifier,
		LayoutQualifierId,
		PrecisionQualifier,
		InterpolationQualifier,
		InvariantQualifier,
		PreciseQualifier,
		FunctionDefinition,
		Block,
		FunctionHeader,
		Parameter,
		ReturnType,
		Statement,
		SimpleStatement,
		SelectionStatement,
		ElseStatement,
		SwitchStatement,
		CaseLabel,
		IterationStatement,
		WhileStatement,
		DoWhileStatement,
		ForStatement,
		JumpStatement,
		ExpressionStatement,
		Condition,
		InterfaceBlock,
		StructSpecifier,
		StructDeclaration,
		StructDeclarator,
		Expression,
		ConstantExpression,
		AssignmentExpression,
		AssignmentOperator,
		ConditionalExpression,
		LogicalOrExpression,
		LogicalXorExpression,
		LogicalAndExpression,
		InclusiveOrExpression,
		ExclusiveOrExpression,
		AndExpression,
		EqualityExpression,
		RelationalExpression,
		ShiftExpression,
		AdditiveExpression,
		MultiplicativeExpression,
		UnaryExpression,
		PostfixExpression,
		PostfixExpressionStart,
		PostfixExpressionContinuation,
		PostfixArrayAccess,
		PrimaryExpression,
		FunctionCall,
		Constructor,
		FieldSelection,
		InitDeclaratorList,
		InitPart,
		Initializer,
		InitList,

		// Preprocessor Rules
		Preprocessor,

		DefinePreprocessor,
		UndefinePreprocessor,
		IfPreprocessor,
		IfDefinedPreprocessor,
		IfNotDefinedPreprocessor,
		ElsePreprocessor,
		ElseIfPreprocessor,
		EndIfPreprocessor,
		ErrorPreprocessor,
		PragmaPreprocessor,
		ExtensionPreprocessor,
		VersionPreprocessor,
		LinePreprocessor,
		TokenString,
		MacroArguments,
		ExcludedCode,

		// Trivia
		LineCommentTrivia,

		BlockCommentTrivia,
		WhiteSpaceTrivia,
		NewLineTrivia,
		TriviaList,
	}
}