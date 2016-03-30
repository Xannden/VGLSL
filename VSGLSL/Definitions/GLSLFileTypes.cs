using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Definitions
{
	internal static class GLSLFileTypes
	{
		[Export]
		[FileExtension(".glsl")]
		[ContentType(GLSLConstants.ContentType)]
		internal static FileExtensionToContentTypeDefinition GLSLFileExtensionDefinition { get; }
	}
}