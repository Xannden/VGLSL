using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Commands
{
	[Export(typeof(IVsTextViewCreationListener))]
	[Name("GLSL Command Provider")]
	[ContentType(GLSLConstants.ContentType)]
	[TextViewRole(PredefinedTextViewRoles.Editable)]
	internal sealed class GLSLCommandHandlersProvider : IVsTextViewCreationListener
	{
		[ImportMany(typeof(ICommand))]
		internal List<ICommand> Commands { get; set; }

		public void VsTextViewCreated(IVsTextView textViewAdapter)
		{
			IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;

			ITextView textView = componentModel.GetService<IVsEditorAdaptersFactoryService>().GetWpfTextView(textViewAdapter);

			if (textView == null)
			{
				return;
			}

			for (int i = 0; i < this.Commands.Count; i++)
			{
				this.Commands[i].Create(textViewAdapter, textView);

				textView.Properties.AddProperty(this.Commands[i]);
			}
		}
	}
}
