using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.IntelliSense.QuickTips
{
	[Export(typeof(IIntellisenseControllerProvider))]
	[Name("GLSL QuickInfo Controller")]
	[ContentType(GLSLConstants.ContentType)]
	internal class GLSLQuickInfoControllerProvider : IIntellisenseControllerProvider
	{
		[Import]
		internal IQuickInfoBroker QuickInfoBroker { get; private set; }

		public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
		{
			return new GLSLQuickInfoController(this, textView, subjectBuffers);
		}
	}
}
