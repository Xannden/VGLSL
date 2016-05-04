using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Intellisense.QuickInfo
{
	[Export(typeof(IIntellisenseControllerProvider))]
	[Name("GLSL QuickInfo Controller")]
	[ContentType(GLSLConstants.ContentType)]
	internal class GLSLQuickInfoControllerProvider : IIntellisenseControllerProvider
	{
		[Import]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field set to by MEF")]
		internal IQuickInfoBroker QuickInfoBroker { get; private set; }

		public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
		{
			return new GLSLQuickInfoController(this, VSSource.GetOrCreate(textView.TextBuffer), textView, subjectBuffers);
		}
	}
}
