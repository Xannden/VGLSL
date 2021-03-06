﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Outlining
{
	[Export(typeof(ITaggerProvider))]
	[TagType(typeof(IOutliningRegionTag))]
	[ContentType(GLSLConstants.ContentType)]
	internal sealed class GLSLOutliningTaggerProvider : ITaggerProvider
	{
		[Import]
		internal IClassificationFormatMapService FormatMapService { get; private set; } = null;

		[Import]
		internal IClassificationTypeRegistryService TypeRegistry { get; private set; } = null;

		public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
		{
			return buffer.Properties.GetOrCreateSingletonProperty(() => new GLSLOutliningTagger(this, VSSource.GetOrCreate(buffer))) as ITagger<T>;
		}
	}
}
