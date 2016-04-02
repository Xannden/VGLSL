using System;

namespace Xannden.VSGLSL.Utility.Theme
{
	internal class ThemeChangedEventArgs : EventArgs
	{
		public ThemeChangedEventArgs(VSTheme theme)
		{
			this.Theme = theme;
		}

		public VSTheme Theme { get; }
	}
}
