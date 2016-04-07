using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Xannden.GLSL.Errors;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Errors
{
	[Export(typeof(ITaggerProvider))]
	[ContentType(GLSLConstants.ContentType)]
	[TagType(typeof(IErrorTag))]
	internal sealed class ErrorTaggerProvider : ITaggerProvider
	{
		public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			VSSource source = VSSource.GetOrCreate(buffer);

			ErrorHandler handler = buffer.Properties.GetOrCreateSingletonProperty(() => new ErrorHandler());

			return new ErrorTagger(handler, source) as ITagger<T>;
		}
	}
}