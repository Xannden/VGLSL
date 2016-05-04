using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Xannden.GLSL.Semantics;

namespace Xannden.VSGLSL.Intellisense.Completions
{
	internal sealed class GLSLCompletion : Completion
	{
		public GLSLCompletion(TextBlock textBlock, Definition definition, ImageSource icon) : base(definition.Name, definition.Name, definition.Name, icon, string.Empty)
		{
			this.TextBlock = textBlock;
		}

		public GLSLCompletion(TextBlock textBlock, string displayText, string discription, ImageSource icon) : base(displayText, displayText, discription, icon, string.Empty)
		{
			this.TextBlock = textBlock;
		}

		public TextBlock TextBlock { get; }
	}
}
