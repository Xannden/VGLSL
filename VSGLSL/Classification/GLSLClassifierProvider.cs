using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Classification
{
	[Export(typeof(IClassifierProvider))]
	[ContentType(GLSLConstants.ContentType)]
	internal sealed class GLSLClassifierProvider : IClassifierProvider
	{
		[Import]
		internal IClassificationTypeRegistryService ClassificationTypeRegistryService { get; private set; } = null;

		public IClassifier GetClassifier(ITextBuffer textBuffer)
		{
			if (textBuffer == null)
			{
				throw new ArgumentNullException(nameof(textBuffer));
			}

			VSSource source = VSSource.GetOrCreate(textBuffer);

			return textBuffer.Properties.GetOrCreateSingletonProperty(() => new GLSLClassifier(source, this.ClassificationTypeRegistryService));
		}
	}
}