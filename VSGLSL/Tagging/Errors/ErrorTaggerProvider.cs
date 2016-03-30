﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Xannden.GLSL.Errors;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Tagging.Errors
{
	[Export(typeof(ITaggerProvider))]
	[ContentType(GLSLConstants.ContentType)]
	[TagType(typeof(IErrorTag))]
	internal sealed class ErrorTaggerProvider : ITaggerProvider
	{
		public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
		{
			VSSource source = VSSource.GetOrCreate(buffer);

			ErrorHandler handler = buffer.Properties.GetOrCreateSingletonProperty(() => new ErrorHandler());

			return new ErrorTagger(handler, source) as ITagger<T>;
		}
	}
}