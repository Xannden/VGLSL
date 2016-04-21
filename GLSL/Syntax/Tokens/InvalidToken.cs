using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tokens
{
	internal sealed class InvalidToken : Token
	{
		public InvalidToken(SyntaxType type, Span span, SourceLine line, string text, SyntaxTrivia leadingTrivia, string message) : base(SyntaxType.InvalidToken, span, line, text, leadingTrivia)
		{
			this.ErrorMessage = message;
			this.ErrorType = type;
		}

		public InvalidToken(Token token, string message) : this(token.SyntaxType, token.Span, token.Line, token.Text, token.LeadingTrivia, message)
		{
			this.TrailingTrivia = token.TrailingTrivia;
		}

		public string ErrorMessage { get; }

		public SyntaxType ErrorType { get; }
	}
}