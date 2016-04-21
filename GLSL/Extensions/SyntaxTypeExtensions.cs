using System.Reflection;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Utility;

namespace Xannden.GLSL.Extensions
{
	public static class SyntaxTypeExtensions
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
			return type == SyntaxType.WhiteSpaceTrivia || type == SyntaxType.LineCommentTrivia || type == SyntaxType.BlockCommentTrivia || type == SyntaxType.NewLineTrivia;
		}

		public static bool IsType(this SyntaxType type)
		{
			return type >= SyntaxType.BoolKeyword && type <= SyntaxType.VoidKeyword;
		}

		public static bool IsPunctuation(this SyntaxType type)
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

		internal static string GetText(this SyntaxType syntaxType)
		{
			return syntaxType.GetType().GetField(syntaxType.ToString())?.GetCustomAttribute<TextAttribute>()?.Text ?? string.Empty;
		}
	}
}
