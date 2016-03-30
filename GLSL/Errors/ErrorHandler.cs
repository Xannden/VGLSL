using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Errors
{
	internal sealed class ErrorHandler
	{
		private List<Error> errors = new List<Error>();

		public void AddError(string message, Span span)
		{
			this.errors.Add(new Error(message, span));
		}

		public void ClearErrors()
		{
			this.errors.Clear();
		}

		public List<Error> GetErrors()
		{
			return this.errors;
		}
	}
}