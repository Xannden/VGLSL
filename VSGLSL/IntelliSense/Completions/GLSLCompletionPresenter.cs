using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.IntelliSense.Completions
{
	[Export(typeof(IUIElementProvider<Completion, ICompletionSession>))]
	[Name("GLSL Completion Presenter")]
	[Order(After = "Default Completion Presenter")]
	[ContentType(GLSLConstants.ContentType)]
	internal sealed class GLSLCompletionPresenter : IUIElementProvider<Completion, ICompletionSession>
	{
		public UIElement GetUIElement(Completion itemToRender, ICompletionSession context, UIElementType elementType)
		{
			GLSLCompletion completion = itemToRender as GLSLCompletion;

			if (completion != null)
			{
				return completion.TextBlock;
			}

			if (elementType == UIElementType.Tooltip)
			{
				return new TextBlock(new Run(itemToRender.Description));
			}
			else
			{
				return null;
			}
		}
	}
}
