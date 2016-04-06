using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Errors
{
	public sealed class ErrorHandler
	{
		private List<GLSLError> errors = new List<GLSLError>();

		public IReadOnlyList<GLSLError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		public void AddError(string message, Span span)
		{
			this.errors.Add(new GLSLError(message, span));
		}

		public void ClearErrors()
		{
			this.errors.Clear();
		}
	}
}