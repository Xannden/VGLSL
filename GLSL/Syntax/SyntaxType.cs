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
		BoolKeyword,

		// Scalers
		FloatKeyword,

		DoubleKeyword,
		IntKeyword,
		UIntKeyword,

		// Vectors
		Vec2Keyword,

		Vec3Keyword,
		Vec4Keyword,
		UVec2Keyword,
		UVec3Keyword,
		UVec4Keyword,
		IVec2Keyword,
		IVec3Keyword,
		IVec4Keyword,
		DVec2Keyword,
		DVec3Keyword,
		DVec4Keyword,
		BVec2Keyword,
		BVec3Keyword,
		BVec4Keyword,

		// Matrices
		Mat2Keyword,

		Mat3Keyword,
		Mat4Keyword,
		Mat2X2Keyword,
		Mat2X3Keyword,
		Mat2X4Keyword,
		Mat3X2Keyword,
		Mat3X3Keyword,
		Mat3X4Keyword,
		Mat4X2Keyword,
		Mat4X3Keyword,
		Mat4X4Keyword,
		DMat2Keyword,
		DMat3Keyword,
		DMat4Keyword,
		DMat2X2Keyword,
		DMat2X3Keyword,
		DMat2X4Keyword,
		DMat3X2Keyword,
		DMat3X3Keyword,
		DMat3X4Keyword,
		DMat4X2Keyword,
		DMat4X3Keyword,
		DMat4X4Keyword,

		// Samplers
		Sampler1DKeyword,

		Sampler2DKeyword,
		Sampler3DKeyword,
		SamplerCubeKeyword,
		Sampler1DShadowKeyword,
		Sampler2DShadowKeyword,
		SamplerCubeShadowKeyword,
		Sampler1DArrayKeyword,
		Sampler2DArrayKeyword,
		Sampler1DArrayShadowKeyword,
		Sampler2DArrayShadowKeyword,
		ISampler1DKeyword,
		ISampler2DKeyword,
		ISampler3DKeyword,
		ISamplerCubeKeyword,
		ISampler1DArrayKeyword,
		ISampler2DArrayKeyword,
		USampler1DKeyword,
		USampler2DKeyword,
		USampler3DKeyword,
		USamplerCubeKeyword,
		USampler1DArrayKeyword,
		USampler2DArrayKeyword,
		Sampler2DRectKeyword,
		Sampler2DRectShadowKeyword,
		ISampler2DRectKeyword,
		USampler2DRectKeyword,
		SamplerBufferKeyword,
		ISamplerBufferKeyword,
		USamplerBufferKeyword,
		SamplerCubeArrayKeyword,
		SamplerCubeArrayShadowKeyword,
		ISamplerCubeArrayKeyword,
		USamplerCubeArrayKeyword,
		Sampler2DMSKeyword,
		ISampler2DMSKeyword,
		USampler2DMSKeyword,
		Sampler2DMSArrayKeyword,
		ISampler2DMSArrayKeyword,
		USampler2DMSArrayKeyword,

		// Images
		Image1DKeyword,

		Image2DKeyword,
		Image3DKeyword,
		IImage1DKeyword,
		IImage2DKeyword,
		IImage3DKeyword,
		UImage1DKeyword,
		UImage2DKeyword,
		UImage3DKeyword,
		Image2DRectKeyword,
		IImage2DRectKeyword,
		UImage2DRectKeyword,
		ImageCubeKeyword,
		IImageCubeKeyword,
		UImageCubeKeyword,
		ImageBufferKeyword,
		IImageBufferKeyword,
		UImageBufferKeyword,
		Image1DArrayKeyword,
		IImage1DArrayKeyword,
		UImage1DArrayKeyword,
		Image2DArrayKeyword,
		IImage2DArrayKeyword,
		UImage2DArrayKeyword,
		ImageCubeArrayKeyword,
		IImageCubeArrayKeyword,
		UImageCubeArrayKeyword,
		Image2DMSKeyword,
		IImage2DMSKeyword,
		UImage2DMSKeyword,
		Image2DMSArrayKeyword,
		IImage2DMSArrayKeyword,
		UImage2DMSArrayKeyword,

		VoidKeyword,

		NoPerspectiveKeyword,
		FlatKeyword,
		SmoothKeyword,
		LayoutKeyword,
		StructKeyword,
		WhileKeyword,
		InvariantKeyword,
		HighPrecisionKeyword,
		MediumPrecisionKeyword,
		LowPrecisionKeyword,
		PrecisionKeyword,
		TrueKeyword,
		FalseKeyword,

		// Reserved Keywords
		CommonKeyword,

		PartitionKeyword,
		ActiveKeyword,
		ASMKeyword,
		ClassKeyword,
		UnionKeyword,
		EnumKeyword,
		TypedefKeyword,
		TemplateKeyword,
		ThisKeyword,
		ResourceKeyword,
		GotoKeyword,
		InlineKeyword,
		NoInlineKeyword,
		PublicKeyword,
		StaticKeyword,
		ExternKeyword,
		ExternalKeyword,
		InterfaceKeyword,
		LongKeyword,
		ShortKeyword,
		HalfKeyword,
		FixedKeyword,
		UnsignedKeyword,
		SuperPKeyword,
		InputKeyword,
		OutputKeyword,
		HVec2Keyword,
		HVec3Keyword,
		HVec4Keyword,
		FVec2Keyword,
		FVec3Keyword,
		FVec4Keyword,
		Sampler3DRectKeyword,
		FilterKeyword,
		SizeofKeyword,
		CastKeyword,
		NamespaceKeyword,
		UsingKeyword,

		// preprocessor keywords
		DefinePreprocessorKeyword,

		PoundToken,
		UndefinePreprocessorKeyword,
		IfPreprocessorKeyword,
		IfDefinedPreprocessorKeyword,
		IfNotDefinedPreprocessorKeyword,
		ElsePreprocessorKeyword,
		ElseIfPreprocessorKeyword,
		EndIfPreprocessorKeyword,
		ErrorPreprocessorKeyword,
		PragmaPreprocessorKeyword,
		ExtensionPreprocessorKeyword,
		VersionPreprocessorKeyword,
		LinePreprocessorKeyword,

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
		DeclarationList,
		ArraySpecifier,
		Type,
		TypeNonArray,
		TypeName,
		TypeQualifier,
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
		StructDefinition,
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