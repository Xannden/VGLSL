namespace Xannden.GLSL.Syntax
{
	public enum SyntaxType
	{
		None,
		EOF,
		InvalidToken,
		Any,

		// punctuation
		LeftParenToken,

		RightParenToken,
		LeftBracketToken,
		RightBracketToken,
		LeftBraceToken,
		RightBraceToken,
		DotToken,
		CommaToken,
		ColonToken,
		EqualToken,
		SemiColonToken,
		ExclamationToken,
		MinusToken,
		TildeToken,
		PlusToken,
		StarToken,
		SlashToken,
		PercentToken,
		LessThenToken,
		GreaterThenToken,
		VerticalBarToken,
		CaretToken,
		AmpersandToken,
		QuestionToken,

		// compound punctuation
		LessThenLessThenToken,

		GreaterThenGreaterThenToken,
		PlusPlusToken,
		MinusMinusToken,
		LessThenEqualToken,
		GreaterThenEqualToken,
		EqualEqualToken,
		ExclamationEqualToken,
		AmpersandAmpersandToken,
		BarBarToken,
		CaretCaretToken,
		StarEqualToken,
		SlashEqualToken,
		PlusEqualToken,
		PercentEqualToken,
		LessThenLessThenEqualToken,
		GreaterThenGreaterThenEqualToken,
		AmpersandEqualToken,
		CaretEqualToken,
		BarEqualToken,
		MinusEqualToken,

		// keywords
		AttributeKeyword,

		ConstKeyword,
		UniformKeyword,
		BufferKeyword,
		SharedKeyword,
		CoherentKeyword,
		VolitileKeyword,
		RestrictKeyword,
		ReadonlyKeyword,
		WriteonlyKeyword,
		AtomicUIntKeyword,
		PreciseKeyword,
		BreakKeyword,
		ContinueKeyword,
		DoKeyword,
		ElseKeyword,
		ForKeyword,
		IfKeyword,
		DiscardKeyword,
		ReturnKeyword,
		SwitchKeyword,
		CaseKeyword,
		DefaultKeyword,
		SubroutineKeyword,
		CentroidKeyword,
		InKeyword,
		OutKeyword,
		InOutKeyword,

		VaryingKeyword,
		PatchKeyword,
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
		Mat2x2Keyword,
		Mat2x3Keyword,
		Mat2x4Keyword,
		Mat3x2Keyword,
		Mat3x3Keyword,
		Mat3x4Keyword,
		Mat4x2Keyword,
		Mat4x3Keyword,
		Mat4x4Keyword,
		DMat2Keyword,
		DMat3Keyword,
		DMat4Keyword,
		DMat2x2Keyword,
		DMat2x3Keyword,
		DMat2x4Keyword,
		DMat3x2Keyword,
		DMat3x3Keyword,
		DMat3x4Keyword,
		DMat4x2Keyword,
		DMat4x3Keyword,
		DMat4x4Keyword,

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
		LayoutQualifierID,
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
		FunctionStatement,
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
		LogicalXOrExpression,
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
		PostFixExpression,
		PostFixExpressionStart,
		PostFixExpressionContinuation,
		PostFixArrayAccess,
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
		WhitespaceTrivia,
		NewLineTrivia,
		TriviaList,
	}

#pragma warning disable SA1649 // File name must match first type name

	public static class Extentions
#pragma warning restore SA1649 // File name must match first type name
	{
		public static bool IsComment(this SyntaxType type)
		{
			return type == SyntaxType.LineCommentTrivia || type == SyntaxType.BlockCommentTrivia;
		}

		public static bool IsPreprocessor(this SyntaxType type)
		{
			return type >= SyntaxType.DefinePreprocessorKeyword && type <= SyntaxType.LinePreprocessorKeyword;
		}

		public static bool IsReserved(this SyntaxType type)
		{
			return type >= SyntaxType.CommonKeyword && type <= SyntaxType.UsingKeyword;
		}

		public static bool IsTrivia(this SyntaxType type)
		{
			return type == SyntaxType.WhitespaceTrivia || type == SyntaxType.LineCommentTrivia || type == SyntaxType.BlockCommentTrivia || type == SyntaxType.NewLineTrivia;
		}

		public static bool IsType(this SyntaxType type)
		{
			return type >= SyntaxType.BoolKeyword && type <= SyntaxType.VoidKeyword;
		}

		public static bool IsPuctuation(this SyntaxType type)
		{
			return type >= SyntaxType.LeftParenToken && type <= SyntaxType.MinusEqualToken;
		}

		public static bool IsKeyword(this SyntaxType type)
		{
			return type >= SyntaxType.AttributeKeyword && type <= SyntaxType.FalseKeyword;
		}

		public static bool IsNumber(this SyntaxType type)
		{
			return type >= SyntaxType.FloatConstToken && type <= SyntaxType.UIntConstToken;
		}

		internal static bool Contains(this SyntaxType[] array, SyntaxType type)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == type)
				{
					return true;
				}
			}

			return false;
		}
	}
}