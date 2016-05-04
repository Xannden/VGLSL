using Xannden.GLSL.Text;

namespace Xannden.GLSL.Errors
{
	public sealed class GLSLError
	{
		public GLSLError(string message, Span span)
		{
			this.Message = message;
			this.Span = span;
		}

		public string Message { get; }

		public Span Span { get; }
	}
}