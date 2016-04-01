using System;
using System.Drawing;
using Microsoft.VisualStudio.PlatformUI;

namespace Xannden.VSGLSL.Utility.Theme
{
	internal static class VSThemeHelper
	{
		private static VSTheme currentTheme = VSTheme.Unkown;

		static VSThemeHelper()
		{
			currentTheme = GetCurrentTheme();
			VSColorTheme.ThemeChanged += VSThemeChanged;
		}

		public static event EventHandler<ThemeChangedEventArgs> ThemeChanged;

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

		private static void VSThemeChanged(Microsoft.VisualStudio.PlatformUI.ThemeChangedEventArgs e)
		{
			VSTheme theme = GetCurrentTheme();

			if (theme != currentTheme)
			{
				ThemeChanged?.Invoke(e, new ThemeChangedEventArgs(theme));
				currentTheme = theme;
			}
		}
	}
}
