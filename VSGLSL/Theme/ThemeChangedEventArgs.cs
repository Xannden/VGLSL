using System;

namespace Xannden.VSGLSL.Theme
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
