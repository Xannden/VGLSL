using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Intellisense.Completions;

namespace Xannden.VSGLSL.Commands
{
	[Export(typeof(IVsTextViewCreationListener))]
	[Name("GLSL Command Provider")]
	[ContentType(GLSLConstants.ContentType)]
	[TextViewRole(PredefinedTextViewRoles.Editable)]
	internal sealed class GLSLCommandHandlersProvider : IVsTextViewCreationListener
	{
		[Import]
		private ICompletionBroker CompletionBroker { get; set; }

		[Import]
		private IQuickInfoBroker QuickInfoBroker { get; set; }

		[Import]
		private ISignatureHelpBroker SignagtureHelpBroker { get; set; }

		[Import]
		private IGlyphService GlyphService { get; set; }

		[Import]
		private IClassificationFormatMapService FormatMapService { get; set; }

		[Import]
		private IClassificationTypeRegistryService TypeRegistry { get; set; }

		public void VsTextViewCreated(IVsTextView textViewAdapter)
		{
			IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;

			ITextView textView = componentModel.GetService<IVsEditorAdaptersFactoryService>().GetWpfTextView(textViewAdapter);

			if (textView == null)
			{
				return;
			}

			GLSLCompletionSource.LoadDefaultCompletions(this.GlyphService, this.FormatMapService, this.TypeRegistry);

			textView.Properties.GetOrCreateSingletonProperty(() => new CommentSelectionCommand(textViewAdapter, textView));
			textView.Properties.GetOrCreateSingletonProperty(() => new UnCommentSelectionCommand(textViewAdapter, textView));
			textView.Properties.GetOrCreateSingletonProperty(() => new CompletionCommand(textViewAdapter, textView, this.CompletionBroker));
			textView.Properties.GetOrCreateSingletonProperty(() => new GoToDefinitionCommand(textViewAdapter, textView));
			textView.Properties.GetOrCreateSingletonProperty(() => new QuickInfoCommand(textViewAdapter, textView, this.QuickInfoBroker));
			textView.Properties.GetOrCreateSingletonProperty(() => new SignatureHelpCommand(textViewAdapter, textView, this.SignagtureHelpBroker));
		}
	}
}