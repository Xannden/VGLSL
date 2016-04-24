﻿using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.IntelliSense.Completions
{
	[Export(typeof(IVsTextViewCreationListener))]
	[Name("GLSL Completions Handler")]
	[ContentType(GLSLConstants.ContentType)]
	[TextViewRole(PredefinedTextViewRoles.Editable)]
	internal class GLSLCompletionCommandHandlerProvider : IVsTextViewCreationListener
	{
		internal IVsEditorAdaptersFactoryService AdapterService { get; set; } = null;

		[Import]
		internal ICompletionBroker CompletionBroker { get; set; }

		[Import]
		internal SVsServiceProvider ServiceProvider { get; set; }

		public void VsTextViewCreated(IVsTextView textViewAdapter)
		{
			IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
			this.AdapterService = componentModel.GetService<IVsEditorAdaptersFactoryService>();

			ITextView textView = this.AdapterService.GetWpfTextView(textViewAdapter);
			if (textView == null)
			{
				return;
			}

			Func<GLSLCompletionCommandHandler> createCommandHandler = () => new GLSLCompletionCommandHandler(textViewAdapter, textView, this);
			textView.Properties.GetOrCreateSingletonProperty(createCommandHandler);
		}
	}
}