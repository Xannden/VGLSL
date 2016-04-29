using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Definitions
{
	internal static class GLSLFileTypes
	{
		[Export]
		[FileExtension(".glsl")]
		[ContentType(GLSLConstants.ContentType)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "field used to by MEF")]
		internal static FileExtensionToContentTypeDefinition GLSLFileExtensionDefinition { get; }
	}
}