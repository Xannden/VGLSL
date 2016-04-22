using System;

namespace Xannden.GLSL.Utility
{
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class TextAttribute : Attribute
	{
		public TextAttribute(string text)
		{
			this.Text = text;
		}

		public string Text { get; }
	}
}
