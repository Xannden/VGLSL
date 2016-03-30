using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Definitions
{
	internal static class GLSLContentTypeDefinition
	{
		[Export]
		[Name(GLSLConstants.ContentType)]
		[BaseDefinition("code")]
		internal static ContentTypeDefinition GLSLContentType { get; }
	}
}