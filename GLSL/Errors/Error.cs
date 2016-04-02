﻿using Xannden.GLSL.Text;

namespace Xannden.GLSL.Errors
{
	public class Error
	{
		internal Error(string message, Span span)
		{
			this.Message = message;
			this.Span = span;
		}

		public string Message { get; }

		public Span Span { get; }
	}
}