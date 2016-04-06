using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Preprocessors
{
	[Export(typeof(ISuggestedActionsSourceProvider))]
	[Name("PreprocessorSuggestedActions")]
	[ContentType(GLSLConstants.ContentType)]
	internal class PreprocessorSuggestedActionsSourceProvider : ISuggestedActionsSourceProvider
	{
		[Import]
		internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

		public ISuggestedActionsSource CreateSuggestedActionsSource(ITextView textView, ITextBuffer textBuffer)
		{
			if (textBuffer == null && textView == null)
			{
				return null;
			}

			Source source = VSSource.GetOrCreate(textBuffer);

			return new PreprocessorSuggestedActionsSource(source, textView);
		}
	}
}