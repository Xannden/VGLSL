using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Errors
{
	public sealed class ErrorHandler
	{
		private List<Error> errors = new List<Error>();

		public void AddError(string message, Span span)
		{
			this.errors.Add(new Error(message, span));
		}

		public List<Error> GetErrors()
		{
			return this.errors;
		}

		public void ClearErrors()
		{
			this.errors.Clear();
		}
	}
}