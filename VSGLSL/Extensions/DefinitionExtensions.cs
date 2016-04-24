using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Xannden.GLSL.Semantics;

namespace Xannden.VSGLSL.Extensions
{
	internal static class DefinitionExtensions
	{
		public static ImageSource GetImageSource(this Definition definition, IGlyphService glyphService)
		{
			ImageSource imageSource = null;

			switch (definition.Kind)
			{
				case DefinitionKind.Field:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupField, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionKind.LocalVariable:
				case DefinitionKind.GlobalVariable:
				case DefinitionKind.Parameter:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionKind.Macro:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMacro, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionKind.TypeName:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupStruct, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionKind.Function:
					imageSource = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMethod, StandardGlyphItem.GlyphItemPublic);
					break;
			}

			return imageSource;
		}

		public static Image GetIcon(this Definition definition, IGlyphService glyphService)
		{
			ImageSource imageSource = definition.GetImageSource(glyphService);

			if (imageSource != null)
			{
				return new Image
				{
					Source = imageSource
				};
			}

			return null;
		}
	}
}
