using System.Drawing;
using Microsoft.VisualStudio.PlatformUI;

namespace Xannden.VSGLSL.Theme
{
	internal static class VSThemeHelper
	{
		public static VSTheme GetCurrentTheme()
		{
			Color color = VSColorTheme.GetThemedColor(EnvironmentColors.EnvironmentBackgroundColorKey);

			// ARGB
			// Blue = (255,41,57,85)
			// Light = (255,238,238,242)
			// Dark = (255,45,45,48)
			if (color == Color.FromArgb(41, 57, 85))
			{
				return VSTheme.Blue;
			}
			else if (color == Color.FromArgb(238, 238, 242))
			{
				return VSTheme.Light;
			}
			else if (color == Color.FromArgb(45, 45, 48))
			{
				return VSTheme.Dark;
			}
			else
			{
				return VSTheme.Unkown;
			}
		}
	}
}
